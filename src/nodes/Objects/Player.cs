using Godot;
using HalfNibbleGame.Autoload;

namespace HalfNibbleGame.Objects;

public class Player : KinematicBody2D
{
    [Export] private float speed = 100;

    private ISpirit? currentSpirit;

    public override void _Ready()
    {
        var startTile = Global.Services.Get<LevelAttributes>().StartTile;
        var tileMap = Global.Services.Get<LevelTileMap>();
        var localPos = tileMap.MapToWorld(startTile);
        var globalPos = tileMap.ToGlobal(localPos);
        GlobalPosition = globalPos;
    }

    public override void _Process(float delta)
    {
        if (!Input.IsActionJustPressed("summon_spirit")) return;

        if (currentSpirit is null)
        {
            currentSpirit = new PhaseSpirit();
            currentSpirit.Begin(this);
        }
        else
        {
            currentSpirit.End(this);
            currentSpirit = null;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        var velocity = Vector2.Zero;

        velocity.x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        velocity.y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");

        // For keyboard input, we need to normalize the vector so diagonal movement isn't faster
        if (velocity.LengthSquared() >= 1)
        {
            velocity = velocity.Normalized();
        }

        velocity *= speed;
        MoveAndSlide(velocity);
    }
}
