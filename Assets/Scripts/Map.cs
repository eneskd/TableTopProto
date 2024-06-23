using System;
using System.Collections.Generic;

[System.Serializable]
public class Map
{
	public string MapName;
	public List<Tile> Tiles = new List<Tile>();

	public Map(string mapName, List<Tile> tiles)
	{
		MapName = mapName;
		Tiles = tiles;

		foreach (Tile tile in Tiles)
		{
			tile.SetMap(this);
		}
	}

	public Tile GetNextTile(int currentTileIndex)
	{
		if (currentTileIndex < Tiles.Count - 1) return Tiles[++currentTileIndex];
		return Tiles[0];
	}

	public MapSav GetSaveData()
	{
		return new MapSav(this);
	}
}

public class MapSav : SaveData
{
	public string MapName;
	public List<TileSav> Tiles;

	public MapSav() {}
	

	public MapSav(string mapName, List<Tile> tiles)
	{
		MapName = mapName;
		Tiles = new List<TileSav>();

		foreach (var tile in tiles)
		{
			Tiles.Add(tile.GetSaveData());
		}
	}

	public MapSav(Map map) : this(map.MapName, map.Tiles)
	{
	}
}
