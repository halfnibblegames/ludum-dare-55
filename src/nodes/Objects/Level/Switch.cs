using Godot;
using HalfNibbleGame.Systems;
using JetBrains.Annotations;

namespace HalfNibbleGame.Objects.Level;

public class Switch : StaticBody2D, ILevelState
{
    [Export] public ChannelKey Channel;

    private ChannelState state;

    public override void _Ready()
    {
        base._Ready();
        this.MakeInteractable(this, nameof(onActivated), _ => true);
        AddToGroup(Constants.LevelStateGroup);

        if (GetNode<Sprite>("Sprite").Material is ShaderMaterial shaderMaterial)
        {
            var copy = (ShaderMaterial) shaderMaterial.Duplicate();
            GetNode<Sprite>("Sprite").Material = copy;
        }
    }

    public void ConsumeChange(ChannelKey key, ChannelState newState)
    {
        if (key != Channel) return;

        state = newState;
        var shaderMaterial = GetNode<Sprite>("Sprite").Material as ShaderMaterial;
        shaderMaterial?.SetShaderParam("hue", LevelState.CalculateHue(Channel));
    }

    [UsedImplicitly]
    private void onActivated(Actor actor)
    {
        var newState = state == ChannelState.Off ? ChannelState.On : ChannelState.Off;
        this.UpdateChannel(Channel, newState);
        GetNode<AudioStreamPlayer>("SFX").Play();
    }
}
