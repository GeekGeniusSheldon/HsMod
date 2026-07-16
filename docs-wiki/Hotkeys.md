# Hotkeys

- [Enabling shortcuts](#enabling-shortcuts)
- [Rebinding](#rebinding)
- [The fixed F4 key](#the-fixed-f4-key)
- [Shortcut list](#shortcut-list)

## Enabling shortcuts

All shortcuts (except `F4`) only work when **Shortcut Status** is enabled in the Global section. It is **off by default**, so if a key does nothing, check this first.

Shortcuts are also suppressed in two situations, to avoid accidents:

- while a text field is focused (for example the Battle.net chat), so typing never triggers a shortcut; and
- while the in-game Mod Settings window is open.

## Rebinding

Every shortcut can be rebound in two ways:

- **In-game menu** - open Mod Settings, find the shortcut, click its button, then press the new key combination. Modifier keys (Ctrl, Shift, Alt) held during capture are included. Right, middle and side mouse buttons can also be captured. Press `Esc` to cancel capture, or the `X` button to clear the binding.
- **`HsMod.cfg`** - edit the value under the Shortcut section. The format is BepInEx's `KeyboardShortcut`, for example `R` or `Z + LeftControl`.

The exact config key for each shortcut is listed in the [Configuration](Configuration.md#shortcut) page and shown in the menu.

> [!TIP]
> Single-letter Battlegrounds keys (R / F / U / H / T) do not clash with Ctrl-based shortcuts like `Left Ctrl+R`, because the modifier is part of the binding.

## The fixed F4 key

`F4` is not rebindable. Pressing it:

- dumps some in-game information into `Hearthstone\BepInEx\` (used by several features),
- reloads the skin configuration (`HsSkins.cfg`), and
- restarts the Showinfo web service.

It is also the key you press to save skin changes before simulating a disconnect. See [HsSkins.cfg](HsSkins.md).

## Shortcut list

Defaults are shown; all are rebindable.

### Speed gear

| Default | Action |
|---|---|
| Up arrow | Increase gear multiplier by 1 |
| Down arrow | Decrease gear multiplier by 1 |
| Left arrow | Reset gear multiplier |
| Right arrow | Set gear multiplier to maximum |

### Battlegrounds shop (shopping phase only)

| Default | Action |
|---|---|
| `R` | Refresh tavern |
| `F` | Freeze / unfreeze tavern |
| `U` | Upgrade tavern tier |
| `H` | Hero power (enters targeting mode if the power needs a target) |
| `T` | Show / hide teammate board (duos) |

### Match actions

| Default | Action |
|---|---|
| Left Ctrl + D | Simulate disconnect (needs auto-exit off and pop-ups allowed) |
| Left Ctrl + Space | Concede |
| Space | End turn / confirm mulligan |
| Left Ctrl + Q | Squelch opponent |
| Left Ctrl + S | Mute / restore volume |
| Left Ctrl + B | Toggle Bob's voice |
| Left Ctrl + C | Copy opponent BattleTag |
| Left mouse | Copy selected opponent BattleTag (Battlegrounds) |
| Left Ctrl + P | Show / hide FPS |

### Emotes

| Default | Action |
|---|---|
| 1 | Greetings |
| 2 | Well Played |
| 3 | Thanks |
| 4 | Wow |
| 5 | Oops |
| 6 | Threaten |

### Collection and pack opening

| Default | Action |
|---|---|
| Left Ctrl + Z | Full disenchant (requires auto-disenchant enabled; collection or pack-opening screens only) |
| Left Ctrl + R | Remove all `new!` markers |

> [!WARNING]
> The store's zero-cost shortcut and adventure automation carry a risk of an account ban. Use discretion.
