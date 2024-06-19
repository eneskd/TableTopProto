
using System;
using UnityEngine;

public class ItemTile : Tile
{
	public ItemData ItemData;
	public int Count = 10;
	
	public override void ExecuteTileLandAction(Pawn pawn, Action callback)
	{
		Debug.Log($"Landed tile {TileIndex}");
		callback?.Invoke();
	}

	public override void ExecuteTileStepAction(Pawn pawn, Action callback)
	{
		Debug.Log($"Touched tile {TileIndex}");
		callback?.Invoke();

	}
}
