using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
	[SerializeField] private UIInventoryItem _uIInventoryItemPrefab;
	[SerializeField] private Transform _uIInventoryItemParent;

	private Dictionary<string, UIInventoryItem> _uiInventoryDictionary;
	private Inventory Inventory;

	protected ItemDatabase _itemDatabase => GameManager.I.ItemDatabase;


	public void Initialize(Inventory inventory)
	{
		Inventory = inventory;
		_uiInventoryDictionary = new Dictionary<string, UIInventoryItem>();

		foreach (var pair in Inventory.ItemDictionary)
		{
			CreateUIInventoryItem(pair.Key, pair.Value.ItemCount);
		}

		Inventory.InventoryChanged += OnInventoryChanged;
		Inventory.InventoryIncreased += OnInventoryIncreased;
		Inventory.InventoryDecreased += OnInventoryDecreased;
	}

	private void OnDestroy()
	{
		Inventory.InventoryChanged -= OnInventoryChanged;
		Inventory.InventoryIncreased -= OnInventoryIncreased;
		Inventory.InventoryDecreased -= OnInventoryDecreased;
	}

	private UIInventoryItem CreateUIInventoryItem(string itemType, int count)
	{
		if (!_itemDatabase.TryGetItem(itemType, out var item)) return null;

		var inventoryItem = Instantiate(_uIInventoryItemPrefab, _uIInventoryItemParent);
		inventoryItem.InitializeItem(item);
		_uiInventoryDictionary.Add(item.ItemType, inventoryItem);
		inventoryItem.UpdateCount(count);
		return inventoryItem;
	}


	private void OnInventoryDecreased(Inventory sender, InventoryChangeEventArgs args)
	{

	}

	private void OnInventoryIncreased(Inventory sender, InventoryChangeEventArgs args)
	{

	}

	private void OnInventoryChanged(Inventory sender, InventoryChangeEventArgs args)
	{
		if (!_uiInventoryDictionary.TryGetValue(args.ItemType, out UIInventoryItem inventoryItem))
		{
			inventoryItem = CreateUIInventoryItem(args.ItemType, args.NewCount);
		}

		inventoryItem.UpdateCount(args.NewCount);
	}


}
