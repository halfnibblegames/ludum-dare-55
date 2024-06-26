﻿using System;
using System.Collections.Generic;
using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Controls;
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

    [Signal] public delegate void LevelReset();
    [Signal] public delegate void LevelCompleted();
    [Signal] public delegate void GameFinished();

    [Signal] public delegate void PlayedDied();

    [Signal] public delegate void SummonStarted();
    [Signal] public delegate void SummonEnded();

    public bool CanSummon => currentLevel?.CanSummonImp ?? false;

    public override void _Ready()
    {
        camera = Global.Services.Get<ShakeCamera>();
        host = Global.Services.Get<Host>();

        Global.Services.ProvideInScene(this);

        if (OS.IsDebugBuild())
        {
            AddChild(new DebugHelpers());
        }

        CallDeferred(nameof(loadFirstLevel));
    }

    private void loadFirstLevel()
    {
        LoadLevel(Global.Prefabs.FirstLevel!);
    }

    public void KillPlayer()
    {
        host.Boom();
        resetLevel();
    }

    public void LoadLevel(PackedScene scene)
    {
        if (currentLevel != null)
        {
            RemoveChild(currentLevel);
            EmitSignal(nameof(LevelCompleted));
        }

        currentLevel = scene.Instance<Level>();
        AddChild(currentLevel);
        resetLevel();

        var cinematic = currentLevel.GetCinematic();
        if (cinematic is not null)
        {
            Global.Services.Get<DialogService>().ShowCinematic(cinematic);
        }
    }

    public override void _Process(float delta)
    {
        if (host.Madness >= host.MadnessCap || Input.IsActionJustPressed("kill_player"))
        {
            EmitSignal(nameof(PlayedDied));
            KillPlayer();
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
        if (Global.Services.TryGet<DialogService>(out var dialogService))
        {
            dialogService.ClearAllDialog();
        }

        camera.LimitLeft = (int) currentLevel.TileMap.GetUsedRect().Position.x;
        camera.LimitTop = (int) currentLevel.TileMap.GetUsedRect().Position.y;
        camera.LimitRight = (int) (currentLevel.TileMap.GetUsedRect().End.x * currentLevel.TileMap.CellSize.x);
        camera.LimitBottom = (int) (currentLevel.TileMap.GetUsedRect().End.y * currentLevel.TileMap.CellSize.y);

        resetPlayerForLevel();
        camera.Position = host.Position;
        camera.ResetSmoothing();

        foreach (var key in Enum.GetValues(typeof(ChannelKey)))
        {
            this.UpdateChannel((ChannelKey) key, ChannelState.Off);
        }
        GetTree().CallGroup(Constants.LevelResetGroup, nameof(ILevelResettable.Reset));
        EmitSignal(nameof(LevelReset));
    }

    private void resetPlayerForLevel()
    {
        if (currentLevel is null) throw new InvalidOperationException();

        var startLocation = currentLevel.TileMap.MapToWorld(currentLevel.StartTile);
        var globalPos = currentLevel.TileMap.ToGlobal(startLocation);
        host.Reset(globalPos);
        activateActor(host);
    }

    public bool SummonForm(Vector2 location)
    {
        if (currentLevel is null) throw new InvalidOperationException();
        if (!currentLevel.CanSummonImp) return false;

        var scene = Global.Prefabs.Imp;
        if (scene?.Instance() is not EldritchActor actor)
        {
            GD.PrintErr("Failed to instantiate summoned form.");
            return false;
        }

        GetParent().AddChild(actor);
        summonedForms.Add(actor);

        Global.Services.Get<Host>().Madness += actor.SummoningMadness;

        actor.Reset(location);
        activateActor(actor);
        EmitSignal(nameof(SummonStarted));
        return true;
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
        EmitSignal(nameof(SummonEnded));
    }

    private void activateActor(Actor actor)
    {
        currentActor?.Suspend();
        actor.MakeActive();
        currentActor = actor;

        if (Global.Services.TryGet<Game>(out var game))
        {
            game.ActorChanged(actor);
        }
    }

    public void FinishGame()
    {
        EmitSignal(nameof(GameFinished));
        Global.Instance.SwitchScene("res://scenes/Victory.tscn");
    }
}
