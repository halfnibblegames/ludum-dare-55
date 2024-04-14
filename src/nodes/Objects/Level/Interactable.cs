using Godot;

namespace HalfNibbleGame.Objects.Level;

public sealed class Interactable : Node2D
{
    [Signal] public delegate void Interacted(Actor actor);

    public void Interact(Actor actor)
    {
        EmitSignal(nameof(Interacted), actor);
    }
}
