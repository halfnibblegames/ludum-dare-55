using Godot;

namespace HalfNibbleGame;

public class Level : Node2D
{
    public TileMap TileMap { get; private set; } = null!;

    [Export] public Vector2 StartTile;

    public override void _Ready()
    {
        TileMap = GetNode<TileMap>("Floor");
    }
}
