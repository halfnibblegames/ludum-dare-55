using Godot;
using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Objects.Level;

public class PressurePlate : DetectingArea2D, ILevelState
{
    private const string unpressedAnimation = "idle";
    private const string pressedAnimation = "Active";

    [Export] public ChannelKey Channel;

    private ChannelState state;
    private AnimatedSprite sprite = null!;

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(Constants.LevelStateGroup);
        sprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    protected override void OnActorEntered(Actor actor)
    {
        if (actor is not Imp)
        {
            this.UpdateChannel(Channel, ChannelState.On);
            sprite.Animation = pressedAnimation;
        }
    }

    protected override void OnActorExited(Actor actor)
    {
        if (actor is not Imp)
        {
            this.UpdateChannel(Channel, ChannelState.Off);
            sprite.Animation = unpressedAnimation;
        }
    }

    public void ConsumeChange(ChannelKey key, ChannelState newState)
    {
        if (key != Channel) return;

        state = newState;
    }
}
