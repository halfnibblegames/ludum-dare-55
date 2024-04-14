using Godot;

namespace HalfNibbleGame.Objects.Level;

public class Switch : StaticBody2D, IResettable
{
    [Export] public SwitchState InitialState = SwitchState.Off;
    private SwitchState state;

    [Signal] public delegate void SwitchStateChanged(SwitchState newState);

    public override void _Ready()
    {
        base._Ready();
        this.MakeInteractable(this, nameof(onActivated));
        AddToGroup(Constants.LevelStateGroup);
        Reset();
    }

    public void Reset()
    {
        state = InitialState;
    }

    private void onActivated(Actor actor)
    {
        state = state == SwitchState.Off ? SwitchState.On : SwitchState.Off;
        EmitSignal(nameof(SwitchStateChanged), state);
    }
}

public enum SwitchState
{
    Off,
    On
}
