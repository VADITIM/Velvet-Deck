using Godot;
using System;

public partial class CustomizePlayers : Control
{

    [Export] public LineEdit LeftPlayerNameEdit;
    [Export] public LineEdit RightPlayerNameEdit;

    private bool leftPlayerConfirmed = false;
    private bool rightPlayerConfirmed = false;

    public override void _Ready()
    {
        RightPlayerNameEdit.Scale = new Vector2(.92f, .92f);
        RightPlayerNameEdit.MouseFilter = Control.MouseFilterEnum.Stop;

        LeftPlayerNameEdit.Text = "";
        RightPlayerNameEdit.Text = "";
        LeftPlayerNameEdit.PlaceholderText = "Home Player";
        RightPlayerNameEdit.PlaceholderText = "Away Player";
        LeftPlayerNameEdit.EditingToggled += OnHomeInputToggle;
        RightPlayerNameEdit.EditingToggled += OnAwayInputToggle;

        CheckBothPlayersConfirmed();
    }

    public void OnHomeInputToggle(bool isEditing)
    {
        if (isEditing)
        {
            LeftPlayerNameEdit.SelectAll();

            leftPlayerConfirmed = false;
            CheckBothPlayersConfirmed();
            Components.Instance.Animations.Player1Turn();
            LeftPlayerNameEdit.PlaceholderText = "";
            return;
        }
        else if (!string.IsNullOrEmpty(LeftPlayerNameEdit.Text.Trim()))
        {
            leftPlayerConfirmed = true;
            FlipHomeInput();
            Components.Instance.Animations.ResetToNeutralPosition();
            CheckBothPlayersConfirmed();
        }
        else
        {
            leftPlayerConfirmed = false;
            CheckBothPlayersConfirmed();
        }
    }

    public void OnAwayInputToggle(bool isEditing)
    {
        if (isEditing)
        {
            rightPlayerConfirmed = false;
            CheckBothPlayersConfirmed();
            Components.Instance.Animations.Player2Turn();
            RightPlayerNameEdit.PlaceholderText = "";
            return;
        }
        else if (!string.IsNullOrEmpty(RightPlayerNameEdit.Text.Trim()))
        {
            rightPlayerConfirmed = true;
            FlipAwayInput();
            Components.Instance.Animations.ResetToNeutralPosition();
            CheckBothPlayersConfirmed();
        }
        else
        {
            rightPlayerConfirmed = false;
            CheckBothPlayersConfirmed();
        }
    }


    public void FlipHomeInput()
    {
        var animationSpeed = 0.25f;
        // Move Home Up & Scale Down
        var HomePlayer = CreateTween();
        var moveHomeUpTween = HomePlayer.Parallel().TweenProperty(RightPlayerNameEdit, "position", new Vector2(140f, RightPlayerNameEdit.Position.Y - 320f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        var scaleUpHomeTween = HomePlayer.Parallel().TweenProperty(RightPlayerNameEdit, "scale", new Vector2(1f, 1f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        // CALLBACK Change ZIndex
        HomePlayer.TweenCallback(Callable.From(() =>
        {
            RightPlayerNameEdit.ZIndex = 1;
            LeftPlayerNameEdit.ZIndex = 0;
            LeftPlayerNameEdit.MouseFilter = Control.MouseFilterEnum.Ignore;
            RightPlayerNameEdit.MouseFilter = Control.MouseFilterEnum.Stop;
        }));

        // Move Away to Offset & Scale Down
        var AwayPlayer = CreateTween();
        var upsetAwayTween = AwayPlayer.Parallel().TweenProperty(LeftPlayerNameEdit, "position", new Vector2(140f, LeftPlayerNameEdit.Position.Y - 80f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        var scaleDownAwayTween = AwayPlayer.Parallel().TweenProperty(LeftPlayerNameEdit, "scale", new Vector2(.92f, .92f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        // Move Home Down
        var moveHomeDownTween = HomePlayer.TweenProperty(RightPlayerNameEdit, "position", new Vector2(140f, RightPlayerNameEdit.Position.Y + 80f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        // After animation completes, make AwayPlayerName active
        AwayPlayer.TweenCallback(Callable.From(() =>
        {
            RightPlayerNameEdit.GrabFocus();
            RightPlayerNameEdit.SelectAll();
        }));
    }

    public void FlipAwayInput()
    {
        var animationSpeed = 0.25f;
        // Move Home Up & Scale Down
        var HomePlayer = CreateTween();
        var moveHomeUpTween = HomePlayer.Parallel().TweenProperty(LeftPlayerNameEdit, "position", new Vector2(140f, LeftPlayerNameEdit.Position.Y - 320f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        var scaleUpHomeTween = HomePlayer.Parallel().TweenProperty(LeftPlayerNameEdit, "scale", new Vector2(1f, 1f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        // CALLBACK Change ZIndex
        HomePlayer.TweenCallback(Callable.From(() =>
        {
            LeftPlayerNameEdit.ZIndex = 1;
            RightPlayerNameEdit.ZIndex = 0;
            RightPlayerNameEdit.MouseFilter = Control.MouseFilterEnum.Ignore;
            LeftPlayerNameEdit.MouseFilter = Control.MouseFilterEnum.Stop;
        }));

        // Move Away to Offset & Scale Down
        var AwayPlayer = CreateTween();
        var upsetAwayTween = AwayPlayer.Parallel().TweenProperty(RightPlayerNameEdit, "position", new Vector2(140f, RightPlayerNameEdit.Position.Y - 80f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        var scaleDownAwayTween = AwayPlayer.Parallel().TweenProperty(RightPlayerNameEdit, "scale", new Vector2(.92f, .92f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        // Move Home Down
        var moveHomeDownTween = HomePlayer.TweenProperty(LeftPlayerNameEdit, "position", new Vector2(140f, LeftPlayerNameEdit.Position.Y + 80f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
    }

    private void CheckBothPlayersConfirmed()
    {
        bool bothConfirmed = leftPlayerConfirmed && rightPlayerConfirmed;

        Components.Instance.TurnManager.SetStartButtonVisible(bothConfirmed);
    }

    public void ResetConfirmationStates()
    {
        leftPlayerConfirmed = false;
        rightPlayerConfirmed = false;
        CheckBothPlayersConfirmed();
    }


    public string GetHomePlayerName()
    {
        return string.IsNullOrEmpty(LeftPlayerNameEdit.Text.Trim()) ? "Home Player" : LeftPlayerNameEdit.Text.Trim();
    }

    public string GetAwayPlayerName()
    {
        return string.IsNullOrEmpty(RightPlayerNameEdit.Text.Trim()) ? "Away Player" : RightPlayerNameEdit.Text.Trim();
    }
}
