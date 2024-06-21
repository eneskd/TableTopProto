
using System;
using UnityEngine;

public class ItemTile : Tile
{
	public ItemData ItemData;
	public int Count = 10;
	
	protected override void StartTileLandAction(Pawn pawn, Action callback)
	{
		pawn.Player.Inventory.AddItem(ItemData.Item, Count);
		Debug.Log($"Landed tile {TileIndex}, Adding {Count} {ItemData.Item.ItemType}");
		callback?.Invoke();
	}

	protected override void StartTileStepAction(Pawn pawn, Action callback)
	{
		Debug.Log($"Touched tile {TileIndex}");
		callback?.Invoke();
	}
}
