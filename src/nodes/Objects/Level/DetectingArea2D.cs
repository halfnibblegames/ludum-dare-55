using Godot;

namespace HalfNibbleGame.Objects.Level;

public abstract class DetectingArea2D : Area2D
{
    public override void _Ready()
    {
        base._Ready();
        Connect("body_entered", this, nameof(onBodyEntered));
    }

    private void onBodyEntered(PhysicsBody2D body)
    {
        if (body is Actor { HasControl: true } actor)
        {
            OnActorEntered(actor);
        }
    }

    protected abstract void OnActorEntered(Actor actor);
}
