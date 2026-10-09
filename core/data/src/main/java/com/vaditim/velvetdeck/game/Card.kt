package com.vaditim.velvetdeck.game

enum class CardKind(val label: String) {
    DRINK("Drink"),
    FOREPLAY("Foreplay"),
    FUN("Fun"),
    SEX("Sex"),
    ROLEPLAY("Roleplay"),
    LOVE("Love"),
    LUCKY("Lucky"),
}

data class Card(val kind: CardKind, val title: String, val description: String, val shots: Int, val seconds: Int)
