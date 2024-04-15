using Godot;
using Godot.Collections;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Systems;
using JetBrains.Annotations;

namespace HalfNibbleGame.Objects;

public class Host : Actor
{
    private const float summoningDuration = 0.3f;

    [Export] private float speed = 100;
    [Export] private float madnessCap = 200;

    protected override float Speed => speed;

    private bool IsDying { get; set; }
    public float Madness { get; set; }
    public float MadnessCap => madnessCap;
    public bool IsDismissingSeal { get; set; }
    
    private float summoningTimeout;
    private Vector2 summonLocation;
    
    public override void _Ready()
    {
        Global.Services.ProvideInScene(this);
    }

    protected override void OnReset()
    {
        base.OnReset();
        Madness = 0;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        
        if (summoningTimeout > 0.0f)
        {
            summoningTimeout -= delta;
            if (summoningTimeout <= 0.0f)
            {
                Global.Services.Get<WorldManager>().SummonForm(summonLocation);
            }
        }
        else
        {
            summoningTimeout = 0.0f;
        }

        if (!IsActive) return;

        Madness = 0;

        if (!HasControl) return;

        if (Input.IsActionJustPressed("summon_imp") && tryChooseSummonLocation(out summonLocation))
        {
            Suspend();
            summoningTimeout = summoningDuration;
        }
    }

    private bool tryChooseSummonLocation(out Vector2 location)
    {
        location = GlobalPosition + SummonDirection * Vector2.Right * 16;
        if (canSummonInLocation(location)) return true;
        location = GlobalPosition + SummonDirection * Vector2.Left * 16;
        if (canSummonInLocation(location)) return true;

        location = default;
        return false;
    }

    private bool canSummonInLocation(Vector2 location)
    {
        var collisions = GetWorld2d().DirectSpaceState.IntersectRay(GlobalPosition, location, new Array(this));
        return collisions.Count == 0;
    }

    protected override string CalculateAnimation() 
        => IsDying ? "death" : !IsActive || IsDismissingSeal ? "summoning" : base.CalculateAnimation();

    [UsedImplicitly]
    public void OnAnimationFinished()
    {
        if (IsDismissingSeal)
        {
            IsDismissingSeal = false;
            ResumeControl();
        }

        if (IsDying)
        {
            IsDying = false;
        }
    }

    public void Boom()
    {
        IsDying = true;
    }
}
