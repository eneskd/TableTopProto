using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionManager : Singleton<ActionManager>
{
	private ActionState _actionState = ActionState.WaitingToRoll;

	public ActionState GetCurrentActionState() => _actionState;

	private LevelManager _levelManager => LevelManager.I;


	private void Update()
	{
		if (_actionState != ActionState.WaitingToRoll) return;
		if (Input.GetKeyDown(KeyCode.Space))
		{
			RollDices(DiceRollManager.I.results);
		}
	}

	public void RollDices(List<int> results)
	{
		_actionState = ActionState.Rolling;
		DiceRollManager.I.ThrowTheDice(results);
	}

	public void DicesRolled(List<int> results)
	{
		var sum = results.Sum();

		Debug.Log(sum);
		MovePawnToTarget(_levelManager.Player.Pawn, sum);
	}

	private void MovePawnToTarget(Pawn pawn, int stepCount)
	{
		_actionState = ActionState.MovingPawn;
		pawn.ExecutePawnMovement(stepCount, StepCallback, LandingCallback, TileActionExecutedCallback);
	}

	private void StepCallback()
	{

	}

	private void LandingCallback()
	{
		_actionState = ActionState.ExecutingTileAction;
	}


	private void TileActionExecutedCallback()
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