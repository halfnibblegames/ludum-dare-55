using System;
using Godot;
using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Controls;

[Tool]
public class ChannelDebugLabel : Label
{
    public override void _EnterTree()
    {
        base._EnterTree();
        if (!Engine.EditorHint) QueueFree();
    }

    public override void _Process(float delta)
    {
        if (!Engine.EditorHint) return;
        base._Process(delta);

        var parent = GetParent();
        if (parent is null) return;

        var channel = parent.Get("Channel");
        if (channel is not int key || key == 0)
        {
            Text = "";
            return;
        }
        Text = Enum.GetName(typeof(ChannelKey), key);
    }

    public override void _Draw()
    {
        if (!Engine.EditorHint) return;
        base._Draw();
    }
}
