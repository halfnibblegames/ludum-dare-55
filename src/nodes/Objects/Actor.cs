using System;
using Godot;

namespace HalfNibbleGame.Objects;

public abstract class Actor : KinematicBody2D
{
    private TileMap? tileMap;
    protected bool IsActive { get; private set; }
    protected abstract float Speed { get; }
    
    private Vector2 velocity;
    private string lastAnimation;
    private bool lastShouldBeMirrored;
    
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

        var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        var newAnimation = CalculateAnimation();
        if (newAnimation != lastAnimation)
        {
            animatedSprite.Animation = newAnimation;
        }

        animatedSprite.FlipH = lastShouldBeMirrored = checkIfSpriteShouldBeMirrored();
    }

    private bool checkIfSpriteShouldBeMirrored()
    {
        // If we're idle then we should not mirror.
        if (velocity is { x: 0, y: 0 })
            return lastShouldBeMirrored;

        return velocity.x < 0;
    }

    protected virtual string CalculateAnimation()
    {
        // We're standing still.
        if (velocity is { x: 0, y: 0 })
            return "idle";
        
        // We're moving straight up or down
        if (velocity.x == 0)
            return velocity.y > 0 ? "south" : "north";

        // If we're going up, use the diagonal up animation, otherwise all horizontal movement is the same.
        return velocity.y >= 0 ? "horizontal" : "diagonal";
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity = Vector2.Zero;
        
        if (!IsActive) return;

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
