namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// ツールバーの各グループを統括。
	/// </summary>
	[Serializable]
	public class ToolbarGroup: Item
	{
		public ToolbarGroup()
		{
			Groups = new List<ToolbarGroupItem>();
		}

		public List<ToolbarGroupItem> Groups { get; set; }
	}

}
