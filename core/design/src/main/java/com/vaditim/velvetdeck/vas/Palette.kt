package com.vaditim.velvetdeck.vas

import androidx.compose.runtime.staticCompositionLocalOf
import androidx.compose.ui.graphics.Color

// The VAS ground, grey ramp and text steps (dna/01-palette.md), plus the colours this game casts: the app's own accent, the seats' choices and the card kinds.
object Palette {
    val ground = Color(0xFF181818)
    val panel = Color(0xD9121212)
    val panelSolid = Color(0xFF121212)
    val surface = Color(0xFF202020)
    val sunkenDeep = Color(0xFF0E0E0E)
    val sunken = Color(0xFF1C1C1C)
    val border = Color(0xFF262626)
    val borderStrong = Color(0xFF2C2C2C)
    val borderControl = Color(0xFF3A3A3A)
    val scrim = Color(0x99000000)

    val textPrimary = Color(0xDEFFFFFF)
    val textBright = Color(0xFFF0F0F0)
    val textBody = Color(0xFFD8D8D8)
    val textMuted = Color(0xFF9A9A9A)
    val textLabel = Color(0xFF8A8A8A)
    val textIcon = Color(0xFF6A6A6A)
    val textFaint = Color(0xFF4A4A4A)

    val danger = Color(0xFFFF6B6B)

    // Velvet: the rose of VAS's secondary family, the game's own colour wherever no player owns the screen.
    val velvet = Color(0xFFFF2B4D)

    // The colours a player can wear, from VAS's electric register so any two of them read as two people, not one palette.
    val seats = listOf(
        Color(0xFFFF2E88),
        Color(0xFFFF2B4D),
        Color(0xFFF09B3A),
        Color(0xFFFFDD1B),
        Color(0xFF2FDE75),
        Color(0xFF0BC993),
        Color(0xFF5BC4FD),
        Color(0xFF3664FC),
        Color(0xFF7E55DD),
        Color(0xFFFD5BFD),
    )

    // Content-owned colours: a card's kind, the same on every turn whoever holds it. Kept from the Godot version, so the deck still looks like itself.
    val drinkCard = Color(0xFF00A3FF)
    val foreplayCard = Color(0xFFE165CA)
    val funCard = Color(0xFFF8CE57)
    val roleplayCard = Color(0xFFA441FF)
    val loveCard = Color(0xFFFF4448)
    val luckyCard = Color(0xFF12E885)
    // The sex card is the one dark card: a ground tinted toward crimson rather than a colour, with crimson ink.
    val sexCardGround = Color(0xFF17080C)
    val sexCardInk = Color(0xFFDC143C)

    // VAS's black-or-white rule for text standing on a filled colour.
    fun isLight(color: Color): Boolean = color.red * 0.299f + color.green * 0.587f + color.blue * 0.114f > 0.5f

    fun inkOn(color: Color): Color = if (isLight(color)) panelSolid else Color.White
}

// The --section-color of this app: set once at the root from whoever owns the screen, read by every leaf, named by none.
val LocalAccent = staticCompositionLocalOf { Palette.velvet }
