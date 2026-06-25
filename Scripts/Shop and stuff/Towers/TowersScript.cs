using Godot;
using System;

public partial class TowersScript : StaticBody2D
{
	protected int id;
	protected Editor editor;
	protected bool held = false;
	protected bool inBounds;
	protected readonly float rotateAngle = Mathf.DegToRad(5f);

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HeldBehaviour();
	}

	public void TowerInit(Editor editor, int id) // @OnReady stuff for all towers
	{
		this.editor = editor;
		this.id = id;
	}

	public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
		InputBehaviour(@event);
    }

	protected virtual void HeldBehaviour() // Code to run each frame for a tower that is picked up
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

	protected virtual void InputBehaviour(InputEvent @event)
	{
		if (@event is InputEventMouseButton)
		{
			InputEventMouseButton mouseButton = @event as InputEventMouseButton; // get it as mouse button
			if (!held) // On left click: if this tower isn't picked up and there isnt already a tower picked up and we are in edit mode, pick up this tower
			{
				if (editor.HavePickedUp() || !editor.CanEdit()) { return; }

				if (mouseButton.ButtonIndex == MouseButton.Left)
				{
					held = true;
					editor.PickedUp(id);
				}
			}
			else
			{
				if (mouseButton.ButtonIndex == MouseButton.Right) // on right click: if this tower is picked up then put it down
				{
					held = false;
					editor.LetGo();
				}
				else if (mouseButton.ButtonIndex == MouseButton.WheelUp) // rotate tower with mouse wheel
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
