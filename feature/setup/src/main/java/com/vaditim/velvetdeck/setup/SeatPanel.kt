package com.vaditim.velvetdeck.setup

import androidx.compose.animation.animateColorAsState
import androidx.compose.animation.core.animateFloatAsState
import androidx.compose.animation.core.tween
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.text.BasicText
import androidx.compose.foundation.text.BasicTextField
import androidx.compose.foundation.text.KeyboardActions
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.focus.onFocusChanged
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.SolidColor
import androidx.compose.ui.graphics.graphicsLayer
import androidx.compose.ui.graphics.lerp
import androidx.compose.ui.graphics.toArgb
import androidx.compose.ui.platform.LocalFocusManager
import androidx.compose.ui.text.input.ImeAction
import androidx.compose.ui.text.input.KeyboardCapitalization
import androidx.compose.ui.unit.dp
import com.vaditim.velvetdeck.game.Player
import com.vaditim.velvetdeck.vas.Motion
import com.vaditim.velvetdeck.vas.Palette
import com.vaditim.velvetdeck.vas.Panel
import com.vaditim.velvetdeck.vas.Shapes
import com.vaditim.velvetdeck.vas.Type
import com.vaditim.velvetdeck.vas.pressable

// One player: the panel wears their colour on its label and edge, so each panel reads as a person before a name is in it.
@Composable
fun SeatPanel(label: String, player: Player, modifier: Modifier = Modifier) {
    val color = Color(player.color)
    val labelColor by animateColorAsState(color, tween(Motion.STATE_MS), label = "seat label")
    val edge by animateColorAsState(lerp(Palette.border, color, 0.35f), tween(Motion.STATE_MS), label = "seat edge")
    Panel(modifier.fillMaxWidth(), label = label, labelColor = labelColor, borderColor = edge) {
        Column(Modifier.padding(start = 18.dp, end = 18.dp, top = 8.dp, bottom = 18.dp)) {
            NameField(player, labelColor)
            Spacer(Modifier.height(18.dp))
            SwatchRow(player)
        }
    }
}

@Composable
private fun NameField(player: Player, color: Color) {
    val focusManager = LocalFocusManager.current
    var isFocused by remember { mutableStateOf(false) }
    val line by animateColorAsState(if (isFocused) color else Palette.borderStrong, tween(Motion.STATE_MS), label = "name line")
    BasicTextField(
        value = player.name,
        onValueChange = { name -> player.name = name.take(NAME_LIMIT) },
        modifier = Modifier.fillMaxWidth().onFocusChanged { state -> isFocused = state.isFocused },
        textStyle = Type.name,
        singleLine = true,
        cursorBrush = SolidColor(color),
        keyboardOptions = KeyboardOptions(capitalization = KeyboardCapitalization.Words, imeAction = ImeAction.Done),
        keyboardActions = KeyboardActions(onDone = { focusManager.clearFocus() }),
        decorationBox = { field ->
            Column {
                Box(Modifier.padding(vertical = 6.dp)) {
                    if (player.name.isEmpty()) BasicText("NAME", style = Type.name.copy(color = Palette.textFaint))
                    field()
                }
                Box(Modifier.fillMaxWidth().height(1.dp).background(line))
            }
        },
    )
}

// The colours on offer, as dots: the chosen one stands at full size with a ring, the rest wait smaller.
@Composable
private fun SwatchRow(player: Player) {
    Row(Modifier.fillMaxWidth(), horizontalArrangement = Arrangement.SpaceBetween, verticalAlignment = Alignment.CenterVertically) {
        Palette.seats.forEach { swatch ->
            val argb = swatch.toArgb()
            val isChosen = player.color == argb
            val scale by animateFloatAsState(if (isChosen) 1f else 0.72f, tween(Motion.STATE_MS, easing = Motion.backOut), label = "swatch")
            val ring by animateColorAsState(if (isChosen) Palette.textBright else Color.Transparent, tween(Motion.STATE_MS), label = "swatch ring")
            Box(
                Modifier
                    .pressable({ player.color = argb }, pressedScale = 0.9f)
                    .size(30.dp)
                    .graphicsLayer { scaleX = scale; scaleY = scale }
                    .border(2.dp, ring, Shapes.capsule)
                    .padding(4.dp)
                    .clip(Shapes.capsule)
                    .background(swatch),
            )
        }
    }
}

private const val NAME_LIMIT = 16
