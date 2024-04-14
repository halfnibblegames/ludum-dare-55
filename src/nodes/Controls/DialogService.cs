using System.Collections;
using System.Collections.Generic;
using Godot;
using HalfNibbleGame.Autoload;
using JetBrains.Annotations;

public enum Speaker
{
    Host,
    Imp,
    Clhubhukhapuslhamazarathoka,
    Unknown
}

public sealed record Dialog(
    string Text,
    Speaker Speaker
);

public sealed record Cinematic(
    List<Dialog> Dialogs,
    bool AllowAutoSkip
);

public class DialogService : Control
{
    private const int visibleYPosition = 556;
    private const int invisibleYPosition = 720;
    private const float autoDismissTimer = 2.0f;
    private const float dialogBoxDisplayDuration = 0.15f;
    private const float charactersPerSecond = 30.0f;

    private readonly Queue<Dialog> dialogQueue = new();
    private Dialog? currentDialog;
    private float elapsedDialogTimer;
    private float dialogDurationInSeconds;
    private bool isDisplaying;
    private bool isVisible;

    private TextureRect portrait = default!;
    private Label dialogTextBox = default!;
    private Label speakerName = default!;
    private Tween tween = default!;
    private bool cinematicIsAutoSkippable;

    public override void _Ready()
    {
        base._Ready();

        Global.Services.ProvideInScene(this);

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
            if (elapsedDialogTimer < dialogDurationInSeconds)
            {
                elapsedDialogTimer = dialogDurationInSeconds;
            }
            else
            {
                currentDialog = null;
            }
        }
        else if (cinematicIsAutoSkippable && elapsedDialogTimer > dialogDurationInSeconds + autoDismissTimer)
        {
            currentDialog = null;
        }

        if (currentDialog is not null)
        {
            elapsedDialogTimer += delta;
            dialogTextBox.PercentVisible = Mathf.Min(1.0f, elapsedDialogTimer / dialogDurationInSeconds);
        }
        else if (dialogQueue.Count > 0)
        {
            elapsedDialogTimer = 0.0f;
            dialogTextBox.PercentVisible = 0.0f;
            currentDialog = dialogQueue.Dequeue();

            dialogDurationInSeconds = (currentDialog.Text.Length / charactersPerSecond);
            dialogTextBox.Text = currentDialog.Text;
            speakerName.Text = currentDialog.Speaker.ToString();
        }
        else
        {
            isDisplaying = false;
            tween.InterpolateProperty(
                @object: this,
                property: "rect_position:y",
                initialVal: RectPosition.y,
                finalVal: invisibleYPosition,
                duration: dialogBoxDisplayDuration
            );
            tween.Start();
        }
    }

    public void ShowDialog(Dialog dialog)
    {
        var cinematic = new Cinematic(new List<Dialog> { dialog }, true);
        ShowCinematic(cinematic);
    }

    public void ShowCinematic(Cinematic cinematic)
    {
        if (dialogQueue.Count > 0)
            return;

        foreach (var dialog in cinematic.Dialogs)
        {
            dialogQueue.Enqueue(dialog);
        }

        cinematicIsAutoSkippable = cinematic.AllowAutoSkip;
        isDisplaying = true;
        tween.InterpolateProperty(
            @object: this,
            property: "rect_position:y",
            initialVal: RectPosition.y,
            finalVal: visibleYPosition,
            duration: dialogBoxDisplayDuration
        );
        tween.Start();
    }

    [UsedImplicitly]
    public void OnAnimationCompleted(object @object, NodePath node)
    {
        isVisible = isDisplaying;
    }

    public void ClearAllDialog()
    {
        dialogQueue.Clear();
        currentDialog = null;
    }
}
