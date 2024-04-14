using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Objects.Level;

public interface ILevelState
{
    void ConsumeChange(ChannelKey key, ChannelState newState);
}
