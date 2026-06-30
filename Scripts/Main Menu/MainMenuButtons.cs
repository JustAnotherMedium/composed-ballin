using Godot;
using System;

public partial class MainMenuButtons : Area2D
{
    private enum MainMenuButtonTypes
    {
        PLAY,
        TUTORIAL,
        EXIT
    }

    [Export]
    private MainMenuButtonTypes buttonType;

    [Signal]
    public delegate void PlayEventHandler();
    [Signal]
    public delegate void TutorialEventHandler();

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
		InputEventMouseButton mouseButton = @event as InputEventMouseButton; // get it as mouse button
        if (mouseButton != null && mouseButton.ButtonIndex == MouseButton.Left)
        {
            switch (buttonType)
            {
                case MainMenuButtonTypes.PLAY:
                    EmitSignal(SignalName.Play);
                    break;
                
                case MainMenuButtonTypes.TUTORIAL:
                    EmitSignal(SignalName.Tutorial);
                    break;

                case MainMenuButtonTypes.EXIT:
                    // bleh nuke game
                    GetTree().Quit();
                    break;
            }
        }
    }
}
