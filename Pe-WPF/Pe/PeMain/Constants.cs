namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;
	using System.IO;
	using System.Diagnostics;
	using System.Configuration;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using System.Windows;
	using System.Windows.Media;

	/// <summary>
	/// 定数。
	/// </summary>
	public static class Constants
	{
		public const string programName = "Pe-WPF";
		/// <summary>
		/// 前回バージョンがこれ未満なら使用許諾を表示
		/// </summary>
		public static readonly Version acceptVersion = new Version(0, 0, 0, 0);

#if DEBUG
		public const string shortcutName = programName + "(DEBUG).lnk";
#elif BETA
		public const string shortcutName = programName + "(BETA).lnk";
#else
		public const string shortcutName = programName + ".lnk";
#endif
#if DEBUG
		public const string buildType = "DEBUG";
#elif BETA
		public const string buildType = "β";
#else
		public const string buildType = "RELEASE";
#endif
		public static readonly string buildProcess = Environment.Is64BitProcess ? "64" : "32";

		public const string keyGuidName = "${GUID}";
		public const string keyIndexExt = "${EXT}";

		public const string binDirectoryName = "bin";
		public const string sbinDirectoryName = "sbin";
		public const string libraryDirectoryName = "lib";
		public const string etcDirectoryName = "etc";
		public const string languageDirectoryName = "lang";
		public const string styleDirectoryName = "style";
		public const string scriptDirectoryName = "script";
		public const string documentDirectoryName = "doc";

		public const string logDirectoryName = "logs";
		public const string settingDirectoryName = "setting";
		public const string backupDirectoryName = "backup";
		public const string archiveDirectoryName = "archive";

		public const string mainSettingFileName = "main-setting.json";
		public const string launcherItemSettingFileName = "item-setting.json";
		public const string launcherGroupItemSettingFileName = "group-item.json";

		public const int indexBodyCachingSize = 3;
		public const string indexBodyBaseFileName = keyGuidName + "." + keyIndexExt;

		public const string noteSaveDirectoryName = "notes";
		public const string noteIndexFileName = "note-index.json";

		public const string clipboardSaveDirectoryName = "clipboards";
		public const string clipboardIndexFileName = "clipboard-index.json";

		public const string templateSaveDirectoryName = "templates";
		public const string templateIndexFileName = "template-index.json";

		public const string styleCommonFileName = "common.css";

		public const string languageDefaultFileName = "default.xml";
		public const string languageSearchPattern = "*.xml";

		public const string timestampFileName = "yyyy-MM-dd_HH-mm-ss";

		public const string languageAcceptDocumentFileName = ".accept.html";

		public static readonly TimeSpan iconLoadWaitTime = TimeSpan.FromMilliseconds(250);
		public const int iconLoadRetryMax = 3;

		public const int screenCountChangeRetryCount = 10;
		public static readonly TimeSpan screenCountChangeWaitTime = TimeSpan.FromMilliseconds(250);

		public static readonly TripleRange<TimeSpan> commandHideTime = new TripleRange<TimeSpan>(
			TimeSpan.FromMilliseconds(250),
			TimeSpan.FromSeconds(2),
			TimeSpan.FromSeconds(10)
		);

		public static readonly TripleRange<double> toolbarTextLength = new TripleRange<double>(
			20,
			80,
			200
		);

		public static readonly TripleRange<TimeSpan> clipboardWaitTime = new TripleRange<TimeSpan>(
			TimeSpan.FromMilliseconds(50),
			TimeSpan.FromMilliseconds(500),
			TimeSpan.FromSeconds(1)
		);

		public static readonly TripleRange<double> clipboardSaveCount = new TripleRange<double>(
			0,
			1024,
			1024 * 10
		);

		public static readonly TripleRange<double> clipboardDuplicationCount = new TripleRange<double>(
			-1,
			50,
			256
		);
		public const double clipboardItemsListWidth = 180;
		public static readonly Size clipboardDefaultWindowSize = new Size(580, 380);

		public const double templateItemsListWidth = 180;
		public const double templateReplaceListWidth = 100;
		public static readonly Size templateDefaultWindowSize = new Size(580,380);

		public static readonly TripleRange<TimeSpan> windowSaveIntervalTime = new TripleRange<TimeSpan>(
			//TimeSpan.FromMinutes(1),
			TimeSpan.FromSeconds(5),
			TimeSpan.FromMinutes(10),
			TimeSpan.FromHours(1)
		);
		public static readonly TripleRange<int> windowSaveCount = new TripleRange<int>(
			3,
			10,
			20
		);

		public static readonly Thickness noteCaptionPadding = new Thickness(2);
		public const double noteCaptionHeight = 20;
		public static readonly Color noteForeColor = Colors.Black;
		public static readonly Color noteBackColor = Color.FromRgb(250, 250, 180);
		public static readonly TripleRange<double> noteFontSize = new TripleRange<double>(
			8,
			10.5,
			72
		);
		public static readonly Size noteDefualtSize = new Size(200, 200);

		/// <summary>
		/// 実行パス
		/// </summary>
		public static readonly string applicationExecutablePath = Assembly.GetExecutingAssembly().Location;
		/// <summary>
		/// 起動ディレクトリ
		/// </summary>
		public static readonly string applicationRootDirectoryPath = Path.GetDirectoryName(applicationExecutablePath);
		/// <summary>
		/// アセンブリバージョン。
		/// </summary>
		public static readonly Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
		/// <summary>
		/// 識別リビジョン。
		/// </summary>
		public static readonly string applicationRevision = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion;
		/// <summary>
		/// アプリケーションバージョン。
		/// </summary>
		public static readonly string applicationVersion = assemblyVersion.ToString() + "-" + applicationRevision;
		/// <summary>
		/// スタートアップ用ショートカットファイルパス。
		/// </summary>
		public static readonly string startupShortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), shortcutName);

		// TODO: T4しちゃっていいんじゃないだろうか
		#region TripleRange

		#region commandHideTime

		public static double CommandHideMinimumTime { get { return commandHideTime.minimum.TotalMilliseconds; } }
		public static double CommandHideMaximumTime { get { return commandHideTime.maximum.TotalMilliseconds; } }

		#endregion

		#region toolbarTextLength

		public static double ToolbarTextMinimumLength { get { return toolbarTextLength.minimum; } }
		public static double ToolbarTextMaximumLength { get { return toolbarTextLength.maximum; } }

		#endregion

		#region clipboardWaitTime

		public static double ClipboardWaitMinimumTime { get { return clipboardWaitTime.minimum.TotalMilliseconds; } }
		public static double ClipboardWaitMaximumTime { get { return clipboardWaitTime.maximum.TotalMilliseconds; } }

		#endregion

		#region clipboardSaveCount

		public static double ClipboardSaveMinimumCount { get { return clipboardSaveCount.minimum; } }
		public static double ClipboardSaveMaximumCount { get { return clipboardSaveCount.maximum; } }

		#endregion

		#region clipboardDuplicationCount

		public static double ClipboardDuplicationMinimumCount { get { return clipboardDuplicationCount.minimum; } }
		public static double ClipboardDuplicationMaximumCount { get { return clipboardDuplicationCount.maximum; } }

		#endregion

		#region noteFontSize

		public static double NoteFontMinimumSize { get { return noteFontSize.minimum; } }
		public static double NoteFontMaximumSize { get { return noteFontSize.maximum; } }

		#endregion

		#endregion

		#region app.config

		public static string UriAbout { get { return ConfigurationManager.AppSettings["uri-about"]; } }
		public static string MailAbout { get { return ConfigurationManager.AppSettings["mail-about"]; } }
		public static string UriDevelopment { get { return ConfigurationManager.AppSettings["uri-development"]; } }
		public static string UriUpdate { get { return ConfigurationManager.AppSettings["uri-update"]; } }
		public static string UriChangelogRelease { get { return ConfigurationManager.AppSettings["uri-changelog-release"]; } }
		public static string UriChangelogRc { get { return ConfigurationManager.AppSettings["uri-changelog-rc"]; } }
		public static string UriForum { get { return ConfigurationManager.AppSettings["uri-forum"]; } }
		public static string UriHelp { get { return ConfigurationManager.AppSettings["uri-help"]; } }
		public static string UriFeedback { get { return ConfigurationManager.AppSettings["uri-feedback"]; } }

		public static int CacheIndexNote { get { return int.Parse(ConfigurationManager.AppSettings["cache-index-note"]); } }
		public static int CacheIndexTemplate { get { return int.Parse(ConfigurationManager.AppSettings["cache-index-template"]); } }
		public static int CacheIndexClipboard { get { return int.Parse(ConfigurationManager.AppSettings["cache-index-clipboard"]); } }

		public static int BackupSettingCount { get { return int.Parse(ConfigurationManager.AppSettings["backup-setting"]); } }
		public static int BackupArchiveCount { get { return int.Parse(ConfigurationManager.AppSettings["backup-archive"]); } }

		#endregion

		#region function

		public static string GetTimestampFileName(DateTime dateTime)
		{
			return dateTime.ToString(timestampFileName);
		}

		public static string GetNowTimestampFileName()
		{
			return GetTimestampFileName(DateTime.Now);

		}
		#endregion
	}
}
