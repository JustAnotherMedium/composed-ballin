using Godot;
using Godot.Collections;
using System;

public partial class Accelerator :TowersScript
{
	[Export]
	private float acceleration;
	private float powerFactor = 100000f;
	[Export]
	private Area2D hitbox;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HeldBehaviour();

		Array<Area2D> areas = hitbox.GetOverlappingAreas();
		foreach (Area2D i in areas)
		{
			RigidBody2D ball = i.GetParent() as RigidBody2D;
			if (ball != null)
			{
				Vector2 accel = Vector2.FromAngle(GlobalRotation);
				accel = accel * (acceleration * powerFactor * (float) delta);
				ball.ApplyCentralForce(accel);
			}
		}
	}
}
