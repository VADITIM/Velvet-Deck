using Godot;
using System;

public partial class TimerController : Control
{
    [Export] public Timer Timer { get; set; }
    [Export] public Panel TimerPanel { get; set; }
    [Export] public Label TimerLabel { get; set; }
    [Export] public Panel CountdownPanel { get; set; }
    [Export] public Label CountdownLabel { get; set; }

    private float countdownTime = 5f;
    private float currentCountdown = 0f;
    private float timerDuration = 0f;
    private float currentTimer = 0f;

    private bool isCountdownActive = false;
    private bool isTimerActive = false;

    public override void _Ready()
    {
        if (Timer != null)
        {
            Timer.Timeout += OnTimerTimeout;
        }

        HideAllTimers();
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
        GD.Print($"StartCountdown called with duration: {timerDurationAfterCountdown}");

        timerDuration = timerDurationAfterCountdown;
        currentCountdown = countdownTime;
        isCountdownActive = true;
        isTimerActive = false;

        ShowCountdownPanel();
        UpdateCountdownDisplay();

        GD.Print($"Starting 5-second countdown before {timerDuration}-second timer");
        GD.Print($"CountdownPanel visible: {CountdownPanel?.Visible}");
        GD.Print($"CountdownLabel text: {CountdownLabel?.Text}");
    }

    private void EndCountdown()
    {
        isCountdownActive = false;
        HideCountdownPanel();
        GD.Print("Countdown ended, starting main timer");
    }

    private void StartTimer()
    {
        currentTimer = timerDuration;
        isTimerActive = true;
        ShowTimerPanel();
        UpdateTimerDisplay();

        GD.Print($"Starting {timerDuration}-second timer");
    }

    private void EndTimer()
    {
        isTimerActive = false;
        HideTimerPanel();

        GD.Print("Timer ended");
    }

    private void OnTimerTimeout()
    {
        EndTimer();
    }

    private void UpdateCountdownDisplay()
    {
        if (CountdownLabel != null)
        {
            CountdownLabel.Text = $"Get Ready: {Mathf.Ceil(currentCountdown)}";
            GD.Print($"Updated countdown display: {CountdownLabel.Text}");
        }
        else
        {
            GD.Print("CountdownLabel is null!");
        }
    }

    private void UpdateTimerDisplay()
    {
        if (TimerLabel != null)
        {
            int minutes = (int)(currentTimer / 60);
            int seconds = (int)(currentTimer % 60);
            TimerLabel.Text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void ShowCountdownPanel()
    {
        GD.Print("ShowCountdownPanel called");
        GD.Print($"CountdownPanel is null: {CountdownPanel == null}");
        GD.Print($"TimerPanel is null: {TimerPanel == null}");

        if (CountdownPanel != null)
        {
            CountdownPanel.Visible = true;
            GD.Print("CountdownPanel set to visible");
        }

        if (TimerPanel != null)
        {
            TimerPanel.Visible = false;
            GD.Print("TimerPanel set to hidden");
        }
    }

    public void ShowTimerPanel()
    {
        if (TimerPanel != null)
        {
            TimerPanel.Visible = true;
        }

        if (CountdownPanel != null)
        {
            CountdownPanel.Visible = false;
        }
    }

    public void HideCountdownPanel()
    {
        if (CountdownPanel != null)
        {
            CountdownPanel.Visible = false;
        }
    }

    public void HideTimerPanel()
    {
        if (TimerPanel != null)
        {
            TimerPanel.Visible = false;
        }
    }

    public void ShowTimer()
    {
        ShowCountdownPanel();
    }

    public void HideTimer()
    {
        HideCountdownPanel();
        HideTimerPanel();
        isCountdownActive = false;
        isTimerActive = false;
    }

    public void HideAllTimers()
    {
        HideCountdownPanel();
        HideTimerPanel();
        isCountdownActive = false;
        isTimerActive = false;
    }

    public bool IsCountdownActive()
    {
        return isCountdownActive;
    }

    public bool IsTimerActive()
    {
        return isTimerActive;
    }

    public bool IsAnyTimerRunning()
    {
        return isCountdownActive || isTimerActive;
    }
}
