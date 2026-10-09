# VAS at the table — Velvet Deck's design adaptation

Velvet Deck is built on **VAS, base register, the whole system**. It takes the phone adaptation VADOS Gallery
made (`VADOS-GALLERY/docs/DESIGN.md`, VAS `platforms/android-compose.md`) and changes only what a card game
needs. Everything not mentioned here is VAS as written.

## Colour

| Who owns the screen | Accent |
|---|---|
| Setup, the empty deck, the options over them | Velvet `#ff2b4d`, VAS's rose |
| The table | The colour of the player whose turn it is |

The accent changes **on the cut**: a card is swiped away in the old player's colour, and only once it has
left does the table, the turn name and every pill take the next player's (`dna/01` § The accent changes on
the cut). The players choose from ten colours out of VAS's electric register, so any two read as two people.

**Card kinds are content colours**, the same on every turn whoever holds the card, kept from the Godot
version so the deck still looks like itself: drink `#00a3ff`, foreplay `#e165ca`, fun `#f8ce57`, roleplay
`#a441ff`, love `#ff4448`, lucky `#12e885`. The sex card is the one dark card: a ground tinted toward crimson
(`#17080c`) with crimson ink (`#dc143c`), VAS's "tint, never swap".

## Surface

- **Panels keep the VAS hairline** (`Panel`: translucent fill, 1px border, squircle, micro-label). Unlike the
  gallery there are no photographs to frame, so the border is the design again.
- **A card's face is filled with its kind's colour; its back is dark with the colour on the edge.** The face
  only says what kind of card it is, so it can shout; the back is read, so the words carry it.
- **Glass only for what floats over the table**: the options sheet blurs whatever is behind it.
- **Squircles on every box**, capsules on pills and dots. No shadows anywhere.

## Type

| Role | Face | Used for |
|---|---|---|
| Display | Wosker | The kind on a card's face, DECK EMPTY. One per screen |
| Techno heading | Audiowide | Player names, card titles, every button |
| Functional | Space Mono | Labels, card text, counts, the timer |

The logo stays the Godot version's image; it is the game's mark, not a line of type.

## Motion

The gallery's tool tempo for controls (nothing a finger waits on over 0.3s), with the cards as the one
exception: turning a card over is the moment the game is played for, so it gets 380ms on `power3.inOut`,
lifting 6% off the table at the middle of the turn.

| Element | Arrives | Leaves |
|---|---|---|
| A stage (setup, table, empty) | Rises 1/40 after a 60ms gate, 260ms `back.out`; its pieces assemble top to bottom 50ms apart | Fades, 120ms |
| The top card | Is the waiting card, grown from 0.92 and half opacity by the swipe itself | Leans 14° per width and leaves past 1.4 widths, 220ms `power2.in`; short of 0.28 of the width it comes home on `back.out` |
| A card's controls (JOKER, BEGIN, the timer) | Pop in once the card has landed face up | Pop away; each timer step becomes the next in place |
| The lucky card | Falls from above the screen, 420ms `back.out` | Lifts away, 160ms |
| The options sheet | Up from 1/6 below, 240ms `back.out`, or with the finger | Down 1/8, 140ms, or with the finger |
| The turn name and the count | Typed over: back to the shared prefix, then the rest | — |
| DECK EMPTY | The bar-sweep | — |

A card whose time is still owed can be tugged but not passed: it pulls back hard (22% of the finger) and
always comes home, so the table answers without a message.

## Haptics

A tick on every press and every countdown second; two clicks when a card is passed or a lucky card falls;
three long pulses when a timer runs out. All silent while VIBRATION is off.

## Voice

Actions are one or two uppercase words: START, BEGIN, JOKER, END, SHUFFLE AGAIN, NEW PLAYERS. END waits
for a second tap rather than asking in a dialog. No explanatory text, no emoji.
