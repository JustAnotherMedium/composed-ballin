using Godot;
using System;

public partial class ShopConstants : Control
{
	public readonly string[] names = 
	{
		"Rectangle", // 1
		"Triangle", // 2
		"Circle", // 3
		"Spinner (Slow)", // 1
		"Spinner (Fast)", // 2
		"Accelerator", // 3
		"Spike", // 1
		"Burner", // 2
		"Turret", // 3
		"Missile" // 4
	};

	public readonly string[] descriptions = 
	{
		"A rectangular platform",
		"A triangular platform",
		"A circular platform",
		"A rectangular platform that spins slow",
		"A rectangular platform that spins fast",
		"Accelerates any ball that enters, towards the direction it is oriented in",
		"Does 1 damage once to anything that touches it",
		"Does .2 damage every tenth of a second to anything nearby. Stacking burners will not increase their damage",
		"Shoots a ball for 5 damage every second",
		"Shoots a ball for 15 damage every second",
	};

	public readonly int[] prices = 
	{
		10, // Platform rect
		10, // Platform Triangle
		10, // platform circle
		25, // spinner
		25, // spinner
		40, // accel
		5, // spike
		45, // burner
		25, // turret
		60, // turret long
	};

	[Export]
	public PackedScene[] towers;
}
