extends Node

func VibrateLight(duration: int = 100):
    if OS.has_feature("android"):
        Input.vibrate_handheld(duration)

func VibrateMedium(duration: int = 250):
    if OS.has_feature("android"):
        Input.vibrate_handheld(duration)
