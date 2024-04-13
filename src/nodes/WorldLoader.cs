using System;
using System.Collections.Generic;
using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Objects;

namespace HalfNibbleGame;

public class WorldLoader : Node2D
{
    private readonly List<Actor> summonedForms = new();

    private Player player = null!;
    private Node? currentLevel;

    public override void _Ready()
    {
        player = Global.Services.Get<Player>();
        currentLevel = GetNodeOrNull<Node2D>("SandboxLevel");
        if (currentLevel is not null) resetPlayerForLevel();

        Global.Services.ProvideInScene(this);
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("kill_player"))
        {
            resetLevel();
        }
    }

    private void resetLevel()
    {
        foreach (var form in summonedForms)
        {
            form.Banish();
        }
        summonedForms.Clear();
    }

    private void resetPlayerForLevel()
    {
        if (currentLevel is null) throw new InvalidOperationException();

        var attributes = currentLevel.GetNode<LevelAttributes>("LevelAttributes");
        var tileMap = currentLevel.GetNode<LevelTileMap>("TileMap");
        player.Reset(attributes.StartTile, tileMap);
        player.MakeActive(true);
    }

    public void SummonForm(Vector2 tile)
    {
        if (currentLevel is null) throw new InvalidOperationException();

        // TODO: allow other forms
        var scene = Global.Prefabs.Imp;
        if (scene?.Instance() is not Actor instance)
        {
            GD.PrintErr("Failed to instantiate summoned form.");
            return;
        }

        GetParent().AddChild(instance);
        instance.Reset(tile, currentLevel.GetNode<LevelTileMap>("TileMap"));
        summonedForms.Add(instance);
    }
}
