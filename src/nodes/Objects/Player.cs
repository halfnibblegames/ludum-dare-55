using Godot;

namespace HalfNibbleGame.Objects;

public class Player : Area2D
{
    [Export] private float speed = 100;

    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
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

        Position += velocity * delta;
    }
}
