using Godot;
using System;

public partial class TowersScript : StaticBody2D
{
	[Export] // remove later or change smth idk
	public int id { get; set; }

	private Editor editor;
	private bool held = false;
	private bool inBounds;
	private readonly float rotateAngle = Mathf.DegToRad(5f);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		editor = GetParent<Editor>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (held)
		{
			Vector2 mousePos = GetGlobalMousePosition();

			Transform2D move = Transform; // Move Tower to Mouse Pos
			move.Origin = mousePos;
			Transform = move;

			Vector2 size = GetViewportRect().Size; // Make sure the mouse is in viewport (otherwise don't register any clicks)
			inBounds = mousePos.X >= 0 && mousePos.X <= size.X && mousePos.Y >= 0 && mousePos.Y <= size.Y;
		}
	}

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton)
		{
			InputEventMouseButton mouseButton = @event as InputEventMouseButton;
			if (!held)
			{
				if (editor.HavePickedUp()) { return; }

				if (mouseButton.ButtonIndex == MouseButton.Left)
				{
					held = true;
					editor.PickedUp(id);
				}
			}
			else
			{
				if (mouseButton.ButtonIndex == MouseButton.Right)
				{
					held = false;
					editor.LetGo();
				}
				else if (mouseButton.ButtonIndex == MouseButton.WheelUp)
				{
					Transform = Transform.Rotated(rotateAngle);
				}
				else if (mouseButton.ButtonIndex == MouseButton.WheelDown)
				{
					Transform = Transform.Rotated(-rotateAngle);
				}
			}
		}
    }

}
