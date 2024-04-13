using System;
using Godot;

namespace HalfNibbleGame.Objects;

public abstract class Actor : KinematicBody2D
{
    private TileMap? tileMap;
    protected bool IsActive { get; private set; }
    protected abstract float Speed { get; }

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

        var camera = GetNode<ShakeCamera>("ShakeCamera");
        camera.LimitLeft = (int) tileMap.GetUsedRect().Position.x;
        camera.LimitTop = (int) tileMap.GetUsedRect().Position.y;
        camera.LimitRight = (int) (tileMap.GetUsedRect().End.x * tileMap.CellSize.x);
        camera.LimitBottom = (int) (tileMap.GetUsedRect().End.y * tileMap.CellSize.y);
        camera.ResetSmoothing();

        OnReset();
    }

    protected virtual void OnReset() {}

    public void MakeActive()
    {
        if (IsActive) return;

        var camera = GetNode<ShakeCamera>("ShakeCamera");
        camera.MakeCurrent();

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

    public override void _PhysicsProcess(float delta)
    {
        if (!IsActive) return;

        var velocity = Vector2.Zero;

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
