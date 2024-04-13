using System;
using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Objects;

namespace HalfNibbleGame;

public class WorldLoader : Node2D
{
    private Player player = null!;
    private Node? currentLevel;

    public override void _Ready()
    {
        player = Global.Services.Get<Player>();
        currentLevel = GetNodeOrNull<Node2D>("SandboxLevel");
        if (currentLevel is not null) resetPlayerForLevel();
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("kill_player"))
        {
            resetPlayerForLevel();
        }
    }

    private void resetPlayerForLevel()
    {
        if (currentLevel is null) throw new InvalidOperationException();

        var attributes = currentLevel.GetNode<LevelAttributes>("LevelAttributes");
        var tileMap = currentLevel.GetNode<LevelTileMap>("TileMap");
        player.Reset(attributes.StartTile, tileMap);
        player.MakeActive(true);
    }
}
