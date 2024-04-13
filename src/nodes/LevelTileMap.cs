using Godot;
using HalfNibbleGame.Autoload;

namespace HalfNibbleGame;

public class LevelTileMap : TileMap
{
    public override void _Ready()
    {
        Global.Services.ProvideInScene(this);
    }
}
