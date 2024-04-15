using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Objects;

namespace HalfNibbleGame.Controls;

public class MadnessMeter : TextureRect
{
    private const int fullMadnessBarSize = 1060;
    private const float maxAberration = 8;
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

        var bgmBus = AudioServer.GetBusIndex("BGM");
        var lowPass = AudioServer.GetBusEffect(bgmBus, 0) as AudioEffectLowPassFilter;
        if (lowPass is null)
        {
            GD.PushWarning("Low pass filter not found on BGM bus.");
            return;
        }
        lowPass.CutoffHz = 11000 - (madnessPercentage * 10000);
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
            initialVal: meterShouldBeVisible ? -104 : 16,
            finalVal: meterShouldBeVisible ? 16 : -104,
            duration: barAnimationDuration
        );

        tween.InterpolateProperty(
            @object: playerPortrait,
            property: "rect_position:y",
            initialVal: meterShouldBeVisible ? -108 : 20,
            finalVal: meterShouldBeVisible ? 20 : -108 ,
            duration: barAnimationDuration
        );

        tween.InterpolateProperty(
            @object: madnessBar,
            property: "rect_position:y",
            initialVal: meterShouldBeVisible ? -156 : 68,
            finalVal: meterShouldBeVisible ? 68 : -156,
            duration: barAnimationDuration
        );

        tween.Start();
        meterIsVisible = meterShouldBeVisible;
    }
}
