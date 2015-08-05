namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

	public class StreamData
	{
		#region property

		public Process Process { get; set; }
		public LauncherItemModel LauncherItem { get; set; }

		#endregion
	}
}
