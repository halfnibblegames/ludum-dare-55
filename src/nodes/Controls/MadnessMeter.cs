using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Objects;

namespace HalfNibbleGame.Controls;

public class MadnessMeter : TextureRect
{
    private const int fullMadnessBarSize = 704;
    private const float maxAberration = 5;

    public override void _Process(float delta)
    {
        // TODO: Make this less disgusting by using events or some other way of observing the player.
        var madnessBar = GetNode<ColorRect>("../MadnessBar");
        var host = Global.Services.Get<Host>();
        var material = GetNode<ColorRect>("../CanvasLayer/PostProcess").Material as ShaderMaterial;

        var madnessPercentage = host.Madness / host.MadnessCap;

        madnessBar.RectSize = madnessBar.RectSize with { x = (int)(fullMadnessBarSize * madnessPercentage) };

        material?.SetShaderParam("amount", madnessPercentage * maxAberration);
    }
}
