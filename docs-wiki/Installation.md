# Installation

- [Before you start](#before-you-start)
- [What BepInEx does](#what-bepinex-does)
- [Windows](#windows)
- [macOS](#macos)
- [Linux](#linux)
- [Verifying the install](#verifying-the-install)
- [Target folder layout](#target-folder-layout)
- [Updating](#updating)

## Before you start

> [!IMPORTANT]
> Three rules that cause most failed installs:
> - Use **BepInEx 5**, not 6 (6 is still pre-release and is not adapted).
> - Use the **x64** BepInEx build. Hearthstone became 64-bit on 2026-07-01.
> - The Hearthstone install path must not contain **non-Latin characters**.

You will need `HsMod.dll` (download it from Releases, or [build it from source](Building-from-source.md)) and the `UnstrippedCorlib` DLLs that ship with the project.

## What BepInEx does

BepInEx is a plugin loader. On Windows it hooks the game through a `winhttp.dll` / Doorstop shim next to `Hearthstone.exe`, loads its own runtime, then loads every DLL in `BepInEx\plugins`. HsMod is one such plugin.

Hearthstone ships a **stripped** version of the .NET core libraries (mscorlib and friends), which removes types the plugin needs. The `unstripped_corlib` folder holds full versions of those libraries, and the `doorstop` override tells Mono to load them first. This is why the `unstripped_corlib` step is not optional: without it the plugin fails to load with type errors.

## Windows

1. Get `HsMod.dll` (Releases or [build from source](Building-from-source.md)).
2. Download [BepInEx x64](https://github.com/BepInEx/BepInEx/releases) and extract it into the Hearthstone root folder (`Hearthstone\`). You should now have `Hearthstone\BepInEx\`, `Hearthstone\doorstop_config.ini` and `Hearthstone\winhttp.dll`.
3. Create `Hearthstone\BepInEx\unstripped_corlib\` and copy **every** DLL from the project's `HsMod/UnstrippedCorlib` into it.
4. Edit `Hearthstone\doorstop_config.ini`:
   - BepInEx **5.4.23.2 or newer**: set `dll_search_path_override = BepInEx\unstripped_corlib`
   - **older** BepInEx: set `dllSearchPathOverride=BepInEx\unstripped_corlib`
5. Put `HsMod.dll` into `Hearthstone\BepInEx\plugins` (create the folder if it does not exist).
6. Launch Hearthstone normally.

> [!TIP]
> Not sure which BepInEx you have? Open `doorstop_config.ini`. If it already contains a `dll_search_path_override` line, you are on 5.4.23.2 or newer and should edit that line rather than adding the old `dllSearchPathOverride` one.

## macOS

1. Download [BepInEx_macos_universal](https://github.com/BepInEx/BepInEx/releases) (BepInEx 5) and extract it into `Hearthstone/`.
2. Copy every DLL from `HsMod/UnstrippedCorlibUnix` into `/Applications/Hearthstone/BepInEx/unstripped_corlib/`.
3. In `run_bepinex.sh`, set:
   - `dll_search_path_override="BepInEx/unstripped_corlib"`
   - `executable_name="Hearthstone.app"`
4. Make the launcher executable: `chmod u+x run_bepinex.sh`.
5. Get a login token (see [client.config](Troubleshooting.md#clientconfig)): from a Battle.net login URL, copy the part after `http://localhost:0/?ST=` and before `&accountId=`.
6. Optionally create a `client.config` so the token persists between launches.
7. Put `HsMod.dll` into `Hearthstone/BepInEx/plugins`.
8. Launch with `./run_bepinex.sh TOKEN`, or `./run_bepinex.sh` when a `client.config` is present.

> [!NOTE]
> The Mono and Unity versions of the `UnstrippedCorlibUnix` DLLs must match your Hearthstone build, or the game will fail to start.

## Linux

1. Get `HsMod.dll` (Releases or build from source).
2. Install Hearthstone for Linux via [0xf4b1/hearthstone-linux](https://github.com/0xf4b1/hearthstone-linux). This usually configures `client.config` for you.
3. Download [BepInEx (unix, v5)](https://github.com/BepInEx/BepInEx/releases) and extract it into the Hearthstone root.
4. Create `hearthstone/BepInEx/unstripped_corlib/` and copy the DLLs from `HsMod/UnstrippedCorlibUnix` into it. (UniTask is extracted from net48 of `OpenMod.UniTask.2021.2.4.1`.)
5. In `run_bepinex.sh`, set:
   - `DOORSTOP_CORLIB_OVERRIDE_PATH="$BASEDIR/BepInEx/unstripped_corlib"`
   - `executable_name="Bin/Hearthstone.x86_64"`
   - then normalize line endings: `sed -i "s/\r/ /g" ./run_bepinex.sh`
6. If `client.config` is missing, follow the macOS token steps.
7. Put `HsMod.dll` into `hearthstone/BepInEx/plugins` (create the folder if missing).
8. `chmod u+x run_bepinex.sh`, then run `./run_bepinex.sh`.

## Verifying the install

After the first launch, confirm all of these:

- `Hearthstone\BepInEx\LogOutput.log` exists and mentions `HsMod`.
- The in-game menu (`Esc`) shows a **Mod Settings** button.
- `Hearthstone\BepInEx\config\HsMod.cfg` was created.

If any are missing, work through this checklist:

| Symptom | Likely cause |
|---|---|
| No `BepInEx` folder appears after launch | BepInEx was not extracted into the game root, or the x86 build was used instead of x64. |
| `LogOutput.log` shows type-load errors | The `unstripped_corlib` DLLs are missing, or the `doorstop` override path is wrong. |
| Plugin loads but no button | `HsMod.dll` is an old build, or "Mod Settings Button" is disabled in the config. |
| Nothing loads at all | The install path contains non-Latin characters. |

## Target folder layout

A correct Windows install looks like this:

```
Hearthstone\
  Hearthstone.exe
  winhttp.dll
  doorstop_config.ini      # dll_search_path_override = BepInEx\unstripped_corlib
  BepInEx\
    core\                  # BepInEx runtime
    plugins\
      HsMod.dll
    unstripped_corlib\     # full corlib DLLs from HsMod/UnstrippedCorlib
    config\
      HsMod.cfg            # created on first run
      HsSkins.cfg          # created on first run
```

## Updating

To update the plugin, replace `HsMod.dll` in `BepInEx\plugins` with the new build; the game picks it up on the next launch. Your `HsMod.cfg` is preserved. BepInEx and the `unstripped_corlib` DLLs only need replacing when a Hearthstone update changes the runtime.
