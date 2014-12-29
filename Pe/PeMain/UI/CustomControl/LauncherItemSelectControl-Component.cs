using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	public enum LauncherItemSelecterType
	{
		Full,
		Name,
		Tag
	}

	public abstract class ItemEventArgs: EventArgs
	{
		public LauncherItem Item { get; set; }
	}
	public class CreateItemEventArg: ItemEventArgs
	{
	}

	public class RemovedItemEventArg: ItemEventArgs
	{
	}

	public class SelectedItemEventArg: ItemEventArgs
	{
	}
}
