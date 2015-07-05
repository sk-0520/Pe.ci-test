namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Controls;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class SelectedNodeAndTreeView
	{
		public IToolbarNode SelectedNode { get; set; }
		public TreeView TreeView { get; set; }
	}
}
