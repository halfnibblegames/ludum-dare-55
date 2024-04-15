using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Objects;

public class Game : Node2D
{
    private bool hostIsPlaying = true;
    private AudioStreamPlayer host = default!;
    private AudioStreamPlayer imp = default!;
    
    public override void _Ready()
    {
        Global.Services.ProvideInScene(this);
        host = GetNode<AudioStreamPlayer>("Host");
        imp = GetNode<AudioStreamPlayer>("Imp");
        host.Play();
    }

    public void ActorChanged(Actor actor)
    {
        var hostShouldPlay = actor is not EldritchActor;
        if (hostShouldPlay == hostIsPlaying)
            return;
        
        if (hostShouldPlay)
        {
            imp.Stop();
            host.Play();
        }
        else
        {
            host.Stop();
            imp.Play();
        }

        hostIsPlaying = hostShouldPlay;
    }
}
