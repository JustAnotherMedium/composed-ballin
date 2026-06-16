using Godot;
using System;

public partial class Spikes : TowersScript
{
	private readonly float damage = 1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TowerInit();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HeldBehaviour();
	}

	public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
		InputBehaviour(@event);
    }

	public void BallEntered(Area2D area)
	{
		BallScript ball = area.GetParent() as BallScript;
		ball?.TakeDamage(damage);
	}
}
