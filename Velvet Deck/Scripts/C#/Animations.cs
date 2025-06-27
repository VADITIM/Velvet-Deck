using Godot;
using System;

public partial class Animations : Node
{
    [Export] public Control Player1Panel;
    [Export] public Control Player2Panel;
    public Vector2 Player1PanelOriginalPosition;
    public Vector2 Player2PanelOriginalPosition;
    public Vector2 backCardOriginalPos;
    public Vector2 frontCardOriginalPos;

    public override void _Ready()
    {
        Player1PanelOriginalPosition = Player1Panel.Position;
        Player2PanelOriginalPosition = Player2Panel.Position;

        ResetToNeutralPosition();
    }

    public void ResetToNeutralPosition()
    {
        Player1Panel.Position = Player1PanelOriginalPosition;
        Player2Panel.Position = Player2PanelOriginalPosition;
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

    private void Player2Turn()
    {
        var animationSpeed = 0.25f;

        var movePlayer1 = CreateTween();
        var tweenProperty1a = movePlayer1.TweenProperty(Player1Panel, "position", Player1PanelOriginalPosition, animationSpeed);
        tweenProperty1a.SetEase(Tween.EaseType.Out);
        tweenProperty1a.SetTrans(Tween.TransitionType.Quad);

        var movePlayer2 = CreateTween();
        var tweenProperty2a = movePlayer2.TweenProperty(Player2Panel, "position", Player2PanelOriginalPosition + new Vector2(-540, 0), animationSpeed);
        tweenProperty2a.SetEase(Tween.EaseType.Out);
        tweenProperty2a.SetTrans(Tween.TransitionType.Quad);
        var tweenProperty2b = movePlayer2.TweenProperty(Player2Panel, "position", Player2PanelOriginalPosition + new Vector2(-500, 0), .15f);
        tweenProperty2b.SetEase(Tween.EaseType.InOut);
        tweenProperty2b.SetTrans(Tween.TransitionType.Sine);
    }

    private void Player1Turn()
    {
        var animationSpeed = 0.2f;

        var movePlayer1 = CreateTween();
        var tweenProperty1a = movePlayer1.TweenProperty(Player1Panel, "position", Player1PanelOriginalPosition + new Vector2(+540, 0), animationSpeed);
        tweenProperty1a.SetEase(Tween.EaseType.Out);
        tweenProperty1a.SetTrans(Tween.TransitionType.Quad);
        var tweenProperty1b = movePlayer1.TweenProperty(Player1Panel, "position", Player1PanelOriginalPosition + new Vector2(+500, 0), .15f);
        tweenProperty1b.SetEase(Tween.EaseType.InOut);
        tweenProperty1b.SetTrans(Tween.TransitionType.Sine);

        var movePlayer2 = CreateTween();
        var tweenProperty2 = movePlayer2.TweenProperty(Player2Panel, "position", Player2PanelOriginalPosition, animationSpeed);
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
        var tweenProperty1 = movePlayer1.TweenProperty(Player1Panel, "position", Player1PanelOriginalPosition + new Vector2(-540, 0), 0.5f);
        tweenProperty1.SetEase(Tween.EaseType.Out);
        tweenProperty1.SetTrans(Tween.TransitionType.Sine);

        var movePlayer2 = CreateTween();
        var tweenProperty2 = movePlayer2.TweenProperty(Player2Panel, "position", Player2PanelOriginalPosition + new Vector2(540, 0), 0.5f);
        tweenProperty2.SetEase(Tween.EaseType.Out);
        tweenProperty2.SetTrans(Tween.TransitionType.Sine);
    }

    public void AnimateDeckEmpty()
    {
        var animationSpeed = 0.3f;

        var movePlayer1 = CreateTween();
        var tweenProperty1 = movePlayer1.TweenProperty(Player1Panel, "position", Player1PanelOriginalPosition, animationSpeed);
        tweenProperty1.SetDelay(.16f);
        tweenProperty1.SetEase(Tween.EaseType.Out);
        tweenProperty1.SetTrans(Tween.TransitionType.Quad);

        var movePlayer2 = CreateTween();
        var tweenProperty2 = movePlayer2.TweenProperty(Player2Panel, "position", Player2PanelOriginalPosition, animationSpeed);
        tweenProperty2.SetDelay(.16f);
        tweenProperty2.SetEase(Tween.EaseType.Out);
        tweenProperty2.SetTrans(Tween.TransitionType.Quad);
    }

    public void FlipFrontCard(Panel FrontCardPanel, Panel BackCardPanel)
    {
        var frontButton = Components.Instance.DeckManager.FrontCardButton;
        var backButton = Components.Instance.DeckManager.BackCardButton;

        var animationSpeed = 0.15f;

        if (frontButton != null)
        {
            frontButton.Disabled = true;
        }

        BackCardPanel.Visible = false;
        BackCardPanel.Scale = new Vector2(0.0f, BackCardPanel.Scale.Y);

        var frontCardFlip = CreateTween();
        var tweenProperty1 = frontCardFlip.TweenProperty(FrontCardPanel, "scale:x", 0.0f, animationSpeed);
        tweenProperty1.SetEase(Tween.EaseType.Out);
        tweenProperty1.SetTrans(Tween.TransitionType.Sine);

        frontCardFlip.TweenCallback(Callable.From(() =>
        {
            FrontCardPanel.Visible = false;
            BackCardPanel.Visible = true;
        }));

        var tweenProperty2 = frontCardFlip.TweenProperty(BackCardPanel, "scale:x", 1.0f, animationSpeed);
        tweenProperty2.SetEase(Tween.EaseType.Out);
        tweenProperty2.SetTrans(Tween.TransitionType.Sine);

        frontCardFlip.TweenCallback(Callable.From(() =>
        {
            if (backButton != null)
            {
                backButton.Disabled = false;
            }
        }));
    }

    public void FlipBackCard(Panel BackCardPanel, Panel FrontCardPanel)
    {
        var frontButton = Components.Instance.DeckManager.FrontCardButton;
        var backButton = Components.Instance.DeckManager.BackCardButton;

        var animationSpeed = 0.3f;

        if (backButton != null)
        {
            backButton.Disabled = true;
        }
        BackCardPanel.Visible = true;
        FrontCardPanel.Visible = false;

        var backCardMove = CreateTween();
        var tweenProperty1 = backCardMove.TweenProperty(BackCardPanel, "position", backCardOriginalPos + new Vector2(BackCardPanel.Position.X, 2000), animationSpeed);
        tweenProperty1.SetEase(Tween.EaseType.Out);
        tweenProperty1.SetTrans(Tween.TransitionType.Sine);

        backCardMove.TweenCallback(Callable.From(() =>
        {
            BackCardPanel.Visible = false;
            Components.Instance.DeckManager.ProgressToNextCard();
        }));

        FrontCardPanel.Position = frontCardOriginalPos + new Vector2(FrontCardPanel.Position.X, 2000);
        var frontCardMove = CreateTween();
        var tweenProperty2 = frontCardMove.TweenProperty(FrontCardPanel, "position", frontCardOriginalPos, animationSpeed);
        tweenProperty2.SetEase(Tween.EaseType.Out);
        tweenProperty2.SetTrans(Tween.TransitionType.Sine);

        frontCardMove.TweenCallback(Callable.From(() =>
        {
            FrontCardPanel.Visible = true;
            if (frontButton != null)
            {
                frontButton.Disabled = false;
            }
        }));
    }
}
