using Godot;
using HalfNibbleGame.Autoload;

namespace HalfNibbleGame;

public class LevelAttributes : Node
{
    [Export] public Vector2 StartTile;

    public override void _Ready()
    {
        Global.Services.ProvideInScene(this);
    }
}
