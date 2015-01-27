using System;
using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.UI.CustomControl
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
