using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDiceInputHandler : MonoBehaviour
{
	[Header("Variables")]
	public int InputCount = 2;
	public int MaxInputCount = 50;

	[Header("References")]
	[SerializeField] private TMP_Dropdown _dropdown;
	[SerializeField] private RectTransform _inputParent;
	[SerializeField] private UIDiceInput _inputPrefab;

	public List<UIDiceInput> Inputs { get; private set; } = new List<UIDiceInput>();


	private void Awake()
	{
		Inputs = new List<UIDiceInput>();
		_dropdown.onValueChanged.AddListener(OnValueChanged);
		_dropdown.ClearOptions();
		InitializeDropdown();
		HandleInputsFields();
	}
	private void OnDestroy()
	{
		_dropdown.onValueChanged.RemoveListener(OnValueChanged);
	}


	public List<int> GetInputs()
	{
		List<int> inputs = new List<int>();

		foreach (var inputField in Inputs)
		{
			inputs.Add(inputField.CurrentInput);
		}

		return inputs;
	}

	private void HandleInputsFields()
	{
		var deltaCount = InputCount - Inputs.Count;
		if (deltaCount < 0)
		{
			for (int i = 0; i < -deltaCount; i++)
			{
				var index = Inputs.Count - 1;
				Destroy(Inputs[index].gameObject);
				Inputs.RemoveAt(index);
			}
		}
		else
		{
			for (int i = 0; i < deltaCount; i++)
			{
				var inputField = Instantiate(_inputPrefab, _inputParent);
				Inputs.Add(inputField);
				inputField.SetIndex(Inputs.Count);
			}
		}
	}

	private void InitializeDropdown()
	{
		_dropdown.ClearOptions();

		List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
		for (int i = 0; i < MaxInputCount; i++)
		{
			options.Add(new TMP_Dropdown.OptionData((i + 1).ToString()));
		}

		_dropdown.AddOptions(options);
		_dropdown.SetValueWithoutNotify(InputCount - 1);
	}

	private void OnValueChanged(int index)
	{
		InputCount = index + 1;
		HandleInputsFields();
	}
}