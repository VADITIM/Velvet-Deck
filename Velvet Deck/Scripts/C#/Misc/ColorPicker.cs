using Godot;
using System;

public partial class ColorPicker : Control
{
    Animations Animations => Components.Instance?.Animations;
    TurnManager TurnManager => Components.Instance?.TurnManager;
    DeckManager DeckManager => Components.Instance?.DeckManager;
    PlayerSetupManager PlayerSetupManager => Components.Instance?.PlayerSetupManager;
    ButtonHandler ButtonHandler => Components.Instance?.ButtonHandler;

    [Export] public ColorRect ColorRect;
    [Export] public Button SelectButton;
    [Export] public Control ColorPickerContainer;

    public bool isColorPickerActive = false;
    public Player currentActivePlayer = Player.Player1;

    public Color leftPlayerColor = Colors.White;
    public Color rightPlayerColor = Colors.White;
    public Color selectedColor = Colors.White;

    public void LeftColorRadius(bool expanded)
    {
        var normalStyleBox = ButtonHandler.LeftPlayerColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
        var hoverStyleBox = ButtonHandler.LeftPlayerColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
        var focusStyleBox = ButtonHandler.LeftPlayerColorButton.GetThemeStylebox("focus") as StyleBoxFlat;

        normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
        hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
        focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;

        if (!expanded)
        {
            var radiusTween = CreateTween();
            radiusTween.TweenMethod(Callable.From<float>((float progress) =>
            {
                var rightRadius = (int)(50f * progress);
                normalStyleBox.CornerRadiusTopRight = rightRadius;
                normalStyleBox.CornerRadiusBottomRight = rightRadius;
                hoverStyleBox.CornerRadiusTopRight = rightRadius;
                hoverStyleBox.CornerRadiusBottomRight = rightRadius;
                focusStyleBox.CornerRadiusTopRight = rightRadius;
                focusStyleBox.CornerRadiusBottomRight = rightRadius;
                ButtonHandler.LeftPlayerColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);
                ButtonHandler.LeftPlayerColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);
                ButtonHandler.LeftPlayerColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);
            }), 0f, 1f, 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
        }
        else
        {
            normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
            hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
            focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;

            var radiusTween = CreateTween();
            radiusTween.TweenMethod(Callable.From<float>((float progress) =>
            {
                var rightRadius = (int)(50f * (1f - progress));
                normalStyleBox.CornerRadiusTopRight = rightRadius;
                normalStyleBox.CornerRadiusBottomRight = rightRadius;
                hoverStyleBox.CornerRadiusTopRight = rightRadius;
                hoverStyleBox.CornerRadiusBottomRight = rightRadius;
                focusStyleBox.CornerRadiusTopRight = rightRadius;
                focusStyleBox.CornerRadiusBottomRight = rightRadius;
                ButtonHandler.LeftPlayerColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);
                ButtonHandler.LeftPlayerColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);
                ButtonHandler.LeftPlayerColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);
            }), 0f, 1f, 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);


        }
    }

    public void RightColorRadius(bool expanded)
    {
        if (!expanded)
        {
			var normalStyleBox = ButtonHandler.RightPlayerColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
			var hoverStyleBox = ButtonHandler.RightPlayerColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
			var focusStyleBox = ButtonHandler.RightPlayerColorButton.GetThemeStylebox("focus") as StyleBoxFlat;

			normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
			hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
			focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;

			var radiusTween = CreateTween();
			radiusTween.TweenMethod(Callable.From<float>((float progress) =>
			{
				var leftRadius = (int)(50f * progress);
				normalStyleBox.CornerRadiusTopLeft = leftRadius;
				normalStyleBox.CornerRadiusBottomLeft = leftRadius;
				hoverStyleBox.CornerRadiusTopLeft = leftRadius;
				hoverStyleBox.CornerRadiusBottomLeft = leftRadius;
				focusStyleBox.CornerRadiusTopLeft = leftRadius;
				focusStyleBox.CornerRadiusBottomLeft = leftRadius;
				ButtonHandler.RightPlayerColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);
				ButtonHandler.RightPlayerColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);
				ButtonHandler.RightPlayerColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);
			}), 0f, 1f, 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

        }
        else
        {
			var normalStyleBox = ButtonHandler.RightPlayerColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
			var hoverStyleBox = ButtonHandler.RightPlayerColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
			var focusStyleBox = ButtonHandler.RightPlayerColorButton.GetThemeStylebox("focus") as StyleBoxFlat;

			normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
			hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
			focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;

			var radiusTween = CreateTween();
			radiusTween.TweenMethod(Callable.From<float>((float progress) =>
			{
				var leftRadius = (int)(50f * (1f - progress));
				normalStyleBox.CornerRadiusTopLeft = leftRadius;
				normalStyleBox.CornerRadiusBottomLeft = leftRadius;
				hoverStyleBox.CornerRadiusTopLeft = leftRadius;
				hoverStyleBox.CornerRadiusBottomLeft = leftRadius;
				focusStyleBox.CornerRadiusTopLeft = leftRadius;
				focusStyleBox.CornerRadiusBottomLeft = leftRadius;
				ButtonHandler.RightPlayerColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);
				ButtonHandler.RightPlayerColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);
				ButtonHandler.RightPlayerColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);
			}), 0f, 1f, 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);


        }
    }

    public string[] colorHex = new string[]
    {
        "#432533",
        "#ff3976",
        "#008ced",
        "#dabb00",
        "#7e55dd",
        "#1eb372",

        "#dc001c",
        "#dc001c",
        "#0044ed",
        "#ff7b00",
        "#cf00c9",
        "#459f29",
    };

    public override void _Ready()
    {
        SelectButton.Pressed += SetSelectedColors;
        UpdateColorRect();

        AddThemeConstantOverride("h_separation", 20);
        AddThemeConstantOverride("v_separation", 20);

        var containerStyle = new StyleBoxFlat();
        containerStyle.ContentMarginLeft = 20;
        containerStyle.ContentMarginRight = 20;
        containerStyle.ContentMarginTop = 20;
        containerStyle.ContentMarginBottom = 20;
        containerStyle.BgColor = Colors.Transparent;
        AddThemeStyleboxOverride("panel", containerStyle);

        foreach (var color in colorHex)
        {
            var colorValue = Color.FromHtml(color);
            var button = new Button
            {
                Text = "",
                Name = color,
                CustomMinimumSize = new Vector2(130, 130)
            };

            var styleBox = new StyleBoxFlat();
            styleBox.BgColor = colorValue;
            styleBox.SetBorderWidthAll(5);
            styleBox.SetCornerRadiusAll(40);

            button.AddThemeStyleboxOverride("normal", styleBox);
            button.AddThemeStyleboxOverride("hover", styleBox);
            button.AddThemeStyleboxOverride("pressed", styleBox);

            button.Pressed += () =>
            {
                SetSelectedColor(colorValue);
                SetSelectedColors();
            };
            AddChild(button);
        }
    }

    public void SetSelectedColor(Color color)
    {
        selectedColor = color;
        UpdateColorRect();
    }



    private void UpdateActivePlayer()
    {
        var customizePlayers = TurnManager?.GetCustomizePlayers();

        if (Animations.LeftPlayerNameEdit.MouseFilter == Control.MouseFilterEnum.Stop)
            currentActivePlayer = Player.Player1;
        else if (Animations.RightPlayerNameEdit.MouseFilter == Control.MouseFilterEnum.Stop)
            currentActivePlayer = Player.Player2;
    }

    public void ShowActivePlayerPanel()
    {
        if (currentActivePlayer == Player.Player1)
            Animations.LeftPlayerTurn();
        else
            Animations.RightPlayerTurn();
    }

    private void ApplyColorToActivePlayer(Color color)
    {
        if (currentActivePlayer == Player.Player1)
            ChangeColorPlayerLeft(color);
        else
            ChangeColorPlayerRight(color);
    }

    private void ChangeColorPlayerLeft(Color color)
    {
        leftPlayerColor = color;

        var PlayerTween = CreateTween();
        PlayerTween.Parallel().TweenProperty(Animations.LeftPlayerPanel, "self_modulate", color.Darkened(0.3f), 0.3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        PlayerTween.Parallel().TweenProperty(Animations.LeftPlayerForegroundPanel, "self_modulate", color.Darkened(0f), 0.3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);

        ApplyColorToLineEdit(Animations.LeftPlayerNameEdit, color);
        ChangeColorLeftButton(color);
    }

    private void ChangeColorPlayerRight(Color color)
    {
        rightPlayerColor = color;

        var PlayerTween = CreateTween();
        PlayerTween.Parallel().TweenProperty(Animations.RightPlayerPanel, "self_modulate", color.Darkened(0.3f), 0.3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        PlayerTween.Parallel().TweenProperty(Animations.RightPlayerForegroundPanel, "self_modulate", color.Darkened(0f), 0.3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);

        ApplyColorToLineEdit(Components.Instance.Animations.RightPlayerNameEdit, color);
        ChangeColorRightButton(color);
    }

    private void ApplyColorToLineEdit(LineEdit lineEdit, Color color)
    {
        if (lineEdit == null) return;

        var existingStyleBox = lineEdit.GetThemeStylebox("normal") as StyleBoxFlat;
        StyleBoxFlat styleBox = existingStyleBox.Duplicate() as StyleBoxFlat;

        var currentColor = styleBox.BgColor;

        var lineEditTween = CreateTween();
        lineEditTween.TweenMethod(Callable.From<Color>((Color interpolatedColor) =>
        {
            styleBox.BgColor = interpolatedColor;
            styleBox.BorderColor = interpolatedColor.Lightened(0.2f);
            lineEdit.AddThemeStyleboxOverride("normal", styleBox);
            lineEdit.AddThemeStyleboxOverride("focus", styleBox);
        }), currentColor, color, 0.3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);

        var luminance = (color.R * 0.299f + color.G * 0.587f + color.B * 0.114f);
        var textColor = luminance > 0.5f ? Colors.Black : Colors.White;
        lineEdit.AddThemeColorOverride("font_color", textColor);
    }

    private void ChangeColorLeftButton(Color color)
    {
        var normalStyleBox = ButtonHandler.LeftPlayerColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
        var hoverStyleBox = ButtonHandler.LeftPlayerColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
        var focusStyleBox = ButtonHandler.LeftPlayerColorButton.GetThemeStylebox("focus") as StyleBoxFlat;

        normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
        hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
        focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;

        var currentColor = normalStyleBox.BgColor;

        var colorTween = CreateTween();
        colorTween.TweenMethod(Callable.From<Color>((Color interpolatedColor) =>
        {
            normalStyleBox.BgColor = interpolatedColor;
            normalStyleBox.BorderColor = interpolatedColor.Lightened(0.2f);
            ButtonHandler.LeftPlayerColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);

            hoverStyleBox.BgColor = interpolatedColor.Lightened(0.1f);
            hoverStyleBox.BorderColor = interpolatedColor.Lightened(0.3f);
            ButtonHandler.LeftPlayerColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);

            focusStyleBox.BgColor = interpolatedColor;
            focusStyleBox.BorderColor = interpolatedColor.Lightened(0.2f);
            ButtonHandler.LeftPlayerColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);
        }), currentColor, color, 0.3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);

        var luminance = (color.R * 0.299f + color.G * 0.587f + color.B * 0.114f);
        var textColor = luminance > 0.5f ? Colors.Black : Colors.White;
        ButtonHandler.LeftPlayerColorButton.AddThemeColorOverride("font_color", textColor);
    }

    private void ChangeColorRightButton(Color color)
    {
        var normalStyleBox = ButtonHandler.RightPlayerColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
        var hoverStyleBox = ButtonHandler.RightPlayerColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
        var focusStyleBox = ButtonHandler.RightPlayerColorButton.GetThemeStylebox("focus") as StyleBoxFlat;

        normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
        hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
        focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;

        var currentColor = normalStyleBox.BgColor;

        var colorTween = CreateTween();
        colorTween.TweenMethod(Callable.From<Color>((Color interpolatedColor) =>
        {
            normalStyleBox.BgColor = interpolatedColor;
            normalStyleBox.BorderColor = interpolatedColor.Lightened(0.2f);
            ButtonHandler.RightPlayerColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);

            hoverStyleBox.BgColor = interpolatedColor.Lightened(0.1f);
            hoverStyleBox.BorderColor = interpolatedColor.Lightened(0.3f);
            ButtonHandler.RightPlayerColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);

            focusStyleBox.BgColor = interpolatedColor;
            focusStyleBox.BorderColor = interpolatedColor.Lightened(0.2f);
            ButtonHandler.RightPlayerColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);
        }), currentColor, color, 0.3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);

        var luminance = (color.R * 0.299f + color.G * 0.587f + color.B * 0.114f);
        var textColor = luminance > 0.5f ? Colors.Black : Colors.White;
        ButtonHandler.RightPlayerColorButton.AddThemeColorOverride("font_color", textColor);
    }

    public void SetSelectedColors() { ApplyColorToActivePlayer(selectedColor); }

    private void UpdateColorRect() { ColorRect.Color = selectedColor; }

    public Player GetCurrentActivePlayer() { return currentActivePlayer; }

    public bool IsColorPickerActive() { return isColorPickerActive; }

    public Color GetPlayer1Color() { return leftPlayerColor; }

    public Color GetPlayer2Color() { return rightPlayerColor; }

    public Color GetCurrentPlayerColor() { return currentActivePlayer == Player.Player1 ? leftPlayerColor : rightPlayerColor; }
}
