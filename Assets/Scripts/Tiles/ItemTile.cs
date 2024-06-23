
using System;
using TMPro;
using UnityEngine;

public class ItemTile : Tile
{
	public ItemData ItemData;
	public int Count = 10;
	public TextMeshPro Text;


	public override void Initialize()
	{
		base.Initialize();
		SetCount(UnityEngine.Random.Range(5, 20));
	}

	public void SetCount(int count)
	{
		Count = count;
		Text.text = count.ToString();
	}

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
	public override TileSav GetSaveData()
	{
		return new ItemTileSav(this);
	}

	public override void ApplySaveData(TileSav saveData)
	{
		if (saveData is ItemTileSav itemTileSav)
		{
			Apply(itemTileSav);
		}
		else
		{
			UnityEngine.Debug.LogError($"{saveData} is not correct type of {typeof(ItemTileSav)}");
		}
	}

	private void Apply(ItemTileSav itemTileSav)
	{
		Count = itemTileSav.Count;
		Text.text = Count.ToString();
	}
}


[Serializable]
public class ItemTileSav : TileSav
{
	public int Count;

	public ItemTileSav() { }
	public ItemTileSav(string tileType, int count) : base(tileType)
	{
		Count = count;
	}

	public ItemTileSav(ItemTile itemTile) : base(itemTile)
	{
		Count = itemTile.Count;
	}

}
