using Godot;
using JetBrains.Annotations;

namespace HalfNibbleGame.Objects.Level;

public class Door : StaticBody2D, IResettable
{
    [Export] public DoorState InitialState = DoorState.Closed;
    private DoorState state;

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(Constants.LevelStateGroup);
        Reset();
    }

    public void Reset()
    {
        state = InitialState;
        applyStateToCollision();
    }

    [UsedImplicitly]
    public void ToggleDoorState(SwitchState switchState)
    {
        state = state == DoorState.Closed ? DoorState.Open : DoorState.Closed;
        applyStateToCollision();
    }

    private void applyStateToCollision()
    {
        var halfTransparent = new Color(Colors.White, 0.4f);
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = state == DoorState.Open;
        GetNode<ColorRect>("ColorRect").Modulate = state == DoorState.Open ? halfTransparent : Colors.White;
    }
}

public enum DoorState
{
    Open,
    Closed
}
