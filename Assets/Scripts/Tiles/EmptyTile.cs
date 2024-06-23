using System;

public class EmptyTile : Tile
{
	protected override void StartTileLandAction(Pawn pawn, Action callback)
	{
		callback?.Invoke();
	}

	protected override void StartTileStepAction(Pawn pawn, Action callback)
	{
		callback?.Invoke();
	}

	public override TileSav GetSaveData()
	{
		return new EmptyTileSav(this);
	}

	public override void ApplySaveData(TileSav saveData)
	{
		if (saveData is EmptyTileSav emptyTileSav)
		{
			// Noting to apply here for now
		}
		else
		{
			UnityEngine.Debug.LogError($"{saveData} is not correct type of {typeof(EmptyTileSav)}");
		}
	}
}

[Serializable]
public class EmptyTileSav : TileSav
{
	public EmptyTileSav() { }

	public EmptyTileSav(string tileType) : base(tileType)
	{
	}

	public EmptyTileSav(EmptyTile tile) : base(tile)
	{
	}
}