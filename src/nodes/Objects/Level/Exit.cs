using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Objects.Level;

public class Exit : DetectingArea2D
{
    [Export] public PackedScene NextLevelScene = null!;

    protected override void OnActorEntered(Actor actor)
    {
        if (actor is Host)
        {
            Global.Services.Get<WorldManager>().CallDeferred(nameof(WorldManager.LoadLevel), NextLevelScene);
        }
    }
}
