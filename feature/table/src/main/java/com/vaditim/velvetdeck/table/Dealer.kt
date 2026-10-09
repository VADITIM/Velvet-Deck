package com.vaditim.velvetdeck.table

import androidx.compose.animation.core.Animatable
import androidx.compose.animation.core.animate
import androidx.compose.animation.core.tween
import androidx.compose.runtime.Composable
import androidx.compose.runtime.Stable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableFloatStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import com.vaditim.velvetdeck.game.Face
import com.vaditim.velvetdeck.game.Table
import com.vaditim.velvetdeck.vas.Motion
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Job
import kotlinx.coroutines.launch
import kotlin.math.abs
import kotlin.math.sign

// The hand that moves the top card: turning it over, and sending it off the table with the finger (dna/05-motion.md §14). The drag is plain state written by the gesture; release only decides, and the card finishes from where the finger left it. The table is told only once the card is gone.
@Stable
class Dealer(private val table: Table, private val scope: CoroutineScope, private val onPassed: (isJoker: Boolean) -> Unit) {
    var drag by mutableFloatStateOf(0f)
        private set
    val flip = Animatable(0f)
    var width by mutableFloatStateOf(1f)
    var isLeaving by mutableStateOf(false)
        private set
    private var settling: Job? = null

    // The card's controls wait until it has landed face up, so they do not pop in under a card still turning.
    val isLanded: Boolean get() = flip.value >= 1f

    // How far the top card is on its way out; the waiting card grows into its place by the same amount.
    val departure: Float get() = (abs(drag) / (width * LEAVE_WIDTHS)).coerceIn(0f, 1f)

    fun turnOver() {
        if (table.face != Face.FRONT || isLeaving) return
        table.turnOver()
        scope.launch { flip.animateTo(1f, tween(Motion.FLIP_MS, easing = Motion.powerThreeInOut)) }
    }

    fun startDrag() {
        settling?.cancel()
    }

    // A card whose time is still owed can be tugged, but it pulls back hard and always comes home.
    fun dragBy(delta: Float) {
        if (isLeaving) return
        drag += if (table.canPass) delta else delta * RESISTANCE
    }

    fun release(velocity: Float) {
        if (isLeaving) return
        val isFlung = abs(velocity) > FLING_VELOCITY && (drag == 0f || sign(velocity) == sign(drag))
        if (table.canPass && (abs(drag) > width * DECIDE_SHARE || isFlung)) {
            throwAway(if (drag != 0f) sign(drag) else sign(velocity), isJoker = false, velocity = velocity)
        } else {
            settling = scope.launch {
                animate(drag, 0f, velocity, tween(Motion.CARD_RETURN_MS, easing = Motion.backOut)) { value, _ -> drag = value }
            }
        }
    }

    fun pass() {
        if (table.canPass && !isLeaving) throwAway(1f, isJoker = false, velocity = 0f)
    }

    // A joker throws the card the other way, so it never looks like the card was done.
    fun joker() {
        if (table.canJoker && !isLeaving) throwAway(-1f, isJoker = true, velocity = 0f)
    }

    private fun throwAway(direction: Float, isJoker: Boolean, velocity: Float) {
        settling?.cancel()
        isLeaving = true
        scope.launch {
            animate(drag, direction * width * LEAVE_WIDTHS, velocity, tween(Motion.CARD_LEAVE_MS, easing = Motion.powerTwoIn)) { value, _ -> drag = value }
            // All in one frame: the waiting card has grown to full size, so it simply becomes the top card face up.
            flip.snapTo(0f)
            table.pass(isJoker)
            drag = 0f
            isLeaving = false
            onPassed(isJoker)
        }
    }

    private companion object {
        const val DECIDE_SHARE = 0.28f
        const val LEAVE_WIDTHS = 1.4f
        const val RESISTANCE = 0.22f
        const val FLING_VELOCITY = 1600f
    }
}

@Composable
fun rememberDealer(table: Table, onPassed: (isJoker: Boolean) -> Unit): Dealer {
    val scope = rememberCoroutineScope()
    return remember(table) { Dealer(table, scope, onPassed) }
}
