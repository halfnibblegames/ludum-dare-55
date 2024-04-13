using System;
using System.Collections.Generic;
using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Objects;

namespace HalfNibbleGame.Systems;

public class Summons : Node2D
{
    private readonly List<Actor> summonedForms = new();

    private Camera2D camera = null!;
    private Player player = null!;
    private Actor? currentActor;
    private Level? currentLevel;

    public override void _Ready()
    {
        camera = Global.Services.Get<ShakeCamera>();
        player = Global.Services.Get<Player>();
        currentLevel = GetNodeOrNull<Level>("SandboxLevel");

        Global.Services.ProvideInScene(this);
        if (currentLevel is not null) resetLevel();
    }

    public override void _Process(float delta)
    {
        camera.Position = currentActor?.Position ?? camera.Position;

        if (Input.IsActionJustPressed("kill_player"))
        {
            resetLevel();
        }
    }

    private void resetLevel()
    {
        if (currentLevel is null) throw new InvalidOperationException();

        foreach (var form in summonedForms)
        {
            form.Banish();
        }
        summonedForms.Clear();
        currentActor = null;

        camera.LimitLeft = (int) currentLevel.TileMap.GetUsedRect().Position.x;
        camera.LimitTop = (int) currentLevel.TileMap.GetUsedRect().Position.y;
        camera.LimitRight = (int) (currentLevel.TileMap.GetUsedRect().End.x * currentLevel.TileMap.CellSize.x);
        camera.LimitBottom = (int) (currentLevel.TileMap.GetUsedRect().End.y * currentLevel.TileMap.CellSize.y);

        resetPlayerForLevel();
        camera.ResetSmoothing();
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
        reparentCamera(actor);
    }

    private void reparentCamera(Actor newParent)
    {
        // var currParent = camera.GetParent();
        // currParent.RemoveChild(camera);
        // newParent.AddChild(camera);
    }
}
