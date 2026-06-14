using Godot;
using Godot.Collections;
using System;

public partial class DespawnerPortal : Node2D
{
	private Area2D boundingBox;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		boundingBox = GetNode<Area2D>("Area2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Array<Area2D> objects = boundingBox.GetOverlappingAreas();

		foreach (Area2D i in objects)
		{
			BallScript ball = i.GetParent<BallScript>();

			if (ball != null)
			{
				ball.QueueFree();
			}
		}
	}
}
