using Godot;
using System;

public partial class ButtonHandler : Node
{
	TurnManager TurnManager => Components.Instance.TurnManager;
	DeckManager DeckManager => Components.Instance.DeckManager;
	Animations Animations => Components.Instance.Animations;
	CardAnimations CardAnimations => Components.Instance.CardAnimations;
	TimerController TimerController => Components.Instance.TimerController;
	ColorPicker ColorPicker => Components.Instance.ColorPicker;

	[Export] public Button StartGameButton;
	[Export] public Button LeftPlayerColorButton;
	[Export] public Button RightPlayerColorButton;

	public Vector2 leftColorButtonSize;
	public Vector2 rightColorButtonSize;
	public Vector2 leftColorButtonPosition;
	public Vector2 rightColorButtonPosition = new Vector2(560, 1060);

	[Export] public Button FrontCardButton;
	[Export] public Button BackCardButton;
	[Export] public Button LuckyCardButton;

	[Export] public Button OptionsButton;
	[Export] public Button OptionButtonTop;
	[Export] public Button OptionButtonMid;
	[Export] public Button OptionButtonBottom;

	public override void _Ready()
	{
		leftColorButtonPosition = LeftPlayerColorButton.Position;
		rightColorButtonPosition = RightPlayerColorButton.Position;
		leftColorButtonSize = LeftPlayerColorButton.Size;
		rightColorButtonSize = RightPlayerColorButton.Size;

		StartGameButton.Pressed += OnStartGamePressed;
		StartGameButton.ButtonDown += StartGameButtonDown;
		StartGameButton.ButtonUp += StartGameButtonUp;

		LeftPlayerColorButton.ButtonDown += LeftColorButtonDown;
		LeftPlayerColorButton.ButtonUp += LeftColorButtonUp;

		RightPlayerColorButton.ButtonDown += RightColorButtonDown;
		RightPlayerColorButton.ButtonUp += RightColorButtonUp;

		LeftPlayerColorButton.Pressed += LeftColorButtonPressed;
		RightPlayerColorButton.Pressed += RightColorButtonPressed;

		FrontCardButton.Pressed += OnFrontCardPressed;
		BackCardButton.Pressed += OnBackCardPressed;
		LuckyCardButton.Pressed += OnLuckyCardPressed;
	}

	// ------------------------------------------------------------------------------------------------
	#region Start Button ------------------------------------------------
	// ------------------------------------------------------------------------------------------------

	public void OnStartGamePressed()
	{
		TurnManager.turnsAssigned = true;
		TurnManager.gameStarted = true;
		DeckManager.gameStarted = true;
		TurnManager.AssignFirstPlayer();
		DeckManager.ShowNextFrontCard();

		Animations.AnimatePlayerSetupScene();
		Animations.StartCardAnimation();
	}

	public void StartGameButtonDown()
	{
		var StartButtonTween = CreateTween();
		StartButtonTween.Parallel().TweenProperty(StartGameButton, "scale", new Vector2(0.95f, 0.95f), .1f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
	}

	public void StartGameButtonUp()
	{
		var StartButtonTween = CreateTween();
		StartButtonTween.Parallel().TweenProperty(StartGameButton, "scale", new Vector2(1f, 1f), .1f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
	}

	#endregion
	// ------------------------------------------------------------------------------------------------
	#region Card Buttons ------------------------------------------------
	// ------------------------------------------------------------------------------------------------

	public void OnFrontCardPressed()
	{
		if (DeckManager.currentCard != null)
		{
			DeckManager.DisplaySecondCard(DeckManager.currentCard);

			CardAnimations.FlipCards();
		}
	}

	public void OnBackCardPressed()
	{
		if (TimerController != null && TimerController.IsAnyTimerRunning()) { return; }

		if (TimerController != null)
		{
			TimerController.HideAllTimers();
			TimerController.isTimerActive = false;
		}

		TurnManager.NextTurn();
		TurnManager.UpdatePlayerTurn();

		DeckManager.DisplayFirstCard(DeckManager.currentCard);
		CardAnimations.MoveCards();
	}

	public void OnLuckyCardPressed()
	{
		CardAnimations.HideLuckyCard();
	}

	#endregion

	// ------------------------------------------------------------------------------------------------
	#region Player Setup Scene ------------------------------------------------
	// ------------------------------------------------------------------------------------------------

	public void LeftColorButtonPressed()
	{
		ColorPicker.currentActivePlayer = Player.Player1;
		ColorPicker.isColorPickerActive = !ColorPicker.isColorPickerActive;

		if (ColorPicker.isColorPickerActive)
		{
			// ColorPicker.ShowActivePlayerPanel();
			Animations.StartLeftPlayerTurn();
			LeftPlayerColorButton.ZIndex = 4;
			RightPlayerColorButton.Visible = true;

			ColorPicker.LeftColorRadius(expanded: false);

			var MoveColorBox = CreateTween();
			MoveColorBox.TweenProperty(ColorPicker.ColorPickerContainer, "position", new Vector2(0, 1060), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

			var MoveColorButton = CreateTween();
			MoveColorButton.Parallel().TweenProperty(LeftPlayerColorButton, "size", new Vector2(920f, leftColorButtonSize.Y), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

			MoveColorButton.TweenCallback(Callable.From(() =>
			{
				RightPlayerColorButton.Visible = false;
			}));
		}
		else
		{
			RightPlayerColorButton.Visible = true;

			ColorPicker.LeftColorRadius(expanded: true);

			var MoveColorBox = CreateTween();
			MoveColorBox.TweenProperty(ColorPicker.ColorPickerContainer, "position", new Vector2(0, 2440), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

			var MoveColorButton = CreateTween();
			MoveColorButton.Parallel().TweenProperty(LeftPlayerColorButton, "size", new Vector2(leftColorButtonSize.X, leftColorButtonSize.Y), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

			MoveColorButton.TweenCallback(Callable.From(() =>
			{
				LeftPlayerColorButton.ZIndex = 3;
				RightPlayerColorButton.Disabled = false;
			}));

			Animations.ResetPlayersPosition();
		}
	}

	public void RightColorButtonPressed()
	{
		var size = RightPlayerColorButton.Size;

		ColorPicker.currentActivePlayer = Player.Player2;
		ColorPicker.isColorPickerActive = !ColorPicker.isColorPickerActive;

		if (ColorPicker.isColorPickerActive)
		{
			RightPlayerColorButton.ZIndex = 4;
			// ColorPicker.ShowActivePlayerPanel();
			Animations.StartRightPlayerTurn();

			LeftPlayerColorButton.Visible = true;

			ColorPicker.RightColorRadius(expanded: false);

			var MoveColorBox = CreateTween();
			MoveColorBox.TweenProperty(ColorPicker.ColorPickerContainer, "position", new Vector2(0, 1060), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

			var MoveColorButton = CreateTween();
			MoveColorButton.Parallel().TweenProperty(RightPlayerColorButton, "position", new Vector2(80f, rightColorButtonPosition.Y), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
			MoveColorButton.Parallel().TweenProperty(RightPlayerColorButton, "size", new Vector2(920f, rightColorButtonSize.Y), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

			MoveColorButton.TweenCallback(Callable.From(() =>
			{
				LeftPlayerColorButton.Visible = false;
			}));
		}
		else
		{
			RightPlayerColorButton.ZIndex = 3;

			LeftPlayerColorButton.Visible = true;

			ColorPicker.RightColorRadius(expanded: true);

			var MoveColorBox = CreateTween();
			MoveColorBox.TweenProperty(ColorPicker.ColorPickerContainer, "position", new Vector2(0, 2440), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

			var MoveColorButton = CreateTween();
			MoveColorButton.Parallel().TweenProperty(RightPlayerColorButton, "position", new Vector2(rightColorButtonPosition.X, rightColorButtonPosition.Y), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
			MoveColorButton.Parallel().TweenProperty(RightPlayerColorButton, "size", new Vector2(rightColorButtonSize.X, rightColorButtonSize.Y), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

			MoveColorButton.TweenCallback(Callable.From(() =>
			{
				RightPlayerColorButton.ZIndex = 3;
				LeftPlayerColorButton.Disabled = false;
			}));

			Animations.ResetPlayersPosition();
		}
	}

	public void LeftColorButtonDown()
	{
		var LeftButtonTween = CreateTween();
		LeftButtonTween.Parallel().TweenProperty(LeftPlayerColorButton, "scale", new Vector2(0.95f, 0.95f), .1f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
	}

	public void LeftColorButtonUp()
	{
		var LeftButtonTween = CreateTween();
		LeftButtonTween.Parallel().TweenProperty(LeftPlayerColorButton, "scale", new Vector2(1f, 1f), .1f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
	}

	public void RightColorButtonDown()
	{
		var RightButtonTween = CreateTween();
		RightButtonTween.Parallel().TweenProperty(RightPlayerColorButton, "scale", new Vector2(0.95f, 0.95f), .1f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
	}

	public void RightColorButtonUp()
	{
		var RightButtonTween = CreateTween();
		RightButtonTween.Parallel().TweenProperty(RightPlayerColorButton, "scale", new Vector2(1f, 1f), .1f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
		RightButtonTween.TweenCallback(Callable.From(() =>
		{
			bool isActive = RightPlayerColorButton.PivotOffset == new Vector2(460, 80);

			if (isActive) RightPlayerColorButton.PivotOffset = new Vector2(220, 80);
			else RightPlayerColorButton.PivotOffset = new Vector2(460, 80);
		}));
	}

	#endregion
}
