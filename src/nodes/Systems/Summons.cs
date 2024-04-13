using System;
using System.Collections.Generic;
using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Objects;

namespace HalfNibbleGame.Systems;

public class Summons : Node2D
{
    private readonly List<Actor> summonedForms = new();

    private Player player = null!;
    private Actor? currentActor;
    private Level? currentLevel;

    public override void _Ready()
    {
        player = Global.Services.Get<Player>();
        currentLevel = GetNodeOrNull<Level>("SandboxLevel");

        Global.Services.ProvideInScene(this);
        if (currentLevel is not null) resetLevel();
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
        currentActor = null;

        resetPlayerForLevel();
    }

    private void resetPlayerForLevel()
    {
        if (currentLevel is null) throw new InvalidOperationException();

        player.Reset(currentLevel.StartTile, currentLevel.TileMap);
        activateActor(player);
    }

    public void SummonForm(Vector2 tile)
    {
        if (currentLevel is null) throw new InvalidOperationException();

        // TODO: allow other forms
        var scene = Global.Prefabs.Imp;
        if (scene?.Instance() is not Actor actor)
        {
            GD.PrintErr("Failed to instantiate summoned form.");
            return;
        }

        GetParent().AddChild(actor);
        summonedForms.Add(actor);

        actor.Reset(tile, currentLevel.TileMap);
        activateActor(actor);
    }

    private void activateActor(Actor actor)
    {
        currentActor?.Suspend();
        actor.MakeActive();
        currentActor = actor;
    }
}
