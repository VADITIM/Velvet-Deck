package com.vaditim.velvetdeck.components

import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.core.animate
import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.animation.scaleIn
import androidx.compose.animation.scaleOut
import androidx.compose.animation.slideInVertically
import androidx.compose.animation.slideOutVertically
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.gestures.Orientation
import androidx.compose.foundation.gestures.draggable
import androidx.compose.foundation.gestures.rememberDraggableState
import androidx.compose.foundation.interaction.MutableInteractionSource
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.BoxScope
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.ColumnScope
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.navigationBarsPadding
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableFloatStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.graphicsLayer
import androidx.compose.ui.layout.onSizeChanged
import androidx.compose.ui.unit.dp
import com.vaditim.velvetdeck.vas.Motion
import com.vaditim.velvetdeck.vas.Palette
import com.vaditim.velvetdeck.vas.Shapes
import com.vaditim.velvetdeck.vas.glass
import kotlinx.coroutines.launch

// The glass sheet (components/17-glass-sheet.md): up from below on the overshoot, down faster with no bounce. The pull follows the finger frame by frame and the scrim fades with it; letting go only decides, and the motion finishes from where the finger left it.
@Composable
fun BoxScope.OverlaySheet(isOpen: Boolean, onClose: () -> Unit, content: @Composable ColumnScope.() -> Unit) {
    val scope = rememberCoroutineScope()
    var pull by remember { mutableFloatStateOf(0f) }
    var height by remember { mutableFloatStateOf(1f) }
    val share = { (pull / height).coerceIn(0f, 1f) }
    LaunchedEffect(isOpen) { if (isOpen) pull = 0f }

    AnimatedVisibility(isOpen, Modifier.matchParentSize(), enter = fadeIn(tween(Motion.OVERLAY_ENTER_MS)), exit = fadeOut(tween(Motion.OVERLAY_LEAVE_MS, easing = Motion.powerTwoIn))) {
        Box(
            Modifier
                .fillMaxSize()
                .graphicsLayer { alpha = 1f - share() }
                .background(Palette.scrim)
                .clickable(remember { MutableInteractionSource() }, indication = null, onClick = onClose),
        )
    }
    AnimatedVisibility(
        isOpen,
        Modifier.align(Alignment.BottomCenter),
        enter = slideInVertically(tween(Motion.OVERLAY_ENTER_MS, easing = Motion.backOut)) { it / 6 } +
            scaleIn(tween(Motion.OVERLAY_ENTER_MS, easing = Motion.backOut), initialScale = 0.96f) +
            fadeIn(tween(Motion.OVERLAY_ENTER_MS, easing = Motion.powerTwoOut)),
        exit = slideOutVertically(tween(Motion.OVERLAY_LEAVE_MS, easing = Motion.powerTwoIn)) { it / 8 } +
            scaleOut(tween(Motion.OVERLAY_LEAVE_MS, easing = Motion.powerTwoIn), targetScale = 0.98f) +
            fadeOut(tween(Motion.OVERLAY_LEAVE_MS, easing = Motion.powerTwoIn)),
    ) {
        Column(
            Modifier
                .fillMaxWidth()
                .padding(10.dp)
                .navigationBarsPadding()
                .onSizeChanged { size -> height = size.height.toFloat().coerceAtLeast(1f) }
                .graphicsLayer { translationY = pull }
                .glass(Shapes.sheet)
                .draggable(
                    rememberDraggableState { delta -> pull = (pull + delta).coerceAtLeast(0f) },
                    Orientation.Vertical,
                    onDragStopped = { velocity ->
                        val isClosing = pull > height * CLOSE_SHARE || velocity > CLOSE_VELOCITY
                        scope.launch {
                            if (isClosing) {
                                animate(pull, height, velocity, tween(Motion.OVERLAY_LEAVE_MS, easing = Motion.powerTwoOut)) { value, _ -> pull = value }
                                // The pull is left where it ended, so the sheet does not jump back up while it fades; it is cleared when the sheet opens again.
                                onClose()
                            } else {
                                animate(pull, 0f, velocity, tween(Motion.OVERLAY_ENTER_MS, easing = Motion.backOut)) { value, _ -> pull = value }
                            }
                        }
                    },
                )
                .padding(horizontal = 18.dp, vertical = 14.dp),
        ) {
            Box(Modifier.align(Alignment.CenterHorizontally).size(36.dp, 4.dp).clip(Shapes.capsule).background(Palette.borderControl))
            content()
        }
    }
}

private const val CLOSE_SHARE = 0.25f
private const val CLOSE_VELOCITY = 1200f
