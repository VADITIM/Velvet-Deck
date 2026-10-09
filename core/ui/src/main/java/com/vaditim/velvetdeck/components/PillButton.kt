package com.vaditim.velvetdeck.components

import androidx.compose.animation.animateColorAsState
import androidx.compose.animation.core.tween
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.defaultMinSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.text.BasicText
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import com.vaditim.velvetdeck.vas.LocalAccent
import com.vaditim.velvetdeck.vas.Motion
import com.vaditim.velvetdeck.vas.Palette
import com.vaditim.velvetdeck.vas.Shapes
import com.vaditim.velvetdeck.vas.Type
import com.vaditim.velvetdeck.vas.pressable

enum class PillLook { FILLED, OUTLINED, DANGER }

// Every action on the table: one or two uppercase words in the heading face, on a capsule sized for a thumb (dna/06, components/03 §5). Filled is the one thing to do next; outlined is the alternative.
@Composable
fun PillButton(text: String, onClick: () -> Unit, modifier: Modifier = Modifier, look: PillLook = PillLook.FILLED) {
    val accent = LocalAccent.current
    // A colour cannot overshoot, so a look changing cross-fades on the plain clock.
    val fill by animateColorAsState(
        when (look) {
            PillLook.FILLED -> accent
            PillLook.OUTLINED -> Color.Transparent
            PillLook.DANGER -> Palette.danger
        },
        tween(Motion.STATE_MS),
        label = "pill fill",
    )
    val ink by animateColorAsState(
        when (look) {
            PillLook.FILLED -> Palette.inkOn(accent)
            PillLook.OUTLINED -> Palette.textBody
            PillLook.DANGER -> Palette.inkOn(Palette.danger)
        },
        tween(Motion.STATE_MS),
        label = "pill ink",
    )
    val edge by animateColorAsState(if (look == PillLook.OUTLINED) Palette.borderControl else Color.Transparent, tween(Motion.STATE_MS), label = "pill edge")
    Box(
        modifier
            .pressable(onClick)
            .defaultMinSize(minHeight = 54.dp)
            .clip(Shapes.capsule)
            .background(fill)
            .border(1.dp, edge, Shapes.capsule)
            .padding(horizontal = 28.dp),
        contentAlignment = Alignment.Center,
    ) {
        BasicText(text.uppercase(), style = Type.action.copy(color = ink))
    }
}
