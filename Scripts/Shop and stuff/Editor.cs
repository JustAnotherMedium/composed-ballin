using Godot;
using System;

public partial class Editor : Node
{
    private int heldId = -1;

    public void PickedUp(int newId)
    {
        heldId = (heldId == -1) ? newId : heldId; 
    }

    public bool HavePickedUp()
    {
        return heldId != -1;
    }

    public void LetGo()
    {
        heldId = -1;
    }

}
