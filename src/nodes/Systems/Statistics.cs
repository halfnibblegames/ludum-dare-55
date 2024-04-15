using System;
using Godot;

namespace HalfNibbleGame.Systems;

public class Statistics : Node
{
    private Label speedTimer = null!;

    private DateTime startTime;
    private float totalMadnessTime;
    private float thisLevelMadnessTime;
    private int deaths;

    private bool isSummonActive;

    public override void _Ready()
    {
        speedTimer = GetNode<Label>("SpeedTimer");
        startTime = DateTime.Now;
        if (!Settings.ShowTimer)
        {
            speedTimer.Hide();
        }
    }

    public override void _Process(float delta)
    {
        if (isSummonActive) thisLevelMadnessTime += delta;
        var timeElapsed = DateTime.Now - startTime;
        speedTimer.Text = $"{timeElapsed.Minutes:00}:{timeElapsed.Seconds:00}";
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
}
