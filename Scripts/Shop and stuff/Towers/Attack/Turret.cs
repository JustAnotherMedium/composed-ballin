using System;
using Godot;
using Godot.Collections;

public partial class Turret : TowersScript
{
	[Export]
	private float damage;
	private float rotation;
	[Export]
	private Area2D range;
	private BallScript target = null;
	[Export]
	private Node2D rangeIndicator;
	[Export]
	private Timer cooldown;
	private bool canFire = false;

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (editor.CanEdit())
		{
			GlobalRotationDegrees = rotation;
			rangeIndicator.Visible = true;
		}
		else
		{
			rangeIndicator.Visible = false;
			if (target == null)
			{
				target = AcquireNewTarget();
				if (target != null) { cooldown.Start(); }
				return;
			}
			
			try
			{
				// follow code
				GlobalRotation = GetAngleBetween(GlobalPosition, target.GlobalPosition);
			}
			catch
			{
				target = null;
				return;
			}

			if (canFire)
			{
				target.TakeDamage(damage);
				canFire = false;
				cooldown.Start();
			}
		}
	}

	private BallScript AcquireNewTarget()
	{
        Array<Area2D> targets = range.GetOverlappingAreas();
		BallScript target = null;
		int i = 0;
		while (target == null && i < targets.Count)
		{
			target = targets[i].GetParent() as BallScript;
			i++;
		}

		return target;
	}

	private void ResetCooldown()
	{
		canFire = true;
	}

	private float GetAngleBetween(Vector2 home, Vector2 dest)
	{
		float deltaX = dest.X - home.X;
		float deltaY = dest.Y - home.Y;

		float theta = Mathf.Atan2(deltaY, deltaX) + (Mathf.Pi / 2f);
		
		return theta;
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
