using Godot;

namespace HalfNibbleGame.Objects.Level;

public class RadiantAnomaly : StaticBody2D, ILevelResettable
{
    private AnimatedSprite sprite = null!;
    private CollisionShape2D collisionShape = null!;

    public override void _Ready()
    {
        base._Ready();
        this.MakeInteractable(this, nameof(onActivated));
        AddToGroup(Constants.LevelResetGroup);
        sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
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
        if (actor is not Host || collisionShape.Disabled) return;

        collisionShape.Disabled = true;
        sprite.Hide();
    }
}
