using Godot;
using HalfNibbleGame.Autoload;

namespace HalfNibbleGame;

public class LevelAttributes : Node
{
    [Export] public Vector2 StartTile;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Global.Services.ProvideInScene(this);
    }
}
