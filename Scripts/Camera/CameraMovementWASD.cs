using Godot;
using System;

public partial class CameraMovementWASD : CharacterBody2D
{
	public const float Speed = 600.0f;
	public const float Acceleration = 12f;

	public override void _PhysicsProcess(double delta)
	{
		// Movement Code
		Vector2 velocity = Velocity;

		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			velocity.X = (float) Mathf.Lerp(velocity.X, direction.X * Speed, delta * Acceleration);
			velocity.Y = (float) Mathf.Lerp(velocity.Y, direction.Y * Speed, delta * Acceleration);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Acceleration);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Acceleration);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
