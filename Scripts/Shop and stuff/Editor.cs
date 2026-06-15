using Godot;
using System;

public partial class Editor : Node
{
    private int heldId = -1; // id of the tower that has been picked up


    
    // When an item is picked up
    public void PickedUp(int newId) // One of them manages to run this method first
    {
        heldId = (heldId == -1) ? newId : heldId; 
    }

    public bool HavePickedUp() // The others check that an item is already held and ignore the click
    {
        return heldId != -1;
    }

    public void LetGo() // this is used to clear the pick up
    {
        heldId = -1;
    }

}
