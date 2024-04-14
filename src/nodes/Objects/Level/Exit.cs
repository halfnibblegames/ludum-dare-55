using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Objects.Level;

public class Exit : DetectingArea2D
{
    [Export] public PackedScene? NextLevelScene;

    protected override void OnActorEntered(Actor actor)
    {
        if (actor is not Host) return;
        var nextLevel = NextLevelScene ?? Global.Prefabs.Sandbox;
        if (nextLevel is null) return;
        Global.Services.Get<WorldManager>().CallDeferred(nameof(WorldManager.LoadLevel), nextLevel);
    }
}
