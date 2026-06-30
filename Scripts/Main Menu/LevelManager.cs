using Godot;
using Godot.Collections;
using System;
using System.Runtime.CompilerServices;

public partial class LevelManager : Node
{
    private static LevelManager instance;

    [Export]
    private Node2D mainMenu;
    [Export]
    private LevelManagerConstants constants;
    private Node2D level;

    public override void _Ready()
    {
        instance = this;
    }

    public static void LoadLevel(int selected)
    {
        instance.RemoveChild(instance.mainMenu);
        instance.level = instance.constants.levels[selected].Instantiate<Node2D>();
        instance.AddChild(instance.level);
    }

    public static void ReturnToMainMenu()
    {
        instance.level.Free(); // ahh free at last, o gabriel now dawns thy reckoning
        instance.AddChild(instance.mainMenu); // and thy gore shall glisten before the temples of man
    }

}

// hack job singleton
// omg hack as in hack club???????
// i hope nobody ever reads these