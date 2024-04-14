using Godot;

namespace HalfNibbleGame.Objects.Level;

public class Switch : StaticBody2D
{
    [Export] public SwitchState State = SwitchState.Off;

    [Signal] public delegate void SwitchStateChanged(SwitchState newState);

    public override void _Ready()
    {
        base._Ready();
        this.MakeInteractable(this, nameof(onActivated));
    }

    private void onActivated(Actor actor)
    {
        State = State == SwitchState.Off ? SwitchState.On : SwitchState.Off;
        EmitSignal(nameof(SwitchStateChanged), State);
    }
}

public enum SwitchState
{
    Off,
    On
}
