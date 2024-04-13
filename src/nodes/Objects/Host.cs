using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Systems;

namespace HalfNibbleGame.Objects;

public class Host : Actor
{
    public const double MaxMadness = 200;

    [Export] private float speed = 100;

    protected override float Speed => speed;

    public float Madness { get; private set; }

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

        if (!IsActive) return;

        if (Input.IsActionJustPressed("summon_spirit"))
        {
            Global.Services.Get<WorldManager>().SummonForm(FindCurrentTile() + Vector2.Right);
        }
    }
}
