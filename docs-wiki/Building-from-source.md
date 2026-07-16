# Building from source

- [Requirements](#requirements)
- [Build](#build)
- [Post-build](#post-build)
- [Project layout](#project-layout)
- [How patches work](#how-patches-work)
- [Adding a feature](#adding-a-feature)
- [Localization](#localization)
- [Version scheme](#version-scheme)

## Requirements

- **.NET SDK 8.x**
- The game and BepInEx reference assemblies bundled in the repository (`HsMod/LibHearthstone`, `HsMod/BepInExCore`, `HsMod/UnstrippedCorlib*`).

No local Hearthstone install is needed to compile: everything the build references is committed to the repo. This is why CI can build the plugin on a clean runner.

## Build

```bash
git clone --depth 1 --branch bepinex5 https://github.com/Pik-4/HsMod
cd HsMod
dotnet build --configuration Release --no-restore
```

The output is `HsMod/Release/HsMod.dll`.

## Post-build

The project's post-build step runs `install.bat`, which copies the compiled DLL into a default Hearthstone install if BepInEx is already configured there. When Hearthstone is installed elsewhere (for example on another drive), copy `HsMod/Release/HsMod.dll` into `<Hearthstone>\BepInEx\plugins\` manually. The DLL is only picked up on the next game launch.

## Project layout

| Path | Role |
|---|---|
| `Main.cs` | Plugin entry point (`BaseUnityPlugin`): command-line parsing, cache cleanup, component bootstrap, the shortcut loop. |
| `PluginConfig.cs` | Every BepInEx `ConfigEntry` and the configuration templates. |
| `Patcher.cs` | Loads and unloads Harmony patches, and wires `SettingChanged` handlers so toggles apply live. |
| `Patches/` | One file per feature area (for example `PatchBattlegrounds.cs`, `PatchEmote.cs`). |
| `ModSettingsUI.cs` | The in-game settings window (IMGUI). |
| `Languages/*.json` | Localization files, compiled in as embedded resources. |
| `WebServer.cs`, `WebApi.cs`, `WebPage.cs` | The built-in Showinfo web server. |
| `LibHearthstone/`, `BepInExCore/`, `UnstrippedCorlib*/` | Reference assemblies for compilation. |

## How patches work

HsMod uses [Harmony](https://harmony.pardeike.net/) to patch game methods at runtime. Each feature is a patch class under `Patches/`, registered in `Patcher.cs`. Patches come in a few shapes:

- **Prefix / Postfix** - run code before or after a game method, and optionally change its arguments or result.
- **Transpiler** - rewrite the method's IL directly, used when a small change deep inside a method is needed.

Because transpilers target specific IL offsets, a Hearthstone update that changes `Assembly-CSharp.dll` can shift those offsets and break a patch. This is also why HsMod can conflict with other mods that patch the same methods.

Some plugin behaviour lives in plain `MonoBehaviour` components created in `Main.cs` (for example the settings window and the Battlegrounds helpers) rather than in Harmony patches. These gate themselves on their own config flags each frame.

## Adding a feature

A typical new toggle-driven feature touches four places:

1. `PluginConfig.cs` - declare a `ConfigEntry` and bind it in `ConfigBind`.
2. `Patches/` - add the patch class (or a component).
3. `Patcher.cs` - register the patch and, if it should toggle live, add a `SettingChanged` handler.
4. `Languages/*.json` - add the `name`, `label` (section) and `description` keys for the setting, in every language file.

## Localization

Language files are keyed strings under `HsMod/Languages`. Each setting uses three keys: `<field>.name`, `<field>.label` (the section) and `<field>.description`. `enUS.json` is the fallback used when a key is missing from the active language, so it must always contain every key. The plugin picks the language from the Hearthstone client first, then the system locale.

## Version scheme

For example, HsMod version `3.0.0.0`:

- **First digit** (3): the Hearthstone major version, e.g. 3 means 26.
- **Second digit** (0): how many times Hearthstone has been updated within that version. It does not map to Hearthstone minor versions, and is not incremented when a Hearthstone update does not change files such as `Assembly-CSharp.dll`.
- **Third digit** (0): incremented by 1 when HsMod gains new features for that Hearthstone version; reset to zero when the second digit changes.
- **Fourth digit** (0): the compile version, mainly tracking bug fixes for the third digit.

A Hearthstone update does not necessarily break HsMod. If the plugin still works, it does not need to be updated from Releases.
