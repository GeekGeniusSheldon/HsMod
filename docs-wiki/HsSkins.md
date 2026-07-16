# HsSkins.cfg

`HsSkins.cfg` is the skin mapping file, used mainly for hero skin replacement in Battlegrounds and standard play. It lives at:

```
Hearthstone\BepInEx\config\HsSkins.cfg
```

and is created automatically on first run.

- [How it relates to the Skin settings](#how-it-relates-to-the-skin-settings)
- [Format](#format)
- [Random replacement](#random-replacement)
- [Getting skin IDs](#getting-skin-ids)
- [Applying changes](#applying-changes)
- [Notes and priority](#notes-and-priority)

## How it relates to the Skin settings

There are two overlapping ways to change cosmetics:

- **The Skin section in the config** - single preferred IDs for the coin, card back, boards, Bob, pets and heroes. Best for one fixed choice.
- **`HsSkins.cfg`** - a mapping table, mainly for **hero** skins, that can map many originals and supports random selection. Best for "randomize my hero skin" or per-hero rules.

## Format

One mapping per line: `originalSkin:replacementSkin`. The colon is a normal (half-width) character. Lines beginning with `#` are comments.

```
# Skin mapping table
# Format: original skin : replacement skin
# Example: Malfurion Stormrage (274) replaced by Grandmaster Malfurion (57761)
274:57761
```

Both sides are numeric skin IDs. The left side is the skin the game would normally show; the right side is what HsMod displays instead.

## Random replacement

Map one original to several replacements separated by commas, and one is chosen at random each time:

```
274:57761,57762,57763
```

This is how you get a "random hero skin" effect for a specific hero.

## Getting skin IDs

- Enable **Show Card ID** in the config, then right-click a skin in the collection to display and copy its ID.
- Press `F4` in-game to write the full current skin list into `Hearthstone\BepInEx\HsMod\`. This is the easiest way to collect the IDs of everything you own.

## Applying changes

- Press `F4` to reload `HsSkins.cfg` and dump the current skin IDs.
- To apply a change **during a match**, press `F4` to save, then simulate a disconnect (see [Hotkeys](Hotkeys.md#match-actions)) so the board and hero refresh with the new skins.

> [!TIP]
> Card **backs** apply instantly and do not need the F4-plus-disconnect dance. Hero and board skins do.

## Notes and priority

> [!WARNING]
> Hero skin replacement is not recommended outside AFK play. It has a lower priority than the "Default Hero" option, and applying it live requires the disconnect refresh.

If skins display incorrectly, re-check `HsSkins.cfg` for typos (a wrong ID or a full-width colon are common mistakes), and as a last resort delete `HsMod.cfg` to reconfigure. See [Troubleshooting](Troubleshooting.md#skins-look-wrong).
