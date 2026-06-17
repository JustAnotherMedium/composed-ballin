using Godot;
using System;

public partial class PlayerVars : Control
{
	private int money = 0;
	private int maxHP = 100;
	private int hp;
	public int Money { get => money; }
	public int MaxHitPoints { get => maxHP; }
	public int HitPoints { get => hp; }

	[Export]
	private TextureProgressBar healthBar;
	[Export]
	private Label healthText;
	[Export]
	private Label moneyText;
	[Export]
	private TextureProgressBar spawnCredits;
	[Export]
	private Label spawnCreditsText;
	[Export]
	private Label waveText;
	[Export]
	private Label TimerText;
	[Export]
	private TextureProgressBar Clock;
	[Export]
	private Control TimerUI;
	private SpawnerScript spawner;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hp = maxHP;

		spawner = GetNode<SpawnerScript>("../../Spawner Portal");
		DespawnerPortal despawner = GetNode<DespawnerPortal>("../../Despawner Portal");
		
		spawner.BallKill += GainMoney;
		spawner.BallDetonation += TakeDamage;
		spawner.WaveEnd += WaveEnd;
		despawner.BallDespawn += TakeDamage;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		healthText.Text = GD.VarToStr(hp);
		healthBar.Value = hp;
		moneyText.Text = money + "$";
		spawnCredits.MaxValue = 5 * spawner.GetWave() + 3;
		spawnCredits.Value = spawner.GetCredits();
		spawnCreditsText.Text = GD.VarToStr(spawner.GetCredits());
		waveText.Text = "Wave: " + spawner.GetWave();
		TimerUI.Visible = spawner.DetonationTimerRunning();
		TimerText.Text = GD.VarToStr(spawner.TimeLeft());
		Clock.Value = spawner.TimeLeft();
	}

	public void GainMoney(int amount) // used when gaining money from kills, or selling towers
	{
		money += amount;
	}

	public void SpendMoney(int amount) // spend money in shop
	{
		money -= amount;
	}

	public void TakeDamage(int amount) // ball detonation or despawn
	{
		hp -= amount;
	}

	public void WaveEnd(int moneyGain, int hpGain) // wave end money gain and hp gain
	{
		hp += hpGain;
		money += moneyGain;
	}

	public void SetMaxHP(int maxHP) // idk, maybe
	{
		this.maxHP = maxHP;
	}
}
