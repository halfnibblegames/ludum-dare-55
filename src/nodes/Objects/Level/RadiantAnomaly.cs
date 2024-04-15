using Godot;
using HalfNibbleGame.Systems;
using JetBrains.Annotations;

namespace HalfNibbleGame.Objects.Level;

public class RadiantAnomaly : StaticBody2D, ILevelResettable
{
    private AnimatedSprite sprite = null!;
    private CollisionShape2D collisionShape = null!;

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        this.MakeInteractable(this, nameof(onActivated), actor => actor is Host && !collisionShape.Disabled);
        AddToGroup(Constants.LevelResetGroup);
        Reset();
    }

    public void Reset()
    {
        collisionShape.Disabled = false;
        sprite.Show();
        sprite.Play();
    }

    private void onActivated(Actor actor)
    {
        if (actor is not Host host || collisionShape.Disabled) return;

        host.IsDismissingSeal = true;
        host.BlockControl();

        collisionShape.Disabled = true;
        sprite.Animation = "broken";
        GetNode<AudioStreamPlayer>("SFX").Play();
    }

    [UsedImplicitly]
    public void OnAnimationFinished()
    {
        if (sprite.Animation == "broken")
        {
            GetNode<AudioStreamPlayer>("SFX").Stop();
            sprite.Hide();
            sprite.Animation = "idle";

            this.UpdateChannel(ChannelKey.Radiant, ChannelState.Off);
        }
    }
}
