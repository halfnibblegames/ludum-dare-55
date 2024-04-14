using Godot;

namespace HalfNibbleGame.Objects.Level;

public static class StaticObjectExtensions
{
    public static void MakeInteractable(this Node2D node, Object target, string eventHandler)
    {
        var interactable = new Interactable();
        interactable.Connect(nameof(Interactable.Interacted), target, eventHandler);
        node.AddChild(interactable);
        interactable.AddToGroup(Constants.InteractiveGroup);
    }
}
