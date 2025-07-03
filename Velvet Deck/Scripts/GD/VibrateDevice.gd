extends Button

func _pressed():
	VibrationController.VibrateLight()
	print("vibrate")


func _pressed2():
	VibrationController.VibrateMedium()
	print("vibrate medium")


func FrontCardPressed():
	pass # Replace with function body.


func OnBackCardPressed():
	pass # Replace with function body.


func OnLuckyCardPressed():
	pass # Replace with function body.
