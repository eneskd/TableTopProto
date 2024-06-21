using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Items/Item Database")]
public class ItemDatabase : ScriptableObject
{
	[SerializeField] private List<ItemData> _items = new List<ItemData>();

	private Dictionary<string, Item> _itemDictionary;

	public void Initialize()
	{
		_itemDictionary = new Dictionary<string, Item>();

		foreach (var item in _items)
		{
			_itemDictionary.Add(item.Item.ItemType, item.Item);
		}
	}

	public bool TryGetItem(string itemType, out Item item)
	{
		if (_itemDictionary.ContainsKey(itemType))
		{
			item = _itemDictionary[itemType];
			return true;
		}

		Debug.LogError($"Item of type {itemType} could not be found in the Item Database.");

		item = new Item();
		return false;
	}
}
