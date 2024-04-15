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

    public static float CalculateHue(ChannelKey key)
    {
        return (key) switch
        {
            (ChannelKey.A) => 0.63f,
            (ChannelKey.B) => 0.0f,
            (ChannelKey.C) => 0.84f,
            (ChannelKey.D) => 0.17f,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public enum ChannelKey
{
    None = 0,
    A,
    B,
    C,
    D,
    Radiant
}

public enum ChannelState
{
    Off,
    On
}
