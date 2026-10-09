package com.vaditim.velvetdeck.table

import androidx.activity.compose.BackHandler
import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.animation.slideInVertically
import androidx.compose.animation.slideOutVertically
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.interaction.MutableInteractionSource
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.aspectRatio
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.navigationBarsPadding
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.statusBarsPadding
import androidx.compose.foundation.layout.width
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.derivedStateOf
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.rotate
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp
import com.vaditim.velvetdeck.components.Glyphs
import com.vaditim.velvetdeck.components.PillButton
import com.vaditim.velvetdeck.components.PillLook
import com.vaditim.velvetdeck.components.Popped
import com.vaditim.velvetdeck.components.RoundButton
import com.vaditim.velvetdeck.components.TypewriterText
import com.vaditim.velvetdeck.components.assemble
import com.vaditim.velvetdeck.game.Card
import com.vaditim.velvetdeck.game.Table
import com.vaditim.velvetdeck.vas.Haptics
import com.vaditim.velvetdeck.vas.LocalAccent
import com.vaditim.velvetdeck.vas.MicroLabel
import com.vaditim.velvetdeck.vas.Motion
import com.vaditim.velvetdeck.vas.Palette
import com.vaditim.velvetdeck.vas.Type
import kotlinx.coroutines.delay

// The game: whose turn it is, the card, and what can be done with it. The accent is the player whose turn it is, and it changes on the cut, once the last card has left (dna/01 § the accent changes on the cut).
@Composable
fun TableScreen(table: Table, isBackFree: Boolean, onEnd: () -> Unit, onOptions: () -> Unit, onDeckEmpty: () -> Unit) {
    val context = LocalContext.current
    val dealer = rememberDealer(table) { Haptics.confirm(context) }
    var isEndArmed by remember { mutableStateOf(false) }
    val isLanded by remember(dealer) { derivedStateOf { dealer.isLanded } }

    LaunchedEffect(table.timerPhase) {
        table.runClock(onSecond = { Haptics.tick(context) }, onTimeUp = { Haptics.alarm(context) })
    }
    LaunchedEffect(table.isEmpty) { if (table.isEmpty) onDeckEmpty() }
    // END waits for a second tap; left alone it disarms itself.
    LaunchedEffect(isEndArmed) {
        if (isEndArmed) {
            delay(Motion.CONFIRM_ARMED_MS)
            isEndArmed = false
        }
    }
    BackHandler(isBackFree) { if (isEndArmed) onEnd() else isEndArmed = true }

    Box(Modifier.fillMaxSize()) {
        Column(Modifier.fillMaxSize().statusBarsPadding().navigationBarsPadding()) {
            Row(Modifier.fillMaxWidth().padding(16.dp), verticalAlignment = Alignment.CenterVertically) {
                RoundButton(Glyphs.close, { isEndArmed = !isEndArmed }, Modifier.assemble(0))
                Spacer(Modifier.width(10.dp))
                Popped(isEndArmed) { PillButton("End", onEnd, look = PillLook.DANGER) }
                Spacer(Modifier.weight(1f))
                RoundButton(Glyphs.options, onOptions, Modifier.assemble(0))
            }
            TurnHeader(table, Modifier.assemble(1).padding(horizontal = 24.dp))
            CardStage(table, dealer, Modifier.weight(1f).fillMaxWidth().assemble(2))
            // The controls' room is always there, so the card never moves when they pop in.
            Row(
                Modifier.fillMaxWidth().height(86.dp).padding(bottom = 16.dp),
                horizontalArrangement = Arrangement.spacedBy(12.dp, Alignment.CenterHorizontally),
                verticalAlignment = Alignment.CenterVertically,
            ) {
                Popped(table.canJoker && isLanded && !dealer.isLeaving) {
                    JokerPill(table.jokersOf(table.seat), Table.JOKERS_PER_PLAYER, dealer::joker)
                }
                if (isLanded) TimerControl(table)
            }
        }
        LuckyOverlay(table)
    }
}

@Composable
private fun TurnHeader(table: Table, modifier: Modifier = Modifier) {
    val accent = LocalAccent.current
    Row(modifier.fillMaxWidth(), verticalAlignment = Alignment.Bottom) {
        Column(Modifier.weight(1f)) {
            MicroLabel("Turn")
            TypewriterText(table.player.name, Type.title.copy(color = accent))
        }
        Column(horizontalAlignment = Alignment.End) {
            MicroLabel("Left")
            TypewriterText(table.remaining.toString(), Type.title.copy(color = Palette.textMuted))
        }
    }
}

// A lucky card falls onto the table from above, over everything, and is tapped away. It keeps the last card it showed while it leaves, so it never empties on its way out.
@Composable
private fun LuckyOverlay(table: Table) {
    val context = LocalContext.current
    var shown by remember { mutableStateOf<Card?>(null) }
    table.lucky?.let { lucky -> shown = lucky }
    LaunchedEffect(table.lucky) { if (table.lucky != null) Haptics.confirm(context) }
    AnimatedVisibility(
        table.lucky != null,
        enter = fadeIn(tween(Motion.OVERLAY_ENTER_MS)) + slideInVertically(tween(Motion.LUCKY_ENTER_MS, easing = Motion.backOut)) { height -> -height },
        exit = fadeOut(tween(Motion.LUCKY_LEAVE_MS, easing = Motion.powerTwoIn)) + slideOutVertically(tween(Motion.LUCKY_LEAVE_MS, easing = Motion.powerTwoIn)) { height -> -height / 3 },
    ) {
        Box(
            Modifier
                .fillMaxSize()
                .background(Palette.scrim)
                .clickable(remember { MutableInteractionSource() }, indication = null) { table.dismissLucky() },
            contentAlignment = Alignment.Center,
        ) {
            shown?.let { card -> LuckyCard(card, Modifier.fillMaxWidth(0.8f).aspectRatio(CARD_RATIO).rotate(LUCKY_TILT)) }
        }
    }
}

private const val LUCKY_TILT = -3f
