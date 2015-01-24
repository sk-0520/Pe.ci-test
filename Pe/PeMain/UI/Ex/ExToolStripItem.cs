namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public abstract class ExToolStripItem: ToolStripItem
	{ }

	public abstract class ExToolStripMenuItem: ToolStripMenuItem
	{ }

	public abstract class ExToolStripSplitButton: ToolStripSplitButton
	{ }

	public abstract class CommonDataToolStripMenuItem: ExToolStripMenuItem, ICommonData
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

	public class LauncherToolStripMenuItem: CommonDataToolStripMenuItem, ILauncherItem
	{
		public LauncherToolStripMenuItem(CommonData commonData)
			: base(commonData)
		{ }

		public LauncherItem LauncherItem { get; set; }
	}


	public abstract class CommonDataToolStripSplitButton: ExToolStripSplitButton, ICommonData
	{
		public CommonDataToolStripSplitButton(CommonData commonData)
			: base()
		{
			CommonData = commonData;
		}

		public CommonData CommonData { get; private set; }
	}

	public class LauncherToolStripSplitButton: CommonDataToolStripSplitButton, ILauncherItem
	{
		public LauncherToolStripSplitButton(CommonData commonData)
			: base(commonData)
		{ }

		public LauncherItem LauncherItem { get; set; }
	}

}
