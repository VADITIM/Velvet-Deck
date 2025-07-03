using Godot;
using System;
using System.Data;

public partial class AnimatePlayerButtons : Control
{
    ButtonHandler ButtonHandler => Components.Instance?.ButtonHandler;
    
    [Export] public LineEdit PlayerRightNameInput;
    [Export] public LineEdit PlayerLeftNameInput;

    Vector2 leftNamePosition;
    Vector2 rightNamePosition;

    public override void _Ready()
    {
        rightNamePosition = PlayerRightNameInput.Position;
        leftNamePosition = PlayerLeftNameInput.Position;

        ButtonHandler.LeftPlayerColorButton.Position = new Vector2(ButtonHandler.leftColorButtonPosition.X - 560, ButtonHandler.leftColorButtonPosition.Y);
        ButtonHandler.RightPlayerColorButton.Position = new Vector2(ButtonHandler.leftColorButtonPosition.X + 1120, ButtonHandler.leftColorButtonPosition.Y);
        AnimateButtonsIn();

        PlayerLeftNameInput.Position = new Vector2(leftNamePosition.X, leftNamePosition.Y - 1000);
        PlayerRightNameInput.Position = new Vector2(rightNamePosition.X, rightNamePosition.Y - 1000);
        AnimateInputsIn();
    }

    public void AnimateButtonsIn()
    {
        ButtonLeftInitialAnim();
        ButtonRightInitialAnim();
    }

    public void AnimateInputsIn()
    {
        float animationSpeed = 1.5f;

        var InputLeftTween = CreateTween();
        InputLeftTween.Parallel().TweenProperty(PlayerLeftNameInput, "position", leftNamePosition, animationSpeed).SetDelay(.5f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);

        var InputRightTween = CreateTween();
        InputRightTween.Parallel().TweenProperty(PlayerRightNameInput, "position", rightNamePosition, animationSpeed).SetDelay(.8f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
    }

    public void ButtonLeftInitialAnim()
    {
        float animationSpeed = 1.35f;

        var LeftButtonTween = CreateTween();
        var leftButton = LeftButtonTween.Parallel().TweenProperty(ButtonHandler.LeftPlayerColorButton, "position:y", ButtonHandler.leftColorButtonPosition.Y - 40, animationSpeed).SetDelay(0).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
        LeftButtonTween.Parallel().TweenProperty(ButtonHandler.LeftPlayerColorButton, "scale", new Vector2(.7f, .7f), animationSpeed).SetDelay(0).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);

        LeftButtonTween.TweenCallback(Callable.From(() =>
        {
            var LeftButtonTween2 = CreateTween();
            LeftButtonTween2.Parallel().TweenProperty(ButtonHandler.LeftPlayerColorButton, "scale", new Vector2(1, 1), animationSpeed).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
            LeftButtonTween2.Parallel().TweenProperty(ButtonHandler.LeftPlayerColorButton, "position", ButtonHandler.leftColorButtonPosition, animationSpeed).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
        }));

    }

    public void ButtonRightInitialAnim()
    {
        float animationSpeed = 1.35f;

        var RightButtonTween = CreateTween();
        RightButtonTween.Parallel().TweenProperty(ButtonHandler.RightPlayerColorButton, "position:y", ButtonHandler.rightColorButtonPosition.Y - 40, animationSpeed).SetDelay(0).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
        RightButtonTween.Parallel().TweenProperty(ButtonHandler.RightPlayerColorButton, "scale", new Vector2(.7f, .7f), animationSpeed).SetDelay(0).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);


        RightButtonTween.TweenCallback(Callable.From(() =>
        {
            var RightButtonTween2 = CreateTween();
            RightButtonTween2.Parallel().TweenProperty(ButtonHandler.RightPlayerColorButton, "scale", new Vector2(1, 1), animationSpeed).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
            RightButtonTween2.Parallel().TweenProperty(ButtonHandler.RightPlayerColorButton, "position", ButtonHandler.rightColorButtonPosition, animationSpeed).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Elastic);
        }));
    }
}
