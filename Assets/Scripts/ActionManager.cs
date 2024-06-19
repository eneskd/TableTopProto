using System;
using System.Collections;
using UnityEngine;

public class ActionManager : Singleton<ActionManager>
{
	private ActionState _actionState = ActionState.WaitingToRoll;

	public ActionState GetCurrentActionState() => _actionState;


	private void Update()
	{
		if (_actionState != ActionState.WaitingToRoll) return;
		if (Input.GetKeyDown(KeyCode.Space))
		{
			var d1 = UnityEngine.Random.Range(0, 6);
			var d2 = UnityEngine.Random.Range(0, 6);

			var total = d1 + d2;

			MovePawnToTarget(total);
		}
	}


	public void MovePawnToTarget(int count)
	{
		_actionState = ActionState.MovingPawn;
		StartCoroutine(PawnMovementCoroutine(LevelManager.I.Player.Pawn, count));

	}

	private IEnumerator PawnMovementCoroutine(Pawn pawn, int count)
	{
		while (true)
		{
			pawn.MoveToNextTile(MovedToTarget);
			count--;

			yield return new WaitUntil(() => !pawn.IsMoving);

			if (count <= 0) break;
		}

		FinishPawnMovement(pawn);
	}

	private void FinishPawnMovement(Pawn pawn)
	{
		_actionState = ActionState.ExecutingTileAction;
		pawn.ExecuteTileAction(TileActionExecuted);

	}

	public void MovePawnToNextTile(Action callback)
	{
		LevelManager.I.Pawn.MoveToNextTile(callback);
	}


	public void MovedToTarget()
	{

	}

	public void TileActionExecuted()
	{
		_actionState = ActionState.WaitingToRoll;
	}
}

public enum ActionState
{
	WaitingToRoll = 0,
	Rolling = 1,
	MovingPawn = 2,
	ExecutingTileAction = 3,
}