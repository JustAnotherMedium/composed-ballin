using Godot;
using System;

// This script will be applied to every ball
// The only diffrences between balls (in code) will be their hp and spawn cost
// so these variables are exported and saved with the specific scene
public partial class BallScript : RigidBody2D
{
	private float hp;
	private int money;
	private SpawnerScript parent;

	private Timer iFrames;
	private bool canTakeDamage = true;

	public void BallInit(int hp, int money, SpawnerScript parent)
	{
		this.hp = hp;
		this.money = money;
		this.parent = parent;
	}

	public void TakeDamage(float damage)
	{
		hp -= canTakeDamage ? damage : 0;
		// damage numbers code
		DamageNumbers.DisplayFloatingNumber(damage, GlobalPosition, DamageNumbers.NumberType.BALL_DAMAGE); // ignore the warning

		if (hp <= 0)
		{
			DamageNumbers.DisplayFloatingNumber(money, GlobalPosition, DamageNumbers.NumberType.MONEY_GAIN);
			parent.BallKillGiveMoney(money);
			QueueFree();
		}
	}
}

