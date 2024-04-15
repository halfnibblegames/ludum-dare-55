using System;
using Godot;
using HalfNibbleGame;

public class Victory : Control
{
    public static TimeSpan TotalTime;
    public static TimeSpan ImpTime;

    public override void _Ready()
    {
        // Always show speedrun timer on all subsequent runs.
        Settings.ShowTimer = true;

        GetNode<Label>("TotalTime").Text = TotalTime.ToString(@"mm\:ss");
        GetNode<Label>("ImpTime").Text = ImpTime.ToString(@"mm\:ss");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Input.IsActionJustPressed("ui_accept") || Input.IsActionJustPressed("ui_select"))
        {
            GetNode<SceneChangerLabel>("RestartButton")._GuiInput(new InputEventMouseButton
            {
                ButtonIndex = (int) ButtonList.Left,
                Pressed = true
            });
        }
    }
}
