package com.vaditim.velvetdeck.game

import androidx.compose.runtime.Stable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue

enum class Seat {
    FIRST,
    SECOND;

    val other: Seat get() = if (this == FIRST) SECOND else FIRST
}

// One player as the setup screen edits them: a name and a colour, kept as 0xAARRGGBB so this module never needs a Compose colour.
@Stable
class Player(name: String, color: Int) {
    var name by mutableStateOf(name)
    var color by mutableIntStateOf(color)

    val isNamed: Boolean get() = name.isNotBlank()
}

@Stable
class Lineup(val first: Player, val second: Player) {
    val isReady: Boolean get() = first.isNamed && second.isNamed

    fun of(seat: Seat): Player = if (seat == Seat.FIRST) first else second
}
