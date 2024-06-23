
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
	public int MapSize = 20;

	public List<Tile> Tiles = new List<Tile>();
	public Transform StartPoint;
	public const string MapSaveExtention = ".map";
	public static string MapSaveDirectory => Application.dataPath + "/Maps/";


	[ContextMenu("Get Data")]
	public void GetMapDatas()
	{
		var files = SaveUtility.GetFiles(MapSaveDirectory);

		foreach (var file in files)
		{
			if (file.EndsWith(".map"))
				Debug.Log(file);
		}
	}

	[ContextMenu("Save Map")]
	public void SaveMap()
	{
		var saveData = LevelManager.I.Map.GetSaveData();
		var path = MapSaveDirectory + saveData.MapName + ".map";
		_ = SaveUtility.SaveData(path, saveData);
	}


	public async Task<Map> LoadMap(string path)
	{
		var result = await SaveUtility.LoadData<MapSav>(path);
		if (result.Success)
		{
			return CreateMap(result.Data);
		}
		else
		{
			Debug.LogError($"Could not loaded map at {path}");
			return null;
		}
	}

	private Map CreateMap(MapSav data)
	{
		var tiles = new List<Tile>();
		var position = StartPoint.position;

		for (int i = 0; i < data.Tiles.Count; i++)
		{
			var tile = CreateTile(data.Tiles[i]);
			position += Vector3.right * tile.TileLength;
			tile.transform.parent = StartPoint.transform;
			tile.transform.position = position;
			tile.SetTileIndex(i);
			tiles.Add(tile);
		}

		return new Map(data.MapName, tiles);
	}

	private Tile CreateTile(TileSav tileSav)
	{
		var tilePrefab = Tiles.Find(x => x.TileType == tileSav.TileType);
		var tile = Instantiate(tilePrefab);
		tile.ApplySaveData(tileSav);

		return tile;
	}

	public async Task<Map> LoadDefaultMap()
	{
		var path = MapSaveDirectory + "Default" + ".map";
		return await LoadMap(path);
	}

	public Map GenerateRandomMap()
	{
		var tiles = new List<Tile>();
		var position = StartPoint.position;
		for (int i = 0; i < MapSize; i++)
		{
			var tilePrefab = Tiles.GetRandomElement();
			position += Vector3.right * tilePrefab.TileLength;
			var tile = Instantiate(tilePrefab, position, Quaternion.identity, StartPoint);
			tile.SetTileIndex(i);
			tile.Initialize();
			tiles.Add(tile);
		}

		var map = new Map("Random", tiles);

		return map;
	}

}
