using System;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
	public float TileLength = 1f;
	public int TileIndex { get; protected set; }

	protected Map _map;

	public void SetMap(Map map)
	{
		_map = map;
	}

	public void SetTileIndex(int index)
	{
		TileIndex = index;
	}

	public virtual Vector3 GetLandingPosition()
	{
		return transform.position;
	}

	public abstract void TileTouchAction(Pawn pawn, Action callback);

	public abstract void TileLandAction(Pawn pawn, Action callback);

}
