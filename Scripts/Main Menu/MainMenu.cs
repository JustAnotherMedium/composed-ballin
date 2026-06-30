using Godot;
using System;

public partial class MainMenu : Node2D
{
	[ExportCategory("Buttons")]
	[Export]
	private MainMenuButtons play;
	[Export]
	private MainMenuButtons tutorial;


	[ExportCategory("Other stuff")]
	[Export]
	private Camera2D camera;
	[Export]
	private Control levelSelect;

    public override void _Ready()
    {
        play.Play += OpenLevelSelect;
		tutorial.Tutorial += OpenTutorial;
    }

	public void OpenMainMenu()
	{
		camera.GlobalPosition = Vector2.Zero;
		levelSelect.SetPosition(new Vector2(-4000, 0));
	}

	private void OpenLevelSelect()
	{
		camera.GlobalPosition = new Vector2(5000, 0);
		levelSelect.SetPosition(Vector2.Zero);
	}

	private void OpenTutorial()
	{
		
	}
}

// I think i was high when i wrote this and everything that ties to it