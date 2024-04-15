using Godot;
using System;

public class Game : Node2D
{
    public override void _Ready()
    {
        GetNode<AudioStreamPlayer>("Soundtrack").Play();
    }
}
