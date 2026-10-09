package com.vaditim.velvetdeck.components

import androidx.compose.ui.graphics.Color
import com.vaditim.velvetdeck.core.ui.R
import com.vaditim.velvetdeck.game.CardKind
import com.vaditim.velvetdeck.vas.Palette

// How a kind of card looks, wherever a card of it is drawn: its colour (the face's fill, the back's edge), the ink standing on that fill, and its glyph.
object KindLook {
    fun color(kind: CardKind): Color = when (kind) {
        CardKind.DRINK -> Palette.drinkCard
        CardKind.FOREPLAY -> Palette.foreplayCard
        CardKind.FUN -> Palette.funCard
        CardKind.SEX -> Palette.sexCardInk
        CardKind.ROLEPLAY -> Palette.roleplayCard
        CardKind.LOVE -> Palette.loveCard
        CardKind.LUCKY -> Palette.luckyCard
    }

    // The face is filled with the kind's colour, except the sex card, which is the one dark card in the deck.
    fun face(kind: CardKind): Color = if (kind == CardKind.SEX) Palette.sexCardGround else color(kind)

    fun ink(kind: CardKind): Color = if (kind == CardKind.SEX) Palette.sexCardInk else Palette.inkOn(color(kind))

    fun glyph(kind: CardKind): Int = when (kind) {
        CardKind.DRINK -> R.drawable.glyph_drink
        CardKind.FOREPLAY -> R.drawable.glyph_foreplay
        CardKind.FUN -> R.drawable.glyph_fun
        CardKind.SEX -> R.drawable.glyph_sex
        CardKind.ROLEPLAY -> R.drawable.glyph_roleplay
        CardKind.LOVE -> R.drawable.glyph_love
        CardKind.LUCKY -> R.drawable.glyph_lucky
    }
}
