package com.vaditim.velvetdeck.components

import androidx.compose.animation.animateColorAsState
import androidx.compose.animation.core.animateDpAsState
import androidx.compose.animation.core.tween
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.offset
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.text.BasicText
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.unit.dp
import com.vaditim.velvetdeck.vas.LocalAccent
import com.vaditim.velvetdeck.vas.Motion
import com.vaditim.velvetdeck.vas.Palette
import com.vaditim.velvetdeck.vas.Shapes
import com.vaditim.velvetdeck.vas.Type
import com.vaditim.velvetdeck.vas.pressable

// One setting, one row: its name and a switch. The whole row is the target, so a thumb never has to find the knob.
@Composable
fun ToggleRow(label: String, isOn: Boolean, onChange: (Boolean) -> Unit, modifier: Modifier = Modifier) {
    Row(
        modifier.fillMaxWidth().pressable({ onChange(!isOn) }, pressedScale = 0.98f).clip(Shapes.field).background(Palette.sunken).padding(horizontal = 18.dp, vertical = 16.dp),
        verticalAlignment = Alignment.CenterVertically,
    ) {
        BasicText(label.uppercase(), Modifier.weight(1f), style = Type.action.copy(color = Palette.textBody))
        Toggle(isOn)
    }
}

// The knob lands on the overshoot; the track takes the accent and the knob goes dark, cross-fading because a colour cannot overshoot.
@Composable
fun Toggle(isOn: Boolean, modifier: Modifier = Modifier) {
    val accent = LocalAccent.current
    val knobOffset by animateDpAsState(if (isOn) 24.dp else 4.dp, tween(Motion.STATE_MS, easing = Motion.backOut), label = "knob")
    val track by animateColorAsState(if (isOn) accent else Palette.sunkenDeep, tween(Motion.STATE_MS), label = "track")
    val edge by animateColorAsState(if (isOn) accent else Palette.borderControl, tween(Motion.STATE_MS), label = "track edge")
    val knob by animateColorAsState(if (isOn) Palette.panelSolid else Palette.textMuted, tween(Motion.STATE_MS), label = "knob ink")
    Box(modifier.size(52.dp, 32.dp).clip(Shapes.capsule).background(track).border(1.dp, edge, Shapes.capsule)) {
        Box(Modifier.align(Alignment.CenterStart).offset(x = knobOffset).size(24.dp).clip(Shapes.capsule).background(knob))
    }
}
