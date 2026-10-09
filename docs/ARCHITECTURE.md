# Architecture

The gallery's shape, smaller. Dependencies only point downward, and features never depend on each other;
the app joins them.

| Module | Package | Owns |
|---|---|---|
| `:app` | `velvetdeck` | `MainActivity`, `VelvetDeckApplication`, and `VelvetDeckApp`: the stage on screen, the game being played, the options sheet |
| `:feature:setup` | `setup` | `SetupScreen`, `SeatPanel`: names and colours |
| `:feature:table` | `table` | `TableScreen`, `CardStage`, `Dealer` (the hand that moves the top card), `Cards` (face, back, lucky), `TableControls` (timer, joker), `EmptyDeckScreen` |
| `:feature:options` | `options` | `OptionsSheet` |
| `:core:ui` | `components` | Pieces used by more than one screen: `PillButton`, `RoundButton` and `Glyphs`, `Toggle`, `OverlaySheet`, `TypewriterText`, `Pop` and `assemble`, `KindLook`, the card glyphs and the logo |
| `:core:design` | `vas` | VAS in Compose: `Palette`, `Faces` and `Type`, `Motion`, `SquircleShape` and `Shapes`, `Panel`, `Modifier.pressable`, `Modifier.glass`, `LabelReveal`, `Haptics` |
| `:core:data` | `game` | The rules: `CardCatalogue`, `Deck`, `Player` and `Lineup`, `Table` |
| `:core:settings` | `settings` | `Settings`: roleplay, vibration, the last players |
| `build-logic` | — | The convention plugins `vados.android.library` and `vados.android.feature` |

## State

- `Table` is one game, as plain snapshot state with the methods that change it: `turnOver`, `begin`,
  `runClock`, `skipClock` (testing only), `pass`, `dismissLucky`. It decides what is allowed (`canPass`,
  `canJoker`) and keeps the lucky cards each player holds (`heldBy`).
- `Dealer` is the table screen's hand: the drag, the flip, the joker's rise and the card's way out. It tells `Table` to pass
  only once the card has left, so names and colours change on the cut.
- `Lineup` is the two players as the setup screen edits them; `Settings` remembers them.
