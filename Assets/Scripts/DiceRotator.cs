using UnityEngine;

public class DiceRotator : MonoBehaviour
{
	[Header("References")]
	public Rigidbody Rigidbody;
	public GameObject[] FaceDetectors;

	[Header("Debug")]
	public int defaultFaceResult = -1;
	public int alteredFaceResult = -1;

	public DiceFaceRotations DiceData => DiceRollManager.I.DiceFaceRotations;


	/// <summary>
	/// For a possible object pooling system,
	/// we could reset the dice back and reuse it again
	/// </summary>
	public void Reset()
	{
		transform.localRotation = Quaternion.identity;
		defaultFaceResult = -1;
		alteredFaceResult = -1;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F12))
		{
			transform.localRotation = Quaternion.identity;
			defaultFaceResult = FindFaceResult();
			RotateDice(alteredFaceResult);
		}
	}

	/// <summary>
	/// Rotate the dice from the defaultFaceResult to alteredFaceResult
	/// </summary>
	/// <param name="alteredFaceResult"></param>
	public void RotateDice(int alteredFaceResult)
	{
		if (alteredFaceResult > 0 || alteredFaceResult <= 6)
		{
			this.alteredFaceResult = alteredFaceResult;
			Vector3 rotation = DiceData.FaceRelativeRotations[defaultFaceResult - 1].Rotation[alteredFaceResult - 1];
			transform.localRotation = Quaternion.Euler(rotation);
		}
		else
		{
			this.alteredFaceResult = defaultFaceResult;
		}
	}

	/// <summary>
	/// Find the result of the roll, the topmost face of the dice
	/// </summary>
	public int FindFaceResult()
	{
		//Since we have all child objects for each face,
		//We just need to find the highest Y value
		int maxIndex = 0;

		for (int i = 1; i < FaceDetectors.Length; i++)
		{
			if (FaceDetectors[maxIndex].transform.position.y < FaceDetectors[i].transform.position.y)
			{
				maxIndex = i;
			}
		}
		defaultFaceResult = maxIndex + 1;
		return defaultFaceResult;
	}
}
