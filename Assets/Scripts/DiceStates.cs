using UnityEngine;

public class DiceStates : MonoBehaviour
{
    [Header("States")]
    public bool IsContactWithFloor;
    public bool IsContactWithDice;
    public bool IsInSimulation = true;
    public bool IsNotMoving = false;
    public bool IsTextureLit = false;

    [Header("References")]
    public DiceRotator DiceLogic;
    public AudioSource SoundCollideFloor;
    public AudioSource SoundCollideDice;

	public DiceFaceRotations DiceData => DiceRollManager.I.DiceFaceRotations;


	/// <summary>
	/// For a possible object pooling system,
	/// we could reset the dice back and reuse it again
	/// </summary>
	public void Reset()
    {
        IsContactWithFloor = false;
        IsContactWithDice = false;
        IsInSimulation = true;
        IsNotMoving = false;
        IsTextureLit = false;
    }


    #region Audio-Related Functions
    //This is to help the Animation Recorder capture the event
    //when the sound should be played
    public void PlaySoundRollLow()
    {
        if (!SoundCollideFloor.isPlaying)
             SoundCollideFloor.Play();
    }

    public void PlaySoundRollHigh()
    {
        if (!SoundCollideDice.isPlaying)
             SoundCollideDice.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Floor"))
        {
            IsContactWithFloor = true;
        }

        if (collision.transform.CompareTag("Dice"))
        {
            IsContactWithDice = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Floor"))
        {
            IsContactWithFloor = false;
        }

        if (collision.transform.CompareTag("Dice"))
        {
            IsContactWithDice = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Floor"))
        {
            IsContactWithFloor = false;
        }

        if (collision.transform.CompareTag("Dice"))
        {
            IsContactWithDice = false;
        }
    }
    #endregion
}
