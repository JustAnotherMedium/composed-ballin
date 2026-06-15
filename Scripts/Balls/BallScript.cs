using Godot;
using System;

// This script will be applied to every ball
// The only diffrences between balls (in code) will be their hp and spawn cost
// so these variables are exported and saved with the specific scene
public partial class BallScript : RigidBody2D
{
	private int hp;
	private int money;
	private SpawnerScript parent;

	public void BallInit(int hp, int money, SpawnerScript parent)
	{
		this.hp = hp;
		this.money = money;
		this.parent = parent;
	}

	public override void _Process(double delta)
	{
	}
}
