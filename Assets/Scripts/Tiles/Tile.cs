using System;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
	[SerializeField] protected Transform _landingPosition;

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
		return _landingPosition.position;
	}

	public virtual void ExecuteTileStepAction(Pawn pawn, Action callback)
	{
		StartTileStepAction(pawn, callback);
	}

	public virtual void ExecuteTileLandAction(Pawn pawn, Action callback)
	{
		StartTileLandAction(pawn, callback);
	}

	protected abstract void StartTileStepAction(Pawn pawn, Action callback);


	protected abstract void StartTileLandAction(Pawn pawn, Action callback);

}
