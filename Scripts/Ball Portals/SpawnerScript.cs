using Godot;
using System;

public partial class SpawnerScript : Node2D
{
	private Node2D ballParent;
	private PackedScene[] balls = new PackedScene[5];

	private int currentWave;
	private int spawnCredits;


	public override void _Ready()
	{
		// fetch all the assets it needs
		ballParent = GetNode<Node2D>("Balls");

		balls[1] = GD.Load<PackedScene>("res://Scenes/Balls/basic_ball.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
