// Android 12 is the floor because the vibrator's composed primitives (VibratorManager, VibrationEffect.Composition) are what every press and the timer's end are made of.
object AndroidLevels {
    const val COMPILE_SDK = 36
    const val MIN_SDK = 31
    const val TARGET_SDK = 36
}
