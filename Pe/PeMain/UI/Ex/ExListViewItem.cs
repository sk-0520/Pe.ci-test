namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;

	public abstract class ExListViewItem: ListViewItem
	{ }

	public class PathListViewItem: ExListViewItem
	{
		public string Path { get; set; }
	}
}
