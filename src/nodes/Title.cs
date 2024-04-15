using Godot;
using HalfNibbleGame;

public class Title : Control
{
    public void ToggleSpeedrunTimer(bool showSpeedrunTimer)
    {
        Settings.ShowTimer = showSpeedrunTimer;
    }

    public void ToggleDeathCount(bool showDeathCount)
    {
        Settings.ShowDeathCount = showDeathCount;
    }
}
