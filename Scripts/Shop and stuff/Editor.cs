using Godot;
using System;

public partial class Editor : Node
{
    private int heldId = -1; // id of the tower that has been picked up

    private bool editable = false;

    [Signal]
    public delegate void EditModeExitEventHandler();

    public override void _Ready()
    {
        SpawnerScript spawner = GetNode<SpawnerScript>("../../../Spawner Portal");
        spawner.WaveEnd += EnterEditMode;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("exit edit mode") && !HavePickedUp())
        {
            editable = false;
            EmitSignal(SignalName.EditModeExit);
        }
    }

    private void EnterEditMode(int a, int b) // these arent used its just that the WaveEnd signal also brodcasts that info and i HAVE to accept it
    {
        editable = true;
    }

    public bool CanEdit()
    {
        return editable;
    }

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
