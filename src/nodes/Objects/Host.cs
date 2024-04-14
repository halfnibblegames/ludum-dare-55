using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Objects;

public class Host : Actor
{
    private const float summoningDuration = 0.3f;

    [Export] private float speed = 100;
    [Export] private float madnessCap = 200;

    protected override float Speed => speed;

    public float Madness { get; set; }
    public float MadnessCap => madnessCap;
    
    private float summoningTimeout;
    
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
                Global.Services.Get<WorldManager>().SummonForm(FindCurrentTile() + Vector2.Right);
            }
        }
        else
        {
            summoningTimeout = 0.0f;
        }

        if (!IsActive) return;

        Madness = 0;

        if (Input.IsActionJustPressed("summon_imp"))
        {
            Suspend();
            summoningTimeout = summoningDuration;
        }
    }

    protected override string CalculateAnimation() 
        => !IsActive ? "summoning" : base.CalculateAnimation();
}
