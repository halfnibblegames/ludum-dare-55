using Godot;

namespace HalfNibbleGame.Objects;

public class PhaseSpirit : ISpirit
{
    public float InitialMadness { get; } = 20;
    public float MadnessPerSecond { get; } = 10;
    public Color Color { get; } = Colors.Aqua;

    private Color oldColor;

    public void Begin(Player player)
    {
        var colorRect = player.GetNode<ColorRect>("ColorRect");
        oldColor = colorRect.Color;
        colorRect.Color = Color;
    }

    public void End(Player player)
    {
        player.GetNode<ColorRect>("ColorRect").Color = oldColor;
    }
}
