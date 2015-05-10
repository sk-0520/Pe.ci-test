namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Linq;
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

			if(Groups.Any()) {
				result.Groups.AddRange(Groups.Select(tgi => tgi.Clone()).Cast<ToolbarGroupItem>());
			}

			return result;
		}

		#endregion
	}

}
