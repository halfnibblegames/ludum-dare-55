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
}

public enum ChannelKey
{
    None = 0,
    A,
    B
}

public enum ChannelState
{
    Off,
    On
}
