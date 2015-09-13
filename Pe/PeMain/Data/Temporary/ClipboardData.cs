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

	public class ClipboardData
	{
		public ClipboardData()
		{
			Type = ClipboardType.None;
			Hash = new HashItemModel();
			Body = new ClipboardBodyItemModel();
		}

		#region property

		public ClipboardType Type { get; set; }
		public HashItemModel Hash { get; set; }
		public ClipboardBodyItemModel Body { get; set; }

		#endregion
	}
}
