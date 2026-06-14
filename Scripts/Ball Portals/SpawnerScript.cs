using Godot;
using System;

public partial class SpawnerScript : Node2D
{
	private Node2D ballParent;
	private PackedScene[] balls = new PackedScene[5];

	private int currentWave;
	private int spawnCredits;
	private bool waveOngoing = true;
	private Timer spawnCooldown;
	private Timer detonationTimer;


	public override void _Ready()
	{
		// fetch all the assets it needs
		ballParent = GetNode<Node2D>("Balls");

		balls[0] = GD.Load<PackedScene>("res://Scenes/Balls/basic_ball.tscn");

		spawnCooldown = GetNode<Timer>("Timers/Spawn Cooldown");
		detonationTimer = GetNode<Timer>("Timers/Detonation Timer");

		spawnCooldown.Start();
	}

	public override void _Process(double delta)
	{

	}

	public void SpawnBall()
	{
		RigidBody2D ball = balls[0].Instantiate<RigidBody2D>();
		ballParent.AddChild(ball);

		if (waveOngoing)
		{
			spawnCooldown.Start();
		}
	}

}
