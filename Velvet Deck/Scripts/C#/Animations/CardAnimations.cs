using Godot;
using System;

public partial class CardAnimations : Control
{
    Animations Animations => Components.Instance?.Animations;
    DeckManager DeckManager => Components.Instance?.DeckManager;
    ButtonHandler ButtonHandler => Components.Instance?.ButtonHandler;
    JokerManager JokerManager => Components.Instance?.JokerManager;

    public void AnimateDeckEmpty()
    {
        var animationSpeed = 0.3f;

        var LeftPlayerTween = CreateTween();
        LeftPlayerTween.TweenProperty(Animations.LeftPlayerPanel, "position", Animations.leftPlayerPanelOriginalPosition, animationSpeed).SetDelay(.16f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);

        var RightPlayerTween = CreateTween();
        RightPlayerTween.TweenProperty(Animations.RightPlayerPanel, "position", Animations.rightPlayerPanelOriginalPosition, animationSpeed).SetDelay(.16f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
    }

    public void FlipCards()
    {
        var animationSpeed = 0.25f;

        var frontButton = ButtonHandler.FrontCardButton;
        var backButton = ButtonHandler.BackCardButton;
        Animations.BackCardPanel.Position = Animations.backCardOriginalPos;
        Animations.FrontCardPanel.Position = Animations.frontCardOriginalPos;

        frontButton.Disabled = true;
        Animations.BackCardPanel.Visible = false;
        Animations.BackCardPanel.Scale = new Vector2(0.0f, Animations.BackCardPanel.Scale.Y);

        var FrontCardFlip = CreateTween();
        FrontCardFlip.Parallel().TweenProperty(Animations.FrontCardPanel, "scale:x", 0.0f, animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        FrontCardFlip.TweenCallback(Callable.From(() =>
        {
            Animations.BackCardPanel.Visible = true;

        }));

        var BackCardFlip = CreateTween();
        BackCardFlip.Parallel().TweenProperty(Animations.BackCardPanel, "scale:x", 1.0f, animationSpeed).SetDelay(animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        BackCardFlip.TweenCallback(Callable.From(() =>
        {
            backButton.Disabled = false;
            Animations.FrontCardPanel.Visible = false;
            Animations.FrontCardPanel.Scale = new Vector2(1.0f, Animations.FrontCardPanel.Scale.Y);
            Animations.FrontCardPanel.Position = Animations.frontCardOriginalPos + 2000 * Vector2.Up;
            DeckManager.ClearFrontCardElements();

            JokerManager.OnBackCardVisible();
        }));
    }

    public void MoveCards()
    {
        Components.Instance.JokerManager.OnBackCardHidden();
        var animationSpeed = 0.4f;
        var frontButton = ButtonHandler.FrontCardButton;
        var backButton = ButtonHandler.BackCardButton;

        backButton.Disabled = true;

        var BackCardTween = CreateTween();
        BackCardTween.TweenProperty(Animations.BackCardPanel, "position", Animations.backCardOriginalPos + 2000 * Vector2.Down, animationSpeed).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quint);
        BackCardTween.TweenCallback(Callable.From(() =>
        {
            Animations.FrontCardPanel.Visible = true;
            Animations.BackCardPanel.Visible = false;
            DeckManager.ProgressToNextCard();

            JokerManager.HideJokerAnimation();
        }));

        var FrontCardTween = CreateTween();
        FrontCardTween.TweenProperty(Animations.FrontCardPanel, "position", Animations.frontCardOriginalPos, animationSpeed).SetDelay(animationSpeed).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quint);
        FrontCardTween.TweenCallback(Callable.From(() =>
        {
            frontButton.Disabled = false;
        }));
    }

    public void ShowLuckyCard()
    {
        ButtonHandler.LuckyCardButton.Disabled = true;

        var animationSpeed = 0.4f;
        Animations.LuckyCardPanel.Position = new Vector2(Animations.luckyCardOriginalPos.X, -1500);

        var LuckyCardTween = CreateTween();
        LuckyCardTween.TweenProperty(Animations.LuckyCardPanel, "position", Animations.luckyCardOriginalPos, animationSpeed).SetDelay(.1f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quint);

        var TimeoutTween = CreateTween();
        TimeoutTween.TweenProperty(Animations.LuckyCardPanel, "scale:x", 1.0f, .1f)
            .SetDelay(.4f)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Sine);
        TimeoutTween.TweenCallback(Callable.From(() =>
        {
            ButtonHandler.LuckyCardButton.Disabled = false;
        }));
    }

    public void HideLuckyCard()
    {
        ButtonHandler.FrontCardButton.Disabled = true;
        ButtonHandler.LuckyCardButton.Disabled = true;
        var animationSpeed = 0.4f;

        var LuckyCardTween = CreateTween();
        LuckyCardTween.TweenProperty(Animations.LuckyCardPanel, "scale:x", 0.0f, .1f)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Sine);
        LuckyCardTween.TweenCallback(Callable.From(() =>
        {
            DeckManager.LuckyHeaderLabel.Text = "";
            DeckManager.LuckyDescriptionLabel.Text = "";
            DeckManager.LuckyCardType.Visible = true;

        }));
        LuckyCardTween.TweenProperty(Animations.LuckyCardPanel, "scale:x", 1.0f, .1f)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Sine);
        LuckyCardTween.TweenProperty(Animations.LuckyCardPanel, "position", new Vector2(Animations.luckyCardOriginalPos.X, 3000), animationSpeed)
            .SetEase(Tween.EaseType.In)
            .SetTrans(Tween.TransitionType.Quint);
        LuckyCardTween.TweenCallback(Callable.From(() =>
        {
            Animations.LuckyCardPanel.Visible = false;
            ButtonHandler.FrontCardButton.Disabled = false;
            ButtonHandler.LuckyCardButton.Disabled = false;
        }));
    }

}
