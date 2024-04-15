using Godot;
using HalfNibbleGame.Autoload;

namespace HalfNibbleGame.Objects.Level;

public sealed class Interactable : Node2D
{
    [Signal] public delegate void Interacted(Actor actor);

    private Node? glow;

    public void Interact(Actor actor)
    {
        EmitSignal(nameof(Interacted), actor);
    }

    public void StartGlow()
    {
        if (glow is not null) return;
        GD.Print("start glow");
        glow = Global.Prefabs.Glow?.Instance();
        if (glow is not null) AddChild(glow);
    }

    public void StopGlow()
    {
        if (glow is null) return;
        GD.Print("end glow");
        glow.QueueFree();
        glow = null;
    }
}
