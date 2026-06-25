using Godot;
using System;

public partial class SpinnerScript : TowersScript
{
	[Export]
	private float speed;
	private float rotation;

	public override void _Ready()
	{
		rotation = GlobalRotationDegrees;
	}

	public override void _Process(double delta)
	{
		HeldBehaviour();

		if (editor.CanEdit())
		{
			GlobalRotationDegrees = rotation;
		}
		else
		{
			GlobalRotationDegrees += speed * (float) delta;
			ConstantAngularVelocity = speed * (float) delta;
		}
	}

	public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
		InputBehaviour(@event);
    }

	protected override void InputBehaviour(InputEvent @event)
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
					rotation += Mathf.RadToDeg(rotateAngle);
				}
				else if (mouseButton.ButtonIndex == MouseButton.WheelDown)
				{
					rotation -= Mathf.RadToDeg(rotateAngle);
				}
			}
		}
	}

}
