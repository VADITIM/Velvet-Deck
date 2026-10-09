package com.vaditim.velvetdeck.options

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.ColumnScope
import androidx.compose.foundation.layout.padding
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import com.vaditim.velvetdeck.components.ToggleRow
import com.vaditim.velvetdeck.settings.Settings
import com.vaditim.velvetdeck.vas.MicroLabel

// The two choices the game has. Roleplay takes effect from the next shuffle; a deck already on the table is not reshuffled under the players.
@Composable
fun ColumnScope.OptionsSheet() {
    MicroLabel("Options", Modifier.padding(top = 14.dp, bottom = 14.dp, start = 4.dp))
    Column(Modifier.padding(bottom = 8.dp), verticalArrangement = Arrangement.spacedBy(8.dp)) {
        ToggleRow("Roleplay cards", Settings.isRoleplayOn, Settings::setRoleplay)
        ToggleRow("Vibration", Settings.isVibrationOn, Settings::setVibration)
    }
}
