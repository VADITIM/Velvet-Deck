extends Node

var vibration_enabled: bool = true

func set_vibration_enabled(enabled: bool):
    vibration_enabled = enabled
    print("Vibration " + ("enabled" if enabled else "disabled"))

func VibrateLight(duration: int = 40):
    if vibration_enabled and OS.has_feature("android"):
        Input.vibrate_handheld(duration)

func VibrateMedium(duration: int = 100):
    if vibration_enabled and OS.has_feature("android"):
        Input.vibrate_handheld(duration)

func VibrateHeavy(duration: int = 200):
    if vibration_enabled and OS.has_feature("android"):
        Input.vibrate_handheld(duration)

func VibrateTimer(duration: int = 3500):
    if vibration_enabled and OS.has_feature("android"):
        Input.vibrate_handheld(duration)
        await get_tree().create_timer(0.5).timeout
        Input.vibrate_handheld(duration)