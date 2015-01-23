namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	public struct DropData
	{
		public DropType DropType { get; set; }
		public ToolStripItem ToolStripItem { get; set; }
		public LauncherItem LauncherItem { get; set; }
		public IEnumerable<string> Files { get; set; }
		public ToolStripItem SrcToolStripItem { get; set; }
	}
}
