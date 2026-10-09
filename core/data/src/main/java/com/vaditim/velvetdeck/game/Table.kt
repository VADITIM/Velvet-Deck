package com.vaditim.velvetdeck.game

import androidx.compose.runtime.Stable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateMapOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import kotlinx.coroutines.delay
import kotlin.random.Random

enum class Face { FRONT, BACK }

// A timed card waits for BEGIN, counts the couple in, then runs; the card can only be passed on once its time is up.
enum class TimerPhase { NONE, WAITING, COUNTDOWN, RUNNING, DONE }

// One game: the deck, whose turn it is, the card on the table and what it is doing. Plain snapshot state; the table screen only reads it and calls these methods.
@Stable
class Table(val lineup: Lineup, isRoleplayOn: Boolean, random: Random = Random.Default) {
    private val deck = Deck(isRoleplayOn, random)

    var seat by mutableStateOf(if (random.nextBoolean()) Seat.FIRST else Seat.SECOND)
        private set
    var card by mutableStateOf(deck.draw())
        private set
    // The card waiting underneath, so it can grow into place while the top one is being swiped away.
    var next by mutableStateOf(deck.peek())
        private set
    var face by mutableStateOf(Face.FRONT)
        private set
    var remaining by mutableIntStateOf(deck.remaining)
        private set
    var lucky by mutableStateOf<Card?>(null)
        private set
    var timerPhase by mutableStateOf(TimerPhase.NONE)
        private set
    // Seconds left in the countdown or on the timer, whichever is running.
    var secondsLeft by mutableIntStateOf(0)
        private set
    private val jokers = mutableStateMapOf(Seat.FIRST to JOKERS_PER_PLAYER, Seat.SECOND to JOKERS_PER_PLAYER)

    val isEmpty: Boolean get() = card == null
    val player: Player get() = lineup.of(seat)

    fun jokersOf(seat: Seat): Int = jokers.getValue(seat)

    // Passing the card on is held back while its time is owed: from the moment it is turned until the timer has run out.
    val canPass: Boolean get() = face == Face.BACK && (timerPhase == TimerPhase.NONE || timerPhase == TimerPhase.DONE)

    // A joker throws the card away instead of doing it, but not once the countdown has started.
    val canJoker: Boolean get() = face == Face.BACK && jokersOf(seat) > 0 && (timerPhase == TimerPhase.NONE || timerPhase == TimerPhase.WAITING)

    fun turnOver() {
        val shown = card ?: return
        if (face == Face.BACK) return
        face = Face.BACK
        timerPhase = if (shown.seconds > 0) TimerPhase.WAITING else TimerPhase.NONE
    }

    fun begin() {
        if (timerPhase != TimerPhase.WAITING) return
        secondsLeft = COUNTDOWN_SECONDS
        timerPhase = TimerPhase.COUNTDOWN
    }

    // Runs whichever clock the phase has started, one second at a time; it is relaunched on every phase change, so cancelling it with the phase is enough to stop it.
    suspend fun runClock(onSecond: () -> Unit, onTimeUp: () -> Unit) {
        when (timerPhase) {
            TimerPhase.COUNTDOWN -> {
                while (secondsLeft > 0) {
                    delay(1000)
                    secondsLeft--
                    if (secondsLeft > 0) onSecond()
                }
                secondsLeft = card?.seconds ?: 0
                timerPhase = TimerPhase.RUNNING
            }
            TimerPhase.RUNNING -> {
                while (secondsLeft > 0) {
                    delay(1000)
                    secondsLeft--
                }
                timerPhase = TimerPhase.DONE
                onTimeUp()
            }
            else -> Unit
        }
    }

    // Called once the card has left the table, so nothing changes colour or name while it is still on its way out.
    fun pass(isJoker: Boolean) {
        if (isJoker) jokers[seat] = jokersOf(seat) - 1
        seat = seat.other
        card = deck.draw()
        next = deck.peek()
        remaining = deck.remaining
        face = Face.FRONT
        timerPhase = TimerPhase.NONE
        secondsLeft = 0
        if (card != null && deck.shouldShowLucky()) lucky = deck.drawLucky()
    }

    fun dismissLucky() {
        lucky = null
    }

    companion object {
        const val JOKERS_PER_PLAYER = 3
        const val COUNTDOWN_SECONDS = 5
    }
}
