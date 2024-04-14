using System.Collections.Generic;
using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Objects;

namespace HalfNibbleGame.Controls;

public class MadnessMeter : TextureRect
{
    private const int fullMadnessBarSize = 1060;
    private const float maxAberration = 5;
    private const float barAnimationDuration = 0.4f;
    private bool meterIsVisible;

    public override void _Process(float delta)
    {
        var madnessBar = GetNode<ColorRect>("../MadnessBar");
        var host = Global.Services.Get<Host>();
        var material = GetNode<ColorRect>("../CanvasLayer/PostProcess").Material as ShaderMaterial;

        ControlVisibility(host, madnessBar);

        var madnessPercentage = host.Madness / host.MadnessCap;
        madnessBar.RectSize = madnessBar.RectSize with { x = (int)(fullMadnessBarSize * madnessPercentage) };
        material?.SetShaderParam("amount", madnessPercentage * maxAberration);
    }

    private void ControlVisibility(Host host, ColorRect madnessBar)
    {
        var meterShouldBeVisible = !host.IsActive;
        if (meterShouldBeVisible == meterIsVisible)
            return;

        var tween = GetNode<Tween>("../Tween");
        var madnessMeter = GetNode<CanvasItem>("../MadnessMeter");

        var playerPortrait = GetNode<TextureRect>("../PlayerPortrait");

        // TODO: Figure out how to get the current eldritch actor.
        // var texture = new ImageTexture { ResourcePath = currentEldritchActor.Portrait };
        // playerPortrait.Texture = texture;

        tween.InterpolateProperty(
            @object: madnessMeter,
            property: "rect_position:y",
            initialVal: meterShouldBeVisible ? 720 : 616,
            finalVal: meterShouldBeVisible ? 616 : 720,
            duration: barAnimationDuration
        );

        tween.InterpolateProperty(
            @object: playerPortrait,
            property: "rect_position:y",
            initialVal: meterShouldBeVisible ? 724 : 620,
            finalVal: meterShouldBeVisible ? 620 : 724 ,
            duration: barAnimationDuration
        );

        tween.InterpolateProperty(
            @object: madnessBar,
            property: "rect_position:y",
            initialVal: meterShouldBeVisible ? 772 : 668,
            finalVal: meterShouldBeVisible ? 668 : 772,
            duration: barAnimationDuration
        );

        tween.Start();
        meterIsVisible = meterShouldBeVisible;
    }
}
