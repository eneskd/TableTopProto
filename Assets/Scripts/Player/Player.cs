public class Player
{
	public string PlayerId { get; protected set; }
	public Pawn Pawn { get; protected set; }
	public Inventory Inventory { get; protected set; }


	public Player(string playerId, Pawn pawn, Inventory inventory)
	{
		PlayerId = playerId;
		Pawn = pawn;
		Inventory = inventory;
	}
}