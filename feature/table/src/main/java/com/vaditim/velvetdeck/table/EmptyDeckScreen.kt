package com.vaditim.velvetdeck.table

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.navigationBarsPadding
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.statusBarsPadding
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import com.vaditim.velvetdeck.components.PillButton
import com.vaditim.velvetdeck.components.PillLook
import com.vaditim.velvetdeck.components.assemble
import com.vaditim.velvetdeck.vas.LabelReveal
import com.vaditim.velvetdeck.vas.MicroLabel
import com.vaditim.velvetdeck.vas.Type

// Every card has been played: the title sweeps in, then the two ways on.
@Composable
fun EmptyDeckScreen(onShuffle: () -> Unit, onNewPlayers: () -> Unit) {
    Column(
        Modifier.fillMaxSize().statusBarsPadding().navigationBarsPadding().padding(24.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.Center,
    ) {
        MicroLabel("All cards played", Modifier.assemble(0))
        Spacer(Modifier.height(14.dp))
        LabelReveal("DECK EMPTY", isShown = true, style = Type.display)
        Spacer(Modifier.height(48.dp))
        PillButton("Shuffle again", onShuffle, Modifier.assemble(2))
        Spacer(Modifier.height(12.dp))
        PillButton("New players", onNewPlayers, Modifier.assemble(3), look = PillLook.OUTLINED)
    }
}
