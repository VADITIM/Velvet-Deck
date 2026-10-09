package com.vaditim.velvetdeck.components

import androidx.compose.foundation.Canvas
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.size
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.StrokeCap
import androidx.compose.ui.graphics.drawscope.DrawScope
import androidx.compose.ui.unit.dp
import com.vaditim.velvetdeck.vas.Palette
import com.vaditim.velvetdeck.vas.Shapes
import com.vaditim.velvetdeck.vas.glass
import com.vaditim.velvetdeck.vas.pressable

// The round glass button of a top row: one glyph, drawn rather than loaded, so it takes any ink.
@Composable
fun RoundButton(glyph: DrawScope.(Color) -> Unit, onClick: () -> Unit, modifier: Modifier = Modifier, ink: Color = Palette.textBody) {
    Box(modifier.pressable(onClick, pressedScale = 0.9f).size(48.dp).glass(Shapes.capsule), contentAlignment = Alignment.Center) {
        Canvas(Modifier.size(20.dp)) { glyph(ink) }
    }
}

object Glyphs {
    // Three sliders at different stops: the options.
    val options: DrawScope.(Color) -> Unit = { ink ->
        val stroke = 1.8.dp.toPx()
        val knob = 2.6.dp.toPx()
        listOf(0.2f to 0.68f, 0.5f to 0.3f, 0.8f to 0.58f).forEach { (row, stop) ->
            val y = size.height * row
            drawLine(ink, Offset(0f, y), Offset(size.width, y), stroke, StrokeCap.Round)
            drawCircle(Palette.panelSolid, knob + stroke, Offset(size.width * stop, y))
            drawCircle(ink, knob, Offset(size.width * stop, y))
        }
    }

    val close: DrawScope.(Color) -> Unit = { ink ->
        val stroke = 2.dp.toPx()
        val inset = size.width * 0.15f
        drawLine(ink, Offset(inset, inset), Offset(size.width - inset, size.height - inset), stroke, StrokeCap.Round)
        drawLine(ink, Offset(size.width - inset, inset), Offset(inset, size.height - inset), stroke, StrokeCap.Round)
    }
}
