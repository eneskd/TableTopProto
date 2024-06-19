using System;

public class LevelManager : Singleton<LevelManager>
{
	public Pawn PawnPrefab;

	public Map Map { get; protected set; }
	public Player Player { get; protected set; }
	public Pawn Pawn { get; protected set; }


	private void Start()
	{
		InitializeLevel();
	}

	public void GenerateMap()
	{
		Map = MapGenerator.I.GenerateRandomMap();
	}

	public void InitializeLevel()
	{
		GenerateMap();
		GeneratePlayer();
	}

	private void GeneratePlayer()
	{
		Pawn = Instantiate(PawnPrefab);
		var inventory = new Inventory();
		Player = new Player("Default", Pawn, inventory);

		Pawn.PlaceAtStart();
	}
}

