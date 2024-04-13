using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Objects;

public abstract class EldritchActor : Actor
{
    private Host host = null!;

    public override void _Ready()
    {
        base._Ready();
        host = Global.Services.Get<Host>();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (!IsActive) return;

        if (Input.IsActionJustPressed("return_to_host"))
        {
            Global.Services.Get<WorldManager>().TryReturnToHost();
        }
    }
}
