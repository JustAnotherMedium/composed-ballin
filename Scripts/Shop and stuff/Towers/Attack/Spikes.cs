using Godot;
using System;

public partial class Spikes : TowersScript
{
	private readonly float damage = 1f;

	public void BallEntered(Area2D area)
	{
		BallScript ball = area.GetParent() as BallScript;
		ball?.TakeDamage(damage);
	}
}
