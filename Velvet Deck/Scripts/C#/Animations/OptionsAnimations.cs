using Godot;
using System;

public partial class OptionsAnimations : Control
{
    ButtonHandler ButtonHandler => Components.Instance?.ButtonHandler;
    PlayerSetupManager PlayerSetupManager => Components.Instance?.PlayerSetupManager;
    Animations Animations => Components.Instance?.Animations;

    [Export] Control Options;

    [Export] Panel OptionsPanel;
    [Export] Panel TopButtonPanel;
    [Export] Panel TopGreenPanel;
    [Export] Panel TopRedPanel;

    [Export] Panel MidButtonPanel;
    [Export] Panel MidGreenPanel;
    [Export] Panel MidRedPanel;

    [Export] Panel BottomButtonPanel;
    [Export] Panel BottomGreenPanel;
    [Export] Panel BottomRedPanel;

    public Vector2 optionTopPosition;
    public Vector2 optionMidPosition;
    public Vector2 optionBottomPosition;

    private Vector2 optionsButtonOriginalPosition;
    private Vector2 topGreenPanelOriginalPosition;
    private Vector2 topRedPanelOriginalPosition;
    private Vector2 midGreenPanelOriginalPosition;
    private Vector2 midRedPanelOriginalPosition;
    private Vector2 bottomGreenPanelOriginalPosition;
    private Vector2 bottomRedPanelOriginalPosition;
    private Vector2 optionsPanelOriginalPosition;
    private Vector2 playerSetupSceneOriginalPosition;
    private Vector2 leftPlayerPanelOriginalPosition;
    private Vector2 rightPlayerPanelOriginalPosition;

    private bool isTopButtonExpanded = false;
    private bool isMidButtonExpanded = false;
    private bool isBottomButtonExpanded = false;

    private bool areTopPanelsExpanded = false;
    private bool areMidPanelsExpanded = false;
    private bool areBottomPanelsExpanded = false;
    private bool isOptionsPanelExpanded = false;
    private bool isOptionsAnimating = false;

    public override void _Ready()
    {
        optionTopPosition = ButtonHandler.OptionButtonTop.Position;
        optionMidPosition = ButtonHandler.OptionButtonMid.Position;
        optionBottomPosition = ButtonHandler.OptionButtonBottom.Position;

        optionsButtonOriginalPosition = ButtonHandler.OptionsButton.Position;
        topGreenPanelOriginalPosition = TopGreenPanel.Position;
        topRedPanelOriginalPosition = TopRedPanel.Position;
        midGreenPanelOriginalPosition = MidGreenPanel.Position;
        midRedPanelOriginalPosition = MidRedPanel.Position;
        bottomGreenPanelOriginalPosition = BottomGreenPanel.Position;
        bottomRedPanelOriginalPosition = BottomRedPanel.Position;
        optionsPanelOriginalPosition = OptionsPanel.Position;
        playerSetupSceneOriginalPosition = PlayerSetupManager.PlayerSetupScene.Position;

        ButtonHandler.OptionsButton.Pressed += OptionsButtonPressed;
        ButtonHandler.OptionsButton.ButtonDown += OptionsButtonDown;
        ButtonHandler.OptionsButton.ButtonUp += OptionsButtonUp;
        ButtonHandler.OptionButtonTop.Pressed += TopOptionPressed;
        ButtonHandler.OptionButtonMid.Pressed += MidOptionPressed;
        ButtonHandler.OptionButtonBottom.Pressed += BottomOptionPressed;
    }

    public void OptionsButtonPressed()
    {
        if (isOptionsAnimating) return;

        if (!isOptionsPanelExpanded)
        {
            leftPlayerPanelOriginalPosition = Animations.LeftPlayerPanel.Position;
            rightPlayerPanelOriginalPosition = Animations.RightPlayerPanel.Position;

            isOptionsAnimating = true;
            var Tween = CreateTween();
            Tween.Parallel().TweenProperty(OptionsPanel, "position", new Vector2(optionsPanelOriginalPosition.X, optionsPanelOriginalPosition.Y - 2340), 1).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);
            Tween.Parallel().TweenProperty(PlayerSetupManager.PlayerSetupScene, "position", new Vector2(PlayerSetupManager.PlayerSetupScene.Position.X, PlayerSetupManager.PlayerSetupScene.Position.Y - 2340), 1).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);
            Tween.Parallel().TweenProperty(ButtonHandler.OptionsButton, "position", new Vector2(optionsButtonOriginalPosition.X, optionsButtonOriginalPosition.Y + 2340), 1).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);

            Tween.Parallel().TweenProperty(Animations.LeftPlayerPanel, "position", new Vector2(Animations.LeftPlayerPanel.Position.X, Animations.LeftPlayerPanel.Position.Y - 2340), 1).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);
            Tween.Parallel().TweenProperty(Animations.RightPlayerPanel, "position", new Vector2(Animations.RightPlayerPanel.Position.X, Animations.RightPlayerPanel.Position.Y - 2340), 1).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);

            Tween.Parallel().TweenProperty(Animations.CardsContainer, "position", new Vector2(Animations.CardsContainer.Position.X, Animations.CardsContainer.Position.Y - 2340), 1).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);

            Tween.TweenCallback(Callable.From(() =>
            {
                isOptionsPanelExpanded = true;
                isOptionsAnimating = false;
            }));
        }
        else
        {
            isOptionsAnimating = true;
            var Tween = CreateTween();
            Tween.Parallel().TweenProperty(OptionsPanel, "position", optionsPanelOriginalPosition, 1).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);
            Tween.Parallel().TweenProperty(PlayerSetupManager.PlayerSetupScene, "position", playerSetupSceneOriginalPosition, 1).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);
            Tween.Parallel().TweenProperty(ButtonHandler.OptionsButton, "position", optionsButtonOriginalPosition, 1).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);

            Tween.Parallel().TweenProperty(Animations.LeftPlayerPanel, "position", leftPlayerPanelOriginalPosition, 1).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);
            Tween.Parallel().TweenProperty(Animations.RightPlayerPanel, "position", rightPlayerPanelOriginalPosition, 1).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);

            Tween.Parallel().TweenProperty(Animations.CardsContainer, "position", Animations.cardsContainerOriginalPosition, 1).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);

            Tween.TweenCallback(Callable.From(() =>
            {
                isOptionsPanelExpanded = false;
                isOptionsAnimating = false;
            }));
        }
    }

    public void OptionsButtonDown()
    {
        var Tween = CreateTween();
        Tween.Parallel().TweenProperty(ButtonHandler.OptionsButton, "scale", new Vector2(0.95f, 0.95f), 0.1f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
    }

    public void OptionsButtonUp()
    {
        var Tween = CreateTween();
        Tween.Parallel().TweenProperty(ButtonHandler.OptionsButton, "scale", new Vector2(1f, 1f), 0.1f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
    }

    public void AnimateTopOptionButton()
    {
        if (!isTopButtonExpanded)
        {
            var Tween = CreateTween();
            Tween.Parallel().TweenProperty(ButtonHandler.OptionButtonTop, "position", new Vector2(optionTopPosition.X + 1080, optionTopPosition.Y), 1.5f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);

            isTopButtonExpanded = true;
        }
        else
        {
            var Tween = CreateTween();
            Tween.Parallel().TweenProperty(ButtonHandler.OptionButtonTop, "position", optionTopPosition, 1.5f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
            isTopButtonExpanded = false;
        }
    }

    public void AnimateOnOffPanels(string buttonType)
    {
        Panel greenPanel = null, redPanel = null;
        Vector2 greenOriginalPos = Vector2.Zero, redOriginalPos = Vector2.Zero;
        bool isPanelsExpanded = false;

        switch (buttonType.ToLower())
        {
            case "top":
                greenPanel = TopGreenPanel;
                redPanel = TopRedPanel;
                greenOriginalPos = topGreenPanelOriginalPosition;
                redOriginalPos = topRedPanelOriginalPosition;
                isPanelsExpanded = areTopPanelsExpanded;
                break;
            case "mid":
                greenPanel = MidGreenPanel;
                redPanel = MidRedPanel;
                greenOriginalPos = midGreenPanelOriginalPosition;
                redOriginalPos = midRedPanelOriginalPosition;
                isPanelsExpanded = areMidPanelsExpanded;
                break;
            case "bottom":
                greenPanel = BottomGreenPanel;
                redPanel = BottomRedPanel;
                greenOriginalPos = bottomGreenPanelOriginalPosition;
                redOriginalPos = bottomRedPanelOriginalPosition;
                isPanelsExpanded = areBottomPanelsExpanded;
                break;
        }

        if (greenPanel == null || redPanel == null) return;

        var Tween = CreateTween();

        if (!isPanelsExpanded)
        {
            // Animate panels out (to the right)
            Tween.Parallel().TweenProperty(greenPanel, "position", new Vector2(greenOriginalPos.X + 1080, greenOriginalPos.Y), 1.5f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
            Tween.Parallel().TweenProperty(redPanel, "position", new Vector2(redOriginalPos.X + 1080, redOriginalPos.Y), 1.5f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);

            // Update state
            switch (buttonType.ToLower())
            {
                case "top": areTopPanelsExpanded = true; break;
                case "mid": areMidPanelsExpanded = true; break;
                case "bottom": areBottomPanelsExpanded = true; break;
            }
        }
        else
        {
            // Animate panels back to original positions
            Tween.Parallel().TweenProperty(greenPanel, "position", greenOriginalPos, 1.5f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
            Tween.Parallel().TweenProperty(redPanel, "position", redOriginalPos, 1.5f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);

            // Update state
            switch (buttonType.ToLower())
            {
                case "top": areTopPanelsExpanded = false; break;
                case "mid": areMidPanelsExpanded = false; break;
                case "bottom": areBottomPanelsExpanded = false; break;
            }
        }
    }


    public void TopOptionPressed()
    {
        AnimateTopOptionButton();
        AnimateOnOffPanels("top");
    }

    public void MidOptionPressed()
    {
        AnimateMidOptionButton();
        AnimateOnOffPanels("mid");
    }

    public void BottomOptionPressed()
    {
        AnimateBottomOptionButton();
        AnimateOnOffPanels("bottom");
    }

    public void AnimateMidOptionButton()
    {
        if (!isMidButtonExpanded)
        {
            var Tween = CreateTween();
            Tween.Parallel().TweenProperty(ButtonHandler.OptionButtonMid, "position", new Vector2(optionMidPosition.X + 1080, optionMidPosition.Y), 1.5f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);

            isMidButtonExpanded = true;
        }
        else
        {
            var Tween = CreateTween();
            Tween.Parallel().TweenProperty(ButtonHandler.OptionButtonMid, "position", optionMidPosition, 1.5f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
            isMidButtonExpanded = false;
        }
    }

    public void AnimateBottomOptionButton()
    {
        if (!isBottomButtonExpanded)
        {
            var Tween = CreateTween();
            Tween.Parallel().TweenProperty(ButtonHandler.OptionButtonBottom, "position", new Vector2(optionBottomPosition.X + 1080, optionBottomPosition.Y), 1.5f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);

            isBottomButtonExpanded = true;
        }
        else
        {
            var Tween = CreateTween();
            Tween.Parallel().TweenProperty(ButtonHandler.OptionButtonBottom, "position", optionBottomPosition, 1.5f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
            isBottomButtonExpanded = false;
        }
    }
}
