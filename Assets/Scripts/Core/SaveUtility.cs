using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.VisualScripting;

public static class SaveUtility
{
	public static bool EnableDebugLog = false;

	public const string SaveFileExtension = ".sav";
	public static string SaveDirectory => Application.persistentDataPath;


	public static string[] GetFiles(string fullPath)
	{
		string directoryPath = Path.GetDirectoryName(fullPath);
		return Directory.GetFiles(directoryPath);
	} 

	public async static Task<bool> SaveData<T>(string fullPath, T data)
	{
		try
		{
			string directoryPath = Path.GetDirectoryName(fullPath);


			if (!Directory.Exists(directoryPath))
				Directory.CreateDirectory(directoryPath);

			if (File.Exists(fullPath))
			{
				Debug.LogWarning($"Overriding the save file at {fullPath}");
			}

			var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
			{
				TypeNameHandling = TypeNameHandling.All,
			});
			

			// JsonSerializer serializer = new JsonSerializer();
			// using (StreamWriter sw = new StreamWriter($@"{fullPath}"))
			// using (JsonWriter writer = new JsonTextWriter(sw))
			// {
			// 	serializer.Serialize(writer, data);
			// }


			await File.WriteAllTextAsync(fullPath, json);

#if UNITY_EDITOR
			if (EnableDebugLog)
				Debug.Log($"{typeof(T)} data saved at {fullPath}");
#endif

			return true;
		}
		catch (Exception e)
		{
			Debug.LogError(e);
			return false;
		}
	}


	public async static Task<bool> SaveData<T>(string folderName, string fileName, T data)
	{
		string fullPath = GetPath(folderName, fileName);
		return await SaveData(fullPath, data);
	}

	public async static Task<bool> SaveData(ISaveUtilityUser saveUtilityUser)
	{
		string fullPath = GetPath(saveUtilityUser.FolderName, saveUtilityUser.FileName);
		return await SaveData(fullPath, saveUtilityUser.GetSaveData());
	}

	public async static Task<LoadResult<T>> LoadData<T>(string fullPath)
	{
		try
		{
			if (File.Exists(fullPath))
			{
				var json = await File.ReadAllTextAsync(fullPath);

				T data = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings()
				{
					TypeNameHandling = TypeNameHandling.Auto,
				});

#if UNITY_EDITOR
				if (EnableDebugLog)
					Debug.Log($"{typeof(T)} data loaded from {fullPath}");
#endif
				return new LoadResult<T>(true, data);
			}

#if UNITY_EDITOR
			Debug.LogWarning($"There is no {typeof(T)} save data {fullPath}");
#endif


			return new LoadResult<T>(false, default(T));
		}
		catch (Exception e)
		{
			Debug.LogError(e);
		}

		return new LoadResult<T>(false, default(T));
	}

	public async static Task<LoadResult<T>> LoadData<T>(string folderName, string fileName)
	{
		string fullPath = GetPath(folderName, fileName);
		return await LoadData<T>(fullPath);
	}

	public async static Task<LoadResult<T>> LoadData<T>(ISaveUtilityUser saveUtilityUser)
	{
		string fullPath = GetPath(saveUtilityUser.FolderName, saveUtilityUser.FileName);
		return await LoadData<T>(fullPath);
	}


	public static void ResetData(string folderName, string fileName)
	{
		string fullPath = GetPath(folderName, fileName);
		ResetData(fullPath);
	}

	public static void ResetData(ISaveUtilityUser saveUtilityUser)
	{
		string fullPath = GetPath(saveUtilityUser.FolderName, saveUtilityUser.FileName);
		ResetData(fullPath);
	}

	public static void ResetData(string fullPath)
	{
		try
		{
			if (File.Exists(fullPath))
			{
				File.Delete(fullPath);
#if UNITY_EDITOR
				if (EnableDebugLog)
					Debug.Log($"Data at {fullPath} is deleted!");
#endif
			}
			else
			{
#if UNITY_EDITOR
				if (EnableDebugLog)
					Debug.LogWarning($"Data at {fullPath} is not exists!.");
#endif
			}
		}
		catch (Exception e)
		{
			Debug.LogError(e);
		}
	}

	public static string CombinePath(string path1, string path2)
	{
		return path1 + "/" + path2;
	}

	public static string GetPath(string folderName, string fileName)
	{
		return $"{SaveDirectory}/{folderName}/{fileName}.{SaveFileExtension}";
	}
}


public struct LoadResult<T>
{
	public bool Success;
	public T Data;

	public LoadResult(bool success, T data)
	{
		Success = success;
		Data = data;
	}
}
