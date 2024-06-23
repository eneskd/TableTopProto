using System.Threading.Tasks;

public class LevelManager : Singleton<LevelManager>
{
	public Pawn PawnPrefab;

	public bool GenerateRandomMap;

	public Map Map { get; protected set; }
	public Player Player { get; protected set; }
	public Pawn Pawn { get; protected set; }

	public async Task InitializeLevel()
	{
		var map = await GenerateMap();
		GeneratePlayer();

		await LoadInventoryData();

		UIManager.I.InitializeUI();

	}

	private async Task<bool> LoadInventoryData()
	{
		return await Player.Inventory.LoadData();
	}

	public async Task<Map> GenerateMap()
	{
		if (GenerateRandomMap)
		{
			Map = MapGenerator.I.GenerateRandomMap();
		}
		else
		{
			Map = await MapGenerator.I.LoadDefaultMap();
		}

		return Map;
	}

	private void GeneratePlayer()
	{
		Pawn = Instantiate(PawnPrefab);
		var inventory = new Inventory();
		Player = new Player("Default", Pawn, inventory);

		Pawn.InitializePawn(Player, Map.Tiles[0]);
	}
}

