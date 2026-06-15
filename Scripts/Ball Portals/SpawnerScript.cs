using Godot;
using Godot.Collections;
using System;

public partial class SpawnerScript : Node2D
{
	
	[ExportCategory("Balls")]
	[Export]
	private Node2D ballParent;
	[Export]
	private PackedScene[] balls;
	[Export]
	private int[] spawnCosts;
	[Export]
	private int[] ballHp;
	[Export]
	private int[] ballEarn;

	private int currentWave = 1;
	private int spawnCredits;
	private float ticketMultiplier = 2.2f;
	private bool waveOngoing = true;
	private Timer spawnCooldown;
	private Timer detonationTimer;

	[Signal]
	public delegate void BallDetonationEventHandler(int damageTaken);
	[Signal]
	public delegate void WaveEndEventHandler(int moneyEarned, int hpHealed);
	private readonly int damagePerDetonation = 4;
	private readonly int baseWaveEndMoneyEarn = 50;
	private readonly int baseFastKillHpBonus = 5;
	private int waveEndMoneyEarn = 50;
	private int fastKillHpBonus = 5;


	public override void _Ready()
	{
		// fetch all the assets it needs
		ballParent = GetNode<Node2D>("Balls");

		/*for (int i = 0; i < balls.Length; i++)
		{
			balls[i] = GD.Load<PackedScene>("res://Scenes/Balls/ballL" + (i + 1) + ".tscn");
		}*/

		spawnCooldown = GetNode<Timer>("Timers/Spawn Cooldown");
		detonationTimer = GetNode<Timer>("Timers/Detonation Timer");

		currentWave = Mathf.Max(1, currentWave); // <=0 wave number will break the spawning code
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
			EmitSignal(SignalName.WaveEnd, waveEndMoneyEarn, 0);
			GD.Print("Wave " + currentWave + " start!");
			spawnCooldown.Start();
		}
	}

	public void SpawnBall()
	{
		if (spawnCredits <= 0) { return; } // In theory this should never happen but just in case

		float tickets = 1f;

		for (int i = 0; i < balls.Length; i++) // Calculate tickets
		{
			if (spawnCosts[i] <= spawnCredits * 0.3)
			{
				tickets += 1f * Mathf.Pow(ticketMultiplier, i);
			}
		}

		float roll = (float) GD.RandRange(0.0, (double) tickets); // Roll raffle
		int ballIndex = -1;

		// GD.Print("Credits: " + spawnCredits + " Tickets: " + tickets + " Roll: " + roll); // debug

		for (int i = 0; i < balls.Length; i++) // see which ball won
		{
			if (roll <= 1f + 1f * Mathf.Pow(ticketMultiplier, i))
			{
				ballIndex = i;
				break;
			}
		}

		// Spawn the ball (crazy)
		SpawnBall(ballIndex);

		// Reset the cooldown
		if (waveOngoing)
		{
			spawnCooldown.Start();
		}

		// If ran out of credits start the detonation timer
		if (spawnCredits <= 0)
		{
			detonationTimer.Start();
		}
	}
	/* How this works:
		First it calculates tickets,
		each ball that has a spawn cost that is at most 30% of available credits,
		has its own share of tickets and can be rolled to spawn. 
		Each increasing level of ball has 2.2x the tickets as the previous level,
		this makes it more likely to roll the highest ball it can, but not impossible to roll lower.
		Then the roll happens and from there it spawns a ball.
		the lowest level costs only 1 credit to spawn and this will make sure all credits are spent and never go negative
	*/

	public void Detonate() // deletes every ball
	{
		Array<Node> balls = ballParent.GetChildren();
		EmitSignal(SignalName.BallDetonation, balls.Count * damagePerDetonation);
		foreach (Node i in balls)
		{
			i.QueueFree();
		}
	}

	private void SpawnBall(int index)
	{
		RigidBody2D ball = balls[index].Instantiate<RigidBody2D>(); // Actually add the ball
		ballParent.AddChild(ball);
		BallScript ballScript = (BallScript) ball; // Give ball its values
		ballScript.BallInit(ballHp[index], ballEarn[index], this);
		spawnCredits -= spawnCosts[index]; // reduce spawn credits
	}
	// If i was ever gonna modify for object pooling it would be here
}
