using Godot;
using HalfNibbleGame.Autoload;

namespace HalfNibbleGame.Objects;

public class Player : Actor
{
    public const double MaxMadness = 200;

    [Export] private float speed = 100;

    protected override float Speed => speed;

    public float Madness { get; private set; }
    public ISpirit? CurrentSpirit { get; private set; }

    public override void _Ready()
    {
        Global.Services.ProvideInScene(this);
    }

    protected override void OnReset()
    {
        base.OnReset();
        Madness = 0;
    }

    public override void _Process(float delta)
    {
        Madness += delta * (CurrentSpirit?.MadnessPerSecond ?? 0);

        if (!Input.IsActionJustPressed("summon_spirit")) return;

        if (CurrentSpirit is null)
        {
            CurrentSpirit = new PhaseSpirit();
            Madness += CurrentSpirit.InitialMadness;
            CurrentSpirit.Begin(this);
        }
        else
        {
            CurrentSpirit.End(this);
            CurrentSpirit = null;
        }
    }
}
