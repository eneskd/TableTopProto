public class ItemContainer
{
	public Item Item;
	public int ItemCount { get; protected set; } = 0;


	public ItemContainer(Item item, int itemCount = 0)
	{
		Item = item;
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



}