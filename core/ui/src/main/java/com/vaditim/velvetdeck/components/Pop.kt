package com.vaditim.velvetdeck.components

import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.EnterTransition
import androidx.compose.animation.ExitTransition
import androidx.compose.animation.core.Animatable
import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.animation.scaleIn
import androidx.compose.animation.scaleOut
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.remember
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.graphicsLayer
import androidx.compose.ui.unit.dp
import com.vaditim.velvetdeck.vas.Motion
import kotlinx.coroutines.delay

// Something appearing where it always stands pops: from nothing, past full size, settled; and back to nothing with a small wind-up. In place, never sliding (components/24-gallery-motion.md §3).
object Pop {
    val enter: EnterTransition = fadeIn(tween(Motion.STATE_MS)) + scaleIn(tween(Motion.STATE_MS, easing = Motion.backOut), initialScale = 0f)
    val exit: ExitTransition = fadeOut(tween(Motion.STATE_MS, easing = Motion.powerTwoIn)) + scaleOut(tween(Motion.STATE_MS, easing = Motion.backIn), targetScale = 0f)
}

@Composable
fun Popped(isShown: Boolean, modifier: Modifier = Modifier, content: @Composable () -> Unit) {
    AnimatedVisibility(isShown, modifier, enter = Pop.enter, exit = Pop.exit) { content() }
}

// A stage's pieces arriving top to bottom, each rising a little and landing on the overshoot, after the cut's short gate. Only on arrival; a piece already standing is never re-assembled.
@Composable
fun Modifier.assemble(order: Int): Modifier {
    val progress = remember { Animatable(0f) }
    LaunchedEffect(Unit) {
        delay((Motion.STAGE_ENTER_DELAY_MS + order * Motion.ASSEMBLE_STAGGER_MS).toLong())
        progress.animateTo(1f, tween(Motion.STAGE_ENTER_MS, easing = Motion.backOut))
    }
    return graphicsLayer {
        val value = progress.value
        alpha = value.coerceIn(0f, 1f)
        translationY = (1f - value) * RISE.toPx()
    }
}

private val RISE = 28.dp
