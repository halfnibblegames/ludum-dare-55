using System.Linq;
using Godot;
using HalfNibbleGame.Objects.Level;

namespace HalfNibbleGame.Objects;

public abstract class Actor : KinematicBody2D
{
    public bool IsActive { get; private set; }
    public bool HasControl { get; private set; }
    protected abstract float Speed { get; }
    
    private Vector2 velocity;
    private bool lastShouldBeMirrored;

    private Interactable? closestInteractable;
    private RayCast2D? interactableRay;

    protected int SummonDirection => lastShouldBeMirrored ? -1 : 1;

    public override void _Ready()
    {
        base._Ready();
        GetNode<AnimatedSprite>("AnimatedSprite").Playing = true;
    }

    public void Reset(Vector2 globalPos)
    {
        GlobalPosition = globalPos;
        OnReset();
    }

    protected virtual void OnReset() {}

    public void MakeActive()
    {
        IsActive = true;
        HasControl = true;
    }

    public void BlockControl()
    {
        HasControl = false;
    }

    public void ResumeControl()
    {
        HasControl = true;
    }

    public void Suspend()
    {
        IsActive = false;
        HasControl = false;
        closestInteractable?.StopGlow();
    }

    public void Banish()
    {
        Suspend();
        QueueFree();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        var newAnimation = CalculateAnimation();
        animatedSprite.Animation = newAnimation;
        animatedSprite.Playing = true;

        animatedSprite.FlipH = lastShouldBeMirrored = checkIfSpriteShouldBeMirrored();

        if (!IsActive) return;

        updateInteractableLineOfSight();

        if (!HasControl) return;

        if (Input.IsActionJustPressed("interact") &&
            isInteractableClose() &&
            !(interactableRay?.IsColliding() ?? false))
        {
            closestInteractable!.Interact(this);
        }
    }

    private void updateInteractableLineOfSight()
    {
        var oldInteractable = closestInteractable;
        closestInteractable = GetTree().GetNodesInGroup(Constants.InteractiveGroup)
            .OfType<Interactable>()
            .Select(interactable => (interactable, interactable.GlobalPosition.DistanceSquaredTo(GlobalPosition)))
            .OrderBy(tuple => tuple.Item2)
            .Select(tuple => tuple.Item1)
            .FirstOrDefault();

        if (oldInteractable != closestInteractable)
        {
            oldInteractable?.StopGlow();
        }

        if (closestInteractable is null)
        {
            interactableRay?.QueueFree();
            interactableRay = null;
            return;
        }

        if (interactableRay is null)
        {
            interactableRay = new RayCast2D();
            interactableRay.Enabled = true;
            interactableRay.ExcludeParent = true;
            AddChild(interactableRay);
        }

        interactableRay.AddException(closestInteractable.GetParent());
        interactableRay.CastTo = (closestInteractable.GlobalPosition - GlobalPosition);

        if (isInteractableClose())
        {
            closestInteractable?.StartGlow();
        }
        else
        {
            closestInteractable?.StopGlow();
        }
    }

    private bool isInteractableClose()
    {
        if (closestInteractable is null) return false;
        return closestInteractable.GlobalPosition.DistanceSquaredTo(GlobalPosition) < 20 * 20;
    }

    private bool checkIfSpriteShouldBeMirrored()
    {
        // If we're idle then we should not mirror.
        if (velocity is { x: 0, y: 0 })
            return lastShouldBeMirrored;

        return velocity.x < 0;
    }

    protected virtual string CalculateAnimation()
    {
        // We're standing still.
        if (velocity is { x: 0, y: 0 })
            return "idle";
        
        // We're moving straight up or down
        if (velocity.x == 0)
            return velocity.y > 0 ? "south" : "north";

        // If we're going up, use the diagonal up animation, otherwise all horizontal movement is the same.
        return velocity.y >= 0 ? "horizontal" : "diagonal";
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity = Vector2.Zero;
        
        if (!HasControl) return;

        velocity.x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        velocity.y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");

        // For keyboard input, we need to normalize the vector so diagonal movement isn't faster
        if (velocity.LengthSquared() >= 1)
        {
            velocity = velocity.Normalized();
        }

        velocity *= Speed;
        MoveAndSlide(velocity);
    }
}
