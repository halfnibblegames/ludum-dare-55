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
                new("Reach the portals to proceed.", Speaker.Clhubhukhapuslhamazarathoka),
                new("...", Speaker.Host),
            }, false),
            2 => new Cinematic(new List<Dialog>
            {
                new("This time around, you need to use your summoning powers to proceed.", Speaker.Clhubhukhapuslhamazarathoka),
                new("I believe you will need my help, human...", Speaker.Unknown),
                new("I literally said that on the last dialog box, do you really wanna make the play tester read more dialog?.", Speaker.Clhubhukhapuslhamazarathoka),
                new("Sheesh, such a bad mood. Hi, Human, my name is Imp. Summon me and I shall help you.", Speaker.Imp),
                new ("Posses me for too long, and pay the consequences MWAHAHAHA", Speaker.Imp),
                new("...", Speaker.Host),
                new("I was expecting a little bit more enthusiasm today tbh.", Speaker.Imp)
            }, false),
            3 => new Cinematic( new List<Dialog>
            {
                new("Seems like this portal is being blocked by Zortrabchlmilekozitna.", Speaker.Clhubhukhapuslhamazarathoka),
                new("He's had too much fun yesterday and now is showing up unnanounced. Get rid of the summoning circles.", Speaker.Clhubhukhapuslhamazarathoka),
                new("Do not make me touch these circles, though, they are bad news for my kind.", Speaker.Imp),
                new("I would not advise touching them even if you could, Zortrabchlmilekozitna is not very hygienic, even for a horror.", Speaker.Clhubhukhapuslhamazarathoka),
                new("...", Speaker.Host),
                new("Is he always like that?", Speaker.Imp),
                new("I know, right? Silent protagonist is like the worst trope.", Speaker.Clhubhukhapuslhamazarathoka),
            }, false),
            4 => new Cinematic( new List<Dialog>
            {
                new("*sigh* Someone forgot to erase the runes here. Stupid humans. Even a single rune is enough to summon parts of old ones.", Speaker.Clhubhukhapuslhamazarathoka),
                new("... parts?", Speaker.Imp),
                new("Do not leave any circle standing to proceed!", Speaker.Clhubhukhapuslhamazarathoka),
                new("...", Speaker.Host),
            }, false),
            6 => new Cinematic( new List<Dialog>
            {
                new("Are people still reading these?", Speaker.Imp),
                new("Oh, I doubt. That's the point where we can start making meta self-referential humour because people stopped caring.", Speaker.Clhubhukhapuslhamazarathoka),
                new("You broke the fourth wall like 4 levels ago.", Speaker.Imp),
                new("\u00af\\_(ツ)_/\u00af", Speaker.Clhubhukhapuslhamazarathoka),
                new("The devs really should've used a font that handles unicode better", Speaker.Imp),
                new("...", Speaker.Host),
            }, false),
            7 => new Cinematic( new List<Dialog>
            {
                new("Is this the part where you reveal you can speak, human?", Speaker.Imp),
                new("...", Speaker.Host),
                new("...", Speaker.Imp),
            }, false),
            8 => new Cinematic( new List<Dialog>
            {
                new("That is it. Final challenge.", Speaker.Clhubhukhapuslhamazarathoka),
                new("This game is significantly more polished than it should, given the amount of scope creep.", Speaker.Imp),
                new("// TODO: Finish last level dialog", Speaker.Unknown),
                new("I'm glad this is almost over.", Speaker.Imp),
                new("...", Speaker.Host),
            }, false),
            _ => null
        };
    }
}
