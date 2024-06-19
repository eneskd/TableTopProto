using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class Mover
{
	private Coroutine _moveCoroutine;

	public virtual void Move(Transform transform, Vector3 from, Vector3 to, Action callback = null)
	{
		_moveCoroutine = CoroutineHolder.I.StartCoroutine(MoveCoroutine(transform, from, to, callback));
	}

	public virtual void CancelMove()
	{
		if (_moveCoroutine != null)
		{
			CoroutineHolder.I.StopCoroutine(_moveCoroutine);
		}
	}

	protected abstract IEnumerator MoveCoroutine(Transform transform, Vector3 from, Vector3 to, Action callback);

}


[Serializable]
public class MoverWithArc : Mover
{
    public float Duration = 1f;
    public float Height = 1f;

    public AnimationCurve MovementCurve;
    public AnimationCurve HeightCurve;

    protected override IEnumerator MoveCoroutine(Transform transform, Vector3 from, Vector3 to, Action callback)
    {

        float t = 0f;
        while (t < Duration)
        {
            t += Time.deltaTime;
            float ratio = t / Duration;
            float arc = Mathf.Lerp(0, Height, HeightCurve.Evaluate(ratio));

            transform.position = Vector3.Lerp(from, to, MovementCurve.Evaluate(ratio)) + arc * Vector3.up;
            yield return null;
        }
        
        callback?.Invoke();
    }
}