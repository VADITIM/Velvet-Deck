package com.vaditim.velvetdeck.setup

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.BoxWithConstraints
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.heightIn
import androidx.compose.foundation.layout.imePadding
import androidx.compose.foundation.layout.navigationBarsPadding
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.statusBarsPadding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import com.vaditim.velvetdeck.components.Glyphs
import com.vaditim.velvetdeck.components.PillButton
import com.vaditim.velvetdeck.components.Popped
import com.vaditim.velvetdeck.components.RoundButton
import com.vaditim.velvetdeck.components.assemble
import com.vaditim.velvetdeck.core.ui.R
import com.vaditim.velvetdeck.game.Lineup

// Who is playing: the logo, one panel per player with a name and a colour, and START once both have a name.
@Composable
fun SetupScreen(lineup: Lineup, onStart: () -> Unit, onOptions: () -> Unit) {
    BoxWithConstraints(Modifier.fillMaxSize().statusBarsPadding().navigationBarsPadding().imePadding()) {
        // At least a screen tall and centred in it, so the setup stands in the middle of a tall phone; it scrolls only once the keyboard takes the room.
        Column(
            Modifier.fillMaxWidth().verticalScroll(rememberScrollState()).heightIn(min = maxHeight).padding(horizontal = 20.dp),
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.Center,
        ) {
            Spacer(Modifier.height(72.dp))
            Image(
                painterResource(R.drawable.logo),
                contentDescription = "Velvet Deck",
                modifier = Modifier.assemble(0).fillMaxWidth().padding(horizontal = 18.dp),
                contentScale = ContentScale.FillWidth,
            )
            Spacer(Modifier.height(40.dp))
            Column(verticalArrangement = Arrangement.spacedBy(12.dp)) {
                SeatPanel("Player 1", lineup.first, Modifier.assemble(1))
                SeatPanel("Player 2", lineup.second, Modifier.assemble(2))
            }
            Spacer(Modifier.height(32.dp))
            // The room START stands in is always there, so the panels never move when it pops in.
            Box(Modifier.height(64.dp), contentAlignment = Alignment.Center) {
                Popped(lineup.isReady) { PillButton("Start", onStart) }
            }
            Spacer(Modifier.height(16.dp))
        }
        RoundButton(Glyphs.options, onOptions, Modifier.align(Alignment.TopEnd).padding(16.dp).assemble(0))
    }
}
