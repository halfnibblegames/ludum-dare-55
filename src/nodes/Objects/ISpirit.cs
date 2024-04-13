using Godot;

namespace HalfNibbleGame.Objects;

public interface ISpirit
{
    float InitialMadness { get; }
    float MadnessPerSecond { get; }
    Color Color { get; }

    void Begin(Player player);
    void End(Player player);
}
