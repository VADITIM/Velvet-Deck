using Godot;
using System;

public partial class Animations : Node
{
    DeckManager DeckManager => Components.Instance?.DeckManager;
    ButtonHandler ButtonHandler => Components.Instance?.ButtonHandler;

    [Export] public Control LeftPlayerPanel;
    [Export] public Control RightPlayerPanel;
    public Vector2 leftPlayerPanelOriginalPosition;
    public Vector2 rightPlayerPanelOriginalPosition;
    [Export] public Panel LeftPlayerForegroundPanel;
    [Export] public Panel RightPlayerForegroundPanel;

    public Vector2 startButtonOriginalPos;
    public Vector2 startButtonOffset;

    [Export] public Panel FrontCardPanel;
    public Vector2 frontCardOriginalPos;
    [Export] public Panel BackCardPanel;
    public Vector2 backCardOriginalPos;
    [Export] public Panel LuckyCardPanel;
    public Vector2 luckyCardOriginalPos;

    [Export] public Panel StartCardFrontPanel;
    public Vector2 startCardFrontOriginalPos;
    [Export] public Panel StartCardBackPanel;
    public Vector2 startCardBackOriginalPos;
    [Export] public TextureRect StartCardBackTypeImage;

    [Export] public TextureRect LogoTop;
    [Export] public TextureRect LogoBottom;

    [Export] public LineEdit LeftPlayerNameEdit;
    [Export] public LineEdit RightPlayerNameEdit;


    public override void _Ready() { CallDeferred(nameof(InitializeComponents)); }

    private void InitializeComponents()
    {
        leftPlayerPanelOriginalPosition = LeftPlayerPanel.Position;
        rightPlayerPanelOriginalPosition = RightPlayerPanel.Position;
        luckyCardOriginalPos = LuckyCardPanel.Position;

        startCardFrontOriginalPos = StartCardFrontPanel.Position;
        startCardBackOriginalPos = StartCardBackPanel.Position;

        startButtonOffset = new Vector2(180, 720);
        ButtonHandler.StartGameButton.Scale = new Vector2(0f, 0f);
        startButtonOriginalPos = ButtonHandler.StartGameButton.Position;
        ButtonHandler.StartGameButton.Position = startButtonOffset;

        backCardOriginalPos = DeckManager.BackCardPanel.Position;
        frontCardOriginalPos = DeckManager.FrontCardPanel.Position;

        ResetPlayersPosition();
    }

    public void AnimateForPlayer(Player activePlayer)
    {
        if (activePlayer == Player.Player1)
            LeftPlayerTurn();
        else
            RightPlayerTurn();
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------
    #region Player Panel Animations ----------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------------------------------------------------

    public void ResetPlayersPosition()
    {
        var LeftPlayerTween = CreateTween();
        LeftPlayerTween.Parallel().TweenProperty(LeftPlayerPanel, "position", leftPlayerPanelOriginalPosition, 0.25f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);

        var RightPlayerTween = CreateTween();
        RightPlayerTween.Parallel().TweenProperty(RightPlayerPanel, "position", rightPlayerPanelOriginalPosition, 0.25f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
    }

    // ------------------------------------------------------------------------------------------------------------------------
    // Panel Animation for Start Scene
    // ------------------------------------------------------------------------------------------------------------------------
    public void StartLeftPlayerTurn()
    {
        var animationSpeed = 0.25f;

        var LeftPlayerTween = CreateTween();
        LeftPlayerTween.TweenProperty(LeftPlayerPanel, "position", leftPlayerPanelOriginalPosition + new Vector2(+540, 0), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
        LeftPlayerTween.TweenProperty(LeftPlayerPanel, "position", leftPlayerPanelOriginalPosition + new Vector2(+500, 0), .15f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Sine);

        var RightPlayerTween = CreateTween();
        RightPlayerTween.TweenProperty(RightPlayerPanel, "position", rightPlayerPanelOriginalPosition, animationSpeed).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quad);
    }

    public void StartRightPlayerTurn()
    {
        var animationSpeed = 0.25f;

        var RightPlayerTween = CreateTween();
        RightPlayerTween.TweenProperty(RightPlayerPanel, "position", rightPlayerPanelOriginalPosition + new Vector2(-540, 0), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
        RightPlayerTween.TweenProperty(RightPlayerPanel, "position", rightPlayerPanelOriginalPosition + new Vector2(-500, 0), .15f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Sine);

        var LeftPlayerTween = CreateTween();
        LeftPlayerTween.TweenProperty(LeftPlayerPanel, "position", leftPlayerPanelOriginalPosition, animationSpeed).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quad);
    }
    // ------------------------------

    // ------------------------------------------------------------------------------------------------------------------------
    // Panel Animation Switching Turns
    // ------------------------------------------------------------------------------------------------------------------------
    public void LeftPlayerTurn()
    {
        var animationSpeed = 0.25f;

        var LeftPlayerTween = CreateTween();
        LeftPlayerTween.TweenProperty(LeftPlayerPanel, "position", leftPlayerPanelOriginalPosition + new Vector2(+540, 0), animationSpeed).SetDelay(.25f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
        LeftPlayerTween.TweenProperty(LeftPlayerPanel, "position", leftPlayerPanelOriginalPosition + new Vector2(+500, 0), .15f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Sine);

        var RightPlayerTween = CreateTween();
        RightPlayerTween.TweenProperty(RightPlayerPanel, "position", rightPlayerPanelOriginalPosition, animationSpeed).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quad);
    }

    public void RightPlayerTurn()
    {
        var animationSpeed = 0.25f;

        var RightPlayerTween = CreateTween();
        RightPlayerTween.TweenProperty(RightPlayerPanel, "position", rightPlayerPanelOriginalPosition + new Vector2(-540, 0), animationSpeed).SetDelay(.25f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
        RightPlayerTween.TweenProperty(RightPlayerPanel, "position", rightPlayerPanelOriginalPosition + new Vector2(-500, 0), .15f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Sine);

        var LeftPlayerTween = CreateTween();
        LeftPlayerTween.TweenProperty(LeftPlayerPanel, "position", leftPlayerPanelOriginalPosition, animationSpeed).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quad);
    }
    // ------------------------------

    #endregion


    // --------------------------------------------------------------------------------------------------------------------------------------------
    #region Input Fields Animations ----------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------------------------------------------------

    public void FlipLeftInput()
    {
        var animationSpeed = 0.25f;

        var LeftPlayerTween = CreateTween();
        LeftPlayerTween.Parallel().TweenProperty(RightPlayerNameEdit, "position", new Vector2(140f, RightPlayerNameEdit.Position.Y - 320f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        LeftPlayerTween.Parallel().TweenProperty(RightPlayerNameEdit, "scale", new Vector2(1f, 1f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        LeftPlayerTween.TweenCallback(Callable.From(() =>
        {
            RightPlayerNameEdit.ZIndex = 1;
            LeftPlayerNameEdit.ZIndex = 0;
            LeftPlayerNameEdit.MouseFilter = Control.MouseFilterEnum.Ignore;
            RightPlayerNameEdit.MouseFilter = Control.MouseFilterEnum.Stop;
        }));

        var RightPlayerTween = CreateTween();
        RightPlayerTween.Parallel().TweenProperty(LeftPlayerNameEdit, "position", new Vector2(140f, LeftPlayerNameEdit.Position.Y - 80f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        RightPlayerTween.Parallel().TweenProperty(LeftPlayerNameEdit, "scale", new Vector2(.92f, .92f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        LeftPlayerTween.TweenProperty(RightPlayerNameEdit, "position", new Vector2(140f, RightPlayerNameEdit.Position.Y + 80f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        RightPlayerTween.TweenCallback(Callable.From(() =>
        {
            RightPlayerNameEdit.GrabFocus();
            RightPlayerNameEdit.SelectAll();
        }));
    }

    public void FlipRightInput()
    {
        var animationSpeed = 0.25f;
        var LeftPlayerTween = CreateTween();
        LeftPlayerTween.Parallel().TweenProperty(LeftPlayerNameEdit, "position", new Vector2(140f, LeftPlayerNameEdit.Position.Y - 320f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        LeftPlayerTween.Parallel().TweenProperty(LeftPlayerNameEdit, "scale", new Vector2(1f, 1f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        LeftPlayerTween.TweenCallback(Callable.From(() =>
        {
            LeftPlayerNameEdit.ZIndex = 1;
            RightPlayerNameEdit.ZIndex = 0;
            RightPlayerNameEdit.MouseFilter = Control.MouseFilterEnum.Ignore;
            LeftPlayerNameEdit.MouseFilter = Control.MouseFilterEnum.Stop;
        }));

        var RightPlayerTween = CreateTween();
        RightPlayerTween.Parallel().TweenProperty(RightPlayerNameEdit, "position", new Vector2(140f, RightPlayerNameEdit.Position.Y - 80f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        RightPlayerTween.Parallel().TweenProperty(RightPlayerNameEdit, "scale", new Vector2(.92f, .92f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        LeftPlayerTween.TweenProperty(LeftPlayerNameEdit, "position", new Vector2(140f, LeftPlayerNameEdit.Position.Y + 80f), animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
    }

    #endregion

    // --------------------------------------------------------------------------------------------------------------------------------------------
    #region Button Animations ----------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------------------------------------------------

    public void AnimateStartButton(bool isActive)
    {
        if (isActive && ButtonHandler?.StartGameButton != null)
        {
            ButtonHandler.StartGameButton.Visible = true;
            var StartButtonTween = CreateTween();
            StartButtonTween.Parallel().TweenProperty(ButtonHandler.StartGameButton, "position", startButtonOriginalPos, 1.1f).SetDelay(.3).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);
            StartButtonTween.Parallel().TweenProperty(ButtonHandler.StartGameButton, "scale", new Vector2(1f, 1f), .4f).SetDelay(.3).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Linear);
        }
    }

    public void AnimatePlayerSetupScene()
    {
        LineInputsOut();
        ColorButtonssOut();
        StartButtonOut();
        LogoOut();
    }

    public void LineInputsOut()
    {
        var leftEditPos = LeftPlayerNameEdit.Position;
        var rightEditPos = RightPlayerNameEdit.Position;

        var LeftPlayerTween = CreateTween();
        LeftPlayerTween.Parallel().TweenProperty(LeftPlayerNameEdit, "position", leftEditPos + new Vector2(-0, -1500), 1.5f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);

        var RightPlayerTween = CreateTween();
        RightPlayerTween.Parallel().TweenProperty(RightPlayerNameEdit, "position", rightEditPos + new Vector2(0, -1500), 1.5f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
    }

    public void ColorButtonssOut()
    {
        var leftButtonPos = ButtonHandler.LeftPlayerColorButton.Position;
        var rightButtonPos = ButtonHandler.RightPlayerColorButton.Position;

        var LeftButtonTween = CreateTween();
        LeftButtonTween.Parallel().TweenProperty(ButtonHandler.LeftPlayerColorButton, "position", leftButtonPos + new Vector2(-1000, 0), 1.35f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
        LeftButtonTween.Parallel().TweenProperty(ButtonHandler.LeftPlayerColorButton, "scale", new Vector2(0.7f, 0.7f), 1.35f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);

        var RightButtonTween = CreateTween();
        RightButtonTween.Parallel().TweenProperty(ButtonHandler.RightPlayerColorButton, "position", rightButtonPos + new Vector2(1000, 0), 1.35f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
        RightButtonTween.Parallel().TweenProperty(ButtonHandler.RightPlayerColorButton, "scale", new Vector2(0.7f, 0.7f), 1.35f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
    }

    public void StartButtonOut()
    {
        var startButtonPos = ButtonHandler.StartGameButton.Position;

        var StartButtonTween = CreateTween();
        StartButtonTween.Parallel().TweenProperty(ButtonHandler.StartGameButton, "position", startButtonPos + new Vector2(0, 1000), 1.5f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
    }

    public void LogoOut()
    {
        var LogoTween = CreateTween();
        LogoTween.Parallel().TweenProperty(LogoTop, "position", new Vector2(1720, -340), 1.3f).SetDelay(.2f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);

        LogoTween.Parallel().TweenProperty(LogoBottom, "position", new Vector2(-1280, 890), 1.3f).SetDelay(.2f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
    }


    #endregion

    // --------------------------------------------------------------------------------------------------------------------------------------------
    #region Game Start Animations ----------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------------------------------------------------

    public void AnimateGameStart()
    {
        StartCardFrontPanel.Visible = false;
        StartCardBackPanel.Visible = false;
        var FrontCardPos = FrontCardPanel.Position;
        FrontCardPanel.Scale = new Vector2(0f, 1f);
        FrontCardPanel.Visible = true;

        var Tween = CreateTween();
        Tween.Parallel().TweenProperty(FrontCardPanel, "scale:x", 1f, .3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        Tween.TweenCallback(Callable.From(() =>
        {
            Components.Instance.TurnManager.UpdatePlayerTurn();
        }));
    }

    public void StartCardAnimation()
    {
        var animationSpeed = .25f;

        FrontCardPanel.Visible = false;
        DeckManager.CardsContainer.Visible = true;

        StartCardFrontPanel.Visible = true;
        StartCardBackPanel.Visible = true;

        StartCardFrontPanel.Position = startCardFrontOriginalPos + 1900 * Vector2.Up;
        StartCardBackPanel.Position = startCardBackOriginalPos + 1900 * Vector2.Up;
        StartCardBackPanel.Scale = new Vector2(0f, 1f);

        var Tween = CreateTween();
        Tween.Parallel().TweenProperty(StartCardFrontPanel, "position", startCardFrontOriginalPos, 1.2).SetDelay(1.2f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);
        Tween.Parallel().TweenProperty(StartCardBackPanel, "position", startCardBackOriginalPos, 1.2).SetDelay(1.2f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);
        var ScaleTween = CreateTween();
        ScaleTween.Parallel().TweenProperty(StartCardFrontPanel, "scale:x", 0f, animationSpeed).SetDelay(1.2f + animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        ScaleTween.Parallel().TweenProperty(StartCardBackPanel, "scale:x", 1f, animationSpeed).SetDelay(1.2f + (animationSpeed * 2)).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        ScaleTween.TweenCallback(Callable.From(() =>
        {
            StartCardFrontPanel.Visible = true;
            StartCardBackPanel.Visible = true;
        }));
        ScaleTween.Parallel().TweenProperty(StartCardBackPanel, "scale:x", 0f, animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        ScaleTween.Parallel().TweenProperty(StartCardFrontPanel, "scale:x", 1f, animationSpeed).SetDelay(animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        ScaleTween.Parallel().TweenProperty(StartCardFrontPanel, "scale:x", 0f, animationSpeed).SetDelay(animationSpeed * 2).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        ScaleTween.TweenCallback(Callable.From(() =>
        {
            AnimateGameStart();
        }));
    }

    #endregion
}
