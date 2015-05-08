namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// ツールバーグループとして名前を管理。
	/// </summary>
	[Serializable]
	public class ToolbarGroupItem: NameItem, ICloneable
	{
		public ToolbarGroupItem()
		{
			ItemNames = new List<string>();
		}

		public List<string> ItemNames { get; set; }

		#region ICloneable

		public object Clone()
		{
			var result = new ToolbarGroupItem() {
				ItemNames = this.ItemNames,
			};
			result.ItemNames.AddRange(ItemNames);

			return result;
		}

		#endregion
	}
}
