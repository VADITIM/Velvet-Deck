using Godot;
using System;

public partial class TimerController : Control
{

	[Export] public Timer Timer { get; set; }
	[Export] public Panel TimerPanel { get; set; }
	private Vector2 timerPanelPosition;
	[Export] public Label TimerLabel { get; set; }
	[Export] public Panel CountdownPanel { get; set; }
	private Vector2 countdownPanelPosition;
	[Export] public Label CountdownLabel { get; set; }
	[Export] public Button BeginnButton { get; set; }

	private float countdownTime = 5f;
	private float currentCountdown = 0f;
	private float timerDuration = 0f;
	private float currentTimer = 0f;

	private bool isCountdownActive = false;
	public bool isTimerActive = false;
	private bool isWaitingForStart = false;

	public override void _Ready()
	{
		timerPanelPosition = TimerPanel.Position;
		countdownPanelPosition = CountdownPanel.Position;

		TimerPanel.Scale = new Vector2(0, 0);
		CountdownPanel.Position = new Vector2(countdownPanelPosition.X, countdownPanelPosition.Y + 600f);

		Timer.Timeout += OnTimerTimeout;
		BeginnButton.Pressed += OnBeginnButtonPressed;
		BeginnButton.ButtonDown += BeginnButtonDown;
		BeginnButton.ButtonUp += BeginnButtonUp;

		HideAllTimers();
	}

	public void AnimateCountdownIn()
	{
		CountdownPanel.Scale = new Vector2(1, 1);
		CountdownPanel.Position = new Vector2(countdownPanelPosition.X, countdownPanelPosition.Y + 600f);

		var Tween = CreateTween();
		Tween.TweenProperty(CountdownPanel, "position", countdownPanelPosition, 0.5f)
			.SetDelay(.3f)
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Quart);
	}

	public void AnimateCountdownOut()
	{
		float duration = 1f;

		var Tween = CreateTween();
		Tween.Parallel().TweenProperty(CountdownPanel, "position", timerPanelPosition, duration)
			.SetEase(Tween.EaseType.InOut)
			.SetTrans(Tween.TransitionType.Elastic);

		Tween.Parallel().TweenProperty(CountdownPanel, "scale", new Vector2(0, 0), duration)
			.SetEase(Tween.EaseType.InOut)
			.SetTrans(Tween.TransitionType.Elastic);

		Tween.TweenCallback(Callable.From(() =>
		{
			CountdownPanel.Position = countdownPanelPosition + new Vector2(0, 600);
		}));
	}

	public void AnimateTimerIn()
	{
		TimerPanel.Scale = new Vector2(0, 0);

		var Tween = CreateTween();
		Tween.Parallel().TweenProperty(TimerPanel, "scale", new Vector2(1, 1), 1f)
			.SetEase(Tween.EaseType.InOut)
			.SetTrans(Tween.TransitionType.Elastic);
	}

	public void AnimateTimerOut()
	{
		var Tween = CreateTween();
		Tween.Parallel().TweenProperty(TimerPanel, "position", new Vector2(timerPanelPosition.X, timerPanelPosition.Y + 600f), 1f)
			.SetEase(Tween.EaseType.InOut)
			.SetTrans(Tween.TransitionType.Elastic);

		Tween.TweenCallback(Callable.From(() =>
		{
			TimerPanel.Scale = new Vector2(0, 0);
			TimerPanel.Position = timerPanelPosition;
		}));
	}

	public override void _Process(double delta)
	{
		if (isCountdownActive)
		{
			currentCountdown -= (float)delta;
			UpdateCountdownDisplay();

			if (currentCountdown <= 0f)
			{
				EndCountdown();
				StartTimer();
			}
		}
		else if (isTimerActive)
		{
			currentTimer -= (float)delta;
			UpdateTimerDisplay();

			if (currentTimer <= 0f)
			{
				EndTimer();
			}
		}
	}

	public void StartCountdown(float timerDurationAfterCountdown)
	{
		timerDuration = timerDurationAfterCountdown;
		isWaitingForStart = true;
		isCountdownActive = false;
		isTimerActive = false;

		ShowStartPrompt();
	}

	private void ShowStartPrompt()
	{
		CountdownLabel.Text = "Beginn";
		BeginnButton.Visible = true;

		AnimateCountdownIn();
	}

	private void OnBeginnButtonPressed()
	{
		if (isWaitingForStart)
		{
			isWaitingForStart = false;
			BeginCountdown();
		}
	}

	private void BeginnButtonDown()
	{
		var Tween = CreateTween();
		Tween.Parallel().TweenProperty(CountdownPanel, "scale", new Vector2(0.95f, 0.95f), .1f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
	}

	private void BeginnButtonUp()
	{
		var Tween = CreateTween();
		Tween.Parallel().TweenProperty(CountdownPanel, "scale", new Vector2(1f, 1f), .1f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
	}

	private void BeginCountdown()
	{
		currentCountdown = countdownTime;
		isCountdownActive = true;

		BeginnButton.Visible = false;

		Components.Instance.VibrationController.Call("set_vibration_enabled", false);
		Components.Instance.JokerManager.OnBackCardHidden();
		UpdateCountdownDisplay();
	}

	private void EndCountdown()
	{
		isCountdownActive = false;
		HideCountdownPanel();
	}

	private void StartTimer()
	{
		currentTimer = timerDuration;
		isTimerActive = true;
		ShowTimerPanel();
		UpdateTimerDisplay();
	}

	private void EndTimer()
	{
		isTimerActive = false;

		Components.Instance.VibrationController.Call("set_vibration_enabled", true);
		Components.Instance.VibrationController.Call("VibrateTimer");
		AnimateTimerOut();
	}

	private void OnTimerTimeout()
	{
		EndTimer();
	}

	private void UpdateCountdownDisplay()
	{
		CountdownLabel.Text = $"Get Ready: {Mathf.Ceil(currentCountdown)}";
	}

	private void UpdateTimerDisplay()
	{
		// Ensure timer doesn't display negative values
		float displayTime = Mathf.Max(currentTimer, 0f);

		int minutes = (int)(displayTime / 60);
		int seconds = (int)(displayTime % 60);
		TimerLabel.Text = $"{minutes:00}:{seconds:00}";
	}

	public void ShowCountdownPanel()
	{
		AnimateCountdownIn();
	}

	public void HideCountdownPanel()
	{
		AnimateCountdownOut();
	}

	public void ShowTimerPanel()
	{
		AnimateTimerIn();
	}

	public void HideTimer()
	{
		AnimateTimerOut();
		isCountdownActive = false;
		isTimerActive = false;
	}

	public void HideAllTimers()
	{
		BeginnButton.Visible = false;
		CountdownPanel.Position = new Vector2(countdownPanelPosition.X, countdownPanelPosition.Y + 600f);
		isCountdownActive = false;
		isTimerActive = false;
		isWaitingForStart = false;
	}

	public bool IsCountdownActive()
	{
		return isCountdownActive;
	}

	public bool IsTimerActive()
	{
		return isTimerActive;
	}

	public bool IsWaitingForStart()
	{
		return isWaitingForStart;
	}

	public bool IsAnyTimerRunning()
	{
		return isCountdownActive || isTimerActive || isWaitingForStart;
	}
}
