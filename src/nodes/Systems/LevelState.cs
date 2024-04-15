using System;
using Godot;
using HalfNibbleGame.Objects.Level;

namespace HalfNibbleGame.Systems;

public static class LevelState
{
    public static void UpdateChannel(this Node node, ChannelKey key, ChannelState newState)
    {
        if (key == ChannelKey.None) return;
        node.GetTree().CallGroup(Constants.LevelStateGroup, nameof(ILevelState.ConsumeChange), key, newState);
    }

    public static float CalculateHue(ChannelKey key, ChannelState state)
    {
        return (key, state) switch
        {
            (ChannelKey.A, ChannelState.Off) => 0.63f,
            (ChannelKey.A, ChannelState.On) => 0.0f,
            (ChannelKey.B, ChannelState.Off) => 0.84f,
            (ChannelKey.B, ChannelState.On) => 0.17f,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public enum ChannelKey
{
    None = 0,
    A,
    B,
    Radiant
}

public enum ChannelState
{
    Off,
    On
}
