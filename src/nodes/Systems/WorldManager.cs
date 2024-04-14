using System;
using System.Collections.Generic;
using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Objects;
using HalfNibbleGame.Objects.Level;

namespace HalfNibbleGame.Systems;

public class WorldManager : Node2D
{
    private static readonly Vector2 worldSize = new(320, 180);
    private const float minCameraDistanceToEdge = 48;

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

        if (OS.IsDebugBuild())
        {
            AddChild(new DebugHelpers());
        }
    }

    public void LoadLevel(PackedScene scene)
    {
        if (currentLevel != null)
        {
            RemoveChild(currentLevel);
        }

        currentLevel = scene.Instance<Level>();
        AddChild(currentLevel);
        resetLevel();
    }

    public override void _Process(float delta)
    {
        // TODO: play death animation
        if (host.Madness >= host.MadnessCap || Input.IsActionJustPressed("kill_player"))
        {
            resetLevel();
            return;
        }

        updateCameraPosition();
    }

    private void updateCameraPosition()
    {
        if (currentActor is null) return;
        if (currentActor == host)
        {
            camera.Position = host.Position;
            return;
        }
        if (currentActor is EldritchActor { CanSeeHost: false })
        {
            camera.Position = currentActor.Position;
            return;
        }

        var hostPosition = host.Position;
        var actorPosition = currentActor.Position;
        var difference = actorPosition - hostPosition;
        if (Mathf.Abs(difference.x) > worldSize.x - 2 * minCameraDistanceToEdge ||
            Mathf.Abs(difference.y) > worldSize.y - 2 * minCameraDistanceToEdge)
        {
            camera.Position = actorPosition;
            return;
        }

        var average = hostPosition + 0.5f * difference;
        camera.Position = average;
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

        GetTree().CallGroup(Constants.LevelStateGroup, nameof(IResettable.Reset));
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
        if (scene?.Instance() is not EldritchActor actor)
        {
            GD.PrintErr("Failed to instantiate summoned form.");
            return;
        }

        GetParent().AddChild(actor);
        summonedForms.Add(actor);

        Global.Services.Get<Host>().Madness += actor.SummoningMadness;

        actor.Reset(tile, currentLevel.TileMap);
        activateActor(actor);
    }

    public void TryReturnToHost()
    {
        if (currentActor is null || summonedForms.Count == 0) return;
        if (currentActor == host) throw new InvalidOperationException();
        if (currentActor != summonedForms[^1]) throw new InvalidOperationException();

        var targetActor = summonedForms.Count > 1 ? summonedForms[^2] : host;
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
