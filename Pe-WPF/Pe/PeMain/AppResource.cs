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
	/// <para>細かい実装は PeMain/AppResource に分離。本ファイルでは定数定義のみにとどめる。</para>
	/// </summary>
	public static partial class AppResource
	{
		#region variable

		[AppResource(AppResourceType.Icon)]
		const string application="/Resources/Icon/App.ico";
		[AppResource(AppResourceType.Icon)]
		const string notFound = "/Resources/Icon/NotFound.ico";
		[AppResource(AppResourceType.Icon)]
		const string launcherToolbarMain = "/Resources/Icon/LauncherToolbarMain.ico";
		[AppResource(AppResourceType.Icon)]
		const string launcherCommand = "/Resources/Icon/LauncherCommand.ico";

		[AppResource(AppResourceType.Icon)]
		const string applicationTasktray
#if DEBUG
				= "/Resources/Icon/Tasktray/App-debug.ico";
#elif BETA
				= "/Resources/Icon/Tasktray/App-beta.ico";
#else
				= "/Resources/Icon/Tasktray/App-release.ico";
#endif
		[AppResource(AppResourceType.Image)]
		const string commonFiltering = "/Resources/Image/Common/Filtering.png";
		[AppResource(AppResourceType.Image)]
		const string commonTemplate = "/Resources/Image/Common/Template.png";
		[AppResource(AppResourceType.Image)]
		const string commonClipboard = "/Resources/Image/Common/Clipboard.png";
		[AppResource(AppResourceType.Image)]
		const string commonSend = "/Resources/Image/Common/Send.png";
		[AppResource(AppResourceType.Image)]
		const string commonPin = "/Resources/Image/Common/Pin.png";
		[AppResource(AppResourceType.Image)]
		const string commonWindowList = "/Resources/Image/Common/Pin.png";
		[AppResource(AppResourceType.Image)]
		const string commonAdd = "/Resources/Image/Common/Add.png";
		[AppResource(AppResourceType.Image)]
		const string commonRemove = "/Resources/Image/Common/Remove.png";
		[AppResource(AppResourceType.Image)]
		const string commonSave = "/Resources/Image/Common/Save.png";
		[AppResource(AppResourceType.Image)]
		const string commonUsingClipboard = "/Resources/Image/Common/UsingClipboard.png";
		[AppResource(AppResourceType.Image)]
		const string commonUp = "/Resources/Image/Common/Up.png";
		[AppResource(AppResourceType.Image)]
		const string commonDown = "/Resources/Image/Common/Down.png";
		

		public static string TemplatePlainPath { get { return "/Resources/Image/Template/TemplatePlain.png"; } }
		public static string TemplateReplacePath { get { return "/Resources/Image/Template/TemplateReplace.png"; } }
		public static string TemplateProgrammablePath { get { return "/Resources/Image/Template/TemplateProgrammable.png"; } }

		public static string ClipboardClearPath { get { return "/Resources/Image/Clipboard/Clear.png"; } }
		public static string ClipboardTextFormatPath { get { return "/Resources/Image/Clipboard/ClipboardText.png"; } }
		public static string ClipboardHtmlPath { get { return "/Resources/Image/Clipboard/ClipboardHtml.png"; } }
		public static string ClipboardRichTextFormatPath { get { return "/Resources/Image/Clipboard/ClipboardRichTextFormat.png"; } }
		public static string ClipboardImagePath { get { return "/Resources/Image/Clipboard/ClipboardImage.png"; } }
		public static string ClipboardFilePath { get { return "/Resources/Image/Clipboard/ClipboardFile.png"; } }
		public static string ClipboardImageFitPath { get { return "/Resources/Image/Clipboard/ImageFit.png"; } }
		public static string ClipboardImageRawPath { get { return "/Resources/Image/Clipboard/ImageRaw.png"; } }

		public static string WindowLoadPath { get { return "/Resources/Image/Window/WindowLoad.png"; } }
		public static string WindowSavePath { get { return "/Resources/Image/Window/WindowSave.png"; } }

		#endregion

		#region property

		public static BitmapSource TemplatePlainImage { get { return GetImage(TemplatePlainPath); } }
		public static BitmapSource TemplateReplaceImage { get { return GetImage(TemplateReplacePath); } }
		public static BitmapSource TemplateProgrammableImage { get { return GetImage(TemplateProgrammablePath); } }

		public static BitmapSource ClipboardClearImage { get { return GetImage(ClipboardClearPath); } }
		public static BitmapSource ClipboardTextFormatImage { get { return GetImage(ClipboardTextFormatPath); } }
		public static BitmapSource ClipboardRichTextFormatImage { get { return GetImage(ClipboardRichTextFormatPath); } }
		public static BitmapSource ClipboardHtmlImage { get { return GetImage(ClipboardHtmlPath); } }
		public static BitmapSource ClipboardImageImage { get { return GetImage(ClipboardImagePath); } }
		public static BitmapSource ClipboardFileImage { get { return GetImage(ClipboardFilePath); } }
		public static BitmapSource ClipboardImageFitImage { get { return GetImage(ClipboardImageFitPath); } }
		public static BitmapSource ClipboardImageRawImage { get { return GetImage(ClipboardImageRawPath); } }

		public static BitmapSource WindowSaveImage { get { return GetImage(WindowLoadPath); } }
		public static BitmapSource WindowLoadImage { get { return GetImage(WindowSavePath); } }
		#endregion


	}
}
