using Godot;
using System;

public partial class LevelSelect : CanvasLayer
{
	private LevelManagerConstants constants;
	[Export]
	private Label name;
	private int selected = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		constants = GetNode<LevelManagerConstants>("../../Constants");
		name.Text = constants.names[selected];
	}

	private void Previous()
	{
		selected = selected - 1 == -1 ? constants.names.Length : selected - 1;
		name.Text = constants.names[selected];
		// image
	}

	private void Next()
	{
		selected = selected + 1 == constants.names.Length ? 0 : selected + 1;
		name.Text = constants.names[selected];
		// image
	}

	private void Play()
	{
		LevelManager.LoadLevel(selected);
	}
}
