package com.vaditim.velvetdeck.options

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.ColumnScope
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import com.vaditim.velvetdeck.components.PillButton
import com.vaditim.velvetdeck.components.PillLook
import com.vaditim.velvetdeck.components.ToggleRow
import com.vaditim.velvetdeck.settings.Settings
import com.vaditim.velvetdeck.vas.MicroLabel
import com.vaditim.velvetdeck.vas.Motion
import kotlinx.coroutines.delay

// The two choices the game has, and ending the game while one is being played. Roleplay takes effect from the next shuffle; a deck already on the table is not reshuffled under the players.
@Composable
fun ColumnScope.OptionsSheet(onEnd: (() -> Unit)?) {
    MicroLabel("Options", Modifier.padding(top = 14.dp, bottom = 14.dp, start = 4.dp))
    Column(Modifier.padding(bottom = 8.dp), verticalArrangement = Arrangement.spacedBy(8.dp)) {
        ToggleRow("Roleplay cards", Settings.isRoleplayOn, Settings::setRoleplay)
        ToggleRow("Vibration", Settings.isVibrationOn, Settings::setVibration)
    }
    onEnd?.let { end -> EndGame(end, Modifier.padding(top = 8.dp, bottom = 8.dp)) }
}

// Ending waits for a second tap rather than asking in a dialog; left alone it disarms itself.
@Composable
private fun EndGame(onEnd: () -> Unit, modifier: Modifier = Modifier) {
    var isArmed by remember { mutableStateOf(false) }
    LaunchedEffect(isArmed) {
        if (isArmed) {
            delay(Motion.CONFIRM_ARMED_MS)
            isArmed = false
        }
    }
    PillButton(
        if (isArmed) "Tap to end" else "End game",
        { if (isArmed) onEnd() else isArmed = true },
        modifier.fillMaxWidth(),
        look = if (isArmed) PillLook.DANGER else PillLook.OUTLINED,
    )
}
