using System;
using UnityEngine;

public class Pawn : MonoBehaviour
{
	[SerializeReference] public Mover Mover = new MoverWithArc();
	public Player Player;

	public Tile CurrentTile { get; protected set; }
	public bool IsMoving { get; protected set; } = false;


	protected Tile _movementTarget;

	protected Map _map => LevelManager.I.Map;


	public void MoveToNextTile(Action callback)
	{
		IsMoving = true;
		_movementTarget = _map.GetNextTile(CurrentTile.TileIndex);

		var landingPosition = _movementTarget.GetLandingPosition();


		Mover.Move(transform, transform.position, landingPosition, () => Moved(callback));
	}

	private void Moved(Action callback)
	{
		CurrentTile = _movementTarget;
		_movementTarget.TileTouchAction(this, null);
		IsMoving = false;
		callback?.Invoke();
	}

	internal void ExecuteTileAction(Action tileActionExecuted)
	{
		CurrentTile.TileLandAction(this, tileActionExecuted);
	}

	internal void PlaceAtStart()
	{
		CurrentTile = _map.Tiles[0];
		transform.position = CurrentTile.GetLandingPosition();
	}
}
