# HsMod Wiki

Welcome to the HsMod wiki. HsMod is a [BepInEx](https://github.com/BepInEx/BepInEx) plugin for Hearthstone that adds automation, quality-of-life tweaks and a set of Battlegrounds tools. It runs inside the game process using Harmony patches.

The [README](../ReadMe.md) is the two-minute overview. This wiki is the detailed reference: how features work, what every setting does, and how to fix things when they break.

## New here? Start with these

1. [Installation](Installation.md) - get BepInEx and the plugin running on your platform.
2. [Configuration](Configuration.md) - how settings work, and what each one does.
3. [Hotkeys](Hotkeys.md) - the keyboard shortcuts and how to rebind them.

## All pages

| Page | What it covers |
|---|---|
| [Installation](Installation.md) | BepInEx setup and the plugin on Windows, macOS and Linux, with per-step checks. |
| [Building from source](Building-from-source.md) | Compiling `HsMod.dll`, project layout, and how the patch system fits together. |
| [Configuration](Configuration.md) | The config file, the templates, and every setting grouped by section. |
| [Hotkeys](Hotkeys.md) | Default shortcuts, how rebinding and key capture work, and modifier keys. |
| [Battlegrounds](Battlegrounds.md) | The MMR overlay, session statistics and CSV format, shop hotkeys and auto-squelch. |
| [HsSkins.cfg](HsSkins.md) | The skin mapping file: format, examples, and the in-match apply workflow. |
| [Troubleshooting](Troubleshooting.md) | Symptom-by-symptom fixes, plus `client.config` for launching without Battle.net. |

## Good to know

> [!NOTE]
> Almost every feature is **off by default** and must be enabled manually, either in the in-game menu (`Esc` -> Mod Settings) or in `HsMod.cfg`.

> [!WARNING]
> Some features carry account risk. Cheating and account-risky options may violate Blizzard's Terms of Service. The anti-cheat block is best-effort and cannot guarantee account safety. Use responsibly.

Other essentials:

- Use **BepInEx 5**. BepInEx 6 is still pre-release and is not adapted for now.
- Hearthstone became a **64-bit** program on 2026-07-01; use the x64 BepInEx build.
- The Hearthstone install path must not contain **non-Latin characters**.
- The plugin does not collect any information about you. License: **AGPL-3.0**.

## About this wiki

The project is maintained by Pik_4 at [github.com/Pik-4/HsMod](https://github.com/Pik-4/HsMod). Bug reports and feature ideas are welcome via [issues](https://github.com/Pik-4/HsMod/issues).
