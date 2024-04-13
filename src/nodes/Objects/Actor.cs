﻿using System;
using Godot;

namespace HalfNibbleGame.Objects;

public abstract class Actor : KinematicBody2D
{
    private TileMap? tileMap;
    protected bool IsActive { get; private set; }
    protected abstract float Speed { get; }
    private Vector2 velocity;

    protected Vector2 FindCurrentTile()
    {
        if (tileMap is null) throw new InvalidOperationException();
        return tileMap.WorldToMap(tileMap.ToLocal(GlobalPosition));
    }

    // ReSharper disable once ParameterHidesMember
    public void Reset(Vector2 tile, TileMap tileMap)
    {
        this.tileMap = tileMap;

        var localPos = tileMap.MapToWorld(tile);
        var globalPos = tileMap.ToGlobal(localPos);
        GlobalPosition = globalPos;

        OnReset();
    }

    protected virtual void OnReset() {}

    public void MakeActive()
    {
        if (IsActive) return;

        IsActive = true;
    }

    public void Suspend()
    {
        if (!IsActive) return;

        IsActive = false;
    }

    public void Banish()
    {
        Suspend();
        QueueFree();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        // TODO: update animation here based on velocity
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!IsActive) return;

        velocity = Vector2.Zero;

        velocity.x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        velocity.y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");

        // For keyboard input, we need to normalize the vector so diagonal movement isn't faster
        if (velocity.LengthSquared() >= 1)
        {
            velocity = velocity.Normalized();
        }

        velocity *= Speed;
        MoveAndSlide(velocity);
    }
}
