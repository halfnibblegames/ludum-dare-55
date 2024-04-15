using Godot;
using HalfNibbleGame;

public class Title : Control
{
    public override void _Ready()
    {
        base._Ready();

        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
    }

    public void ToggleSpeedrunTimer(bool showSpeedrunTimer)
    {
        Settings.ShowTimer = showSpeedrunTimer;
    }

    public void ToggleDeathCount(bool showDeathCount)
    {
        Settings.ShowDeathCount = showDeathCount;
    }
}
