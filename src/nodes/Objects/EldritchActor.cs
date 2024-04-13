using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Objects;

public abstract class EldritchActor : Actor
{
    private Host host = null!;
    private RayCast2D hostRay = null!;
    protected float MadnessPerMinute = 10;
    public float SummoningMadness = 20;

    public bool CanSeeHost => !hostRay.IsColliding();

    public override void _Ready()
    {
        base._Ready();
        host = Global.Services.Get<Host>();
        hostRay = new RayCast2D
        {
            Enabled = true,
            ExcludeParent = true
        };
        hostRay.AddException(host);
        AddChild(hostRay);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (!IsActive) return;

        host.Madness += MadnessPerMinute * delta;
        if (Input.IsActionJustPressed("return_to_host"))
        {
            if (CanSeeHost)
            {
                Global.Services.Get<WorldManager>().TryReturnToHost();
            }
            else
            {
                GD.Print("Attempted to return to host, but host not visible");
            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        hostRay.CastTo = ToLocal(host.GlobalPosition);
        //
        // var space = GetWorld2d().DirectSpaceState;
        // var results = space.IntersectRay(Position, ToLocal(host.GlobalPosition), new Array(this));
        // canSeeHost = results.Count == 0;
    }
}
