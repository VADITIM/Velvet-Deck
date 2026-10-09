package com.vaditim.velvetdeck.vas

import androidx.compose.animation.core.CubicBezierEasing

// The table tempo. Controls keep the gallery's ceiling (nothing a finger waits on over 0.3s); the cards themselves are the one place allowed a beat longer, because turning a card over is the moment the game is played for. See docs/DESIGN.md § Motion.
object Motion {
    // GSAP's curves, as cubic béziers, so the numbers in dna/05-motion.md can be read straight across.
    val backOut = CubicBezierEasing(0.34f, 1.56f, 0.64f, 1f)
    val backIn = CubicBezierEasing(0.36f, 0f, 0.66f, -0.56f)
    val powerTwoOut = CubicBezierEasing(0.5f, 1f, 0.89f, 1f)
    val powerTwoIn = CubicBezierEasing(0.11f, 0f, 0.5f, 0f)
    val powerThreeInOut = CubicBezierEasing(0.65f, 0f, 0.35f, 1f)

    // A stage (setup, table, empty deck) cutting to the next: the new one rises in after a short gate, the old one only fades.
    const val STAGE_ENTER_MS = 260
    const val STAGE_ENTER_DELAY_MS = 60
    const val STAGE_LEAVE_MS = 120

    // The pieces of a stage arriving one after another, top to bottom, rather than as one sheet.
    const val ASSEMBLE_STAGGER_MS = 50

    const val OVERLAY_ENTER_MS = 240
    const val OVERLAY_LEAVE_MS = 140

    const val PRESS_MS = 80
    const val RELEASE_MS = 220
    const val STATE_MS = 220

    // A card turning over: the half that hides the front, then the half that shows the back, on one clock.
    const val FLIP_MS = 380
    // A card let go past the decision point leaves the table, accelerating away.
    const val CARD_LEAVE_MS = 220
    // A card let go short of the decision point comes back to the middle on the overshoot.
    const val CARD_RETURN_MS = 260
    // The waiting card growing into the top card's place once the top one has gone.
    const val CARD_SETTLE_MS = 260

    // The lucky card falling onto the table from above; it is an event, so it travels the whole screen.
    const val LUCKY_ENTER_MS = 420
    const val LUCKY_LEAVE_MS = 160

    // The bar-sweep reveal on a title: the bar grows, then retracts slower because that half is the one read; leaving is a quicker cut.
    const val SWEEP_GROW_MS = 420
    const val SWEEP_RETRACT_MS = 500
    const val SWEEP_LEAVE_MS = 300

    // Text that changes types itself over: the old letters go back one by one at the quicker pace, the new ones come in at the slower.
    const val TYPE_MS = 60L
    const val UNTYPE_MS = 30L
    const val CARET_BLINK_MS = 250L

    // How long END stays armed waiting for its second tap before it quietly disarms.
    const val CONFIRM_ARMED_MS = 3000L

    // The pause between the two clicks of the "done" vibration.
    const val HAPTIC_CONFIRM_GAP_MS = 70
    // The timer's end: three long pulses, so it is felt from across the room.
    const val HAPTIC_ALARM_PULSE_MS = 400L
    const val HAPTIC_ALARM_GAP_MS = 250L
}
