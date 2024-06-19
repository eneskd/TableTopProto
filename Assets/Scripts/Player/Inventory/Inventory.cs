using System.Collections.Generic;

public class Inventory : ISaveUtilityUser
{
	public List<ItemContainer> Items = new List<ItemContainer>();

	public string FileName => "Items";

	public string FolderName => "Inventory";

	public SaveData GetSaveData()
	{
		return new InventorySav(this);
	}
}

public class InventorySav : SaveData
{
	public List<ItemSav> Items;

	public InventorySav(List<ItemContainer> items)
	{
		Items = new List<ItemSav>();
		foreach (var item in items)
		{
			Items.Add(item.GetSaveData());
		}
	}

	public InventorySav(Inventory inventory) : this(inventory.Items) 
	{
		
	}
}