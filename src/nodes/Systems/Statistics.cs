using System;
using Godot;
using HalfNibbleGame.Autoload;
using HalfNibbleGame.Controls;

namespace HalfNibbleGame.Systems;

public class Statistics : Node
{
    private Label speedTimer = null!;

    private float totalTime;
    private float totalMadnessTime;
    private float thisLevelMadnessTime;
    private int deaths;

    private bool isSummonActive;

    public override void _Ready()
    {
        speedTimer = GetNode<Label>("SpeedTimer");
        if (!Settings.ShowTimer)
        {
            speedTimer.Hide();
        }
    }

    public override void _Process(float delta)
    {
        if (isSummonActive) thisLevelMadnessTime += delta;
        var inCinematic = Global.Services.Get<DialogService>().IsInCinematic;

        if (!inCinematic)
        {
            totalTime += delta;
        }

        var timeElapsed = TimeSpan.FromSeconds(totalTime);
        speedTimer.Text = $"{timeElapsed.Minutes:00}:{timeElapsed.Seconds:00}";

        var shouldModulate = inCinematic && DateTime.Now.Second % 2 == 0;
        speedTimer.Modulate = shouldModulate ? Colors.Gray : Colors.White;
    }

    public void OnLevelReset()
    {
        isSummonActive = false;
    }

    public void OnLevelCompleted()
    {
        totalMadnessTime += thisLevelMadnessTime;
        thisLevelMadnessTime = 0;
    }

    public void OnPlayerDeath()
    {
        deaths++;
    }

    public void OnSummonStarted()
    {
        isSummonActive = true;
    }

    public void OnSummonEnded()
    {
        isSummonActive = false;
    }

    public void OnGameFinished()
    {
        Victory.TotalTime = TimeSpan.FromSeconds(totalTime);
        Victory.ImpTime = TimeSpan.FromSeconds(totalMadnessTime);
    }
}
