package com.vaditim.velvetdeck.options

import androidx.compose.animation.animateColorAsState
import androidx.compose.animation.core.tween
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.text.BasicText
import androidx.compose.ui.Alignment
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import com.vaditim.velvetdeck.vas.LocalAccent
import com.vaditim.velvetdeck.vas.Palette
import com.vaditim.velvetdeck.vas.Shapes
import com.vaditim.velvetdeck.vas.Type
import com.vaditim.velvetdeck.vas.pressable
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
import com.vaditim.velvetdeck.settings.Language
import com.vaditim.velvetdeck.settings.Settings
import com.vaditim.velvetdeck.settings.Strings
import com.vaditim.velvetdeck.vas.MicroLabel
import com.vaditim.velvetdeck.vas.Motion
import kotlinx.coroutines.delay

// The two choices the game has, and ending the game while one is being played. Roleplay takes effect from the next shuffle; a deck already on the table is not reshuffled under the players.
@Composable
fun ColumnScope.OptionsSheet(onEnd: (() -> Unit)?) {
    MicroLabel(Strings.options, Modifier.padding(top = 14.dp, bottom = 14.dp, start = 4.dp))
    Column(Modifier.padding(bottom = 8.dp), verticalArrangement = Arrangement.spacedBy(8.dp)) {
        ToggleRow(Strings.roleplayCards, Settings.isRoleplayOn, Settings::setRoleplay)
        ToggleRow(Strings.vibration, Settings.isVibrationOn, Settings::setVibration)
        LanguageRow()
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
        if (isArmed) Strings.tapToEnd else Strings.endGame,
        { if (isArmed) onEnd() else isArmed = true },
        modifier.fillMaxWidth(),
        look = if (isArmed) PillLook.DANGER else PillLook.OUTLINED,
    )
}

// The language is chosen from two capsules in the row, the chosen one filled with the accent.
@Composable
private fun LanguageRow(modifier: Modifier = Modifier) {
    Row(
        modifier.fillMaxWidth().clip(Shapes.field).background(Palette.sunken).padding(start = 18.dp, end = 10.dp, top = 8.dp, bottom = 8.dp),
        verticalAlignment = Alignment.CenterVertically,
    ) {
        BasicText(Strings.language.uppercase(), Modifier.weight(1f), style = Type.action.copy(color = Palette.textBody))
        Row(horizontalArrangement = Arrangement.spacedBy(6.dp)) {
            Language.entries.forEach { language -> LanguageChoice(language, language == Settings.language) }
        }
    }
}

@Composable
private fun LanguageChoice(language: Language, isChosen: Boolean) {
    val accent = LocalAccent.current
    val fill by animateColorAsState(if (isChosen) accent else Color.Transparent, tween(Motion.STATE_MS), label = "language fill")
    val ink by animateColorAsState(if (isChosen) Palette.inkOn(accent) else Palette.textMuted, tween(Motion.STATE_MS), label = "language ink")
    Box(
        Modifier.pressable({ Settings.setLanguage(language) }).clip(Shapes.capsule).background(fill).padding(horizontal = 16.dp, vertical = 10.dp),
        contentAlignment = Alignment.Center,
    ) {
        BasicText(language.shortName, style = Type.action.copy(color = ink))
    }
}
