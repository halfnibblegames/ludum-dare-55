using Godot;
using HalfNibbleGame.Objects;

namespace HalfNibbleGame.Controls;

public class MadnessMeter : TextureRect
{
    private const int FullMadnessBarSize = 704;

    public override void _Process(float delta)
    {
        // TODO: Make this less disgusting by using events or some other way of observing the player.
        var madnessBar = GetNode<ColorRect>("../MadnessBar");
        var portrait = GetNode<TextureRect>("../PlayerPortrait");
        var player = GetNode<Player>("../ViewportContainer/Viewport/World/SandboxLevel/Player");
        Visible = portrait.Visible = madnessBar.Visible = player is not null;

        if (player is null)
            return;

        var madnessPercentage = player.Madness / Player.MaxMadness;
        madnessBar.RectSize = madnessBar.RectSize with { x = (int)(FullMadnessBarSize * madnessPercentage) };
        madnessBar.Color = player.CurrentSpirit?.Color ?? madnessBar.Color;
    }
}
