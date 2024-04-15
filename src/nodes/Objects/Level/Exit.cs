using System.Security.Cryptography;
using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Objects.Level;

public class Exit : DetectingArea2D, ILevelState
{
    private const string openAnimation = "glow";
    private const string closedAnimation = "tentacles";

    [Export] public PackedScene? NextLevelScene;
    [Export] public int RadiantSealsNeeded;
    public bool IsOpen => numberOfDismissedRadiantSeals >= RadiantSealsNeeded;

    private AnimatedSprite animatedSprite = null!;
    private CollisionShape2D tentacleCollision = null!;

    private int numberOfDismissedRadiantSeals;

    public override void _Ready()
    {
        base._Ready();

        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        tentacleCollision = GetNode<CollisionShape2D>("TentacleBody/TentacleCollision");
        animatedSprite.Play();
        AddToGroup(Constants.LevelStateGroup);
        AddToGroup(Constants.LevelResetGroup);

        updateOpenState();
    }

    public override void Reset()
    {
        base.Reset();

        numberOfDismissedRadiantSeals = 0;
        updateOpenState();
    }

    protected override void OnActorEntered(Actor actor)
    {
        if (actor is not Host) return;
        if (!IsOpen) return;
        
        var nextLevel = NextLevelScene ?? Global.Prefabs.Sandbox;
        if (nextLevel is null) return;
        Global.Services.Get<WorldManager>().CallDeferred(nameof(WorldManager.LoadLevel), nextLevel);
    }

    public void ConsumeChange(ChannelKey key, ChannelState newState)
    {
        if (key != ChannelKey.Radiant) return;
        numberOfDismissedRadiantSeals += newState == ChannelState.Off ? 1 : -1;
        updateOpenState();
    }

    private void updateOpenState()
    {
        animatedSprite.Animation = IsOpen ? openAnimation : closedAnimation;
        tentacleCollision.Disabled = IsOpen;
    }
}
