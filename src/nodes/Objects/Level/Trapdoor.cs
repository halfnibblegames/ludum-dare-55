﻿using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Objects.Level;

public class Trapdoor : DetectingArea2D, ILevelResettable
{
    private const string idleAnimation = "idle";
    private const string openAnimation = "Activation";

    private Actor? capturedActor;
    private AnimatedSprite sprite = null!;

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(Constants.LevelStateGroup);
        sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        sprite.Connect("animation_finished", this, nameof(onAnimationFinished));
    }

    protected override void OnActorEntered(Actor actor)
    {
        if (actor is Imp) return;
        actor.BlockControl();
        capturedActor = actor;
        actor.GlobalPosition = GlobalPosition;
        sprite.Animation = openAnimation;
        sprite.Frame = 0;
        sprite.Play();
    }

    private void onAnimationFinished()
    {
        if (capturedActor is null) return;

        Global.Services.Get<WorldManager>().CallDeferred(nameof(WorldManager.KillPlayer));
        Reset();
    }

    public void Reset()
    {
        capturedActor = null;
        sprite.Animation = idleAnimation;
        sprite.Frame = 0;
        sprite.Play();
    }
}