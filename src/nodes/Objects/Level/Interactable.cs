using System;
using Godot;
using HalfNibbleGame.Autoload;

namespace HalfNibbleGame.Objects.Level;

public sealed class Interactable : Node2D
{
    [Signal] public delegate void Interacted(Actor actor);

    private readonly Func<Actor, bool> canInteract;
    private Actor? glowActor;
    private Node? glow;

    public Interactable(Func<Actor, bool> canInteract)
    {
        this.canInteract = canInteract;
    }

    public override void _Process(float delta)
    {
        if (glow is not null && !canInteract(glowActor!))
        {
            StopGlow(glowActor!);
        }
    }

    public void Interact(Actor actor)
    {
        if (!canInteract(actor)) return;
        EmitSignal(nameof(Interacted), actor);
    }

    public void StartGlow(Actor actor)
    {
        if (glow is not null) return;
        glow = Global.Prefabs.Glow?.Instance();
        glowActor = actor;
        if (glow is not null) AddChild(glow);
    }

    public void StopGlow(Actor actor)
    {
        if (glow is null || actor != glowActor) return;
        glow.QueueFree();
        glow = null;
        glowActor = null;
    }
}
