using System;
using System.Collections;
using UnityEngine;

public static class HelperCoroutines
{
	public static IEnumerator MoveObjectFromToTarget(Transform transform, Vector3 startLocation, Vector3 targetLocation, float duration, AnimationCurve animationCurve, Action callback)
	{
		float t = 0f;
		transform.position = startLocation;
		while (t < duration)
		{
			t += Time.deltaTime;
			float ratio = t / duration;

			transform.position = Vector3.Lerp(startLocation, targetLocation, animationCurve.Evaluate(ratio));

			yield return null;
		}

		transform.position = targetLocation;

		callback?.Invoke();
	}

	public static IEnumerator DelayedCallback(float delay, Action callback, bool unscaledTime = false)
	{
		if (unscaledTime)
			yield return new WaitForSecondsRealtime(delay);
		else
			yield return new WaitForSeconds(delay);

		callback?.Invoke();
	}
}