namespace ContentTypeTextNet.Library.SharedLibrary.Event
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class AppbarFullScreenEventArgs: AppbarEventArgs
	{
		public AppbarFullScreenEventArgs(bool fullScreen)
		{
			FullScreen = fullScreen;
		}

		public bool FullScreen { get; private set; }
		public bool Handled { get; set; }
	}
}
