using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Dice Data")]
public class DiceFaceRotations : ScriptableObject
{
	public List<FaceRelativeRotation> FaceRelativeRotations;

	[System.Serializable]
	public struct FaceRelativeRotation
	{
		public int Value;
		public List<Vector3> Rotation;
	}
}