using Godot;
using JetBrains.Annotations;

namespace HalfNibbleGame.Objects.Level;

public class Door : StaticBody2D
{
    [Export] public DoorState State = DoorState.Closed;

    public override void _Ready()
    {
        base._Ready();
        applyStateToCollision();
    }

    [UsedImplicitly]
    public void ToggleDoorState(SwitchState switchState)
    {
        State = State == DoorState.Closed ? DoorState.Open : DoorState.Closed;
        applyStateToCollision();
    }

    private void applyStateToCollision()
    {
        var halfTransparent = new Color(Colors.White, 0.4f);
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = State == DoorState.Open;
        GetNode<ColorRect>("ColorRect").Modulate = State == DoorState.Open ? halfTransparent : Colors.White;
    }
}

public enum DoorState
{
    Open,
    Closed
}
