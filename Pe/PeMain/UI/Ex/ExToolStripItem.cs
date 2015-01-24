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

	public class FileToolStripMenuItem: ExToolStripMenuItem
	{
		public FileToolStripMenuItem(CommonData commonData, string filePath)
			: base()
		{
			CommonData = commonData;
			FilePath = filePath;
		}

		public CommonData CommonData { get; private set; }
		public string FilePath { get; private set; }
	}
}
