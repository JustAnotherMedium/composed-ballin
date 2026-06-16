using Godot;
using Godot.Collections;
using System;

public partial class Burner : TowersScript
{
	[Export]
	private float damage;
	private Area2D hitbox;

	protected override void TowerInit()
	{
		base.TowerInit();
		hitbox = GetNode<Area2D>("HitBox");
	}

	public override void _Process(double delta)
	{
		HeldBehaviour();

		Array<Area2D> areas = hitbox.GetOverlappingAreas();
		foreach (Area2D i in areas)
		{
			BallScript ball = i.GetParent() as BallScript;
			ball?.TakeBurnerDamage(damage);
		}
	}
}
