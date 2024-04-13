namespace HalfNibbleGame.Objects;

public interface ISpirit
{
    float InitialMadness { get; }
    float MadnessPerSecond { get; }

    void Begin(Player player);
    void End(Player player);
}
