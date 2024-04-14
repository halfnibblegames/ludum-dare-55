using System.Collections.Generic;
using Godot;

namespace HalfNibbleGame;

public class Level : Node2D
{
    public TileMap TileMap { get; private set; } = null!;

    [Export] public Vector2 StartTile;
    [Export] public int LevelIdentifier;

    public override void _Ready()
    {
        TileMap = GetNode<TileMap>("Floor");
    }
}

public static class LevelExtensions
{
    public static Cinematic? GetCinematic(this Level level)
    {
        return level.LevelIdentifier switch
        {
            1 => new Cinematic(new List<Dialog>
            {
                new("Welcome, human vessel. I am clhubhukhapuslhamazarathoka, the eldritch horror in charge of this facility. My friends call be clhubs.", Speaker.Unknown),
                new("You can call me clhubhukhapuslhamazarathoka, for I have absolutely no respect for you puny mortals and will not allow insubordination.", Speaker.Clhubhukhapuslhamazarathoka),
                new("Today is your last day in cultist academy. Should you pass the trials ahead of you, it means you will serve as a good host for my kin.", Speaker.Clhubhukhapuslhamazarathoka)
            }, false),
            _ => null
        };
    }
}
