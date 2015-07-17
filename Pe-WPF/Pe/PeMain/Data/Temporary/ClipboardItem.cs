namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class ClipboardItem
	{
		public ClipboardItem()
		{
			Type = ClipboardType.None;
			Body = new ClipboardBodyItemModel();
		}

		#region property

		public ClipboardType Type { get; set; }
		public ClipboardBodyItemModel Body { get; set; }

		#endregion
	}
}
