using Godot;
using System;


// This script will be applied to every ball
// The only diffrences between balls (in code) will be their hp and spawn cost
// so these variables are exported and saved with the specific scene
public partial class BallScript : RigidBody2D
{
	[ExportGroup("Ball Properties")]
	[Export]
	public int HitPoints { get; set; } // Think of it like the max hp
	[Export]
	public int SpawnCost { get; set; }
	
	private int hp;


	public override void _Ready()
	{
		hp = HitPoints;
	}

	public override void _Process(double delta)
	{
	}
}
