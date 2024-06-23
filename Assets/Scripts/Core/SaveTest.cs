using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour, ISaveUtilityUser
{
	public List<string> Strings = new List<string>();
	public List<float> Floats = new List<float>();
	public List<int> Ints = new List<int>();

	public string FileName => "Test";
	public string FolderName => "Test";

	
	[ContextMenu("Save")]
	public async void Save()
	{
		var data = GetSaveData();
		var success = await SaveUtility.SaveData(this);
		Debug.Log(success);
	}

	[ContextMenu("Load")]
	public async void Load()
	{
		var loadData = await SaveUtility.LoadData<TestSav>(this);
		if (loadData.Success)
		{
			Strings = loadData.Data.Strings;
			Floats = loadData.Data.Floats;
			Ints = loadData.Data.Ints;
		}
		else
		{
			Debug.LogWarning("Could not load");
		}
	}

	[ContextMenu("Reset")]
	public  void Reset()
	{
		SaveUtility.ResetData(this);
	}

	public SaveData GetSaveData()
	{
		return new TestSav(this);
	}
}

public class TestSav : SaveData
{
	public List<string> Strings = new List<string>();
	public List<float> Floats = new List<float>();
	public List<int> Ints = new List<int>();

	public TestSav()
	{

	}

	public TestSav(List<string> strings, List<float> floats, List<int> ints)
	{
		Strings = strings;
		Floats = floats;
		Ints = ints;
	}

	public TestSav(SaveTest saveTest)
	{
		Strings = saveTest.Strings;
		Floats = saveTest.Floats;
		Ints = saveTest.Ints;
	}
}
