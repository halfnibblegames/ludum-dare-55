using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Objects;

public abstract class EldritchActor : Actor
{
    private Host host = null!;
    protected float MadnessPerMinute = 10;
    public float SummoningMadness = 20;

    public override void _Ready()
    {
        base._Ready();
        host = Global.Services.Get<Host>();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (!IsActive) return;
        
        host.Madness += MadnessPerMinute * delta;
        if (Input.IsActionJustPressed("return_to_host"))
        {
            Global.Services.Get<WorldManager>().TryReturnToHost();
        }
    }
}
