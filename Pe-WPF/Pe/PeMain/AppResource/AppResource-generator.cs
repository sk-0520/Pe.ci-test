
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

	partial class AppResource
	{
		/*
		このソースは自動生成のため AppResource-generator.tt を編集すること。

		生成元フィールド数: 30
		*/
		#region Icon: Application

		/// <summary>
		/// [Icon] Applicationのリソースパス。
		/// <para>/Resources/Icon/App.ico</para>
		/// </summary>
		public static string ApplicationPath
		{
			get { return application; }
		}

		/// <summary>
		/// [Icon] Applicationのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <param name="iconScale">アイコンサイズ</param>
		/// <param name="logger">ログ。不要であれば null を指定(するか引数を与えない)。</param>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource GetApplicationIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(ApplicationPath, iconScale, logger);
		}

		#region IconScale

		/// <summary>
		/// [Icon] Applicationのイメージソース(IconScale.Small)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationIconSmall
		{
			get
			{
				return GetApplicationIcon(IconScale.Small);
			}
		}
		/// <summary>
		/// [Icon] Applicationのイメージソース(IconScale.Normal)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationIconNormal
		{
			get
			{
				return GetApplicationIcon(IconScale.Normal);
			}
		}
		/// <summary>
		/// [Icon] Applicationのイメージソース(IconScale.Big)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationIconBig
		{
			get
			{
				return GetApplicationIcon(IconScale.Big);
			}
		}
		/// <summary>
		/// [Icon] Applicationのイメージソース(IconScale.Large)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationIconLarge
		{
			get
			{
				return GetApplicationIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion application
		#region Icon: NotFound

		/// <summary>
		/// [Icon] NotFoundのリソースパス。
		/// <para>/Resources/Icon/NotFound.ico</para>
		/// </summary>
		public static string NotFoundPath
		{
			get { return notFound; }
		}

		/// <summary>
		/// [Icon] NotFoundのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <param name="iconScale">アイコンサイズ</param>
		/// <param name="logger">ログ。不要であれば null を指定(するか引数を与えない)。</param>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource GetNotFoundIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(NotFoundPath, iconScale, logger);
		}

		#region IconScale

		/// <summary>
		/// [Icon] NotFoundのイメージソース(IconScale.Small)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource NotFoundIconSmall
		{
			get
			{
				return GetNotFoundIcon(IconScale.Small);
			}
		}
		/// <summary>
		/// [Icon] NotFoundのイメージソース(IconScale.Normal)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource NotFoundIconNormal
		{
			get
			{
				return GetNotFoundIcon(IconScale.Normal);
			}
		}
		/// <summary>
		/// [Icon] NotFoundのイメージソース(IconScale.Big)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource NotFoundIconBig
		{
			get
			{
				return GetNotFoundIcon(IconScale.Big);
			}
		}
		/// <summary>
		/// [Icon] NotFoundのイメージソース(IconScale.Large)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource NotFoundIconLarge
		{
			get
			{
				return GetNotFoundIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion notFound
		#region Icon: LauncherToolbarMain

		/// <summary>
		/// [Icon] LauncherToolbarMainのリソースパス。
		/// <para>/Resources/Icon/LauncherToolbarMain.ico</para>
		/// </summary>
		public static string LauncherToolbarMainPath
		{
			get { return launcherToolbarMain; }
		}

		/// <summary>
		/// [Icon] LauncherToolbarMainのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <param name="iconScale">アイコンサイズ</param>
		/// <param name="logger">ログ。不要であれば null を指定(するか引数を与えない)。</param>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource GetLauncherToolbarMainIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(LauncherToolbarMainPath, iconScale, logger);
		}

		#region IconScale

		/// <summary>
		/// [Icon] LauncherToolbarMainのイメージソース(IconScale.Small)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherToolbarMainIconSmall
		{
			get
			{
				return GetLauncherToolbarMainIcon(IconScale.Small);
			}
		}
		/// <summary>
		/// [Icon] LauncherToolbarMainのイメージソース(IconScale.Normal)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherToolbarMainIconNormal
		{
			get
			{
				return GetLauncherToolbarMainIcon(IconScale.Normal);
			}
		}
		/// <summary>
		/// [Icon] LauncherToolbarMainのイメージソース(IconScale.Big)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherToolbarMainIconBig
		{
			get
			{
				return GetLauncherToolbarMainIcon(IconScale.Big);
			}
		}
		/// <summary>
		/// [Icon] LauncherToolbarMainのイメージソース(IconScale.Large)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherToolbarMainIconLarge
		{
			get
			{
				return GetLauncherToolbarMainIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion launcherToolbarMain
		#region Icon: LauncherCommand

		/// <summary>
		/// [Icon] LauncherCommandのリソースパス。
		/// <para>/Resources/Icon/LauncherCommand.ico</para>
		/// </summary>
		public static string LauncherCommandPath
		{
			get { return launcherCommand; }
		}

		/// <summary>
		/// [Icon] LauncherCommandのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <param name="iconScale">アイコンサイズ</param>
		/// <param name="logger">ログ。不要であれば null を指定(するか引数を与えない)。</param>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource GetLauncherCommandIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(LauncherCommandPath, iconScale, logger);
		}

		#region IconScale

		/// <summary>
		/// [Icon] LauncherCommandのイメージソース(IconScale.Small)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherCommandIconSmall
		{
			get
			{
				return GetLauncherCommandIcon(IconScale.Small);
			}
		}
		/// <summary>
		/// [Icon] LauncherCommandのイメージソース(IconScale.Normal)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherCommandIconNormal
		{
			get
			{
				return GetLauncherCommandIcon(IconScale.Normal);
			}
		}
		/// <summary>
		/// [Icon] LauncherCommandのイメージソース(IconScale.Big)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherCommandIconBig
		{
			get
			{
				return GetLauncherCommandIcon(IconScale.Big);
			}
		}
		/// <summary>
		/// [Icon] LauncherCommandのイメージソース(IconScale.Large)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource LauncherCommandIconLarge
		{
			get
			{
				return GetLauncherCommandIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion launcherCommand
		#region Icon: ApplicationTasktray

		/// <summary>
		/// [Icon] ApplicationTasktrayのリソースパス。
		/// <para>/Resources/Icon/Tasktray/App-debug.ico</para>
		/// </summary>
		public static string ApplicationTasktrayPath
		{
			get { return applicationTasktray; }
		}

		/// <summary>
		/// [Icon] ApplicationTasktrayのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <param name="iconScale">アイコンサイズ</param>
		/// <param name="logger">ログ。不要であれば null を指定(するか引数を与えない)。</param>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource GetApplicationTasktrayIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(ApplicationTasktrayPath, iconScale, logger);
		}

		#region IconScale

		/// <summary>
		/// [Icon] ApplicationTasktrayのイメージソース(IconScale.Small)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayIconSmall
		{
			get
			{
				return GetApplicationTasktrayIcon(IconScale.Small);
			}
		}
		/// <summary>
		/// [Icon] ApplicationTasktrayのイメージソース(IconScale.Normal)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayIconNormal
		{
			get
			{
				return GetApplicationTasktrayIcon(IconScale.Normal);
			}
		}
		/// <summary>
		/// [Icon] ApplicationTasktrayのイメージソース(IconScale.Big)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayIconBig
		{
			get
			{
				return GetApplicationTasktrayIcon(IconScale.Big);
			}
		}
		/// <summary>
		/// [Icon] ApplicationTasktrayのイメージソース(IconScale.Large)。
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ApplicationTasktrayIconLarge
		{
			get
			{
				return GetApplicationTasktrayIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion applicationTasktray
		#region Image: CommonFiltering

		/// <summary>
		/// [Image] CommonFilteringのリソースパス。
		/// <para>/Resources/Image/Common/Filtering.png</para>
		/// </summary>
		public static string CommonFilteringPath
		{
			get { return commonFiltering; }
		}

		/// <summary>
		/// [Image] CommonFilteringのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonFilteringImage
		{
			get { return GetImage(CommonFilteringPath); }
		}
		#endregion commonFiltering
		#region Image: CommonTemplate

		/// <summary>
		/// [Image] CommonTemplateのリソースパス。
		/// <para>/Resources/Image/Common/Template.png</para>
		/// </summary>
		public static string CommonTemplatePath
		{
			get { return commonTemplate; }
		}

		/// <summary>
		/// [Image] CommonTemplateのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonTemplateImage
		{
			get { return GetImage(CommonTemplatePath); }
		}
		#endregion commonTemplate
		#region Image: CommonClipboard

		/// <summary>
		/// [Image] CommonClipboardのリソースパス。
		/// <para>/Resources/Image/Common/Clipboard.png</para>
		/// </summary>
		public static string CommonClipboardPath
		{
			get { return commonClipboard; }
		}

		/// <summary>
		/// [Image] CommonClipboardのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonClipboardImage
		{
			get { return GetImage(CommonClipboardPath); }
		}
		#endregion commonClipboard
		#region Image: CommonSend

		/// <summary>
		/// [Image] CommonSendのリソースパス。
		/// <para>/Resources/Image/Common/Send.png</para>
		/// </summary>
		public static string CommonSendPath
		{
			get { return commonSend; }
		}

		/// <summary>
		/// [Image] CommonSendのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonSendImage
		{
			get { return GetImage(CommonSendPath); }
		}
		#endregion commonSend
		#region Image: CommonPin

		/// <summary>
		/// [Image] CommonPinのリソースパス。
		/// <para>/Resources/Image/Common/Pin.png</para>
		/// </summary>
		public static string CommonPinPath
		{
			get { return commonPin; }
		}

		/// <summary>
		/// [Image] CommonPinのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonPinImage
		{
			get { return GetImage(CommonPinPath); }
		}
		#endregion commonPin
		#region Image: CommonWindowList

		/// <summary>
		/// [Image] CommonWindowListのリソースパス。
		/// <para>/Resources/Image/Common/Pin.png</para>
		/// </summary>
		public static string CommonWindowListPath
		{
			get { return commonWindowList; }
		}

		/// <summary>
		/// [Image] CommonWindowListのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonWindowListImage
		{
			get { return GetImage(CommonWindowListPath); }
		}
		#endregion commonWindowList
		#region Image: CommonAdd

		/// <summary>
		/// [Image] CommonAddのリソースパス。
		/// <para>/Resources/Image/Common/Add.png</para>
		/// </summary>
		public static string CommonAddPath
		{
			get { return commonAdd; }
		}

		/// <summary>
		/// [Image] CommonAddのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonAddImage
		{
			get { return GetImage(CommonAddPath); }
		}
		#endregion commonAdd
		#region Image: CommonRemove

		/// <summary>
		/// [Image] CommonRemoveのリソースパス。
		/// <para>/Resources/Image/Common/Remove.png</para>
		/// </summary>
		public static string CommonRemovePath
		{
			get { return commonRemove; }
		}

		/// <summary>
		/// [Image] CommonRemoveのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonRemoveImage
		{
			get { return GetImage(CommonRemovePath); }
		}
		#endregion commonRemove
		#region Image: CommonSave

		/// <summary>
		/// [Image] CommonSaveのリソースパス。
		/// <para>/Resources/Image/Common/Save.png</para>
		/// </summary>
		public static string CommonSavePath
		{
			get { return commonSave; }
		}

		/// <summary>
		/// [Image] CommonSaveのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonSaveImage
		{
			get { return GetImage(CommonSavePath); }
		}
		#endregion commonSave
		#region Image: CommonUsingClipboard

		/// <summary>
		/// [Image] CommonUsingClipboardのリソースパス。
		/// <para>/Resources/Image/Common/UsingClipboard.png</para>
		/// </summary>
		public static string CommonUsingClipboardPath
		{
			get { return commonUsingClipboard; }
		}

		/// <summary>
		/// [Image] CommonUsingClipboardのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonUsingClipboardImage
		{
			get { return GetImage(CommonUsingClipboardPath); }
		}
		#endregion commonUsingClipboard
		#region Image: CommonUp

		/// <summary>
		/// [Image] CommonUpのリソースパス。
		/// <para>/Resources/Image/Common/Up.png</para>
		/// </summary>
		public static string CommonUpPath
		{
			get { return commonUp; }
		}

		/// <summary>
		/// [Image] CommonUpのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonUpImage
		{
			get { return GetImage(CommonUpPath); }
		}
		#endregion commonUp
		#region Image: CommonDown

		/// <summary>
		/// [Image] CommonDownのリソースパス。
		/// <para>/Resources/Image/Common/Down.png</para>
		/// </summary>
		public static string CommonDownPath
		{
			get { return commonDown; }
		}

		/// <summary>
		/// [Image] CommonDownのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource CommonDownImage
		{
			get { return GetImage(CommonDownPath); }
		}
		#endregion commonDown
		#region Image: TemplatePlain

		/// <summary>
		/// [Image] TemplatePlainのリソースパス。
		/// <para>/Resources/Image/Template/TemplatePlain.png</para>
		/// </summary>
		public static string TemplatePlainPath
		{
			get { return templatePlain; }
		}

		/// <summary>
		/// [Image] TemplatePlainのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource TemplatePlainImage
		{
			get { return GetImage(TemplatePlainPath); }
		}
		#endregion templatePlain
		#region Image: TemplateReplace

		/// <summary>
		/// [Image] TemplateReplaceのリソースパス。
		/// <para>/Resources/Image/Template/TemplateReplace.png</para>
		/// </summary>
		public static string TemplateReplacePath
		{
			get { return templateReplace; }
		}

		/// <summary>
		/// [Image] TemplateReplaceのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource TemplateReplaceImage
		{
			get { return GetImage(TemplateReplacePath); }
		}
		#endregion templateReplace
		#region Image: TemplateProgrammable

		/// <summary>
		/// [Image] TemplateProgrammableのリソースパス。
		/// <para>/Resources/Image/Template/TemplateProgrammable.png</para>
		/// </summary>
		public static string TemplateProgrammablePath
		{
			get { return templateProgrammable; }
		}

		/// <summary>
		/// [Image] TemplateProgrammableのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource TemplateProgrammableImage
		{
			get { return GetImage(TemplateProgrammablePath); }
		}
		#endregion templateProgrammable
		#region Image: ClipboardClear

		/// <summary>
		/// [Image] ClipboardClearのリソースパス。
		/// <para>/Resources/Image/Clipboard/Clear.png</para>
		/// </summary>
		public static string ClipboardClearPath
		{
			get { return clipboardClear; }
		}

		/// <summary>
		/// [Image] ClipboardClearのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardClearImage
		{
			get { return GetImage(ClipboardClearPath); }
		}
		#endregion clipboardClear
		#region Image: ClipboardTextFormat

		/// <summary>
		/// [Image] ClipboardTextFormatのリソースパス。
		/// <para>/Resources/Image/Clipboard/ClipboardText.png</para>
		/// </summary>
		public static string ClipboardTextFormatPath
		{
			get { return clipboardTextFormat; }
		}

		/// <summary>
		/// [Image] ClipboardTextFormatのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardTextFormatImage
		{
			get { return GetImage(ClipboardTextFormatPath); }
		}
		#endregion clipboardTextFormat
		#region Image: ClipboardHtml

		/// <summary>
		/// [Image] ClipboardHtmlのリソースパス。
		/// <para>/Resources/Image/Clipboard/ClipboardHtml.png</para>
		/// </summary>
		public static string ClipboardHtmlPath
		{
			get { return clipboardHtml; }
		}

		/// <summary>
		/// [Image] ClipboardHtmlのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardHtmlImage
		{
			get { return GetImage(ClipboardHtmlPath); }
		}
		#endregion clipboardHtml
		#region Image: ClipboardRichTextFormat

		/// <summary>
		/// [Image] ClipboardRichTextFormatのリソースパス。
		/// <para>/Resources/Image/Clipboard/ClipboardRichTextFormat.png</para>
		/// </summary>
		public static string ClipboardRichTextFormatPath
		{
			get { return clipboardRichTextFormat; }
		}

		/// <summary>
		/// [Image] ClipboardRichTextFormatのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardRichTextFormatImage
		{
			get { return GetImage(ClipboardRichTextFormatPath); }
		}
		#endregion clipboardRichTextFormat
		#region Image: ClipboardImage

		/// <summary>
		/// [Image] ClipboardImageのリソースパス。
		/// <para>/Resources/Image/Clipboard/ClipboardImage.png</para>
		/// </summary>
		public static string ClipboardImagePath
		{
			get { return clipboardImage; }
		}

		/// <summary>
		/// [Image] ClipboardImageのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardImageImage
		{
			get { return GetImage(ClipboardImagePath); }
		}
		#endregion clipboardImage
		#region Image: ClipboardFile

		/// <summary>
		/// [Image] ClipboardFileのリソースパス。
		/// <para>/Resources/Image/Clipboard/ClipboardFile.png</para>
		/// </summary>
		public static string ClipboardFilePath
		{
			get { return clipboardFile; }
		}

		/// <summary>
		/// [Image] ClipboardFileのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardFileImage
		{
			get { return GetImage(ClipboardFilePath); }
		}
		#endregion clipboardFile
		#region Image: ClipboardImageFit

		/// <summary>
		/// [Image] ClipboardImageFitのリソースパス。
		/// <para>/Resources/Image/Clipboard/ImageFit.png</para>
		/// </summary>
		public static string ClipboardImageFitPath
		{
			get { return clipboardImageFit; }
		}

		/// <summary>
		/// [Image] ClipboardImageFitのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardImageFitImage
		{
			get { return GetImage(ClipboardImageFitPath); }
		}
		#endregion clipboardImageFit
		#region Image: ClipboardImageRaw

		/// <summary>
		/// [Image] ClipboardImageRawのリソースパス。
		/// <para>/Resources/Image/Clipboard/ImageRaw.png</para>
		/// </summary>
		public static string ClipboardImageRawPath
		{
			get { return clipboardImageRaw; }
		}

		/// <summary>
		/// [Image] ClipboardImageRawのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource ClipboardImageRawImage
		{
			get { return GetImage(ClipboardImageRawPath); }
		}
		#endregion clipboardImageRaw
		#region Image: WindowLoad

		/// <summary>
		/// [Image] WindowLoadのリソースパス。
		/// <para>/Resources/Image/Window/WindowLoad.png</para>
		/// </summary>
		public static string WindowLoadPath
		{
			get { return windowLoad; }
		}

		/// <summary>
		/// [Image] WindowLoadのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource WindowLoadImage
		{
			get { return GetImage(WindowLoadPath); }
		}
		#endregion windowLoad
		#region Image: WindowSave

		/// <summary>
		/// [Image] WindowSaveのリソースパス。
		/// <para>/Resources/Image/Window/WindowSave.png</para>
		/// </summary>
		public static string WindowSavePath
		{
			get { return windowSave; }
		}

		/// <summary>
		/// [Image] WindowSaveのイメージソース。
		/// <para>初回のみ生成を行う。</para>
		/// </summary>
		/// <returns>イメージソース。AppResourceで管理されるためユーザーコードで操作はしないこと。</returns>
		public static BitmapSource WindowSaveImage
		{
			get { return GetImage(WindowSavePath); }
		}
		#endregion windowSave
	}
}

