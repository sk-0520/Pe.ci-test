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

	public class LauncherItemSelecterEventArgs: EventArgs
	{}

	public abstract class LauncherItemSelecterItemEventArgs: LauncherItemSelecterEventArgs
	{
		public LauncherItem Item { get; set; }
	}

	public class CreateItemEventArg: LauncherItemSelecterItemEventArgs
	{ }

	public class RemovedItemEventArg: LauncherItemSelecterItemEventArgs
	{ }

	public class SelectedItemEventArg: LauncherItemSelecterItemEventArgs
	{ }
}
