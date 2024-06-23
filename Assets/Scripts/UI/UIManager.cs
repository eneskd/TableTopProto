using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	[Header("References")]
	public UIInventory UIInventory;
	public UIDiceInputHandler UIDiceInputHandler;
	public Button RollButton;


	private void Start()
	{
		RollButton.onClick.AddListener(OnRollButtonClicked);
		ActionManager.I.ActionStateChanged += ActionStateChanged;
	}

	private void OnDestroy()
	{
		RollButton.onClick.RemoveListener(OnRollButtonClicked);
		ActionManager.I.ActionStateChanged -= ActionStateChanged;
	}

	public void InitializeUI()
	{
		UIInventory.Initialize(LevelManager.I.Player.Inventory);
	}

	private void ActionStateChanged(ActionState state)
	{
		RollButton.interactable = state == ActionState.WaitingToRoll;
	}

	private void OnRollButtonClicked()
	{
		var inputs = UIDiceInputHandler.GetInputs();
		ActionManager.I.RollDices(inputs);
	}
}