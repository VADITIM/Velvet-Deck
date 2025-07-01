using Godot;
using System;

public partial class ColorPicker : GridContainer
{
    [Export] public ColorRect ColorRect;
    [Export] public Button SelectButton;
    [Export] public Button HomeColorButton;
    [Export] public Button AwayColorButton;
    [Export] public Control ColorPickerContainer;

    [Export] public Panel Player1Panel;
    [Export] public Panel Player2Panel;
    [Export] public Panel Player1PanelBackground;
    [Export] public Panel Player2PanelBackground;

    private bool isColorPickerActive = false;
    private Player currentActivePlayer = Player.Player1;

    private Color player1Color = Colors.White;
    private Color player2Color = Colors.White;

    private Vector2 originalPosAway;
    private Vector2 originalSizeAway;

    public string[] colorHex = new string[]
    {
        "#432533",
        "#f34f6d",
        "#4f6bd3",
        "#464f4f"
    };

    private Color selectedColor = Colors.White;

    public override void _Ready()
    {

        originalPosAway = new Vector2(AwayColorButton.Position.X, AwayColorButton.Position.Y);
        originalSizeAway = AwayColorButton.Size;

        SelectButton.Pressed += SetSelectedColors;
        HomeColorButton.Pressed += OnHomeColorButtonPressed;
        AwayColorButton.Pressed += OnAwayColorButtonPressed;
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
                CustomMinimumSize = new Vector2(120, 120)
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
                SetSelectedColors(); // Apply the color immediately
            };
            AddChild(button);
        }
    }

    public void SetSelectedColors()
    {
        ApplyColorToActivePlayer(selectedColor);
        GD.Print($"Applied color {selectedColor} to {currentActivePlayer}");
    }

    public void SetSelectedColor(Color color)
    {
        selectedColor = color;
        UpdateColorRect();
    }

    private void UpdateColorRect()
    {
        ColorRect.Color = selectedColor;
    }

    public void OnHomeColorButtonPressed()
    {
        currentActivePlayer = Player.Player1;
        isColorPickerActive = !isColorPickerActive;

        if (isColorPickerActive)
        {
            HomeColorButton.ZIndex = 4;
            ShowActivePlayerPanel();

            var normalStyleBox = HomeColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
            normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
            normalStyleBox.SetCornerRadiusAll(50);
            HomeColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);

            var hoverStyleBox = HomeColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
            hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
            hoverStyleBox.SetCornerRadiusAll(50);
            HomeColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);

            var focusStyleBox = HomeColorButton.GetThemeStylebox("focus") as StyleBoxFlat;
            focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;
            focusStyleBox.SetCornerRadiusAll(50);
            HomeColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);

            var MoveColorBox = CreateTween();
            MoveColorBox.TweenProperty(ColorPickerContainer, "position", new Vector2(0, 1060), 0.25f)
            .SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Cubic);

            var MoveColorButton = CreateTween();

            MoveColorButton.Parallel().TweenProperty(HomeColorButton, "size", new Vector2(920f, originalSizeAway.Y), 0.25f)
            .SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Cubic);
        }
        else
        {
            var normalStyleBox = HomeColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
            normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
            normalStyleBox.CornerRadiusTopRight = 0;
            normalStyleBox.CornerRadiusBottomRight = 0;
            HomeColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);

            var hoverStyleBox = HomeColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
            hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
            hoverStyleBox.CornerRadiusTopRight = 0;
            hoverStyleBox.CornerRadiusBottomRight = 0;
            HomeColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);

            var focusStyleBox = HomeColorButton.GetThemeStylebox("focus") as StyleBoxFlat;
            focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;
            focusStyleBox.CornerRadiusTopRight = 0;
            focusStyleBox.CornerRadiusBottomRight = 0;
            HomeColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);

            var MoveColorBox = CreateTween();
            MoveColorBox.TweenProperty(ColorPickerContainer, "position", new Vector2(0, 2440), 0.25f)
            .SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Cubic);

            var MoveColorButton = CreateTween();
            MoveColorButton.Parallel().TweenProperty(HomeColorButton, "size", new Vector2(originalSizeAway.X, originalSizeAway.Y), 0.25f)
            .SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Cubic);

            MoveColorButton.TweenCallback(Callable.From(() =>
            {
                // Reset the position of the HomeColorButton to its original position
                HomeColorButton.ZIndex = 3;
            }));

            Components.Instance.Animations.ResetToNeutralPosition();
        }
    }

    public void OnAwayColorButtonPressed()
    {
        currentActivePlayer = Player.Player2;
        isColorPickerActive = !isColorPickerActive;

        if (isColorPickerActive)
        {
            AwayColorButton.ZIndex = 4;
            ShowActivePlayerPanel();

            // Set all corner radii to 50 when color picker is open
            var normalStyleBox = AwayColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
            if (normalStyleBox != null)
            {
                normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
                normalStyleBox.SetCornerRadiusAll(50);
                AwayColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);
            }

            var hoverStyleBox = AwayColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
            if (hoverStyleBox != null)
            {
                hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
                hoverStyleBox.SetCornerRadiusAll(50);
                AwayColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);
            }

            var focusStyleBox = AwayColorButton.GetThemeStylebox("focus") as StyleBoxFlat;
            if (focusStyleBox != null)
            {
                focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;
                focusStyleBox.SetCornerRadiusAll(50);
                AwayColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);
            }

            var MoveColorBox = CreateTween();
            MoveColorBox.TweenProperty(ColorPickerContainer, "position", new Vector2(0, 1060), 0.25f)
            .SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Cubic);

            var MoveColorButton = CreateTween();
            MoveColorButton.Parallel().TweenProperty(AwayColorButton, "position", new Vector2(80f, originalPosAway.Y), 0.25f)
            .SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Cubic);
            MoveColorButton.Parallel().TweenProperty(AwayColorButton, "size", new Vector2(920f, originalSizeAway.Y), 0.25f)
            .SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Cubic);
        }
        else
        {
            AwayColorButton.ZIndex = 3;

            // Reset top-left and bottom-left corner radii to 0 when color picker is closed
            var normalStyleBox = AwayColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
            if (normalStyleBox != null)
            {
                normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
                normalStyleBox.CornerRadiusTopLeft = 0;
                normalStyleBox.CornerRadiusBottomLeft = 0;
                // Keep top-right and bottom-right as they were (should be 50 from the open state)
                AwayColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);
            }

            var hoverStyleBox = AwayColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
            if (hoverStyleBox != null)
            {
                hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
                hoverStyleBox.CornerRadiusTopLeft = 0;
                hoverStyleBox.CornerRadiusBottomLeft = 0;
                // Keep top-right and bottom-right as they were (should be 50 from the open state)
                AwayColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);
            }

            var focusStyleBox = AwayColorButton.GetThemeStylebox("focus") as StyleBoxFlat;
            if (focusStyleBox != null)
            {
                focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;
                focusStyleBox.CornerRadiusTopLeft = 0;
                focusStyleBox.CornerRadiusBottomLeft = 0;
                // Keep top-right and bottom-right as they were (should be 50 from the open state)
                AwayColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);
            }

            var MoveColorBox = CreateTween();
            MoveColorBox.TweenProperty(ColorPickerContainer, "position", new Vector2(0, 2440), 0.25f)
            .SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Cubic);

            var MoveColorButton = CreateTween();
            MoveColorButton.Parallel().TweenProperty(AwayColorButton, "position", new Vector2(originalPosAway.X, originalPosAway.Y), 0.25f)
            .SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Cubic);
            MoveColorButton.Parallel().TweenProperty(AwayColorButton, "size", new Vector2(originalSizeAway.X, originalSizeAway.Y), 0.25f)
            .SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Cubic);

            Components.Instance.Animations.ResetToNeutralPosition();
        }
    }

    private void UpdateActivePlayer()
    {
        var turnManager = Components.Instance.TurnManager;
        var customizePlayers = turnManager?.GetCustomizePlayers();

        if (customizePlayers.HomePlayerName.MouseFilter == Control.MouseFilterEnum.Stop)
        {
            currentActivePlayer = Player.Player1;
        }
        else if (customizePlayers.AwayPlayerName.MouseFilter == Control.MouseFilterEnum.Stop)
        {
            currentActivePlayer = Player.Player2;
        }
    }

    private void ShowActivePlayerPanel()
    {
        if (Components.Instance?.Animations != null)
        {
            if (currentActivePlayer == Player.Player1)
            {
                Components.Instance.Animations.Player1Turn();
            }
            else
            {
                Components.Instance.Animations.Player2Turn();
            }
        }
    }

    private void ApplyColorToActivePlayer(Color color)
    {
        if (Components.Instance?.Animations == null) return;

        if (currentActivePlayer == Player.Player1)
        {
            ApplyColorToPlayer1Elements(color);
        }
        else
        {
            ApplyColorToPlayer2Elements(color);
        }
    }

    private void ApplyColorToPlayer1Elements(Color color)
    {
        player1Color = color;
        Player1Panel.SelfModulate = color.Darkened(0.3f);
        Player1PanelBackground.SelfModulate = color.Darkened(0f);

        ApplyColorToLineEdit(Components.Instance.CustomizePlayers.HomePlayerName, color);
    }

    private void ApplyColorToPlayer2Elements(Color color)
    {
        player2Color = color;
        Player2Panel.SelfModulate = color.Darkened(0.3f);
        Player2PanelBackground.SelfModulate = color.Darkened(0f);

        ApplyColorToLineEdit(Components.Instance.CustomizePlayers.AwayPlayerName, color);
    }

    private void ApplyColorToLineEdit(LineEdit lineEdit, Color color)
    {
        if (lineEdit == null) return;

        var existingStyleBox = lineEdit.GetThemeStylebox("normal") as StyleBoxFlat;
        StyleBoxFlat styleBox;

        styleBox = existingStyleBox.Duplicate() as StyleBoxFlat;

        styleBox.BgColor = color;
        styleBox.BorderColor = color.Lightened(0.2f);

        lineEdit.AddThemeStyleboxOverride("normal", styleBox);
        lineEdit.AddThemeStyleboxOverride("focus", styleBox);

        var luminance = (color.R * 0.299f + color.G * 0.587f + color.B * 0.114f);
        var textColor = luminance > 0.5f ? Colors.Black : Colors.White;
        lineEdit.AddThemeColorOverride("font_color", textColor);
    }

    public Player GetCurrentActivePlayer()
    {
        return currentActivePlayer;
    }

    public bool IsColorPickerActive()
    {
        return isColorPickerActive;
    }

    public Color GetPlayer1Color()
    {
        return player1Color;
    }

    public Color GetPlayer2Color()
    {
        return player2Color;
    }

    public Color GetCurrentPlayerColor()
    {
        return currentActivePlayer == Player.Player1 ? player1Color : player2Color;
    }
}
