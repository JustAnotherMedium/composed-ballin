using Godot;
using System;

public partial class Editor : Node
{
    private int heldId = -1; // id of the tower that has been picked up
    private int currentHighestID = 0; // id is guarenteed to be unused by any tower
    private bool editable = true;
	private ShopConstants shopConstants;

    [Signal]
    public delegate void EditModeExitEventHandler();

    public override void _Ready()
    {
        shopConstants = GetNode<ShopConstants>("../UI/Shop/ShopUi");

        SpawnerScript spawner = GetNode<SpawnerScript>("../Spawner Portal");
        spawner.ShopSetup += EnterEditMode;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("exit edit mode") && !HavePickedUp())
        {
            editable = false;
            EmitSignal(SignalName.EditModeExit);
        }
    }

    private void EnterEditMode()
    {
        editable = true;
    }

    public bool CanEdit()
    {
        return editable;
    }

    public void SpawnTower(int index)
    {
        PackedScene scene = shopConstants.towers[index];
        TowersScript tower = scene.Instantiate<TowersScript>();
        tower.TowerInit(this, currentHighestID);
        AddChild(tower);
        tower.GlobalPosition = GetViewport().GetCamera2D().GlobalPosition;
        currentHighestID++;
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
