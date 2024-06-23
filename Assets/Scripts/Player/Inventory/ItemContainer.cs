using System;

public class ItemContainer
{
	public string ItemType;
	public int ItemCount { get; protected set; } = 0;

	public ItemContainer(string itemType, int itemCount = 0)
	{
		ItemType = itemType;
		ItemCount = itemCount;
	}

	public void AddItem(int count)
	{
		ItemCount += count;
	}

	public bool CanRemoveItem(int count)
	{
		return ItemCount >= count;
	}

	public bool RemoveItem(int count)
	{
		if (CanRemoveItem(count))
		{
			ItemCount -= count;
			return true;
		}

		return false;
	}

	public ItemSav GetSaveData()
	{
		return new ItemSav(this);
	}
}


[Serializable]
public class ItemSav : SaveData
{
	public string ItemType;
	public int ItemCount;

	public ItemSav() { }

	public ItemSav(string itemType, int itemCount)
	{
		ItemType = itemType;
		ItemCount = itemCount;
	}

	public ItemSav(ItemContainer itemContainer) : this(itemContainer.ItemType, itemContainer.ItemCount)
	{

	}
}

