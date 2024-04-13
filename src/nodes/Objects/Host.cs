using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Objects;

public class Host : Actor
{
    private const float summoningDuration = 0.7f;
    public const double MaxMadness = 200;

    [Export] private float speed = 100;

    protected override float Speed => speed;

    public float Madness { get; private set; }
    
    private float summoningTimeout = 0.0f;
    
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
            
        if (Input.IsActionJustPressed("summon_imp"))
        {
            Suspend();
            summoningTimeout = summoningDuration;
        }
    }

    protected override string CalculateAnimation() 
        => summoningTimeout > 0.0f ? "summoning" : base.CalculateAnimation();
}
