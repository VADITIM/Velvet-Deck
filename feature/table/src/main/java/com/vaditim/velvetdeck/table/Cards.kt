package com.vaditim.velvetdeck.table

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.text.BasicText
import androidx.compose.foundation.text.TextAutoSize
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.ColorFilter
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.vaditim.velvetdeck.components.KindLook
import com.vaditim.velvetdeck.core.ui.R
import com.vaditim.velvetdeck.game.Card
import com.vaditim.velvetdeck.game.CardKind
import com.vaditim.velvetdeck.vas.MicroLabel
import com.vaditim.velvetdeck.vas.Palette
import com.vaditim.velvetdeck.vas.Shapes
import com.vaditim.velvetdeck.vas.Type

// The face a card is dealt with: only its kind, as a filled colour, a glyph and the kind's name in the display face. The one display line on the table.
@Composable
fun CardFace(kind: CardKind, modifier: Modifier = Modifier) {
    val ink = KindLook.ink(kind)
    Box(
        modifier
            .fillMaxSize()
            .clip(Shapes.card)
            .background(KindLook.face(kind))
            .border(1.5.dp, if (kind == CardKind.SEX) Palette.sexCardInk else Color.Transparent, Shapes.card),
    ) {
        MicroLabel("Velvet Deck", Modifier.align(Alignment.TopStart).padding(start = 20.dp, top = 18.dp), Type.microLabel.copy(color = ink.copy(alpha = 0.7f)))
        Column(Modifier.align(Alignment.Center), horizontalAlignment = Alignment.CenterHorizontally) {
            Image(
                painterResource(KindLook.glyph(kind)),
                contentDescription = null,
                modifier = Modifier.size(120.dp),
                colorFilter = ColorFilter.tint(ink),
            )
            Spacer(Modifier.height(26.dp))
            // The name shrinks to fit rather than being cut, so FOREPLAY stands as whole as FUN.
            BasicText(
                kind.label.uppercase(),
                Modifier.padding(horizontal = 20.dp),
                style = Type.display.copy(color = ink),
                maxLines = 1,
                autoSize = TextAutoSize.StepBased(minFontSize = 28.sp, maxFontSize = Type.display.fontSize),
            )
        }
    }
}

// The side that is read: the card's name, what to do, and what it is worth. Dark, so the words carry it; the kind's colour stays on the edge and the label.
@Composable
fun CardBack(card: Card, modifier: Modifier = Modifier) {
    val color = KindLook.color(card.kind)
    Box(
        modifier
            .fillMaxSize()
            .clip(Shapes.card)
            .background(if (card.kind == CardKind.SEX) Palette.sexCardGround else Palette.panelSolid)
            .border(1.5.dp, color, Shapes.card)
            .padding(horizontal = 22.dp, vertical = 20.dp),
    ) {
        Column(Modifier.fillMaxSize()) {
            Row(Modifier.fillMaxWidth(), verticalAlignment = Alignment.CenterVertically) {
                MicroLabel(card.kind.label, Modifier.weight(1f), Type.microLabel.copy(color = color))
                Shots(card.shots, color)
            }
            Spacer(Modifier.weight(0.7f))
            BasicText(card.title, style = Type.title)
            Spacer(Modifier.height(14.dp))
            BasicText(card.description, style = Type.body.copy(fontSize = 15.sp, lineHeight = 23.sp))
            Spacer(Modifier.weight(1f))
            if (card.seconds > 0) {
                Row(verticalAlignment = Alignment.CenterVertically) {
                    MicroLabel("Time", Modifier.weight(1f))
                    BasicText(clock(card.seconds), style = Type.value.copy(color = Palette.textBody))
                }
            }
        }
    }
}

// The lucky card: a rule for the rest of the game rather than a task, so it is filled like a face but carries its words.
@Composable
fun LuckyCard(card: Card, modifier: Modifier = Modifier) {
    val ink = KindLook.ink(CardKind.LUCKY)
    Column(
        modifier
            .fillMaxSize()
            .clip(Shapes.card)
            .background(KindLook.face(CardKind.LUCKY))
            .padding(horizontal = 22.dp, vertical = 20.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
    ) {
        Row(Modifier.fillMaxWidth(), verticalAlignment = Alignment.CenterVertically) {
            MicroLabel("Lucky", Modifier.weight(1f), Type.microLabel.copy(color = ink.copy(alpha = 0.7f)))
            Shots(card.shots, ink)
        }
        Spacer(Modifier.weight(0.6f))
        Image(painterResource(R.drawable.glyph_lucky), contentDescription = null, modifier = Modifier.size(84.dp), colorFilter = ColorFilter.tint(ink))
        Spacer(Modifier.height(24.dp))
        BasicText(card.title, style = Type.title.copy(color = ink, textAlign = TextAlign.Center))
        Spacer(Modifier.height(14.dp))
        BasicText(card.description, style = Type.body.copy(color = ink, fontSize = 15.sp, lineHeight = 23.sp, textAlign = TextAlign.Center))
        Spacer(Modifier.weight(1f))
    }
}

// What a card is worth in shots, out of three: full glasses in the kind's colour, the rest as faint outlines. A card worth nothing names its own drink, so it shows none.
@Composable
private fun Shots(count: Int, color: Color) {
    if (count <= 0) return
    Row(horizontalArrangement = Arrangement.spacedBy(3.dp)) {
        repeat(MAX_SHOTS) { index ->
            val isFull = index < count
            Image(
                painterResource(if (isFull) R.drawable.glyph_shot_full else R.drawable.glyph_shot_empty),
                contentDescription = null,
                modifier = Modifier.size(18.dp),
                colorFilter = ColorFilter.tint(if (isFull) color else color.copy(alpha = 0.3f)),
            )
        }
    }
}

fun clock(seconds: Int): String = "%02d:%02d".format(seconds / 60, seconds % 60)

private const val MAX_SHOTS = 3
