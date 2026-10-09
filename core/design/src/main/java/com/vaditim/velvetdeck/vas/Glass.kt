package com.vaditim.velvetdeck.vas

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.runtime.Composable
import androidx.compose.runtime.staticCompositionLocalOf
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.Shape
import androidx.compose.ui.unit.dp
import dev.chrisbanes.haze.HazeState
import dev.chrisbanes.haze.HazeStyle
import dev.chrisbanes.haze.HazeTint
import dev.chrisbanes.haze.hazeEffect

// The surface the options sheet and the round top buttons stand on: the table behind, blurred and darkened. A floating pane reads as glass; a hairline around it would read as a hole cut in the table.
val LocalHazeState = staticCompositionLocalOf<HazeState?> { null }

object Glass {
    val style = HazeStyle(backgroundColor = Palette.ground, tint = HazeTint(Color.Black.copy(alpha = 0.55f)), blurRadius = 24.dp, noiseFactor = 0.03f)
}

@Composable
fun Modifier.glass(shape: Shape): Modifier {
    val state = LocalHazeState.current
    val clipped = this.clip(shape)
    // Without a blur source behind it, it is a plain VAS panel: solid fill and hairline.
    return if (state == null) clipped.background(Palette.panelSolid).border(1.dp, Palette.border, shape) else clipped.hazeEffect(state, Glass.style)
}
