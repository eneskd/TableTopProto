using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class WeightedList<T> : IEnumerable<T>
{
	private List<WeightedItem> _weightedItems = new List<WeightedItem>();

	public int Count => _weightedItems.Count;

	private float _weightSum;
	private Random _random = new Random();
	private bool _shouldRecalculateSum = true;

	public WeightedList(List<T> items, List<float> weights)
	{
		for (int i = 0; i < items.Count; i++)
		{
			WeightedItem newItem = new WeightedItem(items[i], weights[i]);
			_weightedItems.Add(newItem);
		}

		_random = new Random();
	}

	public WeightedList()
	{
		_random = new Random();
	}

	public virtual T GetItem(int index)
	{
		return _weightedItems[index].Item;
	}


	public virtual float GetWeight(int index)
	{
		return _weightedItems[index].Weight;
	}

	public virtual void AddItem(T item, float weight)
	{
		_weightedItems.Add(new WeightedItem(item, weight));
		_shouldRecalculateSum = true;
	}

	public virtual bool Contains(T item)
	{
		WeightedItem weightedItem = _weightedItems.FirstOrDefault(x => Equals(x.Item, item));
		return weightedItem is null;
	}

	public virtual void RemoveItem(T item)
	{
		WeightedItem weightedItem = GetWeightedItem(item);
		if (weightedItem == null) return;
		RemoveItem(weightedItem);
	}

	public virtual void RemoveAt(int index)
	{
		int i = index;
		int lastIndex = _weightedItems.Count - 1;

		_weightedItems[i] = _weightedItems[lastIndex];
		_weightedItems.RemoveAt(lastIndex);

		_shouldRecalculateSum = true;
	}


	public virtual T GetWeightedRandomItem()
	{
		if (_shouldRecalculateSum)
			CalculateWeightSum();


		int index = 0;
		int lastIndex = Count - 1;

		_random ??= new Random();

		// Unity's random does not work outside main thread. Casting to float might not give the expected distribution;
		// Should be good enough;
		float rng = (float)_random.NextDouble() * _weightSum;
		while (index <= lastIndex)
		{
			if (rng < _weightedItems[index].Weight)
			{
				return _weightedItems[index].Item;
			}

			rng -= _weightedItems[index].Weight;
			index++;
		}

		return _weightedItems[lastIndex].Item;
	}

	public virtual List<T> GetAllItems()
	{
		List<T> allItems = new List<T>();
		for (int i = 0; i < _weightedItems.Count; i++)
		{
			allItems.Add(_weightedItems[i].Item);
		}

		return allItems;
	}

	public virtual List<float> GetAllWeights()
	{
		List<float> allWeights = new List<float>();
		for (int i = 0; i < _weightedItems.Count; i++)
		{
			allWeights.Add(_weightedItems[i].Weight);
		}

		return allWeights;
	}

	public virtual List<T> GetWeightedRandomItems(int count)
	{
		if (Count <= count) return GetAllItems();
		List<T> randomItems = new List<T>(count);

		var newList = new WeightedList<T>(GetAllItems(), GetAllWeights());

		for (int i = 0; i < count; i++)
		{
			T choosenItem = newList.GetWeightedRandomItem();
			randomItems.Add(choosenItem);
			newList.RemoveItem(choosenItem);
		}

		return randomItems;
	}

	public virtual float GetWeightOfItem(T item)
	{
		return GetWeightedItem(item).Weight;
	}

	public List<float> GetWeightOfItems(List<T> items)
	{
		List<float> weights = new List<float>();
		foreach (var item in items)
		{
			weights.Add(GetWeightOfItem(item));
		}

		return weights;
	}

	public float GetWeightSum()
	{
		CalculateWeightSum();
		return _weightSum;
	}

	private void CalculateWeightSum()
	{
		_weightSum = 0;
		for (int i = 0; i < _weightedItems.Count; ++i)
		{
			_weightSum += _weightedItems[i].Weight;
		}
	}

	public int FindIndex(T item)
	{
		return _weightedItems.FindIndex(x => Equals(x.Item, item));
	}


	private WeightedItem GetWeightedItem(T item)
	{
		return _weightedItems.FirstOrDefault(x => Equals(x.Item, item));
	}

	private void RemoveItem(WeightedItem item)
	{
		int i = _weightedItems.IndexOfItem(item);
		int lastIndex = _weightedItems.Count - 1;

		_weightedItems[i] = _weightedItems[lastIndex];
		_weightedItems.RemoveAt(lastIndex);

		_shouldRecalculateSum = true;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return new WeightedListEnumerator(_weightedItems);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}


	[Serializable]
	private class WeightedItem
	{
		public T Item;

		[Range(0, 100)] public float Weight;

		public WeightedItem(T item, float weight)
		{
			Item = item;
			Weight = weight;
		}
	}

	private class WeightedListEnumerator : IEnumerator<T>
	{
		private List<WeightedItem> _weightedItems;

		int position = -1;

		public WeightedListEnumerator(List<WeightedItem> weightedItems)
		{
			_weightedItems = weightedItems;
		}

		public bool MoveNext()
		{
			position++;
			return (position < _weightedItems.Count);
		}

		public void Reset()
		{
			position = -1;
		}

		object IEnumerator.Current => Current;

		public T Current
		{
			get
			{
				try
				{
					return _weightedItems[position].Item;
				}
				catch (IndexOutOfRangeException)
				{
					throw new InvalidOperationException();
				}
			}
		}

		private void ReleaseUnmanagedResources()
		{
			_weightedItems = null;
		}


		protected virtual void Dispose(bool disposing)
		{
			ReleaseUnmanagedResources();
			if (disposing)
			{
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~WeightedListEnumerator()
		{
			Dispose(false);
		}
	}

}

