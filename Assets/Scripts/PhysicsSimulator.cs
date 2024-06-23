using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class can be made more flexible using an interface like IRecordable
// For sake of brevity  I will leave this as is.
public class PhysicsSimulator : MonoBehaviour
{
	[Header("Recording Variables")]
	[SerializeReference] private int _recordingFrameLength = 5 * 60;  //In frames
	[SerializeReference] private float _targetYPosition = -12.6f;

	public List<GameObject> ObjectsToRecord { get; private set; } = new List<GameObject>();
	public List<RecordingData> RecordingDataList { get; private set; } = new List<RecordingData>();

	public event System.Action PlaybackCompleted;


	//Debug
	private Coroutine _playbackCoroutine = null;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F11))
		{
			PlayRecording();
		}
	}

	public void StartSimulation(List<GameObject> targets)
	{
		if (_playbackCoroutine != null)
		{
			StopCoroutine(_playbackCoroutine);
			_playbackCoroutine = null;
		}

		RecordingDataList.Clear();
		ObjectsToRecord.Clear();
		ObjectsToRecord = targets;

		EnablePhysics();
		GetInitialState();
		Simulate();
	}

	// Get and Record initial states
	private void GetInitialState()
	{
		foreach (var gameObject in ObjectsToRecord)
		{
			Vector3 initialPosition = gameObject.transform.position;
			Quaternion initialRotation = gameObject.transform.rotation;

			Rigidbody rb = gameObject.GetComponent<Rigidbody>();
			rb.maxAngularVelocity = 1000;

			RecordingData data = new RecordingData(rb, initialPosition, initialRotation);
			RecordingDataList.Add(data);
		}
	}

	private void Simulate()
	{
		Physics.simulationMode = SimulationMode.Script;

		//Begin recording position and rotation for every frame
		for (int i = 0; i < _recordingFrameLength; i++)
		{
			//For every gameObject
			for (int j = 0; j < ObjectsToRecord.Count; j++)
			{
				Vector3 position = ObjectsToRecord[j].transform.position;
				Quaternion rotation = ObjectsToRecord[j].transform.rotation;

				bool isNotMoving = CheckObjectHasStopped(DiceRollManager.I.DiceDataList[j].Rb);

				// Unstuck the dice
				if (isNotMoving && position.y > _targetYPosition)
					DiceRollManager.I.DiceDataList[j].Rb.AddTorque(new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100)));

				bool isContactWithArena = DiceRollManager.I.DiceDataList[j].DiceStates.IsContactWithFloor;
				bool isContactWithDice = DiceRollManager.I.DiceDataList[j].DiceStates.IsContactWithDice;

				RecordedFrame frame = new RecordedFrame(position, rotation, isContactWithArena, isContactWithDice, isNotMoving);
				RecordingDataList[j].RecordedAnimation.Add(frame);
			}
			Physics.Simulate(Time.fixedDeltaTime);
		}

		Physics.simulationMode = SimulationMode.FixedUpdate;
	}

	public void PlayRecording()
	{
		if (_playbackCoroutine == null && RecordingDataList.Count > 0)
		{
			_playbackCoroutine = StartCoroutine(PlayAnimation());
		}
	}

	private IEnumerator PlayAnimation()
	{
		DisablePhysics();
		ResetToInitialState();

		int stopFrameCount = 0;
		//Play the animation frame by frame
		for (int i = 0; i < _recordingFrameLength; i++)
		{
			bool allDicesAreStopped = true;

			//For every objects
			for (int j = 0; j < RecordingDataList.Count; j++)
			{
				Vector3 position = RecordingDataList[j].RecordedAnimation[i].Position;
				Quaternion rotation = RecordingDataList[j].RecordedAnimation[i].Rotation;
				ObjectsToRecord[j].transform.position = position;
				ObjectsToRecord[j].transform.rotation = rotation;

				allDicesAreStopped = allDicesAreStopped && RecordingDataList[j].RecordedAnimation[i].IsNotMoving;
				//Play Sound whenever contact happens
				if (RecordingDataList[j].RecordedAnimation[i].IsContactWithArena)
				{
					// Play roll sound
				}
				if (RecordingDataList[j].RecordedAnimation[i].IsContactWithDice)
				{
					// Play contact sound
				}

				//When the dice stops rolling, lit the texture
				if (RecordingDataList[j].RecordedAnimation[i].IsNotMoving == true)
				{
					// Play stop sound
				}
			}
			yield return new WaitForFixedUpdate();

			if (allDicesAreStopped)
			{
				stopFrameCount++;
				if (stopFrameCount > 10)
				{
					Debug.Log($"Frame count {i}");
					break;
				}

			}
			else
			{
				stopFrameCount = 0;
			}
		}


		_playbackCoroutine = null;

		PlaybackFinished();
	}

	private void PlaybackFinished()
	{
		Debug.Log("Playback finished");
		PlaybackCompleted?.Invoke();
	}

	public bool CheckObjectHasStopped(Rigidbody rb)
	{
		return (rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero);
	}

	public void ResetToInitialState()
	{
		for (int i = 0; i < ObjectsToRecord.Count; i++)
		{
			ObjectsToRecord[i].transform.position = RecordingDataList[i].InitialPosition;
			ObjectsToRecord[i].transform.rotation = RecordingDataList[i].InitialRotation;
		}
	}

	public void EnablePhysics()
	{
		//Enable Rigidbody
		for (int i = 0; i < RecordingDataList.Count; i++)
		{
			RecordingDataList[i].Rb.useGravity = true;
			RecordingDataList[i].Rb.isKinematic = false;
		}
	}

	public void DisablePhysics()
	{
		//Disable Rigidbody
		for (int i = 0; i < RecordingDataList.Count; i++)
		{
			RecordingDataList[i].Rb.useGravity = false;
			RecordingDataList[i].Rb.isKinematic = true;
		}
	}


	[System.Serializable]
	public struct RecordedFrame
	{
		public Vector3 Position;
		public Quaternion Rotation;
		public bool IsContactWithArena;
		public bool IsContactWithDice;
		public bool IsNotMoving;

		public RecordedFrame(Vector3 position, Quaternion rotation, bool isContactWithArena, bool isContactWithDice, bool isNotMoving)
		{
			Position = position;
			Rotation = rotation;
			IsContactWithArena = isContactWithArena;
			IsContactWithDice = isContactWithDice;
			IsNotMoving = isNotMoving;
		}
	}

	[System.Serializable]
	public struct RecordingData
	{
		public Rigidbody Rb;
		public Vector3 InitialPosition;
		public Quaternion InitialRotation;
		public List<RecordedFrame> RecordedAnimation;

		public RecordingData(Rigidbody rb, Vector3 initialPosition, Quaternion initialRotation)
		{
			Rb = rb;
			InitialPosition = initialPosition;
			InitialRotation = initialRotation;
			RecordedAnimation = new List<RecordedFrame>();
		}
	}
}
