using Godot;
using System;

public partial class Animations : Node
{
    [Export] public Control LeftPlayerPanel;
    [Export] public Control RightPlayerPanel;
    public Vector2 leftPlayerPanelOriginalPosition;
    public Vector2 rightPlayerPanelOriginalPosition;

    [Export] public Panel FrontCardPanel;
    [Export] public Panel BackCardPanel;
    public Vector2 backCardOriginalPos;
    public Vector2 frontCardOriginalPos;

    public override void _Ready()
    {
        leftPlayerPanelOriginalPosition = LeftPlayerPanel.Position;
        rightPlayerPanelOriginalPosition = RightPlayerPanel.Position;

        CallDeferred(nameof(InitializeCardPositions));
        ResetToNeutralPosition();
    }

    private void InitializeCardPositions()
    {
        if (Components.Instance?.DeckManager != null)
        {
            backCardOriginalPos = Components.Instance.DeckManager.BackCardPanel.Position;
            frontCardOriginalPos = Components.Instance.DeckManager.FrontCardPanel.Position;
        }
    }

    public void ResetToNeutralPosition()
    {
        var Player1 = CreateTween();
        var player1Tween = Player1.TweenProperty(LeftPlayerPanel, "position", leftPlayerPanelOriginalPosition, 0.25f)
        .SetEase(Tween.EaseType.Out)
        .SetTrans(Tween.TransitionType.Quad);

        var Player2 = CreateTween();
        var player2Tween = Player2.TweenProperty(RightPlayerPanel, "position", rightPlayerPanelOriginalPosition, 0.25f)
        .SetEase(Tween.EaseType.Out)
        .SetTrans(Tween.TransitionType.Quad);
    }

    public void AnimateForPlayer(Player activePlayer)
    {
        if (activePlayer == Player.Player1)
        {
            Player1Turn();
        }
        else
        {
            Player2Turn();
        }
    }

    public void Player2Turn()
    {
        var animationSpeed = 0.25f;

        var movePlayer1 = CreateTween();
        var tweenProperty1a = movePlayer1.TweenProperty(LeftPlayerPanel, "position", leftPlayerPanelOriginalPosition, animationSpeed);
        tweenProperty1a.SetEase(Tween.EaseType.Out);
        tweenProperty1a.SetTrans(Tween.TransitionType.Quad);

        var movePlayer2 = CreateTween();
        var tweenProperty2a = movePlayer2.TweenProperty(RightPlayerPanel, "position", rightPlayerPanelOriginalPosition + new Vector2(-540, 0), animationSpeed);
        tweenProperty2a.SetEase(Tween.EaseType.Out);
        tweenProperty2a.SetTrans(Tween.TransitionType.Quad);
        var tweenProperty2b = movePlayer2.TweenProperty(RightPlayerPanel, "position", rightPlayerPanelOriginalPosition + new Vector2(-500, 0), .15f);
        tweenProperty2b.SetEase(Tween.EaseType.InOut);
        tweenProperty2b.SetTrans(Tween.TransitionType.Sine);
    }

    public void Player1Turn()
    {
        var animationSpeed = 0.2f;

        var movePlayer1 = CreateTween();
        var tweenProperty1a = movePlayer1.TweenProperty(LeftPlayerPanel, "position", leftPlayerPanelOriginalPosition + new Vector2(+540, 0), animationSpeed);
        tweenProperty1a.SetEase(Tween.EaseType.Out);
        tweenProperty1a.SetTrans(Tween.TransitionType.Quad);
        var tweenProperty1b = movePlayer1.TweenProperty(LeftPlayerPanel, "position", leftPlayerPanelOriginalPosition + new Vector2(+500, 0), .15f);
        tweenProperty1b.SetEase(Tween.EaseType.InOut);
        tweenProperty1b.SetTrans(Tween.TransitionType.Sine);

        var movePlayer2 = CreateTween();
        var tweenProperty2 = movePlayer2.TweenProperty(RightPlayerPanel, "position", rightPlayerPanelOriginalPosition, animationSpeed);
        tweenProperty2.SetEase(Tween.EaseType.Out);
        tweenProperty2.SetTrans(Tween.TransitionType.Quad);
    }

    public void ChangeTurn()
    {
        AnimatePlayerPanels();
    }

    public void AnimatePlayerPanels()
    {
        var movePlayer1 = CreateTween();
        var tweenProperty1 = movePlayer1.TweenProperty(LeftPlayerPanel, "position", leftPlayerPanelOriginalPosition + new Vector2(-540, 0), 0.5f);
        tweenProperty1.SetEase(Tween.EaseType.Out);
        tweenProperty1.SetTrans(Tween.TransitionType.Sine);

        var movePlayer2 = CreateTween();
        var tweenProperty2 = movePlayer2.TweenProperty(RightPlayerPanel, "position", rightPlayerPanelOriginalPosition + new Vector2(540, 0), 0.5f);
        tweenProperty2.SetEase(Tween.EaseType.Out);
        tweenProperty2.SetTrans(Tween.TransitionType.Sine);
    }

    public void AnimateDeckEmpty()
    {
        var animationSpeed = 0.3f;

        var movePlayer1 = CreateTween();
        var tweenProperty1 = movePlayer1.TweenProperty(LeftPlayerPanel, "position", leftPlayerPanelOriginalPosition, animationSpeed);
        tweenProperty1.SetDelay(.16f);
        tweenProperty1.SetEase(Tween.EaseType.Out);
        tweenProperty1.SetTrans(Tween.TransitionType.Quad);

        var movePlayer2 = CreateTween();
        var tweenProperty2 = movePlayer2.TweenProperty(RightPlayerPanel, "position", rightPlayerPanelOriginalPosition, animationSpeed);
        tweenProperty2.SetDelay(.16f);
        tweenProperty2.SetEase(Tween.EaseType.Out);
        tweenProperty2.SetTrans(Tween.TransitionType.Quad);
    }

    public void FlipCards(Panel FrontCardPanel, Panel BackCardPanel)
    {
        var animationSpeed = 0.15f;

        var frontButton = Components.Instance.DeckManager.FrontCardButton;
        var backButton = Components.Instance.DeckManager.BackCardButton;
        BackCardPanel.Position = backCardOriginalPos;
        FrontCardPanel.Position = frontCardOriginalPos;

        frontButton.Disabled = true;
        BackCardPanel.Visible = false;
        BackCardPanel.Scale = new Vector2(0.0f, BackCardPanel.Scale.Y);

        var FrontCardFlip = CreateTween();
        var flipFrontCard = FrontCardFlip.TweenProperty(FrontCardPanel, "scale:x", 0.0f, animationSpeed)
        .SetEase(Tween.EaseType.Out)
        .SetTrans(Tween.TransitionType.Sine);
        FrontCardFlip.TweenCallback(Callable.From(() =>
        {
            BackCardPanel.Visible = true;
        }));

        var BackCardFlip = CreateTween();
        var flipBackTween = BackCardFlip.TweenProperty(BackCardPanel, "scale:x", 1.0f, animationSpeed)
        .SetDelay(animationSpeed)
        .SetEase(Tween.EaseType.Out)
        .SetTrans(Tween.TransitionType.Sine);
        BackCardFlip.TweenCallback(Callable.From(() =>
        {
            backButton.Disabled = false;
            FrontCardPanel.Visible = false;
            FrontCardPanel.Scale = new Vector2(1.0f, FrontCardPanel.Scale.Y);
            FrontCardPanel.Position = frontCardOriginalPos + 2000 * Vector2.Up;
        }));
    }

    public void MoveCards()
    {
        var animationSpeed = 0.6f;
        var frontButton = Components.Instance.DeckManager.FrontCardButton;
        var backButton = Components.Instance.DeckManager.BackCardButton;

        backButton.Disabled = true;

        var backCardMove = CreateTween();
        var backCardPanel = backCardMove.TweenProperty(BackCardPanel, "position", backCardOriginalPos + 2000 * Vector2.Down, animationSpeed)
        .SetEase(Tween.EaseType.InOut)
        .SetTrans(Tween.TransitionType.Quint);
        backCardMove.TweenCallback(Callable.From(() =>
        {
            FrontCardPanel.Visible = true;
            BackCardPanel.Visible = false;
            Components.Instance.DeckManager.ProgressToNextCard();
        }));

        var frontCardMove = CreateTween();
        var frontCardTween = frontCardMove.TweenProperty(FrontCardPanel, "position", frontCardOriginalPos, animationSpeed)
        .SetDelay(animationSpeed)
        .SetEase(Tween.EaseType.InOut)
        .SetTrans(Tween.TransitionType.Quint);
        frontCardMove.TweenCallback(Callable.From(() =>
        {
            frontButton.Disabled = false;
        }));
    }
}
