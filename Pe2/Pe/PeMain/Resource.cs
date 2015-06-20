namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	/// <summary>
	/// Peで使用するリソース関係。
	/// <para>自分の持ち物くらい好きにさわらせてくれ。</para>
	/// </summary>
	public static class Resource
	{
		#region static

		static Caching<string, BitmapSource> imageCaching = new Caching<string, BitmapSource>();

		#endregion

		#region variable

		public const string applicationIconPath = "/Resources/Icon/App.ico";
		public const string launcherToolbarMainIconPath = "/Resources/Icon/LauncherToolbarMain.ico";
		
		#endregion

		#region property
		#endregion

		#region function

		static BitmapSource GetIcon(string path, IconScale iconScale, ILogger logger = null)
		{
			using(var icon = new IconWrapper(path, iconScale)) {
				return icon.MakeBitmapSource();
			}
		}

		static public BitmapSource GetApplicationIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(applicationIconPath, iconScale, logger);
		}

		static public BitmapSource GetLauncherToolbarMainIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(launcherToolbarMainIconPath, iconScale, logger);
		}

		#endregion
	}
}
