using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionManager : Singleton<ActionManager>
{
	public ActionState ActionState
	{
		get => _actionState; 
		set
		{
			_actionState = value;
			ActionStateChanged?.Invoke(_actionState);
		}
	}

	public ActionState _actionState;


	public event Action<ActionState> ActionStateChanged;

	private LevelManager _levelManager => LevelManager.I;


	private void Update()
	{
		if (ActionState != ActionState.WaitingToRoll) return;
		if (Input.GetKeyDown(KeyCode.Space))
		{
			RollDices(DiceRollManager.I.results);
		}
	}

	public void RollDices(List<int> results)
	{
		ActionState = ActionState.Rolling;
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
		ActionState = ActionState.MovingPawn;
		pawn.ExecutePawnMovement(stepCount, StepCallback, LandingCallback, TileActionExecutedCallback);
	}

	private void StepCallback()
	{

	}

	private void LandingCallback()
	{
		ActionState = ActionState.ExecutingTileAction;
	}


	private void TileActionExecutedCallback()
	{
		ActionState = ActionState.WaitingToRoll;
	}


}

public enum ActionState
{
	WaitingToRoll = 0,
	Rolling = 1,
	MovingPawn = 2,
	ExecutingTileAction = 3,
}