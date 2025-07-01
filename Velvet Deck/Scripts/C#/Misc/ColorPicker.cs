using Godot;
using System;

public partial class ColorPicker : GridContainer
{
    [Export] public ColorRect ColorRect;
    [Export] public Button SelectButton;
    [Export] public Control ColorPickerContainer;

    [Export] public Button LeftPlayerColorButton;
    [Export] public Button RightPlayerColorButton;

    [Export] public Panel LeftPlayerPanel;
    [Export] public Panel RightPlayerPanel;
    [Export] public Panel LeftPlayerForegroundPanel;
    [Export] public Panel RightPlayerForegroundPanel;

    private bool isColorPickerActive = false;
    private Player currentActivePlayer = Player.Player1;

    private Color leftPlayerColor = Colors.White;
    private Color rightPlayerColor = Colors.White;

    private Vector2 originalPosAway;
    private Vector2 originalSizeAway;

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

    private Color selectedColor = Colors.White;

    public override void _Ready()
    {

        originalPosAway = new Vector2(RightPlayerColorButton.Position.X, RightPlayerColorButton.Position.Y);
        originalSizeAway = RightPlayerColorButton.Size;

        SelectButton.Pressed += SetSelectedColors;
        LeftPlayerColorButton.Pressed += OnHomeColorButtonPressed;
        RightPlayerColorButton.Pressed += OnAwayColorButtonPressed;
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

    public void HomePressed()
    {
        GD.Print("lol");
    }

    public void OnHomeColorButtonPressed()
    {
        currentActivePlayer = Player.Player1;
        isColorPickerActive = !isColorPickerActive;

        if (isColorPickerActive)
        {
            LeftPlayerColorButton.ZIndex = 4;
            ShowActivePlayerPanel();
            RightPlayerColorButton.Visible = true;

            var normalStyleBox = LeftPlayerColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
            normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
            normalStyleBox.SetCornerRadiusAll(50);
            LeftPlayerColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);

            var hoverStyleBox = LeftPlayerColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
            hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
            hoverStyleBox.SetCornerRadiusAll(50);
            LeftPlayerColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);

            var focusStyleBox = LeftPlayerColorButton.GetThemeStylebox("focus") as StyleBoxFlat;
            focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;
            focusStyleBox.SetCornerRadiusAll(50);
            LeftPlayerColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);

            var MoveColorBox = CreateTween();
            MoveColorBox.TweenProperty(ColorPickerContainer, "position", new Vector2(0, 1060), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

            var MoveColorButton = CreateTween();
            MoveColorButton.Parallel().TweenProperty(LeftPlayerColorButton, "size", new Vector2(920f, originalSizeAway.Y), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

            MoveColorButton.TweenCallback(Callable.From(() =>
            {
                RightPlayerColorButton.Visible = false;
            }));
        }
        else
        {

            RightPlayerColorButton.Visible = true;
            var normalStyleBox = LeftPlayerColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
            normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
            normalStyleBox.CornerRadiusTopRight = 0;
            normalStyleBox.CornerRadiusBottomRight = 0;
            LeftPlayerColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);

            var hoverStyleBox = LeftPlayerColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
            hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
            hoverStyleBox.CornerRadiusTopRight = 0;
            hoverStyleBox.CornerRadiusBottomRight = 0;
            LeftPlayerColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);

            var focusStyleBox = LeftPlayerColorButton.GetThemeStylebox("focus") as StyleBoxFlat;
            focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;
            focusStyleBox.CornerRadiusTopRight = 0;
            focusStyleBox.CornerRadiusBottomRight = 0;
            LeftPlayerColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);

            var MoveColorBox = CreateTween();
            MoveColorBox.TweenProperty(ColorPickerContainer, "position", new Vector2(0, 2440), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

            var MoveColorButton = CreateTween();
            MoveColorButton.Parallel().TweenProperty(LeftPlayerColorButton, "size", new Vector2(originalSizeAway.X, originalSizeAway.Y), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

            MoveColorButton.TweenCallback(Callable.From(() =>
            {
                LeftPlayerColorButton.ZIndex = 3;
                RightPlayerColorButton.Disabled = false;
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
            RightPlayerColorButton.ZIndex = 4;
            ShowActivePlayerPanel();

            var normalStyleBox = RightPlayerColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
            normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
            normalStyleBox.SetCornerRadiusAll(50);
            RightPlayerColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);

            var hoverStyleBox = RightPlayerColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
            hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
            hoverStyleBox.SetCornerRadiusAll(50);
            RightPlayerColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);

            var focusStyleBox = RightPlayerColorButton.GetThemeStylebox("focus") as StyleBoxFlat;
            focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;
            focusStyleBox.SetCornerRadiusAll(50);
            RightPlayerColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);

            var MoveColorBox = CreateTween();
            MoveColorBox.TweenProperty(ColorPickerContainer, "position", new Vector2(0, 1060), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

            var MoveColorButton = CreateTween();
            MoveColorButton.Parallel().TweenProperty(RightPlayerColorButton, "position", new Vector2(80f, originalPosAway.Y), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
            MoveColorButton.Parallel().TweenProperty(RightPlayerColorButton, "size", new Vector2(920f, originalSizeAway.Y), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
        }
        else
        {
            RightPlayerColorButton.ZIndex = 3;

            var normalStyleBox = RightPlayerColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
            normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
            normalStyleBox.CornerRadiusTopLeft = 0;
            normalStyleBox.CornerRadiusBottomLeft = 0;
            RightPlayerColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);

            var hoverStyleBox = RightPlayerColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
            hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
            hoverStyleBox.CornerRadiusTopLeft = 0;
            hoverStyleBox.CornerRadiusBottomLeft = 0;
            RightPlayerColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);

            var focusStyleBox = RightPlayerColorButton.GetThemeStylebox("focus") as StyleBoxFlat;
            focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;
            focusStyleBox.CornerRadiusTopLeft = 0;
            focusStyleBox.CornerRadiusBottomLeft = 0;
            RightPlayerColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);

            var MoveColorBox = CreateTween();
            MoveColorBox.TweenProperty(ColorPickerContainer, "position", new Vector2(0, 2440), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

            var MoveColorButton = CreateTween();
            MoveColorButton.Parallel().TweenProperty(RightPlayerColorButton, "position", new Vector2(originalPosAway.X, originalPosAway.Y), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
            MoveColorButton.Parallel().TweenProperty(RightPlayerColorButton, "size", new Vector2(originalSizeAway.X, originalSizeAway.Y), 0.25f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

            MoveColorButton.TweenCallback(Callable.From(() =>
            {
                RightPlayerColorButton.ZIndex = 3;
                LeftPlayerColorButton.Disabled = false;
            }));

            Components.Instance.Animations.ResetToNeutralPosition();
        }
    }

    private void UpdateActivePlayer()
    {
        var turnManager = Components.Instance.TurnManager;
        var customizePlayers = turnManager?.GetCustomizePlayers();

        if (customizePlayers.LeftPlayerNameEdit.MouseFilter == Control.MouseFilterEnum.Stop)
        {
            currentActivePlayer = Player.Player1;
        }
        else if (customizePlayers.RightPlayerNameEdit.MouseFilter == Control.MouseFilterEnum.Stop)
        {
            currentActivePlayer = Player.Player2;
        }
    }

    private void ShowActivePlayerPanel()
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

    private void ApplyColorToActivePlayer(Color color)
    {
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
        leftPlayerColor = color;
        LeftPlayerPanel.SelfModulate = color.Darkened(0.3f);
        LeftPlayerForegroundPanel.SelfModulate = color.Darkened(0f);

        ApplyColorToLineEdit(Components.Instance.CustomizePlayers.LeftPlayerNameEdit, color);
        ApplyColorToHomeButton(color);
    }

    private void ApplyColorToPlayer2Elements(Color color)
    {
        rightPlayerColor = color;
        RightPlayerPanel.SelfModulate = color.Darkened(0.3f);
        RightPlayerForegroundPanel.SelfModulate = color.Darkened(0f);

        ApplyColorToLineEdit(Components.Instance.CustomizePlayers.RightPlayerNameEdit, color);
        ApplyColorToAwayButton(color);
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

    private void ApplyColorToHomeButton(Color color)
    {
        var normalStyleBox = LeftPlayerColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
        var hoverStyleBox = LeftPlayerColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
        var focusStyleBox = LeftPlayerColorButton.GetThemeStylebox("focus") as StyleBoxFlat;

        normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
        normalStyleBox.BgColor = color;
        normalStyleBox.BorderColor = color.Lightened(0.2f);
        LeftPlayerColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);

        hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
        hoverStyleBox.BgColor = color.Lightened(0.1f);
        hoverStyleBox.BorderColor = color.Lightened(0.3f);
        LeftPlayerColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);

        focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;
        focusStyleBox.BgColor = color;
        focusStyleBox.BorderColor = color.Lightened(0.2f);
        LeftPlayerColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);

        var luminance = (color.R * 0.299f + color.G * 0.587f + color.B * 0.114f);
        var textColor = luminance > 0.5f ? Colors.Black : Colors.White;
        LeftPlayerColorButton.AddThemeColorOverride("font_color", textColor);
    }

    private void ApplyColorToAwayButton(Color color)
    {
        var normalStyleBox = RightPlayerColorButton.GetThemeStylebox("normal") as StyleBoxFlat;
        var hoverStyleBox = RightPlayerColorButton.GetThemeStylebox("hover") as StyleBoxFlat;
        var focusStyleBox = RightPlayerColorButton.GetThemeStylebox("focus") as StyleBoxFlat;

        normalStyleBox = normalStyleBox.Duplicate() as StyleBoxFlat;
        normalStyleBox.BgColor = color;
        normalStyleBox.BorderColor = color.Lightened(0.2f);
        RightPlayerColorButton.AddThemeStyleboxOverride("normal", normalStyleBox);

        hoverStyleBox = hoverStyleBox.Duplicate() as StyleBoxFlat;
        hoverStyleBox.BgColor = color.Lightened(0.1f);
        hoverStyleBox.BorderColor = color.Lightened(0.3f);
        RightPlayerColorButton.AddThemeStyleboxOverride("hover", hoverStyleBox);

        focusStyleBox = focusStyleBox.Duplicate() as StyleBoxFlat;
        focusStyleBox.BgColor = color;
        focusStyleBox.BorderColor = color.Lightened(0.2f);
        RightPlayerColorButton.AddThemeStyleboxOverride("focus", focusStyleBox);

        var luminance = (color.R * 0.299f + color.G * 0.587f + color.B * 0.114f);
        var textColor = luminance > 0.5f ? Colors.Black : Colors.White;
        RightPlayerColorButton.AddThemeColorOverride("font_color", textColor);
    }

    public Player GetCurrentActivePlayer() { return currentActivePlayer; }

    public bool IsColorPickerActive() { return isColorPickerActive; }

    public Color GetPlayer1Color() { return leftPlayerColor; }

    public Color GetPlayer2Color() { return rightPlayerColor; }

    public Color GetCurrentPlayerColor() { return currentActivePlayer == Player.Player1 ? leftPlayerColor : rightPlayerColor; }
}
