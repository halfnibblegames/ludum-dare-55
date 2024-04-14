using Godot;

namespace HalfNibbleGame.Objects.Level;

public abstract class DetectingArea2D : Area2D, ILevelResettable
{
    private bool signalsConnected;
    private Actor? currentActor;

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(Constants.LevelResetGroup);
    }

    public void Reset()
    {
        if (signalsConnected) return;
        Connect("body_entered", this, nameof(onBodyEntered));
        Connect("body_exited", this, nameof(onBodyExited));
        signalsConnected = true;
    }

    private void onBodyEntered(PhysicsBody2D body)
    {
        if (body is Actor { HasControl: true } actor)
        {
            OnActorEntered(actor);
            currentActor = actor;
        }
    }

    private void onBodyExited(PhysicsBody2D body)
    {
        if (body == currentActor)
        {
            OnActorExited(currentActor);
            currentActor = null;
        }
    }

    protected abstract void OnActorEntered(Actor actor);
    protected virtual void OnActorExited(Actor actor) { }
}
