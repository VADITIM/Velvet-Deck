using Godot;
using System;

public partial class CustomizePlayers : Control
{

    [Export] public LineEdit HomePlayerName;
    [Export] public LineEdit AwayPlayerName;

    private bool homePlayerConfirmed = false;
    private bool awayPlayerConfirmed = false;

    public override void _Ready()
    {
        AwayPlayerName.Scale = new Vector2(.92f, .92f);
        AwayPlayerName.MouseFilter = Control.MouseFilterEnum.Stop;

        HomePlayerName.Text = "";
        AwayPlayerName.Text = "";
        HomePlayerName.PlaceholderText = "Home Player";
        AwayPlayerName.PlaceholderText = "Away Player";
        HomePlayerName.EditingToggled += OnHomeInputToggle;
        AwayPlayerName.EditingToggled += OnAwayInputToggle;

        CheckBothPlayersConfirmed();
    }

    public void OnHomeInputToggle(bool isEditing)
    {
        if (isEditing)
        {
            homePlayerConfirmed = false;
            CheckBothPlayersConfirmed();
            Components.Instance.Animations.Player1Turn();
            HomePlayerName.PlaceholderText = "";
            return;
        }
        else if (!string.IsNullOrEmpty(HomePlayerName.Text.Trim()))
        {
            homePlayerConfirmed = true;
            FlipHomeInput();
            Components.Instance.Animations.ResetToNeutralPosition();
            CheckBothPlayersConfirmed();
        }
        else
        {
            homePlayerConfirmed = false;
            CheckBothPlayersConfirmed();
        }
    }

    public void OnAwayInputToggle(bool isEditing)
    {
        if (isEditing)
        {
            awayPlayerConfirmed = false;
            CheckBothPlayersConfirmed();
            Components.Instance.Animations.Player2Turn();
            AwayPlayerName.PlaceholderText = "";
            return;
        }
        else if (!string.IsNullOrEmpty(AwayPlayerName.Text.Trim()))
        {
            awayPlayerConfirmed = true;
            FlipAwayInput();
            Components.Instance.Animations.ResetToNeutralPosition();
            CheckBothPlayersConfirmed();
        }
        else
        {
            awayPlayerConfirmed = false;
            CheckBothPlayersConfirmed();
        }
    }


    public void FlipHomeInput()
    {
        var animationSpeed = 0.25f;
        // Move Home Up & Scale Down
        var HomePlayer = CreateTween();
        var moveHomeUpTween = HomePlayer.Parallel().TweenProperty(AwayPlayerName, "position", new Vector2(140f, AwayPlayerName.Position.Y - 320f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        var scaleUpHomeTween = HomePlayer.Parallel().TweenProperty(AwayPlayerName, "scale", new Vector2(1f, 1f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        // CALLBACK Change ZIndex
        HomePlayer.TweenCallback(Callable.From(() =>
        {
            AwayPlayerName.ZIndex = 1;
            HomePlayerName.ZIndex = 0;
            HomePlayerName.MouseFilter = Control.MouseFilterEnum.Ignore;
            AwayPlayerName.MouseFilter = Control.MouseFilterEnum.Stop;
        }));

        // Move Away to Offset & Scale Down
        var AwayPlayer = CreateTween();
        var upsetAwayTween = AwayPlayer.Parallel().TweenProperty(HomePlayerName, "position", new Vector2(140f, HomePlayerName.Position.Y - 80f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        var scaleDownAwayTween = AwayPlayer.Parallel().TweenProperty(HomePlayerName, "scale", new Vector2(.92f, .92f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        // Move Home Down
        var moveHomeDownTween = HomePlayer.TweenProperty(AwayPlayerName, "position", new Vector2(140f, AwayPlayerName.Position.Y + 80f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
    }

    public void FlipAwayInput()
    {
        var animationSpeed = 0.25f;
        // Move Home Up & Scale Down
        var HomePlayer = CreateTween();
        var moveHomeUpTween = HomePlayer.Parallel().TweenProperty(HomePlayerName, "position", new Vector2(140f, HomePlayerName.Position.Y - 320f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        var scaleUpHomeTween = HomePlayer.Parallel().TweenProperty(HomePlayerName, "scale", new Vector2(1f, 1f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        // CALLBACK Change ZIndex
        HomePlayer.TweenCallback(Callable.From(() =>
        {
            HomePlayerName.ZIndex = 1;
            AwayPlayerName.ZIndex = 0;
            AwayPlayerName.MouseFilter = Control.MouseFilterEnum.Ignore;
            HomePlayerName.MouseFilter = Control.MouseFilterEnum.Stop;
        }));

        // Move Away to Offset & Scale Down
        var AwayPlayer = CreateTween();
        var upsetAwayTween = AwayPlayer.Parallel().TweenProperty(AwayPlayerName, "position", new Vector2(140f, AwayPlayerName.Position.Y - 80f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        var scaleDownAwayTween = AwayPlayer.Parallel().TweenProperty(AwayPlayerName, "scale", new Vector2(.92f, .92f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        // Move Home Down
        var moveHomeDownTween = HomePlayer.TweenProperty(HomePlayerName, "position", new Vector2(140f, HomePlayerName.Position.Y + 80f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
    }

    private void CheckBothPlayersConfirmed()
    {
        bool bothConfirmed = homePlayerConfirmed && awayPlayerConfirmed;

        Components.Instance.TurnManager.SetStartButtonVisible(bothConfirmed);
    }

    public void ResetConfirmationStates()
    {
        homePlayerConfirmed = false;
        awayPlayerConfirmed = false;
        CheckBothPlayersConfirmed();
    }


    public string GetHomePlayerName()
    {
        return string.IsNullOrEmpty(HomePlayerName.Text.Trim()) ? "Home Player" : HomePlayerName.Text.Trim();
    }

    public string GetAwayPlayerName()
    {
        return string.IsNullOrEmpty(AwayPlayerName.Text.Trim()) ? "Away Player" : AwayPlayerName.Text.Trim();
    }
}
