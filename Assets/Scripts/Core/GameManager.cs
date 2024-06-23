using System.Threading.Tasks;

public class GameManager : Singleton<GameManager>
{
	
	public ItemDatabase ItemDatabase;

	public async void  Start()
	{
		InitializeGame();

		await OpenLevel();
	}
	
	public void InitializeGame()
	{
		ItemDatabase.Initialize();
	}
	public async Task OpenLevel()
	{
		await LevelManager.I.InitializeLevel();
	}
}
