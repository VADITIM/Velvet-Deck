package com.vaditim.velvetdeck.vas

import android.content.Context
import android.os.VibrationEffect
import android.os.Vibrator
import android.os.VibratorManager
import com.vaditim.velvetdeck.settings.Settings

// The phone's own vibrator rather than view haptics, which One UI mutes or flattens to one buzz depending on system settings. Every call is silent while VIBRATION is off in the options.
object Haptics {
    // Two short clicks: a card has been dealt to the other player, a joker spent.
    fun confirm(context: Context) {
        val vibrator = vibratorOf(context) ?: return
        val click = VibrationEffect.Composition.PRIMITIVE_CLICK
        val effect = if (vibrator.areAllPrimitivesSupported(click)) {
            VibrationEffect.startComposition()
                .addPrimitive(click, CONFIRM_STRENGTH)
                .addPrimitive(click, CONFIRM_STRENGTH, Motion.HAPTIC_CONFIRM_GAP_MS)
                .compose()
        } else {
            VibrationEffect.createWaveform(longArrayOf(0, CLICK_MS, Motion.HAPTIC_CONFIRM_GAP_MS.toLong(), CLICK_MS), -1)
        }
        vibrator.vibrate(effect)
    }

    // One light tick: every press, every countdown second.
    fun tick(context: Context) {
        val vibrator = vibratorOf(context) ?: return
        val tick = VibrationEffect.Composition.PRIMITIVE_TICK
        val effect = if (vibrator.areAllPrimitivesSupported(tick)) {
            VibrationEffect.startComposition().addPrimitive(tick, TICK_STRENGTH).compose()
        } else {
            VibrationEffect.createPredefined(VibrationEffect.EFFECT_TICK)
        }
        vibrator.vibrate(effect)
    }

    // A card's time is up: three long pulses, felt even with the phone face down on the bed.
    fun alarm(context: Context) {
        val vibrator = vibratorOf(context) ?: return
        val pulse = Motion.HAPTIC_ALARM_PULSE_MS
        val gap = Motion.HAPTIC_ALARM_GAP_MS
        vibrator.vibrate(VibrationEffect.createWaveform(longArrayOf(0, pulse, gap, pulse, gap, pulse), -1))
    }

    private fun vibratorOf(context: Context): Vibrator? {
        if (!Settings.isVibrationOn) return null
        return context.getSystemService(VibratorManager::class.java)?.defaultVibrator?.takeIf { it.hasVibrator() }
    }

    private const val CONFIRM_STRENGTH = 0.8f
    private const val TICK_STRENGTH = 0.6f
    private const val CLICK_MS = 18L
}
