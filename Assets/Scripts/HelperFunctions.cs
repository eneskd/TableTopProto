using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions 
{
	public static int IndexOfItem<T>(this IEnumerable<T> collection, T item)
	{
		if (collection == null)
		{
			Debug.LogError("IndexOfItem Caused: source collection is null");
			return -1;
		}

		var index = 0;
		foreach (var i in collection)
		{
			if (Equals(i, item)) return index;
			++index;
		}

		return -1;
	}

	public static T GetRandomElement<T>(this List<T> list)
	{
		return list[Random.Range(0, list.Count)];
	}
}
