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
}