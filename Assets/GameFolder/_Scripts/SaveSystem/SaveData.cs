using System.Collections.Generic;

namespace SKC.AIF.Save
{
	public class SaveData
	{
		public int Version;
		public Dictionary<string, object> Saves = new Dictionary<string, object>();
	}
}
