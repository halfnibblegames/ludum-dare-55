using System.Collections.Generic;
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
    public string Portrait = "res://assets/imp_portrait.png";

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

        if (!HasControl) return;

        if (Input.IsActionJustPressed("return_to_host"))
        {
            if (CanSeeHost)
            {
                Global.Services.Get<WorldManager>().TryReturnToHost();
            }
            else
            {
                Global.Services.Get<DialogService>().ShowDialog(
                    new List<Dialog>
                    {
                        new("Damn, I need to see my body to go back to it...", Speaker.Imp)
                    }
                );
            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        hostRay.CastTo = ToLocal(host.GlobalPosition);
    }
}
