using System;
using TMPro;
using UnityEngine;

public class UIDiceInput : MonoBehaviour
{
	[Range(1, 6)] public int CurrentInput = 3;
	[SerializeField] private TextMeshProUGUI _diceNumberText;
	[SerializeField] private TMP_InputField _inputField;

	private void Awake()
	{
		_inputField.onEndEdit.AddListener(OnValueChanged);
		_inputField.text = CurrentInput.ToString();
	}

	private void OnDestroy()
	{
		_inputField.onEndEdit.RemoveListener(OnValueChanged);
	}

	public bool ValidetInput(string input)
	{
		if (int.TryParse(input, out int value))
		{
			if (value > 0 && value <= 6)
			{
				CurrentInput = value;
				return true;
			}
			else
			{
				CurrentInput = Mathf.Clamp(value, 1, 6);
				_inputField.text = CurrentInput.ToString();
				return true;
			}
		}

		return false;
	}

	internal void SetIndex(int index)
	{
		_diceNumberText.SetText($"Dice {index}:");
	}



	private void OnValueChanged(string input)
	{
		if (!ValidetInput(input))
			_inputField.text = CurrentInput.ToString();
	}
}
