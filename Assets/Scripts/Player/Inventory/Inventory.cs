using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Inventory : ISaveUtilityUser
{
	public Dictionary<string, ItemContainer> ItemDictionary { get; protected set; } = new Dictionary<string, ItemContainer>();

	public delegate void InventoryChangeEventHandler(Inventory sender, InventoryChangeEventArgs args);

	public InventoryChangeEventHandler InventoryChanged;
	public InventoryChangeEventHandler InventoryIncreased;
	public InventoryChangeEventHandler InventoryDecreased;


	public string FileName => "Items";
	public string FolderName => "Inventory";

	public Inventory()
	{
		ItemDictionary = new Dictionary<string, ItemContainer>();
	}

	public void AddItem(Item item, int count)
	{
		if (!ItemDictionary.TryGetValue(item.ItemType, out ItemContainer container))
		{
			container = new ItemContainer(item.ItemType);
			ItemDictionary.Add(item.ItemType, container);

		}

		var oldCount = container.ItemCount;
		container.AddItem(count);
		var newCount = container.ItemCount;

		var args = new InventoryChangeEventArgs(item.ItemType, newCount, oldCount);

		InventoryIncreased?.Invoke(this, args);
		InventoryChanged?.Invoke(this, args);

		SaveData();
	}

	public bool RemoveItem(Item item, int count)
	{
		if (!ItemDictionary.TryGetValue(item.ItemType, out ItemContainer container))
			return false;

		var oldCount = container.ItemCount;

		bool success = container.RemoveItem(count);

		if (success)
		{
			var newCount = container.ItemCount;
			var args = new InventoryChangeEventArgs(item.ItemType, newCount, oldCount);

			InventoryDecreased?.Invoke(this, args);
			InventoryChanged?.Invoke(this, args);
		}

		SaveData();

		return success;
	}

	public SaveData GetSaveData()
	{
		return new InventorySav(ItemDictionary);
	}

	public async Task SaveData()
	{
		await SaveUtility.SaveData(this);
	}

	public async Task<bool> LoadData()
	{
		var result = await SaveUtility.LoadData<InventorySav>(this);

		if (result.Success)
		{
			ApplyData(result.Data);
		}

		return result.Success;
	}

	private void ApplyData(InventorySav data)
	{
		foreach (var item in data.Items)
		{
			ItemDictionary.Add(item.ItemType, new ItemContainer(item.ItemType, item.ItemCount));
		}
	}
}

public class InventorySav : SaveData
{
	public List<ItemSav> Items;

	public InventorySav(Dictionary<string, ItemContainer> items)
	{
		Items = new List<ItemSav>();
		foreach (var item in items)
		{
			Items.Add(item.Value.GetSaveData());
		}
	}
}

public class InventoryChangeEventArgs : EventArgs
{
	public string ItemType;
	public int NewCount;
	public int OldCount;
	public int Delta;

	public InventoryChangeEventArgs(string itemType, int newCount, int oldCount)
	{
		ItemType = itemType;
		NewCount = newCount;
		OldCount = oldCount;
		Delta = newCount - oldCount;
	}
}