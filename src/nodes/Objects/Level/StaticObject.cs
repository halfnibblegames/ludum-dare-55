using System;
using Godot;
using Object = Godot.Object;

namespace HalfNibbleGame.Objects.Level;

public static class StaticObjectExtensions
{
    public static void MakeInteractable(
        this Node2D node, Object target, string eventHandler, Func<Actor, bool> canInteract)
    {
        var interactable = new Interactable(canInteract);
        interactable.Connect(nameof(Interactable.Interacted), target, eventHandler);
        node.AddChild(interactable);
        interactable.AddToGroup(Constants.InteractiveGroup);
    }
}
