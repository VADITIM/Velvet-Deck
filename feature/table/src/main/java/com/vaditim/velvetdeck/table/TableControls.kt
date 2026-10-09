package com.vaditim.velvetdeck.table

import androidx.compose.animation.AnimatedContent
import androidx.compose.animation.SizeTransform
import androidx.compose.animation.core.LinearEasing
import androidx.compose.animation.core.animateFloatAsState
import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.scaleIn
import androidx.compose.animation.togetherWith
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.ui.graphics.Color
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.text.BasicText
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.drawBehind
import androidx.compose.ui.geometry.Size
import androidx.compose.ui.unit.dp
import com.vaditim.velvetdeck.components.Pop
import com.vaditim.velvetdeck.components.PillButton
import com.vaditim.velvetdeck.game.Table
import com.vaditim.velvetdeck.game.TimerPhase
import com.vaditim.velvetdeck.vas.LocalAccent
import com.vaditim.velvetdeck.vas.MicroLabel
import com.vaditim.velvetdeck.vas.Motion
import com.vaditim.velvetdeck.vas.Palette
import com.vaditim.velvetdeck.vas.Shapes
import com.vaditim.velvetdeck.vas.Type
import com.vaditim.velvetdeck.vas.pressable

// The timer under a timed card. Each step becomes the next in place: BEGIN pops away and the countdown pops in where it stood, never sliding (components/24 §3).
@Composable
fun TimerControl(table: Table, modifier: Modifier = Modifier) {
    AnimatedContent(
        targetState = table.timerPhase,
        modifier = modifier,
        transitionSpec = {
            (fadeIn(tween(Motion.STATE_MS, Motion.STATE_MS)) + scaleIn(tween(Motion.STATE_MS, Motion.STATE_MS, Motion.backOut), initialScale = 0f))
                .togetherWith(Pop.exit)
                .using(SizeTransform(clip = false))
        },
        label = "timer",
    ) { phase ->
        when (phase) {
            TimerPhase.WAITING -> PillButton("Begin", { table.begin() })
            TimerPhase.COUNTDOWN -> Readout("Ready", table.secondsLeft.toString(), share = 0f)
            TimerPhase.RUNNING -> {
                val total = (table.card?.seconds ?: 1).coerceAtLeast(1)
                // The fill moves smoothly between the once-a-second readings rather than in steps.
                val elapsed by animateFloatAsState(1f - table.secondsLeft / total.toFloat(), tween(1000, easing = LinearEasing), label = "timer fill")
                Readout("Time", clock(table.secondsLeft), share = elapsed)
            }
            TimerPhase.NONE, TimerPhase.DONE -> Box(Modifier.size(0.dp))
        }
    }
}

// A reading in a capsule: the accent fill grows inside its own edge, so the capsule is a gauge standing in the table rather than turning into the colour (dna/03 § grids of controls).
@Composable
private fun Readout(label: String, value: String, share: Float) {
    val accent = LocalAccent.current
    Row(
        Modifier
            .height(54.dp)
            .clip(Shapes.capsule)
            .background(Palette.panelSolid)
            .drawBehind { drawRect(accent.copy(alpha = 0.28f), size = Size(size.width * share.coerceIn(0f, 1f), size.height)) }
            .border(1.dp, accent, Shapes.capsule)
            .padding(horizontal = 24.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(14.dp),
    ) {
        MicroLabel(label, style = Type.microLabel.copy(color = accent))
        BasicText(value, style = Type.readout)
    }
}

// JOKER and the jokers this player has left, as pips in their colour.
@Composable
fun JokerPill(left: Int, total: Int, onClick: () -> Unit, modifier: Modifier = Modifier) {
    val accent = LocalAccent.current
    Row(
        modifier
            .pressable(onClick)
            .height(54.dp)
            .clip(Shapes.capsule)
            .border(1.dp, Palette.borderControl, Shapes.capsule)
            .padding(horizontal = 22.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(12.dp),
    ) {
        BasicText("JOKER", style = Type.action.copy(color = Palette.textBody))
        Pips(left, total, accent)
    }
}

@Composable
fun Pips(left: Int, total: Int, color: Color, modifier: Modifier = Modifier) {
    Row(modifier, horizontalArrangement = Arrangement.spacedBy(5.dp), verticalAlignment = Alignment.CenterVertically) {
        repeat(total) { index ->
            Box(Modifier.size(7.dp).clip(Shapes.capsule).background(if (index < left) color else Palette.borderControl))
        }
    }
}

// For testing only: finishes the clock at once on every timed card. Turn off before a release that is not for testing.
const val IS_SKIP_SHOWN = true

// Small and outlined, so it reads as the tester's control rather than part of the game.
@Composable
fun SkipChip(onClick: () -> Unit, modifier: Modifier = Modifier) {
    Box(
        modifier
            .pressable(onClick)
            .height(36.dp)
            .clip(Shapes.capsule)
            .border(1.dp, Palette.borderControl, Shapes.capsule)
            .padding(horizontal = 14.dp),
        contentAlignment = Alignment.Center,
    ) {
        MicroLabel("Skip")
    }
}
