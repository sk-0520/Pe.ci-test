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
		public const string buildType =
#if DEBUG
			"DEBUG";
#elif BETA
			"β";
#else
			"RELEASE";
#endif
		public static readonly string buildProcess = Environment.Is64BitProcess ? "64" : "32";

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

		#endregion
	}
}
