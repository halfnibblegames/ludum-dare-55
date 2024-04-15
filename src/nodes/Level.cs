using System.Collections.Generic;
using Godot;
using HalfNibbleGame.Controls;

namespace HalfNibbleGame;

public class Level : Node2D
{
    public TileMap TileMap { get; private set; } = null!;

    [Export] public Vector2 StartTile;
    [Export] public int LevelIdentifier;
    [Export] public bool CanSummonImp = true;

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
                new("Today is your last day in cultist academy. Should you pass the trials ahead of you, it means you will serve as a good host for my kin.", Speaker.Clhubhukhapuslhamazarathoka),
                new("...", Speaker.Host),
            }, false),
            2 => new Cinematic(new List<Dialog>
            {
                new("I believe you will need my help...", Speaker.Unknown),
                new("My name is Angus. Summon me and I shall help you.", Speaker.Imp),
                new ("Posses me for too long, and pay the consequences MWAHAHAHA", Speaker.Imp),
                new("...", Speaker.Host),
                new("I was expecting a little bit more enthusiasm, but I'll work with what I have.", Speaker.Imp)
            }, false),
            3 => new Cinematic( new List<Dialog>
            {
                new("Seems like your body is perfectly capable of controlling another entities. Good. This is...", Speaker.Clhubhukhapuslhamazarathoka),
                new("BTW, I'm small enough that I can walk across trapdoors without activating them", Speaker.Imp),
                new("Do not, ever, do expository dialog while I'm monologuing, inferior being.", Speaker.Clhubhukhapuslhamazarathoka),
                new("Don't go meta on me, these dialogs are meant to introduce each level's new mechanic.", Speaker.Imp),
                new("...", Speaker.Host),
                new("Is he always like that?", Speaker.Imp),
                new("I know, right? Silent protagonist is like the worst trope.", Speaker.Clhubhukhapuslhamazarathoka),
            }, false),
            _ => null
        };
    }
}
