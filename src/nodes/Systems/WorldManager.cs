using System;
using System.Collections.Generic;
using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Objects;

namespace HalfNibbleGame.Systems;

public class WorldManager : Node2D
{
    private readonly List<Actor> summonedForms = new();

    private Camera2D camera = null!;
    private Host host = null!;
    private Actor? currentActor;
    private Level? currentLevel;

    public override void _Ready()
    {
        camera = Global.Services.Get<ShakeCamera>();
        host = Global.Services.Get<Host>();
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
        camera.Position = host.Position;
        camera.ResetSmoothing();
    }

    private void resetPlayerForLevel()
    {
        if (currentLevel is null) throw new InvalidOperationException();

        host.Reset(currentLevel.StartTile, currentLevel.TileMap);
        activateActor(host);
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

    public void TryReturnToHost()
    {
        if (currentActor is null || summonedForms.Count == 0) return;
        if (currentActor == host) throw new InvalidOperationException();
        if (currentActor != summonedForms[^1]) throw new InvalidOperationException();

        var targetActor = summonedForms.Count > 1 ? summonedForms[^2] : host;
        // TODO: check line of sight
        var oldActor = currentActor;
        activateActor(targetActor);
        oldActor.Banish();
        summonedForms.RemoveAt(summonedForms.Count - 1);
    }

    private void activateActor(Actor actor)
    {
        currentActor?.Suspend();
        actor.MakeActive();
        currentActor = actor;
    }
}
