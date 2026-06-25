using Godot;
using System;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;

public partial class ShopScript : Node
{
	[ExportCategory("Other Scripts")]
	[Export]
	private ShopConstants shopConstants;
	private Editor editor;
	private PlayerVars playerVars;

	[ExportCategory("Shop screens")]
	[Export]
	private Control playerUI;
	[Export]
	private Control shop;

	[ExportCategory("Shop nodes")]
	[Export]
	private Label money;
	[Export]
	private TabContainer tabs;
	[Export]
	private Label[] names;
	[Export]
	private Label[] prices;
	[Export]
	private Label[] descriptions;
	[Export]
	private Label buyButton;

	private int selected = -1;
	private bool shopOpen = false;
	private int previousTab = 0;

	private int[,] tabIndexMap =
	{
		{0, 1, 2, -1}, // Platforms
		{3, 4, 5, -1}, // Mobility
		{6, 7, 8, 9}, // Attack
		{10, 11, 12, -1} // Support
	};

    public override void _Ready()
    {
        playerVars = GetNode<PlayerVars>("../Player UI");
		editor = GetNode<Editor>("../../Editor");

		SpawnerScript spawner = GetNode<SpawnerScript>("../../Spawner Portal");

        spawner.ShopSetup += EnableShop;
		editor.EditModeExit += DisableShop;
    }

    public override void _Process(double delta)
    {
        playerUI.Visible = editor.CanEdit() && !shopOpen;
		money.Text = playerVars.Money + " $ ";

		shop.SetPosition(new Vector2(shopOpen ? 0 : -shop.Size.X, 0));
    }

	private void EnableShop()
	{
		playerUI.Visible = true;
	}

	private void DisableShop()
	{
		shopOpen = playerUI.Visible = shop.Visible = false;

	}

	private void ToggleShop()
	{
		shopOpen = !shopOpen;
		shop.Visible = shopOpen;
	}

	private void ClearSelected(int tab)
	{
		selected = -1;
		
		names[previousTab].Text = " Nothing selected";
		prices[previousTab].Text = "0 $ ";
		descriptions[previousTab].Text = "";
		buyButton.Text = "Click to select";

		previousTab = tab;
	}

	private void BuyObject()
	{
		if (selected >= 0)
		{
			int price = shopConstants.prices[selected];
			if (playerVars.Money >= price)
			{
				playerVars.SpendMoney(price);
				editor.SpawnTower(selected);
			}
		}
	}

	private void Button(int button)
	{
		selected = tabIndexMap[tabs.CurrentTab, button];

		names[tabs.CurrentTab].Text = " " + shopConstants.names[selected];
		prices[tabs.CurrentTab].Text = shopConstants.prices[selected] + " $ ";
		descriptions[tabs.CurrentTab].Text = shopConstants.descriptions[selected];
		buyButton.Text = "BUY " + shopConstants.names[selected] + " for $" + shopConstants.prices[selected];
	}

	private void Button1()
	{
		Button(0);
	}

	private void Button2()
	{
		Button(1);
	}

	private void Button3()
	{
		Button(2);
	}

	private void Button4()
	{
		Button(3);
	}
}
