namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using ContentTypeTextNet.Pe.PeMain.IF;

	/// <summary>
	/// ツールバーの各グループを統括。
	/// </summary>
	[Serializable]
	public class ToolbarGroup: Item, IDeepClone
	{
		public ToolbarGroup()
		{
			Groups = new List<ToolbarGroupItem>();
		}

		public List<ToolbarGroupItem> Groups { get; set; }

		#region IDeepClone

		public IDeepClone DeepClone()
		{
			var result = new ToolbarGroup();

			foreach(var item in Groups) {
				result.Groups.Add((ToolbarGroupItem)item.Clone());
			}

			return result;
		}

		#endregion
	}

}
