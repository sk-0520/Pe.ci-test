using ContentTypeTextNet.Library.SharedLibrary.View.ApplicationDesktopToolbar;
namespace ContentTypeTextNet.Library.SharedLibrary.View
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	public class ApplicationDesktopToolbar: Window
	{
		public ApplicationDesktopToolbar():
			base()
		{ }

		#region event

		public event EventHandler<ApplicationDesktopToolbarFullScreenEventArgs> ApplicationDesktopToolbarFullScreen = delegate { };

		/// <summary>
		/// ApplicationDesktopToolbarFullScreenを発行。
		/// </summary>
		/// <param name="fullScreen"></param>
		protected void OnAppbarFullScreen(bool fullScreen)
		{
			var e = new ApplicationDesktopToolbarFullScreenEventArgs(fullScreen);
			NowFullScreen = e.FullScreen;
			ApplicationDesktopToolbarFullScreen(this, e);
		}

		#endregion

		#region property

		/// <summary>
		/// 現在フルスクリーンか。
		/// </summary>
		public bool NowFullScreen { get; private set; }

		/// <summary>
		/// バー登録に使用するメッセージ文字列。
		/// </summary>
		public string MessageString { get; set; }

		#endregion
	}
}
