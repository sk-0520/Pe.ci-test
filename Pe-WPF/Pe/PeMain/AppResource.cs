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
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	/// <summary>
	/// Peで使用するリソース関係。
	/// <para>自分の持ち物くらい好きにさわらせてくれ。</para>
	/// </summary>
	public static class AppResource
	{
		#region static

		static IconCaching<string> _iconCaching = new IconCaching<string>();
		static Caching<string,BitmapSource> _imageCaching = new Caching<string,BitmapSource>();

		#endregion

		#region variable

		public static string ApplicationIconPath { get { return "/Resources/Icon/App.ico"; } }
		public static string NotFoundIconPath { get { return "/Resources/Icon/NotFound.ico"; } }
		public static string LauncherToolbarMainIconPath { get { return "/Resources/Icon/LauncherToolbarMain.ico"; } }
		public static string LauncherCommandIconPath { get { return "/Resources/Icon/LauncherCommand.ico"; } }

		public static string FilteringPath { get { return "/Resources/Image/Filtering.png"; } }

		public static string TemplatePath { get { return "/Resources/Image/Template.png"; } }

		public static string TemplatePlainPath { get { return "/Resources/Image/TemplatePlain.png"; } }
		public static string TemplateReplacePath { get { return "/Resources/Image/TemplateReplace.png"; } }
		public static string TemplateProgrammablePath { get { return "/Resources/Image/TemplateProgrammable.png"; } }

		#endregion

		#region property

		public static BitmapSource TemplateImage { get { return GetImage(TemplatePath); } }

		public static BitmapSource TemplatePlainImage { get { return GetImage(TemplatePlainPath); } }
		public static BitmapSource TemplateReplaceImage { get { return GetImage(TemplateReplacePath); } }
		public static BitmapSource TemplateProgrammableImage { get { return GetImage(TemplateProgrammablePath); } }

		#endregion

		#region function

		static BitmapSource GetImage(string path)
		{
			return _imageCaching.Get(path, () => {
				var uri = SharedConstants.GetEntryUri(path);
				return new BitmapImage(uri);
			});
		}

		static BitmapSource GetIcon(string path, IconScale iconScale, ILogger logger = null)
		{
			return _iconCaching[iconScale].Get(path, () => {
				using(var icon = new IconWrapper(path, iconScale)) {
					return icon.MakeBitmapSource();
				}
			});
		}

		static public BitmapSource GetApplicationIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(ApplicationIconPath, iconScale, logger);
		}

		static public BitmapSource GetNotFoundIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(NotFoundIconPath, iconScale, logger);
		}

		static public BitmapSource GetLauncherToolbarMainIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(LauncherToolbarMainIconPath, iconScale, logger);
		}

		static public BitmapSource GetLauncherCommandIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(LauncherCommandIconPath, iconScale, logger);
		}

		#endregion
	}
}
