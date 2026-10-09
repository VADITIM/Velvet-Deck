package com.vaditim.velvetdeck.components

import androidx.compose.animation.core.LinearEasing
import androidx.compose.animation.core.RepeatMode
import androidx.compose.animation.core.animateFloat
import androidx.compose.animation.core.infiniteRepeatable
import androidx.compose.animation.core.rememberInfiniteTransition
import androidx.compose.animation.core.tween
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.text.BasicText
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.graphicsLayer
import androidx.compose.ui.platform.LocalDensity
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.unit.dp
import com.vaditim.velvetdeck.vas.Motion
import kotlinx.coroutines.delay

// Text that changes types itself over (components/09-typewriter.md): back to the prefix the two share at the quicker pace, then the rest at the slower, with a block caret blinking while it runs. Text already standing is never re-typed.
@Composable
fun TypewriterText(text: String, style: TextStyle, modifier: Modifier = Modifier) {
    var shown by remember { mutableStateOf(text) }
    var isTyping by remember { mutableStateOf(false) }
    LaunchedEffect(text) {
        if (shown == text) return@LaunchedEffect
        isTyping = true
        val shared = shown.commonPrefixWith(text)
        while (shown.length > shared.length) {
            shown = shown.dropLast(1)
            delay(Motion.UNTYPE_MS)
        }
        while (shown.length < text.length) {
            shown = text.take(shown.length + 1)
            delay(Motion.TYPE_MS)
        }
        isTyping = false
    }
    val blink by rememberInfiniteTransition(label = "caret").animateFloat(
        initialValue = 1f,
        targetValue = 0f,
        animationSpec = infiniteRepeatable(tween(Motion.CARET_BLINK_MS.toInt(), easing = LinearEasing), RepeatMode.Reverse),
        label = "caret blink",
    )
    val caretHeight = with(LocalDensity.current) { style.fontSize.toDp() }
    Row(modifier, verticalAlignment = Alignment.CenterVertically) {
        BasicText(shown, style = style, maxLines = 1)
        Box(
            Modifier
                .padding(start = 3.dp)
                .size(caretHeight * 0.5f, caretHeight)
                .graphicsLayer { alpha = if (isTyping) (if (blink > 0.5f) 1f else 0f) else 0f }
                .background(style.color),
        )
    }
}
