using Godot;
using Godot.Collections;
using System;

public partial class LevelManagerConstants : Node
{
    [Export]
    public PackedScene[] levels;

    public string[] names =
    {
        ""
    };
}
