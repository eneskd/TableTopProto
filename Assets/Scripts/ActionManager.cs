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
			var d1 = UnityEngine.Random.Range(0, 6);
			var d2 = UnityEngine.Random.Range(0, 6);

			var total = d1 + d2;

			MovePawnToTarget(_levelManager.Player.Pawn, total);
		}
	}


	private void MovePawnToTarget(Pawn pawn, int stepCount)
	{
		_actionState = ActionState.MovingPawn;
		pawn.ExecutePawnMovement(stepCount, StepCallback, LandingCallback, TileActionExecutedCallback);
	}

	public void StepCallback()
	{

	}

	public void LandingCallback()
	{
		_actionState = ActionState.ExecutingTileAction;
	}


	public void TileActionExecutedCallback()
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