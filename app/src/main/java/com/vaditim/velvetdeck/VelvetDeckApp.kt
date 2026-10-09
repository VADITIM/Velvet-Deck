package com.vaditim.velvetdeck

import androidx.activity.compose.BackHandler
import androidx.compose.animation.AnimatedContent
import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.animation.slideInVertically
import androidx.compose.animation.togetherWith
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.runtime.Composable
import androidx.compose.runtime.CompositionLocalProvider
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.toArgb
import com.vaditim.velvetdeck.components.OverlaySheet
import com.vaditim.velvetdeck.game.Lineup
import com.vaditim.velvetdeck.game.Player
import com.vaditim.velvetdeck.game.Table
import com.vaditim.velvetdeck.options.OptionsSheet
import com.vaditim.velvetdeck.settings.Settings
import com.vaditim.velvetdeck.setup.SetupScreen
import com.vaditim.velvetdeck.table.EmptyDeckScreen
import com.vaditim.velvetdeck.table.TableScreen
import com.vaditim.velvetdeck.vas.LocalAccent
import com.vaditim.velvetdeck.vas.LocalHazeState
import com.vaditim.velvetdeck.vas.Motion
import com.vaditim.velvetdeck.vas.Palette
import dev.chrisbanes.haze.HazeState
import dev.chrisbanes.haze.hazeSource

private enum class Stage { SETUP, TABLE, EMPTY }

// The shell: which stage is on screen, the game being played, and the options sheet over all of it.
@Composable
fun VelvetDeckApp() {
    val lineup = remember {
        Lineup(
            Player(Settings.firstName, Settings.firstColor.takeIf { it != 0 } ?: Palette.seats[0].toArgb()),
            Player(Settings.secondName, Settings.secondColor.takeIf { it != 0 } ?: Palette.seats[6].toArgb()),
        )
    }
    var stage by remember { mutableStateOf(Stage.SETUP) }
    var table by remember { mutableStateOf<Table?>(null) }
    var isOptionsOpen by remember { mutableStateOf(false) }
    val hazeState = remember { HazeState() }

    fun deal() {
        lineup.first.name = lineup.first.name.trim()
        lineup.second.name = lineup.second.name.trim()
        Settings.rememberPlayers(lineup.first.name, lineup.first.color, lineup.second.name, lineup.second.color)
        table = Table(lineup, Settings.isRoleplayOn)
        stage = Stage.TABLE
    }

    // Each stage wears its own accent, read from the stage it is rather than the one being cut to, so a stage leaving keeps its colour all the way out.
    fun accentOf(of: Stage): Color = table?.takeIf { of == Stage.TABLE }?.let { Color(it.player.color) } ?: Palette.velvet

    BackHandler(isOptionsOpen) { isOptionsOpen = false }
    BackHandler(stage == Stage.EMPTY && !isOptionsOpen) { stage = Stage.SETUP }
    // Back on the table opens the options, where the game is ended.
    BackHandler(stage == Stage.TABLE && !isOptionsOpen) { isOptionsOpen = true }

    CompositionLocalProvider(LocalAccent provides accentOf(stage), LocalHazeState provides hazeState) {
        Box(Modifier.fillMaxSize().background(Palette.ground)) {
            AnimatedContent(
                targetState = stage,
                modifier = Modifier.fillMaxSize().hazeSource(hazeState),
                transitionSpec = {
                    (fadeIn(tween(Motion.STAGE_ENTER_MS, Motion.STAGE_ENTER_DELAY_MS, Motion.powerTwoOut)) +
                        slideInVertically(tween(Motion.STAGE_ENTER_MS, Motion.STAGE_ENTER_DELAY_MS, Motion.backOut)) { height -> height / 40 })
                        .togetherWith(fadeOut(tween(Motion.STAGE_LEAVE_MS, easing = Motion.powerTwoIn)))
                },
                label = "stage",
            ) { shown ->
                // Nothing inside a stage blurs the stage itself; the round buttons there stand as plain panels.
                CompositionLocalProvider(LocalAccent provides accentOf(shown), LocalHazeState provides null) {
                    when (shown) {
                        Stage.SETUP -> SetupScreen(lineup, onStart = ::deal, onOptions = { isOptionsOpen = true })
                        Stage.TABLE -> table?.let { current ->
                            TableScreen(current, onOptions = { isOptionsOpen = true }, onDeckEmpty = { stage = Stage.EMPTY })
                        }
                        Stage.EMPTY -> EmptyDeckScreen(onShuffle = ::deal, onNewPlayers = { stage = Stage.SETUP })
                    }
                }
            }
            OverlaySheet(isOptionsOpen, onClose = { isOptionsOpen = false }) {
                OptionsSheet(
                    onEnd = if (stage == Stage.TABLE) {
                        {
                            isOptionsOpen = false
                            stage = Stage.SETUP
                        }
                    } else {
                        null
                    },
                )
            }
        }
    }
}
