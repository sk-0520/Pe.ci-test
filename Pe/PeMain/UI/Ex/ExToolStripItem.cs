namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.PeMain.Data;

	public abstract class ExToolStripItem: ToolStripItem
	{ }

	public abstract class ExToolStripMenuItem: ToolStripMenuItem
	{ }

	public abstract class CommonDataToolStripMenuItem: ExToolStripMenuItem
	{
		public CommonDataToolStripMenuItem(CommonData commonData)
			: base()
		{
			CommonData = commonData;
		}

		public CommonData CommonData { get; private set; }
	}

	public class FileToolStripMenuItem: CommonDataToolStripMenuItem
	{
		public FileToolStripMenuItem(CommonData commonData)
			: base(commonData)
		{ }

		public string Path { get; set; }
	}

	public class LauncherToolStripMenuItem: CommonDataToolStripMenuItem
	{
		public LauncherToolStripMenuItem(CommonData commonData)
			: base(commonData)
		{ }

		public LauncherItem LauncherItem { get; set; }
	}
}
