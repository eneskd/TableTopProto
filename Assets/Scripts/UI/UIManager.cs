public class UIManager : Singleton<UIManager>
{
	public UIInventory UIInventory;

	public void InitializeUI()
	{
		UIInventory.Initialize(LevelManager.I.Player.Inventory);
	}
}