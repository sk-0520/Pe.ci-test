namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class SelectedNodeAndLauncherItem
	{
		#region property

		public IToolbarNode SelectedNode { get; set; }
		public LauncherItemModel LauncherItem { get; set; }

		#endregion
	}
}
