package com.vaditim.velvetdeck.vas

import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.Font
import androidx.compose.ui.text.font.FontFamily
import androidx.compose.ui.unit.sp
import com.vaditim.velvetdeck.core.design.R

// Declared once here and nowhere else (dna/02-typography.md): Wosker for display, Audiowide for headings and buttons, Mono for everything functional.
object Faces {
    val mono = FontFamily(Font(R.font.mono))
    val heading = FontFamily(Font(R.font.audiowide))
    val display = FontFamily(Font(R.font.wosker))
}

object Type {
    // The micro-label: 0.63rem mono, uppercase, 0.19rem tracking — tracking kept as the same share of the size.
    val microLabel = TextStyle(fontFamily = Faces.mono, fontSize = 10.sp, letterSpacing = 3.sp, color = Palette.textLabel)
    // The one display line on a screen: the kind on a card's face, DECK EMPTY at the end.
    val display = TextStyle(fontFamily = Faces.display, fontSize = 56.sp, lineHeight = 56.sp, letterSpacing = 1.sp, color = Palette.textBright)
    val title = TextStyle(fontFamily = Faces.heading, fontSize = 26.sp, lineHeight = 30.sp, letterSpacing = 0.5.sp, color = Palette.textBright)
    val name = TextStyle(fontFamily = Faces.heading, fontSize = 24.sp, letterSpacing = 0.5.sp, color = Palette.textBright)
    val body = TextStyle(fontFamily = Faces.mono, fontSize = 16.sp, lineHeight = 25.sp, color = Palette.textBody)
    val value = TextStyle(fontFamily = Faces.mono, fontSize = 13.sp, color = Palette.textMuted)
    // A reading the eye comes back to every second (the timer), in tabular figures so the digits never shuffle.
    val readout = TextStyle(fontFamily = Faces.mono, fontSize = 22.sp, letterSpacing = 2.sp, fontFeatureSettings = "tnum", color = Palette.textBright)
    val action = TextStyle(fontFamily = Faces.heading, fontSize = 13.sp, letterSpacing = 1.5.sp)
}
