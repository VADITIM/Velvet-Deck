# Velvet Deck

A card game for couples, native Android (Kotlin + Jetpack Compose), on the same stack as VADOS Gallery.
The Godot version it replaces is kept, untouched, on the `godot-port` branch.

This project is built on VAS (VADOS APPLICATION SYSTEMS), **base register, the whole system**. Read
`.claude/skills/vas/SKILL.md`. If it is missing, clone `https://github.com/VADITIM/VADOS-APPLICATION-SYSTEMS`
into `.claude/skills/vas` (it is not committed here).

Where this app departs from VAS, and why, is in `docs/DESIGN.md`; it overrides the base modules where they
disagree. The module layout and where new code goes are in `docs/ARCHITECTURE.md`.

- Leaf composables never name an accent; they read `LocalAccent`. The accent is the player whose turn it is, and it changes on the cut.
- Every pressable uses `Modifier.pressable`, never the ripple.
- Durations and curves come from `core/design/.../vas/Motion.kt`, never inline numbers.
- Every gesture that changes the UI is driven by the finger: the card moves with the swipe, release only decides, and the table is told once the card has gone (VAS `dna/05-motion.md` §14).
- Game rules live in `:core:data` (`Deck`, `Table`); composables only read that state and call its methods.
- The cards are `CardCatalogue.kt`, carried over word for word from the Godot version.
- No explanatory text in the UI. Labels and state (a name, a count, a time) are fine.
- No abbreviations in identifiers (`dna/09-code-style.md`); comments say why, on one line.

## Build and run

```bash
export JAVA_HOME="/c/Program Files/Eclipse Adoptium/jdk-17.0.20.101-hotspot" ANDROID_HOME="$LOCALAPPDATA/Android/Sdk"
./gradlew.bat assembleRelease
adb install -r app/build/outputs/apk/release/app-release.apk
```

Where there is no Android SDK, CI (`.github/workflows/build.yml`) is the compiler: push, then read the run.

## Releasing

Work directly on `master`: commit and push there, no feature branches. Every push to `master` publishes
the APK as the release `v<versionName>`.

Every change raises `versionName` in `app/build.gradle.kts` (and `versionCode`): 2.0.0, 2.0.1, and so on.
Every change also rewrites `RELEASE_NOTES.md`: a short changelog of that version, a few plain bullets of what
changed for the players. It is the release's text.

Commit subjects are `<versionName>: <what changed, plainly>`, with bullets in the body when there is more than one thing.
No co-author or "generated with" lines.
