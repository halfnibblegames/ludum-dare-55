using Godot;
using HalfNibbleGame.Autoload;

namespace HalfNibbleGame.Objects;

public class Player : KinematicBody2D
{
    public const double MaxMadness = 200;

    [Export] private float speed = 100;

    public float Madness { get; private set; }
    public ISpirit? CurrentSpirit { get; private set; }

    public override void _Ready()
    {
        var startTile = Global.Services.Get<LevelAttributes>().StartTile;
        var tileMap = Global.Services.Get<LevelTileMap>();
        var localPos = tileMap.MapToWorld(startTile);
        var globalPos = tileMap.ToGlobal(localPos);
        GlobalPosition = globalPos;

        var camera = GetNode<ShakeCamera>("ShakeCamera");
        camera.LimitLeft = (int) tileMap.GetUsedRect().Position.x;
        camera.LimitTop = (int) tileMap.GetUsedRect().Position.y;
        camera.LimitRight = (int) (tileMap.GetUsedRect().End.x * tileMap.CellSize.x);
        camera.LimitBottom = (int) (tileMap.GetUsedRect().End.y * tileMap.CellSize.y);
    }

    public override void _Process(float delta)
    {
        Madness += delta * (CurrentSpirit?.MadnessPerSecond ?? 0);

        if (!Input.IsActionJustPressed("summon_spirit")) return;

        if (CurrentSpirit is null)
        {
            CurrentSpirit = new PhaseSpirit();
            Madness += CurrentSpirit.InitialMadness;
            CurrentSpirit.Begin(this);
        }
        else
        {
            CurrentSpirit.End(this);
            CurrentSpirit = null;
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
