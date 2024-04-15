using Godot;
using JetBrains.Annotations;

namespace HalfNibbleGame.Autoload;

[UsedImplicitly]
public sealed class Prefabs : Node
{
    [Export] public PackedScene? Sandbox;
    [Export] public PackedScene? Imp;

    [Export] public PackedScene? Glow;
    [Export] public PackedScene? FirstLevel;

    [Export] public Texture? HorrorPortrait;
    [Export] public Texture? HostPortrait;
    [Export] public Texture? ImpPortrait;
    [Export] public Texture? UnknownPortrait;
    
    public override void _Ready()
    {
        Global.Services.ProvidePersistent(this);
    }
}
