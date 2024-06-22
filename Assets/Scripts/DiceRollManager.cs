using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceRollManager : Singleton<DiceRollManager>
{
	[Header("References")]
	public GameObject DicePrefab;
	public PhysicsSimulator PhysicsSimulator;
	public DiceFaceRotations DiceFaceRotations;
	public Transform DiceRollPosition;

	[Header("Debug")]
	public List<int> results = new List<int>() { 1, 2, 3, 4, 5, 6 };

	public List<DiceReferences> DiceDataList { get; private set; } = new List<DiceReferences>();

	private void Start()
	{
		PhysicsSimulator.PlaybackCompleted += PlaybackCompleted;
	}

	private void OnDestroy()
	{
		PhysicsSimulator.PlaybackCompleted -= PlaybackCompleted;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			ThrowTheDice(results);
		}
	}

	private void PlaybackCompleted()
	{
		var results = DiceDataList.Select(x => x.DiceRotator.FindFaceResult()).ToList();
		ActionManager.I.DicesRolled(results);
	}

	public void ThrowTheDice(List<int> targetResults)
	{
		ClearOldDices();
		GenerateDice(targetResults.Count);

		//Generate list of dices, then put it into the simulation
		List<GameObject> diceList = new List<GameObject>();
		for (int i = 0; i < targetResults.Count; i++)
		{
			diceList.Add(DiceDataList[i].DiceObject);
		}
		PhysicsSimulator.StartSimulation(diceList);

		//Record the dice roll result
		for (int i = 0; i < targetResults.Count; i++)
		{
			int result = DiceDataList[i].DiceRotator.FindFaceResult();
		}

		PhysicsSimulator.ResetToInitialState();

		// Rotate the dice to get target result
		for (int i = 0; i < targetResults.Count; i++)
		{
			DiceDataList[i].DiceRotator.RotateDice(targetResults[i]);
		}

		PhysicsSimulator.PlayRecording();
	}

	private void ClearOldDices()
	{
		for (int i = DiceDataList.Count - 1; i >= 0; i--)
		{
			Destroy(DiceDataList[i].DiceObject);
		}

		DiceDataList.Clear();
	}

	private void GenerateDice(int count)
	{
		for (int i = 0; i < count; i++)
		{
			DiceReferences newDiceData = new DiceReferences(Instantiate(DicePrefab));
			DiceDataList.Add(newDiceData);
		}


		//Set the position and rotation
		for (int i = 0; i < DiceDataList.Count; i++)
		{
			InitialState initial = SetInitialState();

			DiceDataList[i].DiceRotator.Reset();
			DiceDataList[i].DiceObject.transform.position = initial.Position;
			DiceDataList[i].DiceObject.transform.rotation = initial.Rotation;
			DiceDataList[i].Rb.useGravity = true;
			DiceDataList[i].Rb.isKinematic = false;
			DiceDataList[i].Rb.velocity = initial.Force;
			DiceDataList[i].Rb.AddTorque(initial.Torque, ForceMode.VelocityChange);
		}
	}

	private InitialState SetInitialState()
	{
		// Randomize X, Y, Z position in the bounding box
		float x = DiceRollPosition.position.x + Random.Range(-DiceRollPosition.localScale.x * 0.5f, DiceRollPosition.localScale.x * 0.5f);
		float y = DiceRollPosition.position.y + Random.Range(-DiceRollPosition.localScale.y * 0.5f, DiceRollPosition.localScale.y * 0.5f);
		float z = DiceRollPosition.position.z + Random.Range(-DiceRollPosition.localScale.z * 0.5f, DiceRollPosition.localScale.z * 0.5f);
		Vector3 position = new Vector3(x, y, z);

		x = Random.Range(0, 360);
		y = Random.Range(0, 360);
		z = Random.Range(0, 360);
		Quaternion rotation = Quaternion.Euler(x, y, z);

		x = Random.Range(-10, 10);
		y = Random.Range(0, 10);
		z = Random.Range(-10, 10);
		Vector3 force = new Vector3(x, -y, z);

		x = Random.Range(-10, 10);
		y = Random.Range(-10, 10);
		z = Random.Range(-10, 10);
		Vector3 torque = new Vector3(x, y, z);

		return new InitialState(position, rotation, force, torque);
	}


	/// <summary>
	/// The data containing all references to all dices
	/// so we only need to do GetComponent call once in the script
	/// </summary>
	[System.Serializable]
	public struct DiceReferences
	{
		public GameObject DiceObject;
		public DiceStates DiceStates;
		public DiceRotator DiceRotator;
		public Rigidbody Rb;


		public DiceReferences(GameObject diceObject)
		{
			DiceObject = diceObject;
			Rb = diceObject.GetComponent<Rigidbody>();
			DiceRotator = diceObject.transform.GetChild(0).GetComponent<DiceRotator>();
			DiceStates = diceObject.GetComponent<DiceStates>();
			Rb.maxAngularVelocity = 1000;
		}
	}

	/// <summary>
	/// This is a struct to hold all data needed to initialize the dice
	/// </summary>
	[System.Serializable]
	public struct InitialState
	{
		public Vector3 Position;
		public Quaternion Rotation;
		public Vector3 Force;
		public Vector3 Torque;

		public InitialState(Vector3 position, Quaternion rotation, Vector3 force, Vector3 torque)
		{
			Position = position;
			Rotation = rotation;
			Force = force;
			Torque = torque;
		}
	}
}
