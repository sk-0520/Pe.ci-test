using System;

namespace PeMain.UI
{
	partial class HomeForm
	{
		void Initialize()
		{
			#if DEBUG
			this.commandLauncher.Enabled = true;
			#endif
		}
	}
}
