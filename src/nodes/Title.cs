using Godot;
using HalfNibbleGame;
using HalfNibbleGame.Autoload;

public class Title : Control
{
    public override void _Ready()
    {
        base._Ready();

        Global.Prefabs.GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
    }

    public void ToggleSpeedrunTimer(bool showSpeedrunTimer)
    {
        Settings.ShowTimer = showSpeedrunTimer;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Input.IsActionJustPressed("ui_accept") || Input.IsActionJustPressed("ui_select"))
        {
            GetNode<SceneChangerLabel>("StartButton")._GuiInput(new InputEventMouseButton
            {
                ButtonIndex = (int) ButtonList.Left,
                Pressed = true
            });
        }
    }
}
