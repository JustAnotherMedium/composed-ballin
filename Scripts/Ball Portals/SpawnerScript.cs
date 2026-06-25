using Godot;
using Godot.Collections;
using System;
using System.Reflection;

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
	[Signal]
	public delegate void BallKillEventHandler(int moneyEarned);

	private int currentWave = 0;
	private int spawnCredits;
	private float ticketMultiplier = 2.2f;
	private bool waveOngoing = false;
	[Export]
	private Timer spawnCooldown;
	[Export]
	private Timer detonationTimer;

	[Signal]
	public delegate void BallDetonationEventHandler(int damageTaken);
	[Signal]
	public delegate void WaveEndEventHandler(int moneyEarned, int hpHealed);
	[Signal]
	public delegate void ShopSetupEventHandler(); // Same as the Wave End signal but used by the shop and editor node to handle their stuff
	private readonly int damagePerDetonation = 4;
	private readonly int baseWaveEndMoneyEarn = 50;
	private readonly int baseFastKillHpBonus = 5;
	private int waveEndMoneyEarn = 50;
	private int fastKillHpBonus = 5;
	private bool awardedWaveEnd = false;


	public override void _Ready()
	{
		Editor editor = GetNode<Editor>("../Editor");
		editor.EditModeExit += StartNextWave;

		currentWave = Mathf.Max(0, currentWave); // <=0 wave number will break the spawning code
	}

	public override void _Process(double delta)
	{
		if (!waveOngoing) { return; }

		if (!awardedWaveEnd && ballParent.GetChildCount() == 0 && spawnCredits <= 0)
		{
			EmitSignal(SignalName.WaveEnd, waveEndMoneyEarn, 0);
			EmitSignal(SignalName.ShopSetup);
			waveOngoing = false;
			awardedWaveEnd = true;
			DamageNumbers.DisplayFloatingNumber(waveEndMoneyEarn, GetViewport().GetCamera2D().GlobalPosition, DamageNumbers.NumberType.WAVE_END_MONEY);
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
		if (!waveOngoing || (waveOngoing && spawnCredits > 0)) { return; }

		Array<Node> balls = ballParent.GetChildren();
		EmitSignal(SignalName.BallDetonation, balls.Count * damagePerDetonation);
		foreach (Node i in balls)
		{
			Node2D ball = i as Node2D;
			DamageNumbers.DisplayFloatingNumber(damagePerDetonation, ball.GlobalPosition, DamageNumbers.NumberType.PLAYER_DAMAGE);
			i.QueueFree();
		}
	}

	private void StartNextWave()
	{
		currentWave++;
		spawnCredits = 5 * currentWave + 3; // (20 + (5 * beaconCount)) * currentWave; (use this one later)
		GD.Print("Wave " + currentWave + " start!");
		spawnCooldown.Start();
		waveOngoing = true;
		awardedWaveEnd = false;
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

	public void BallKillGiveMoney(int moneyEarned)
	{
		EmitSignal(SignalName.BallKill, moneyEarned);
	}

	public int GetCredits()
	{
		return spawnCredits;
	}

	public int GetWave()
	{
		return currentWave;
	}

	public bool GetWaveOngoing()
	{
		return waveOngoing;
	}

	public bool DetonationTimerRunning()
	{
		return detonationTimer.TimeLeft != 0;
	}

	public double TimeLeft()
	{
		return Mathf.RoundToInt(detonationTimer.TimeLeft * 10) / 10.0;
		
	}
}
// GetViewport().GlobalCanvasTransform.Origin