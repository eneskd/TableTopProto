using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	public List<Tile> Tiles = new List<Tile>();

	public Map(List<Tile> tiles)
	{
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
}
