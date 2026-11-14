package com.vaditim.velvetdeck.settings

// Every word of the interface in both languages. Reading one inside a composable follows Settings.language, so a change of language redraws at once.
object Strings {
    private fun pick(english: String, german: String): String = if (Settings.language == Language.GERMAN) german else english

    val start get() = pick("Start", "Start")
    val begin get() = pick("Begin", "Beginnen")
    val ready get() = pick("Ready", "Bereit")
    val time get() = pick("Time", "Zeit")
    val skip get() = pick("Skip", "Weiter")
    val joker get() = "JOKER"
    val left get() = pick("Left", "Übrig")
    val card get() = pick("Card", "Karte")
    val lucky get() = pick("Lucky", "Glück")
    val name get() = "NAME"
    val options get() = pick("Options", "Optionen")
    val roleplayCards get() = pick("Roleplay cards", "Rollenspielkarten")
    val vibration get() = "Vibration"
    val language get() = pick("Language", "Sprache")
    val endGame get() = pick("End game", "Spiel beenden")
    val tapToEnd get() = pick("Tap to end", "Zum Beenden tippen")
    val allCardsPlayed get() = pick("All cards played", "Alle Karten gespielt")
    val deckEmpty get() = pick("DECK EMPTY", "DECK LEER")
    val shuffleAgain get() = pick("Shuffle again", "Neu mischen")
    val newPlayers get() = pick("New players", "Neue Spieler")
    val playerOne get() = pick("Player 1", "Spieler 1")
    val playerTwo get() = pick("Player 2", "Spieler 2")
}
