package com.vaditim.velvetdeck.game

import kotlin.math.max
import kotlin.random.Random

// The deck as the Godot version dealt it: everything shuffled together, a sex card every 10 to 13 cards but only once both players have had a foreplay card since the last one, and lucky cards held apart to fall in now and then.
class Deck(isRoleplayOn: Boolean, private val random: Random = Random.Default) {
    private val cards = ArrayDeque<Card>()
    private val luckyCards: List<Card>

    init {
        val pool = CardCatalogue.all.filter { card -> isRoleplayOn || card.kind != CardKind.ROLEPLAY }
        luckyCards = pool.filter { card -> card.kind == CardKind.LUCKY }
        val sexCards = pool.filter { card -> card.kind == CardKind.SEX }.shuffled(random)
        val rest = pool.filter { card -> card.kind != CardKind.LUCKY && card.kind != CardKind.SEX }.shuffled(random)
        cards.addAll(order(rest, sexCards))
    }

    val remaining: Int get() = cards.size

    fun draw(): Card? = cards.removeFirstOrNull()

    fun peek(): Card? = cards.firstOrNull()

    fun drawLucky(): Card? = luckyCards.randomOrNull(random)

    fun shouldShowLucky(): Boolean = random.nextDouble() < LUCKY_CHANCE

    private fun order(shuffled: List<Card>, sexCards: List<Card>): List<Card> {
        val foreplay = shuffled.filter { card -> card.kind == CardKind.FOREPLAY }.toMutableList()
        val other = shuffled.filter { card -> card.kind != CardKind.FOREPLAY }.toMutableList()
        val result = mutableListOf<Card>()
        // Cards alternate between the two players, so whose turn a card lands on is its index's parity.
        val hadForeplay = BooleanArray(2)
        var sexIndex = 0
        var count = 0
        var seat = 0
        var nextSexAt = max(nextSexGap(), 2)
        while (count < nextSexAt || sexIndex < sexCards.size || foreplay.isNotEmpty() || other.isNotEmpty()) {
            if (count == nextSexAt && sexIndex < sexCards.size) {
                if (hadForeplay[0] && hadForeplay[1]) {
                    result += sexCards[sexIndex++]
                    nextSexAt += nextSexGap()
                    hadForeplay.fill(false)
                    count++
                    seat = 1 - seat
                    continue
                }
                // Not both warmed up yet, so the sex card waits one more card.
                nextSexAt++
            }
            val isBothWarm = hadForeplay[0] && hadForeplay[1]
            val isForeplayDue = foreplay.isNotEmpty() && !isBothWarm &&
                (nextSexAt - count <= 2 || (result.lastOrNull()?.kind != CardKind.FOREPLAY && random.nextDouble() < FOREPLAY_CHANCE))
            val next = when {
                isForeplayDue -> foreplay.removeAt(0).also { hadForeplay[seat] = true }
                other.isNotEmpty() -> other.removeAt(0)
                foreplay.isNotEmpty() -> foreplay.removeAt(0).also { hadForeplay[seat] = true }
                else -> break
            }
            result += next
            count++
            seat = 1 - seat
        }
        result += sexCards.drop(sexIndex)
        result += foreplay
        result += other
        return result
    }

    private fun nextSexGap(): Int = random.nextInt(10, 14)

    private companion object {
        const val LUCKY_CHANCE = 0.07
        const val FOREPLAY_CHANCE = 0.3
    }
}
