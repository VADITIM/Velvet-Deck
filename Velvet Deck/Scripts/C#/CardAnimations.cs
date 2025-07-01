using Godot;
using System;

public partial class CardAnimations : Button
{
    [Export] public Panel BackCardPanel = Components.Instance?.DeckManager?.BackCardPanel;
    [Export] public Panel FrontCardPanel = Components.Instance?.DeckManager?.FrontCardPanel;
    private Vector2 backCardOriginalPos;
    private Vector2 frontCardOriginalPos;

    public override void _Ready()
    {
        backCardOriginalPos = BackCardPanel.Position;
        frontCardOriginalPos = FrontCardPanel.Position;
    }

    public void OnFrontCardPressed()
    {
        var animationSpeed = 0.25f;

        var frontButton = Components.Instance.DeckManager.FrontCardButton;
        var backButton = Components.Instance.DeckManager.BackCardButton;
        BackCardPanel.Position = backCardOriginalPos;
        FrontCardPanel.Position = frontCardOriginalPos;

        BackCardPanel.Visible = false;
        BackCardPanel.Scale = new Vector2(0.0f, BackCardPanel.Scale.Y);

        var FrontCardFlip = CreateTween();
        var flipFrontTween = FrontCardFlip.TweenProperty(FrontCardPanel, "scale:x", 0.0f, animationSpeed);
        flipFrontTween.SetEase(Tween.EaseType.Out);
        flipFrontTween.SetTrans(Tween.TransitionType.Sine);

        FrontCardFlip.TweenCallback(Callable.From(() =>
        {
            FrontCardPanel.Visible = false;
            BackCardPanel.Visible = true;
        }));

        var BackCardFlip = CreateTween();
        var flipBackTween = BackCardFlip.TweenProperty(BackCardPanel, "scale:x", 1.0f, animationSpeed);
        flipBackTween.SetEase(Tween.EaseType.Out);
        flipBackTween.SetTrans(Tween.TransitionType.Sine);

        BackCardFlip.TweenCallback(Callable.From(() =>
        {
            backButton.Disabled = false;
        }));
    }

    public void OnBackCardPressed()
    {
        var frontButton = Components.Instance.DeckManager.FrontCardButton;
        var backButton = Components.Instance.DeckManager.BackCardButton;

        var animationSpeed = 0.3f;

        BackCardPanel.Visible = true;
        FrontCardPanel.Visible = false;
        backButton.Disabled = true;

        var BackCardMoveOut = CreateTween();
        var moveBackTween = BackCardMoveOut.TweenProperty(BackCardPanel, "position", new Vector2(0, 2000), animationSpeed);
        moveBackTween.SetEase(Tween.EaseType.Out);
        moveBackTween.SetTrans(Tween.TransitionType.Sine);

        BackCardMoveOut.TweenCallback(Callable.From(() =>
        {
            BackCardPanel.Visible = false;
            Components.Instance.DeckManager.ProgressToNextCard();
        }));

        var FrontCardMoveIn = CreateTween();
        var frontCardTween = FrontCardMoveIn.TweenProperty(FrontCardPanel, "position", Vector2.Zero, animationSpeed);
        frontCardTween.SetEase(Tween.EaseType.Out);
        frontCardTween.SetTrans(Tween.TransitionType.Sine);

        FrontCardMoveIn.TweenCallback(Callable.From(() =>
        {
            FrontCardPanel.Visible = true;
            frontButton.Disabled = false;
        }));
    }
}
