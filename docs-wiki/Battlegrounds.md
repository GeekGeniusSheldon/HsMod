# Battlegrounds

Tools specific to Battlegrounds. All are configured in the settings menu under the **Battlegrounds** section (skins live under **Skin**, hotkeys under **Shortcut**).

- [Opponent MMR overlay](#opponent-mmr-overlay)
- [Session statistics](#session-statistics)
- [Shop hotkeys](#shop-hotkeys)
- [Auto-squelch](#auto-squelch)
- [Cosmetics and other options](#cosmetics-and-other-options)

## Opponent MMR overlay

Shows each opponent's Battlegrounds MMR directly under their name in the leaderboard overlay, so you can read the lobby at a glance.

**How it works**

- When a match starts (during hero selection), the plugin fetches the public Battlegrounds leaderboard for your region in the background.
- Player names are read from the game's own leaderboard tiles, so no hovering over portraits is needed.
- When you hover a tile, the MMR is inserted under the name using the game's own name text, so it matches the native style.

**Details**

- Works in both **solo** and **duos**. Solo and duos use different leaderboards, and the correct one is chosen automatically per game.
- Players outside the tracked leaderboard (the public list only covers roughly the top ratings) are shown as `8000` with a down arrow, meaning "below the tracked range".
- Each leaderboard is cached to `Hearthstone\BepInEx\HsMod\bgrank\`. If the network is briefly unavailable, the last cached list is used.
- The leaderboard is refreshed periodically; ratings can lag real life by a small margin between refreshes.
- Setting: `isBgRankEnable` (on by default).

> [!NOTE]
> Names that are hidden (for example a streamer mode) will not be found in the leaderboard, so no MMR is shown for them rather than a misleading "below range".

> [!TIP]
> **Privacy.** The overlay only makes a read-only request for the public leaderboard by region. Nothing about you or your account is sent.

## Session statistics

Logs every ranked Battlegrounds game and shows the history in the settings menu under the **BG Session Stats** tab.

**What is recorded**

Each finished ranked game stores: time, mode (solo or duos), final place, rating before, rating after, the MMR change, and the hero played.

**How it captures data**

- The rating before the match is read when the game starts; the final place is read from your hero as players are eliminated.
- After the match ends, the plugin waits for the server to send the updated rating, then records the change. Reconnecting mid-match is handled, and if two games finish very quickly in a row the earlier one is still saved.
- If the updated rating never arrives (for example you close the game immediately), the entry is saved as "uncertain" and marked with a `?` instead of a delta.

**The stats tab**

- A bar chart of MMR change per game (green up, red down), with a horizontal scrollbar for long histories and a per-game place label.
- A summary line: number of games, average place, total MMR change and the rating range.
- Filters: **today / 7 days / month / all-time** and **solo / duos**. The mode filter defaults to whichever mode has more games, and your last filter choice is remembered.

**Data file**

History is written to `Hearthstone\BepInEx\HsMod\bg_session.csv` and reloaded on startup, so it persists across sessions. The format is semicolon-separated:

```
time;mode;place;rating_before;rating_after;delta;note;hero
2026-07-15 18:03:20;duo;2;5637;5668;31;;Ragnaros
```

The `note` column is `uncertain` when the rating change could not be confirmed, and `synced` for a reconciliation entry: when the current rating does not match the last recorded one (games played on another device, or a crash before the result was saved), the difference is logged as a single `synced` row so the MMR history stays correct. You can open the file in any spreadsheet tool.

- Setting: `isBgSessionStatsEnable` (on by default).

> [!NOTE]
> Only ranked games are recorded. Games versus AI, the tutorial and friendly lobbies are ignored.

## Shop hotkeys

Keyboard shortcuts for the tavern, active **only during the shopping phase**:

| Default | Action |
|---|---|
| `R` | Refresh tavern |
| `F` | Freeze / unfreeze tavern |
| `U` | Upgrade tavern tier |
| `H` | Hero power (enters targeting mode if the power needs a target) |
| `T` | Show / hide teammate board (duos) |

They emulate a real click on the corresponding button through the game's own input path, so behaviour matches clicking with the mouse. All are rebindable on the [Hotkeys](Hotkeys.md) page and require "Shortcut Status" to be enabled.

> [!IMPORTANT]
> These are single-letter keys. They are ignored while a text field is focused (for example the Battle.net chat), so typing does not trigger the shop.

## Auto-squelch

Automatically squelches all opponents' emotes at the start of a match.

- Your own and your teammate's emotes are left untouched.
- It runs once per match; manually un-squelching a player afterwards is not overridden.
- Setting: `isBgAutoSquelchEnable` (off by default).

## Cosmetics and other options

- **Fast battle** - skips part of the combat animation (also works with Mercenaries PvE). Set under Hearthstone as Quick Battle.
- **Silence Bob** - mutes Bob in the tavern.
- **Golden Battlegrounds** - golden tavern visuals (requires golden card effects; does not apply to minions or questlines).
- **Unlock Season Ticket** - unlocks season-pass perks (choose 1 of 4 heroes instead of 1 of 2).
- **Unlock Collection** - a local, visual-only unlock of all Battlegrounds cosmetics so you can browse and pick any skin. It does not grant real ownership and opponents do not see unowned cosmetics.
- **Skins** - Battlegrounds board, finisher and Bob skins are set in the Skin section. See [HsSkins.cfg](HsSkins.md).
