using Godot;
using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Objects.Level;

public class Door : StaticBody2D, ILevelState
{
    private const string openAnimation = "opened";
    private const string closeAnimation = "closed";

    [Export] public ChannelKey Channel;
    [Export] public DoorState InitialState = DoorState.Closed;
    private DoorState state;

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(Constants.LevelStateGroup);

        state = InitialState;
        applyStateToCollision();
    }

    public void ConsumeChange(ChannelKey key, ChannelState newState)
    {
        if (key != Channel) return;

        state = toDoorState(newState);
        applyStateToCollision();
    }

    private DoorState toDoorState(ChannelState channelState)
    {
        if (InitialState == DoorState.Closed)
        {
            return channelState == ChannelState.On ? DoorState.Open : DoorState.Closed;
        }

        return channelState == ChannelState.On ? DoorState.Closed : DoorState.Open;
    }

    private void applyStateToCollision()
    {
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = state == DoorState.Open;
        GetNode<AnimatedSprite>("AnimatedSprite").Animation = state == DoorState.Open ? openAnimation : closeAnimation;
    }
}

public enum DoorState
{
    Open,
    Closed
}
