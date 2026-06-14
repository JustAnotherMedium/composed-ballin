using Godot;
using Godot.Collections;
using System;

public partial class SpawnerScript : Node2D
{
	private Node2D ballParent;
	private PackedScene[] balls = new PackedScene[5];
	private int[] spawnCosts = {1, 15, 45, 60, 85};

	private int currentWave = 10;
	private int spawnCredits;
	private float ticketMultiplier = 2.2f;
	private bool waveOngoing = true;
	private Timer spawnCooldown;
	private Timer detonationTimer;


	public override void _Ready()
	{
		// fetch all the assets it needs
		ballParent = GetNode<Node2D>("Balls");

		for (int i = 0; i < 5; i++)
		{
			balls[i] = GD.Load<PackedScene>("res://Scenes/Balls/ballL" + (i + 1) + ".tscn");
		}

		spawnCooldown = GetNode<Timer>("Timers/Spawn Cooldown");
		detonationTimer = GetNode<Timer>("Timers/Detonation Timer");

		spawnCredits = 20 * currentWave; // This is only here because i might want to start on a different wave
		GD.Print("Wave " + currentWave + " start!");
		spawnCooldown.Start();
	}

	public override void _Process(double delta)
	{
		if (!waveOngoing) { return; }

		if (ballParent.GetChildCount() == 0 && spawnCredits <= 0)
		{
			currentWave++;
			spawnCredits = 20 * currentWave; // (20 + (5 * beaconCount)) * currentWave; (use this one later)
			GD.Print("Wave " + currentWave + " start!");
			spawnCooldown.Start();
		}
	}

	public void SpawnBall()
	{
		if (spawnCredits <= 0) { return; } // In theory this should never happen but just in case

		float tickets = 1f;

		for (int i = 0; i < 5; i++)
		{
			if (spawnCosts[i] <= spawnCredits * 0.3)
			{
				tickets += 1f * Mathf.Pow(ticketMultiplier, i);
			}
		}

		float roll = (float) GD.RandRange(0.0, (double) tickets);
		int ballIndex = -1;

		GD.Print("Credits: " + spawnCredits + " Tickets: " + tickets + " Roll: " + roll);

		if (roll <= 2f)
		{
			ballIndex = 0;
		}
		else if (roll <= 1f + 1f * Mathf.Pow(ticketMultiplier, 1))
		{
			ballIndex = 1;
		}
		else if (roll <= 1f + 1f * Mathf.Pow(ticketMultiplier, 2))
		{
			ballIndex = 2;
		}
		else if (roll <= 1f + 1f * Mathf.Pow(ticketMultiplier, 3))
		{
			ballIndex = 3;
		}
		else
		{
			ballIndex = 4;
		}

		spawnCredits -= spawnCosts[ballIndex];
		RigidBody2D ball = balls[ballIndex].Instantiate<RigidBody2D>();
		ballParent.AddChild(ball);

		if (waveOngoing)
		{
			spawnCooldown.Start();
		}

		if (spawnCredits <= 0)
		{
			detonationTimer.Start();
		}
	}

	public void Detonate()
	{
		Array<Node> balls = ballParent.GetChildren();
		foreach (Node i in balls)
		{
			i.QueueFree();
		}
	}

}
