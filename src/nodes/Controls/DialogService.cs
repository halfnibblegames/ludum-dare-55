using System.Collections;
using System.Collections.Generic;
using Godot;
using HalfNibbleGame.Autoload;
using JetBrains.Annotations;

public enum Speaker
{
    Host,
    Imp
}

public sealed record Dialog(
    string Text,
    Speaker Speaker
);

public class DialogService : Control
{
    private const int visibleYPosition = 556;
    private const int invisibleYPosition = 720;
    private const float dialogBoxDisplayDuration = 0.4f;
    private const float textDisplayDuration = 1.0f;

    private readonly Queue<Dialog> dialogQueue = new();
    private Dialog? currentDialog;
    private float elapsedDialogTimer;
    private bool isDisplaying;
    private bool isVisible;

    private TextureRect portrait = default!;
    private Label dialogTextBox = default!;
    private Label speakerName = default!;
    private Tween tween = default!;

    public override void _Ready()
    {
        base._Ready();

        Global.Services.ProvidePersistent(this);

        portrait = GetNode<TextureRect>("Portrait");
        dialogTextBox = GetNode<Label>("DialogText");
        speakerName = GetNode<Label>("SpeakerName");
        tween = GetNode<Tween>("Tween");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (!isVisible)
            return;

        if (Input.IsActionJustPressed("ui_accept"))
        {
            if (elapsedDialogTimer < textDisplayDuration)
            {
                elapsedDialogTimer = textDisplayDuration;
            }
            else
            {
                currentDialog = null;
            }
        }

        if (currentDialog is not null)
        {
            elapsedDialogTimer = Mathf.Min(textDisplayDuration, elapsedDialogTimer + delta);
            dialogTextBox.PercentVisible = elapsedDialogTimer / textDisplayDuration;
        }
        else if (dialogQueue.Count > 0)
        {
            elapsedDialogTimer = 0.0f;
            currentDialog = dialogQueue.Dequeue();
            dialogTextBox.Text = currentDialog.Text;
        }
        else
        {
            isDisplaying = false;
            tween.InterpolateProperty(
                @object: this,
                property: "rect_position:y",
                initialVal: isVisible ? visibleYPosition : invisibleYPosition,
                finalVal: invisibleYPosition,
                duration: dialogBoxDisplayDuration
            );
            tween.Start();
        }
    }

    public void ShowDialog(IEnumerable<Dialog> dialogs)
    {
        foreach (var dialog in dialogs)
        {
            dialogQueue.Enqueue(dialog);
        }

        isDisplaying = true;
        tween.InterpolateProperty(
            @object: this,
            property: "rect_position:y",
            initialVal: isVisible ? visibleYPosition : invisibleYPosition,
            finalVal: visibleYPosition,
            duration: dialogBoxDisplayDuration
        );
        tween.Start();
    }

    [UsedImplicitly]
    public void OnAnimationCompleted()
    {
        isVisible = isDisplaying;
    }
}
