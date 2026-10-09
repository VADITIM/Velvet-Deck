package com.vaditim.velvetdeck.table

import androidx.compose.animation.AnimatedContent
import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.EnterTransition
import androidx.compose.animation.core.Animatable
import androidx.compose.animation.core.animateFloatAsState
import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.animation.slideOutVertically
import androidx.compose.animation.togetherWith
import androidx.compose.foundation.Canvas
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.interaction.MutableInteractionSource
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.BoxWithConstraints
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.WindowInsets
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.navigationBars
import androidx.compose.foundation.layout.navigationBarsPadding
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.statusBarsPadding
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.derivedStateOf
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.Path
import androidx.compose.ui.graphics.drawscope.DrawScope
import androidx.compose.ui.graphics.graphicsLayer
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.platform.LocalDensity
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.min
import com.vaditim.velvetdeck.components.Glyphs
import com.vaditim.velvetdeck.components.Popped
import com.vaditim.velvetdeck.components.RoundButton
import com.vaditim.velvetdeck.components.TypewriterText
import com.vaditim.velvetdeck.components.assemble
import com.vaditim.velvetdeck.game.Card
import com.vaditim.velvetdeck.game.Seat
import com.vaditim.velvetdeck.game.Table
import com.vaditim.velvetdeck.game.TimerPhase
import com.vaditim.velvetdeck.vas.Haptics
import com.vaditim.velvetdeck.vas.LocalAccent
import com.vaditim.velvetdeck.vas.MicroLabel
import com.vaditim.velvetdeck.vas.Motion
import com.vaditim.velvetdeck.vas.Palette
import com.vaditim.velvetdeck.vas.Type

// The game: whose turn it is, the card, and what can be done with it. The accent is the player whose turn it is, and it changes on the cut, once the last card has left (dna/01 § the accent changes on the cut). Ending the game lives in the options.
@Composable
fun TableScreen(table: Table, onOptions: () -> Unit, onDeckEmpty: () -> Unit) {
    val context = LocalContext.current
    val dealer = rememberDealer(table) { Haptics.confirm(context) }
    val isLanded by remember(dealer) { derivedStateOf { dealer.isLanded } }

    LaunchedEffect(table.timerPhase) {
        table.runClock(onSecond = { Haptics.tick(context) }, onTimeUp = { Haptics.alarm(context) })
    }
    LaunchedEffect(table.isEmpty) { if (table.isEmpty) onDeckEmpty() }

    Box(Modifier.fillMaxSize()) {
        PlayerSlices(table)
        Column(Modifier.fillMaxSize().statusBarsPadding().navigationBarsPadding()) {
            CardCount(table.remaining, Modifier.align(Alignment.CenterHorizontally).padding(top = 6.dp).assemble(0))
            Row(Modifier.fillMaxWidth().padding(start = 24.dp, end = 16.dp), verticalAlignment = Alignment.CenterVertically) {
                TypewriterText(table.player.name, Type.title.copy(color = LocalAccent.current), Modifier.weight(1f).assemble(1))
                RoundButton(Glyphs.options, onOptions, Modifier.assemble(0))
            }
            CardStage(table, dealer, Modifier.weight(1f).fillMaxWidth().assemble(2))
            // The controls' room is always there, so the card never moves when they pop in.
            Row(
                Modifier.fillMaxWidth().height(70.dp),
                horizontalArrangement = Arrangement.spacedBy(12.dp, Alignment.CenterHorizontally),
                verticalAlignment = Alignment.CenterVertically,
            ) {
                Popped(table.canJoker && isLanded && !dealer.isLeaving) {
                    JokerPill(table.jokersOf(table.seat), Table.JOKERS_PER_PLAYER, dealer::joker)
                }
                if (isLanded) TimerControl(table)
                Popped(IS_SKIP_SHOWN && isLanded && table.timerPhase in SKIPPABLE) { SkipChip(table::skipClock) }
            }
            // Where a player's lucky card rests, its header showing.
            Spacer(Modifier.height(DOCK_PEEK))
        }
        LuckyLayer(table)
    }
}

// How many cards are left, small and centred under the camera.
@Composable
private fun CardCount(remaining: Int, modifier: Modifier = Modifier) {
    Row(modifier, horizontalArrangement = Arrangement.spacedBy(6.dp), verticalAlignment = Alignment.CenterVertically) {
        TypewriterText(remaining.toString(), Type.value.copy(color = Palette.textBody, fontFeatureSettings = "tnum"))
        MicroLabel("Left")
    }
}

// Each player keeps a slanted band of their colour along their own edge, the first on the left and the second on the right. The one whose turn it is shows in full; the other stays as a trace.
@Composable
private fun PlayerSlices(table: Table) {
    val firstPresence by animateFloatAsState(if (table.seat == Seat.FIRST) 1f else 0f, tween(Motion.STATE_MS, easing = Motion.powerTwoOut), label = "slices")
    val first = Color(table.lineup.first.color)
    val second = Color(table.lineup.second.color)
    Canvas(Modifier.fillMaxSize()) {
        slice(first, firstPresence, isLeft = true)
        slice(second, 1f - firstPresence, isLeft = false)
    }
}

private fun DrawScope.slice(color: Color, presence: Float, isLeft: Boolean) {
    val strength = SLICE_TRACE + (1f - SLICE_TRACE) * presence
    val top = size.width * SLICE_TOP
    val bottom = size.width * SLICE_BOTTOM
    fun x(inset: Float) = if (isLeft) inset else size.width - inset
    val path = Path().apply {
        moveTo(x(0f), 0f)
        lineTo(x(top), 0f)
        lineTo(x(bottom), size.height)
        lineTo(x(0f), size.height)
        close()
    }
    // Strongest at the screen's edge, fading toward the slanted side.
    drawPath(path, Brush.horizontalGradient(listOf(color.copy(alpha = 0.16f * strength), color.copy(alpha = 0.03f * strength)), startX = x(0f), endX = x(top)))
    drawLine(color.copy(alpha = 0.5f * strength), Offset(x(top), 0f), Offset(x(bottom), size.height), strokeWidth = 1.dp.toPx())
}

// A lucky card falls onto the table from above while the table darkens behind it; tapped away, it slides down and stays at the bottom of the screen in front of its player, its header showing, while that player has the turn.
@Composable
private fun LuckyLayer(table: Table) {
    val context = LocalContext.current
    LaunchedEffect(table.lucky) { if (table.lucky != null) Haptics.confirm(context) }
    // The overlay only fades; the card moves on its own.
    AnimatedVisibility(
        table.lucky != null,
        enter = fadeIn(tween(Motion.OVERLAY_ENTER_MS)),
        exit = fadeOut(tween(Motion.OVERLAY_LEAVE_MS, easing = Motion.powerTwoIn)),
    ) {
        Box(Modifier.fillMaxSize().background(Palette.scrim).clickable(remember { MutableInteractionSource() }, indication = null) { table.dismissLucky() })
    }
    // On the cut the last player's card goes down with them, and the next player's rises into its place.
    AnimatedContent(
        targetState = table.seat,
        transitionSpec = { EnterTransition.None togetherWith slideOutVertically(tween(Motion.CARD_LEAVE_MS, easing = Motion.powerTwoIn)) { height -> height } },
        label = "lucky hand",
    ) { seat -> LuckyHand(table, seat) }
}

@Composable
private fun LuckyHand(table: Table, seat: Seat) {
    val falling = table.lucky.takeIf { table.seat == seat }
    val held = table.heldBy(seat).lastOrNull()
    // It keeps the last card it showed, so it never empties while it moves.
    var shown by remember { mutableStateOf<Card?>(null) }
    (falling ?: held)?.let { card -> shown = card }
    BoxWithConstraints(Modifier.fillMaxSize()) {
        val density = LocalDensity.current
        val cardWidth = min(maxWidth * 0.8f, 340.dp)
        val cardHeight = cardWidth / CARD_RATIO
        val screen = constraints.maxHeight.toFloat()
        val cardPx = with(density) { cardHeight.toPx() }
        val centre = (screen - cardPx) / 2f
        val docked = screen - with(density) { DOCK_PEEK.toPx() } - WindowInsets.navigationBars.getBottom(density)
        val above = -cardPx * 1.1f
        val y = remember { Animatable(if (falling != null) above else screen) }
        LaunchedEffect(falling, held) {
            when {
                falling != null -> {
                    y.snapTo(above)
                    y.animateTo(centre, tween(Motion.LUCKY_ENTER_MS, easing = Motion.backOut))
                }
                held != null -> y.animateTo(docked, tween(Motion.LUCKY_DOCK_MS, easing = Motion.powerThreeInOut))
                else -> y.snapTo(screen)
            }
        }
        shown?.let { card ->
            LuckyCard(
                card,
                Modifier.align(Alignment.TopCenter).size(cardWidth, cardHeight).graphicsLayer {
                    translationY = y.value
                    // Tilted while it is being read, laid straight once it rests.
                    rotationZ = LUCKY_TILT * ((docked - y.value) / (docked - centre)).coerceIn(0f, 1f)
                },
            )
        }
    }
}

private val SKIPPABLE = setOf(TimerPhase.WAITING, TimerPhase.COUNTDOWN, TimerPhase.RUNNING)
private val DOCK_PEEK = 56.dp
private const val LUCKY_TILT = -3f
private const val SLICE_TOP = 0.16f
private const val SLICE_BOTTOM = 0.05f
private const val SLICE_TRACE = 0.3f
