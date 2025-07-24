using Godot;
using System;

public partial class JokerManager : Control
{
    TurnManager TurnManager => Components.Instance?.TurnManager;
    DeckManager DeckManager => Components.Instance?.DeckManager;
    CardAnimations CardAnimations => Components.Instance?.CardAnimations;
    TimerController TimerController => Components.Instance?.TimerController;

    [Export] Label LeftPlayerJokerCount;
    private Vector2 leftJokerPos;

    [Export] Label RightPlayerJokerCount;
    private Vector2 rightJokerPos;

    [Export] Button JokerButton;
    private Vector2 jokerButtonPos;

    private int leftPlayerJokers = 3;
    private int rightPlayerJokers = 3;

    public override void _Ready()
    {
        jokerButtonPos = JokerButton.Position;
        JokerButton.Position = new Vector2(jokerButtonPos.X, jokerButtonPos.Y + 600f);

        leftJokerPos = LeftPlayerJokerCount.Position;
        LeftPlayerJokerCount.Position = new Vector2(jokerButtonPos.X + 80f, leftJokerPos.Y);

        rightJokerPos = RightPlayerJokerCount.Position;
        RightPlayerJokerCount.Position = new Vector2(jokerButtonPos.X + 80f, rightJokerPos.Y);

        JokerButton.Disabled = true;
        LeftPlayerJokerCount.Visible = false;
        RightPlayerJokerCount.Visible = false;

        UpdateJokerDisplays();

        JokerButton.Pressed += OnJokerButtonPressed;
    }

    public void AnimateJokerButton()
    {
        var Tween = CreateTween();
        Tween.TweenProperty(JokerButton, "position", new Vector2(jokerButtonPos.X, jokerButtonPos.Y), 1f)
        .SetEase(Tween.EaseType.InOut)
        .SetTrans(Tween.TransitionType.Elastic);
    }

    public void OnBackCardVisible()
    {
        if (!TurnManager.IsGameStarted() || GetCurrentPlayerJokerCount() <= 0) return;

        ShowJokerAnimation();
    }

    public void OnBackCardHidden()
    {
        HideJokerAnimation();
    }

    private void ShowJokerAnimation(Action onComplete = null)
    {
        float duration = 0.3f;
        float delay = .3f;

        var ButtonTween = CreateTween();
        ButtonTween.Parallel().TweenProperty(JokerButton, "position", jokerButtonPos, duration)
        .SetEase(Tween.EaseType.Out)
        .SetTrans(Tween.TransitionType.Quart);

        if (onComplete != null)
        {
            ButtonTween.TweenCallback(Callable.From(onComplete));
        }

        ButtonTween.TweenCallback(Callable.From(() =>
        {
            JokerButton.Disabled = false;
        }));

        Player currentPlayer = TurnManager.GetCurrentPlayer();

        if (currentPlayer == Player.Player1)
        {
            var LeftTween = CreateTween();
            LeftTween.TweenCallback(Callable.From(() =>
            {
                LeftPlayerJokerCount.Visible = true;
            })).SetDelay(delay);

            LeftTween.Parallel().TweenProperty(LeftPlayerJokerCount, "position", leftJokerPos, duration)
            .SetDelay(delay)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Back);
        }
        else
        {
            var RightTween = CreateTween();
            RightTween.TweenCallback(Callable.From(() =>
            {
                RightPlayerJokerCount.Visible = true;
            })).SetDelay(delay);

            RightTween.Parallel().TweenProperty(RightPlayerJokerCount, "position", rightJokerPos, duration)
            .SetDelay(delay)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Back);
        }
    }

    public void HideJokerAnimation()
    {
        float duration = 0.3f;
        float delay = 0.15f;
        JokerButton.Disabled = true;

        var ButtonTween = CreateTween();
        ButtonTween.Parallel().TweenProperty(JokerButton, "position", new Vector2(jokerButtonPos.X, jokerButtonPos.Y + 600f), 0.3f)
        .SetDelay(delay)
        .SetEase(Tween.EaseType.In)
        .SetTrans(Tween.TransitionType.Quart);

        var leftTween = CreateTween();
        leftTween.Parallel().TweenProperty(LeftPlayerJokerCount, "position", new Vector2(jokerButtonPos.X + 80f, leftJokerPos.Y), duration)
        .SetEase(Tween.EaseType.In)
        .SetTrans(Tween.TransitionType.Quart);
        leftTween.TweenCallback(Callable.From(() =>
        {
            LeftPlayerJokerCount.Visible = false;
        }));

        var rightTween = CreateTween();
        rightTween.Parallel().TweenProperty(RightPlayerJokerCount, "position", new Vector2(jokerButtonPos.X + 80f, rightJokerPos.Y), duration)
        .SetEase(Tween.EaseType.In)
        .SetTrans(Tween.TransitionType.Quart);
        rightTween.TweenCallback(Callable.From(() =>
        {
            RightPlayerJokerCount.Visible = false;
        }));
    }

    private void UpdateJokerDisplays()
    {
        LeftPlayerJokerCount.Text = leftPlayerJokers.ToString();
        RightPlayerJokerCount.Text = rightPlayerJokers.ToString();
    }

    private int GetCurrentPlayerJokerCount()
    {
        if (TurnManager == null) return 0;

        Player currentPlayer = TurnManager.GetCurrentPlayer();
        return currentPlayer == Player.Player1 ? leftPlayerJokers : rightPlayerJokers;
    }

    private void UseJoker()
    {
        if (TurnManager == null) return;

        Player currentPlayer = TurnManager.GetCurrentPlayer();

        if (currentPlayer == Player.Player1)
        {
            if (leftPlayerJokers > 0)
                leftPlayerJokers--;
        }
        else
        {
            if (rightPlayerJokers > 0)
                rightPlayerJokers--;
        }

        UpdateJokerDisplays();
    }

    public void OnJokerButtonPressed()
    {
        if (GetCurrentPlayerJokerCount() <= 0) return;

        // Animate countdown out properly, then clean up
        TimerController.AnimateCountdownOut();
        UseJoker();
        SkipCardWithAnimation();
    }

    private void SkipCardWithAnimation()
    {
        // Don't instantly hide timers here - let the animation play first
        // Just reset the timer states without moving positions
        if (DeckManager.TimerController != null)
        {
            DeckManager.TimerController.isTimerActive = false;
        }

        TurnManager.NextTurn();
        TurnManager.UpdatePlayerTurn();

        CardAnimations.MoveCards();
    }

    public int GetLeftPlayerJokers() => leftPlayerJokers;
    public int GetRightPlayerJokers() => rightPlayerJokers;

    public void ResetJokers()
    {
        leftPlayerJokers = 3;
        rightPlayerJokers = 3;
        UpdateJokerDisplays();
    }
}
