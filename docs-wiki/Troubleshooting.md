# Troubleshooting

- [The plugin does not load](#the-plugin-does-not-load)
- [A shortcut does nothing](#a-shortcut-does-nothing)
- [Settings do not save](#settings-do-not-save)
- [The wrong language is used](#the-wrong-language-is-used)
- [Skins look wrong](#skins-look-wrong)
- [Game buttons stop responding](#game-buttons-stop-responding)
- [Battlegrounds MMR is missing](#battlegrounds-mmr-is-missing)
- [Conflicts with other mods](#conflicts-with-other-mods)
- [General recovery](#general-recovery)
- [client.config](#clientconfig)

## The plugin does not load

Work down this list in order:

1. Confirm **BepInEx 5** (not 6) and the **x64** build (Hearthstone is 64-bit since 2026-07-01).
2. Open `doorstop_config.ini` and confirm the override line points to `BepInEx\unstripped_corlib` (`dll_search_path_override` for 5.4.23.2+, `dllSearchPathOverride` for older).
3. Confirm `BepInEx\unstripped_corlib\` actually contains the DLLs from `HsMod/UnstrippedCorlib`. Type-load errors in the log almost always mean these are missing.
4. Confirm the Hearthstone install path has **no non-Latin characters**.
5. After launching, check `Hearthstone\BepInEx\LogOutput.log` exists and mentions HsMod. The log is the fastest way to see the real error.

See the [Installation checklist](Installation.md#verifying-the-install) for the same steps mapped to symptoms.

## A shortcut does nothing

- **Shortcut Status** (Global section) is off by default. Enable it.
- Shortcuts are suppressed while a text field is focused (e.g. chat) or while the Mod Settings window is open.
- Battlegrounds shop keys only work during the shopping phase.

## Settings do not save

Check whether another Hearthstone plugin is enabled; a conflicting plugin can prevent the config from being written.

## The wrong language is used

The plugin detects the language from the Hearthstone client first, then from the system locale. If it picks the wrong one:

- set the language in the settings menu, or
- edit `HsMod.Init.Language` in `HsMod.cfg`.

> [!IMPORTANT]
> Changing the language may invalidate the old configuration, because settings are stored under localized names. Back up `HsMod.cfg` first, and re-check your enabled options afterwards.

## Skins look wrong

- Verify `HsSkins.cfg` for typos: a wrong ID or a full-width colon are the usual causes. See [HsSkins.cfg](HsSkins.md).
- Remember that hero and board skins need an F4 save plus a simulated disconnect to appear in a live match; card backs apply instantly.
- As a last resort, delete `HsMod.cfg` to reconfigure.

## Game buttons stop responding

Close and reopen the game. If it persists, delete the relevant `.cfg` in `Hearthstone\BepInEx\config\` and reconfigure.

## Battlegrounds MMR is missing

- MMR only appears in Battlegrounds and needs the leaderboard to have loaded. It fetches in the background at match start.
- If the data source is unreachable, the last cached leaderboard is used; if there is no cache, no MMR is shown.
- Hidden names (streamer mode) are not in the leaderboard, so they show nothing rather than a value.

## Conflicts with other mods

HsMod may conflict with mods that patch `Assembly-CSharp.dll` (for example MixMod). Overlapping method patches can misalign IL offsets and produce unexpected results. The plugin does not detect whether the original method was already modified. If you run multiple `Assembly-CSharp` mods and see odd behaviour, disable the others to isolate the cause.

## General recovery

When something breaks, first delete the relevant `.cfg` (usually in `BepInEx\config\`) and reconfigure. If the problem remains, attach your `HsMod.cfg` to an issue on the upstream repository, though a timely answer is not guaranteed.

## client.config

`client.config` lets you start Hearthstone bypassing Battle.net. Place it next to `Hearthstone.exe`:

```ini
[Config]
Version = 3
[Aurora]
VerifyWebCredentials = "TOKEN"
ClientCheck = 0
Env.Override = 1
Env = us.actual.battle.net
```

Get a token by opening a Battle.net login URL for your region and copying the part after `http://localhost:0/?ST=` and before `&accountId=`:

```
https://us.battle.net/login/en/?app=wtcg
https://eu.battle.net/login/en/?app=wtcg
https://tw.battle.net/login/zh/?app=wtcg
https://kr.battle.net/login/zh/?app=wtcg
https://account.battlenet.com.cn/login/zh-cn/?app=wtcg
```

The `Env` value for China is `cn.actual.battlenet.com.cn`; otherwise it matches the first two characters of the token. If the token becomes obsolete and the game stops opening, update it in `client.config`. With the plugin enabled, `./Hearthstone.exe VerifyWebCredentials` also works and no longer strictly requires a `client.config`.
