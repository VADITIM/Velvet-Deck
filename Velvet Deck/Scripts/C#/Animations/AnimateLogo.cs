using Godot;
using System;

public partial class AnimateLogo : Control
{
    [Export] TextureRect LogoTop;
    [Export] TextureRect LogoBot;
    Vector2 logoTopOriginalPos;
    Vector2 logoBotOriginalPos;
    Vector2 logoTopOffsetPosition = new Vector2(-680, 620);
    Vector2 logoOffsetPositionBot = new Vector2(1220, -80);

    public override void _Ready()
    {
        logoTopOriginalPos = LogoTop.Position;
        logoBotOriginalPos = LogoBot.Position;

        LogoTop.Position = logoTopOffsetPosition;
        LogoBot.Position = logoOffsetPositionBot;

        AnimateLogoIn();
    }

    public void AnimateLogoIn()
    {
        float animationSpeed = 1f;

        var logoTopTween = CreateTween();
        logoTopTween.Parallel().TweenProperty(LogoTop, "position", logoTopOriginalPos, animationSpeed)
            .SetDelay(.25f)
            .SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Elastic);

        var logoBotTween = CreateTween();
        logoBotTween.Parallel().TweenProperty(LogoBot, "position", logoBotOriginalPos, animationSpeed)
            .SetDelay(.25f)
            .SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Elastic);
    }
}
