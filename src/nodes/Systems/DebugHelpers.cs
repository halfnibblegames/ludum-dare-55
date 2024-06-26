﻿using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Objects;

namespace HalfNibbleGame.Systems;

public class DebugHelpers : Node
{
    public override void _Ready()
    {
        if (!OS.IsDebugBuild())
        {
            GD.PushWarning("Attempting to load DebugHelpers in a non-debug build. This should not happen.");
            QueueFree();
        }
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("debug_reset_madness"))
        {
            Global.Services.Get<Host>().Madness = 0;
        }

        if (Input.IsActionJustPressed("debug_toggle_collision"))
        {
            var shape = Global.Services.Get<Host>().GetNode<CollisionShape2D>("CollisionShape2D");
            shape.Disabled = !shape.Disabled;
        }
    }
}
