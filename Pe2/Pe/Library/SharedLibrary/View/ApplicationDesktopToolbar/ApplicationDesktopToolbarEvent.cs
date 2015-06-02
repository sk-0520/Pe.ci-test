namespace ContentTypeTextNet.Library.SharedLibrary.View
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// アプリケーションデスクトップツールバーから発行される独自イベント。
	/// </summary>
	public abstract class ApplicationDesktopToolbarEventArgs: EventArgs
	{ }

	/// <summary>
	/// フルスクリーンイベント。
	/// </summary>
	public class ApplicationDesktopToolbarFullScreenEventArgs: ApplicationDesktopToolbarEventArgs
	{
		public ApplicationDesktopToolbarFullScreenEventArgs(bool fullScreen)
		{
			FullScreen = fullScreen;
		}

		/// <summary>
		/// 検知したフルスクリーン状態。
		/// 
		/// 真でフルスクリーン。
		/// </summary>
		public bool FullScreen { get; private set; }
	}
}
