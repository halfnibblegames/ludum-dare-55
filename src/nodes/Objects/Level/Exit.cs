using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Objects.Level;

public class Exit : DetectingArea2D
{
    private const string openAnimation = "glow";

    [Export] public PackedScene? NextLevelScene;
    private AnimatedSprite animatedSprite = null!;

    public override void _Ready()
    {
        base._Ready();

        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        animatedSprite.Animation = openAnimation;
        animatedSprite.Play();
    }

    protected override void OnActorEntered(Actor actor)
    {
        if (actor is not Host) return;
        var nextLevel = NextLevelScene ?? Global.Prefabs.Sandbox;
        if (nextLevel is null) return;
        Global.Services.Get<WorldManager>().CallDeferred(nameof(WorldManager.LoadLevel), nextLevel);
    }
}
