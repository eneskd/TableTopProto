
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
	public int MapSize = 20;

	public List<Tile> Tiles = new List<Tile>();
	public Transform StartPoint;

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
			tiles.Add(tile);

		}

		var map = new Map(tiles);

		return map;
	}
}
