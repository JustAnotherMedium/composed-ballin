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

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
		InputEventMouseButton mouseButton = @event as InputEventMouseButton; // get it as mouse button
        if (mouseButton != null && mouseButton.ButtonIndex == MouseButton.Left)
        {
            switch (buttonType)
            {
                case MainMenuButtonTypes.PLAY:
                    GD.Print("Play the game!");
                    break;
                
                case MainMenuButtonTypes.TUTORIAL:
                    GD.Print("How to play the game!");
                    break;

                case MainMenuButtonTypes.EXIT:
                    GD.Print("Don't want to play the game...");
                    break;
            }
        }
    }
}
