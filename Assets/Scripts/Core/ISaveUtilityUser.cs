public interface ISaveUtilityUser
{
	public string FileName { get; }
	public string FolderName { get; }
	public SaveData GetSaveData();


}
