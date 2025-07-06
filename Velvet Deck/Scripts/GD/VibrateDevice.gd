extends Button

func _pressed():
	VibrationController.VibrateLight()
	print("vibrate")


func _pressed2():
	VibrationController.VibrateMedium()
	print("vibrate medium")
