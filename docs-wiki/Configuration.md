# Configuration

- [How settings are stored](#how-settings-are-stored)
- [Configuration templates](#configuration-templates)
- [Settings by section](#settings-by-section)

## How settings are stored

HsMod uses the standard BepInEx configuration system. Every setting lives in a single TOML file:

```
Hearthstone\BepInEx\config\HsMod.cfg
```

The file is created on first launch and rewritten whenever a value changes. Each setting belongs to a **section** (the `[Section]` header in the file), which matches the tabs in the in-game menu (Global, Optimization, Battlegrounds, and so on).

There are two ways to edit settings, and they stay in sync:

- **In-game menu** - press `Esc`, then **Mod Settings**. This is the recommended way: it has search, live editing, key-binding capture, and sliders for numeric ranges. Changes apply immediately.
- **The `HsMod.cfg` file** - edit it in any text editor while the game is closed. Useful for backups and for copying a configuration between machines.

> [!IMPORTANT]
> The section and setting names in `HsMod.cfg` are **localized**. If you change the plugin language, BepInEx re-creates the entries under the new names and the old values are left behind as orphaned lines. Back up `HsMod.cfg` before switching language, and re-check your enabled options afterwards.

> [!NOTE]
> Almost every feature is **off by default**. A few internal options are marked *Advanced* and are hidden in the menu unless you enable "Show Advanced".

## Configuration templates

The **Configuration Template** setting (Global section) applies a preset bundle of settings in one click, then resets itself back to `DoNothing`. It is the fastest way to switch between play styles.

| Template | What it sets up |
|---|---|
| `DoNothing` | Default. Applies nothing. |
| `AwayFromKeyboard` | AFK / idle-farming preset: hides pop-ups and toasts, auto-answers dialogs, auto-opens reward boxes, auto-exits on error, disables the idle kick, enables quick pack opening, skips hero intros, and blocks opponent emotes. |
| `AntiAwayFromKeyboard` | Interactive preset: keeps pop-ups and toasts visible, enables the idle kick, allows a small opponent emote limit, and keeps card effects on. |

You can apply a template and then fine-tune individual settings afterwards.

## Settings by section

The tables below cover the notable settings per section. The in-game menu always lists the complete set with live descriptions.

### Global

| Setting | Notes |
|---|---|
| HsMod Status | Master on/off switch for the whole plugin. A restart is suggested after changing it. |
| HsMod Language | Preferred plugin language. Requires a restart; may invalidate the old config (back up first). |
| Configuration Template | Apply a preset bundle (see above). |
| Shortcut Status | Master on/off switch for all keyboard shortcuts. Off by default. |
| Speed Gear Status + Speed Multiplier | Fast or slow game speed. `1` and `0` are normal speed; negative values slow down. Range configurable to 32x. |
| Game Frame Rate | Target frame rate; `-1` leaves it unchanged (default may be 30). |
| Show FPS | Toggle the on-screen FPS counter (also `Left Ctrl+P`). |
| Mod Settings Button | Show or hide the "Mod Settings" button in the game menu. |

### Optimization

| Setting | Notes |
|---|---|
| In-Game Messages | Show ads, nerf-patch and ladder-settlement messages. Enable this if the shop will not open. |
| Popup Messages + Popup Response | Show pop-ups, or choose how to auto-answer them when hidden. |
| Settlement Display | Show task, achievement and level-up toasts. Hiding them may also hide some reward prompts. |
| Auto Open Boxes | Auto-open reward chests from Arena, Duels and Mercenaries. |
| Auto Exit on Error | Exit the game automatically when an error occurs. |
| Allow Disconnect | Do not disconnect after long inactivity. |
| Show Card Quantity | Show the true count (9+) in the collection. |
| Show Card ID | Show and copy a card's CardID when right-clicking a skin. |
| Deck Share Code Check | Remove the deck share-code check. |
| Show Concede | Allow conceding a run at 0-0. |
| Auto Replace Cards | Skip the card-replacement window and auto-replace. |
| Check deck valid for gamemode | Warn before using a Standard deck in Wild. |

### Pack opening

| Setting | Notes |
|---|---|
| Quick Pack Opening | Reveal pack contents instantly with the space bar. |
| Auto Pack Opening | Open every pack regardless of type. Use with care; may have bugs. |
| Auto Disenchant | Auto-disenchant fully-refundable cards while opening. Re-disabled on every launch and must be re-enabled manually. |

### Friends

| Setting | Notes |
|---|---|
| Auto Report | Auto-report opponents (nickname, cheating, scripting, malicious concede) after a match. Can generate game logs. |
| Spectate Display Cards | Rotate the opponent's hand when spectating a friend. |

### Hearthstone

| Setting | Notes |
|---|---|
| Quick Battle | Skip parts of Battlegrounds and Mercenary AI combat animations. |
| Show Full Name | Show the opponent's full Battle.net name and allow adding them. |
| Show Ladder Rank | Show the opponent's ladder rank before Legend. |
| Card Tracker | Predict the opponent's cards and hint choices. May misidentify. |
| Card Reveal | Show known cards face-up. May trigger an auto-reconnect. |
| Skip Hero Intro | Skip the hero introduction / mulligan intro. |
| Unlimited Emote | Emote without cooldown (1.5s minimum interval). |
| Thinking Emote | Allow thinking emotes. |
| Emote Limit | Cap received opponent emotes: `0` blocks from the start, `-1` is unlimited. |
| Opponent Card Effects | Show the opponent's card effects. Overrides all effect settings. |
| Signature / Golden / Highest Card Effects | Force signature art, golden, or the highest available card visuals. |
| Card Art Export | Save card art to `\Hearthstone\CardTexturesDownload`. Avoid during matches (performance). |

### Mercenary

| Setting | Notes |
|---|---|
| Auto Collect Rewards | Auto-collect Mercenary rewards and block the chest pop-up. |
| Allow Zoom | Allow zooming during a Mercenary battle. May have bugs. |
| Diamond Skin Replacement | Use a diamond skin when possible (lower priority than Highest Card Effects). |
| Random Skin | Use a random non-diamond skin. |

### Battlegrounds

| Setting | Notes |
|---|---|
| Opponent MMR | Show opponents' MMR under their name. See [Battlegrounds](Battlegrounds.md#opponent-mmr-overlay). |
| Session Statistics | Log ranked games and chart them in the menu. See [Battlegrounds](Battlegrounds.md#session-statistics). |
| Auto-Squelch Emotes | Squelch all opponents at match start. Off by default. |
| Silence Bob | Mute Bob in the tavern. |
| Golden Battlegrounds | Golden tavern visuals (requires golden card effects; not minions or questlines). |
| Unlock Season Ticket | Unlock season-pass perks (choose 1 of 4 instead of 1 of 2). |
| Unlock Collection | Local, visual-only unlock of all Battlegrounds cosmetics. |

### Skin

| Setting | Notes |
|---|---|
| Coin, Card Back, Boards, Bob, Pets | Preferred cosmetic IDs. `-1` means no change; some accept `0` to hide. |
| Hero / Opponent Hero / Default Hero | Hero skin replacement. Lower priority than Default Hero. See [HsSkins.cfg](HsSkins.md) for the recommended workflow. |
| Anti-censorship | Load original card art instead of the censored art used for some languages. |

> [!TIP]
> Card Back changes apply instantly. Most other skin changes need an F4 save plus a simulated disconnect to appear in a live match. See [HsSkins.cfg](HsSkins.md).

### Shortcut

All keyboard shortcuts and their defaults are documented on the [Hotkeys](Hotkeys.md) page.

### Development

| Setting | Notes |
|---|---|
| Hearthstone Log / Match Log | Log file paths (relative to Hearthstone). The match log uses comma-separated fields. |
| Auto Quit Timer | Quit automatically after N seconds in a match; `<= 0` disables it. |
| Web Server Port | Showinfo web server port. Default `58744`. |
| Web Page Background Image | Background image for the web pages. |
| Webshell | Enable the web shell at `/shell`. |
| Internal Mode | Switch to internal mode (requires a restart). |
| Buy Adventure / Karazhan Fix | Adventure automation. **Account-ban risk.** |
| Simulate Pack Opening | Enable simulated pack opening (restart suggested; may skew pack stats). |

### Simulation

Controls for simulated pack opening and device collection:

- **Device**: template, OS, screen type and model (the OS / screen / model fields apply only when the template is set to `Custom`).
- **Packs**: count and booster type (changes the pack icon).
- **Randomization**: random result, random rarity, random quality, and random additional effects (diamond or alternate art).
- **Fixed cards**: five card DbID slots with a quality per slot, plus a catch-up card count.

See the in-game **Simulation** section for the full set with live descriptions.
