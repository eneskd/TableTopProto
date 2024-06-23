using System;
using UnityEngine;

public class Pawn : MonoBehaviour
{
	[SerializeField] private MoverWithArc _fowardMover = new MoverWithArc();
	[SerializeField] private MoverWithArc _returnMover = new MoverWithArc();
	[SerializeField] private AudioSource _stepSound;
	[SerializeField] private AudioSource _landingSound;


	public Player Player { get; protected set; }
	public Tile CurrentTile { get; protected set; }
	public bool IsMoving { get; protected set; } = false;
	public int RemainingSteps { get; protected set; } = 0;


	protected Tile _movementTarget;
	protected Map _map => LevelManager.I.Map;
	

	protected Action _stepCallback;
	protected Action _landingCallback;
	protected Action _actionFinishedCallback;


	public void InitializePawn(Player player, Tile startTile)
	{
		Player = player;
		CurrentTile = startTile;
		transform.position = CurrentTile.GetLandingPosition();
	}

	public void ExecutePawnMovement(int stepCout, Action stepCallback, Action landingCallback, Action actionFinishedCallback)
	{
		if (IsMoving)
		{
			Debug.LogError($"{this} pawn is already moving! Wait until action finished!");
			return;
		}


		RemainingSteps = stepCout;

		_stepCallback = stepCallback;
		_landingCallback = landingCallback;
		_actionFinishedCallback = actionFinishedCallback;

		StartSteps();
	}



	private void StartSteps()
	{
		IsMoving = true;
		
		StepToNextTile();
	}

	private void StepToNextTile()
	{
		IsMoving = true;
		_movementTarget = _map.GetNextTile(CurrentTile.TileIndex);

		var landingPosition = _movementTarget.GetLandingPosition();

		if (_movementTarget.TileIndex == 0)
			_returnMover.Move(transform, transform.position, landingPosition, Moved);
		else
			_fowardMover.Move(transform, transform.position, landingPosition, Moved);
	}

	private void Moved()
	{
		CurrentTile = _movementTarget;
		CurrentTile.ExecuteTileStepAction(this, null);
		_stepSound.Play();
		RemainingSteps--;
		_stepCallback?.Invoke();

		if (RemainingSteps > 0)
		{
			StepToNextTile();
		}
		else 
		{
			FinishMovement();
		}
	}

	private void FinishMovement()
	{
		IsMoving = false;
		_landingCallback?.Invoke();
		_landingSound.Play();
		ExecuteTileAction(TileActionExecuted);
	}

	private void ExecuteTileAction(Action tileActionExecuted)
	{
		CurrentTile.ExecuteTileLandAction(this, tileActionExecuted);
	}

	private void TileActionExecuted()
	{
		_actionFinishedCallback?.Invoke();
	}

	
}
