namespace SKC.AIF.Save
{
	public class SaveUpgrader
	{
		public void CheckAndUpgrade(SaveData saveData, int latestSaveVersion)
		{
			if (saveData.Version < latestSaveVersion)
			{
				// Your custom save upgrade implementation
			}
		}
	}
}
