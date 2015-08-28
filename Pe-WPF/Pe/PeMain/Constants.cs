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
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using System.Windows;
	using System.Windows.Media;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	/// <summary>
	/// 定数。
	/// </summary>
	public static partial class Constants
	{
		public const string programName = "Pe-WPF";
		public const string updateProgramDirectoryName = "Updater";
		public const string updateProgramName = updateProgramDirectoryName + ".exe";
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
		public const string launcherItemSettingFileName = "launcher-items.json";
		public const string launcherGroupItemSettingFileName = "group-items.json";

		public const string changelogFileName = "changelog.xml";
		public const string componentListFileNam = "components.xml";

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

		public const string languageAcceptDocumentExtension = "accept.html";

		public const string extensionBinaryFile = "dat";
		public const string extensionJsonFile = "json";

		public const FileType fileTypeMainSetting = FileType.Json;
		public const FileType fileTypeLauncherItemSetting = FileType.Json;
		public const FileType fileTypeLauncherGroupSetting = FileType.Json;
		public const FileType fileTypeNoteIndex = FileType.Json;
		public const FileType fileTypeNoteBody = FileType.Json;
		public const FileType fileTypeTemplateIndex = FileType.Json;
		public const FileType fileTypeTemplateBody = FileType.Json;
		public const FileType fileTypeClipboardIndex = FileType.Json;
		public const FileType fileTypeClipboardBody = FileType.Binary;

		public static readonly TimeSpan iconLoadWaitTime = TimeSpan.FromMilliseconds(250);
		public const int iconLoadRetryMax = 3;

		public const int updateArchiveCount = 15;
#if DEBUG
		public static readonly TimeSpan updateWaitTime = TimeSpan.FromSeconds(1);
#else
		public static readonly TimeSpan updateWaitTime = TimeSpan.FromSeconds(30);
#endif

		public const int screenCountChangeRetryCount = 10;
		public static readonly TimeSpan screenCountChangeWaitTime = TimeSpan.FromMilliseconds(250);

		static readonly TripleRange<double> defaultFontSize = new TripleRange<double>(
			8,
			12.5,
			72
		);

		public static Size loggingDefaultWindowSize = new Size(320, 480);

		[ConstantsRange]
		public static readonly TripleRange<double> streamFontSize = new TripleRange<double>(
			defaultFontSize.minimum,
			defaultFontSize.median,
			defaultFontSize.maximum
		);
		public static readonly ColorPairItemModel streamOutputColor = new ColorPairItemModel(
			Colors.White,
			Colors.Black
		);
		public static readonly ColorPairItemModel streamErrorColor = new ColorPairItemModel(
			Colors.Red,
			Colors.Black
		);

		[ConstantsRange]
		public static readonly TripleRange<TimeSpan> commandHideTime = new TripleRange<TimeSpan>(
			TimeSpan.FromMilliseconds(250),
			TimeSpan.FromSeconds(2),
			TimeSpan.FromSeconds(10)
		);
		[ConstantsRange]
		public static readonly TripleRange<double> commandWindowWidth = new TripleRange<double>(
			250,
			400,
			800
		);
		[ConstantsRange]
		public static readonly TripleRange<double> commandFontSize = new TripleRange<double>(
			defaultFontSize.minimum,
			defaultFontSize.median,
			defaultFontSize.maximum
		);

		[ConstantsRange]
		public static readonly TripleRange<int> toolbarTextLength = new TripleRange<int>(
			20,
			80,
			200
		);
		[ConstantsRange]
		public static readonly TripleRange<double> toolbarFontSize = new TripleRange<double>(
			8,
			14,
			64
		);

		[ConstantsRange]
		public static readonly TripleRange<TimeSpan> toolbarHideWaitTime = new TripleRange<TimeSpan>(
			TimeSpan.FromMilliseconds(500),
			TimeSpan.FromSeconds(3),
			TimeSpan.FromSeconds(10)
		);

		[ConstantsRange]
		public static readonly TripleRange<TimeSpan> toolbarHideAnimateTime = new TripleRange<TimeSpan>(
			TimeSpan.FromMilliseconds(100),
			TimeSpan.FromMilliseconds(250),
			TimeSpan.FromSeconds(1)
		);

		[ConstantsRange]
		public static readonly TripleRange<TimeSpan> clipboardWaitTime = new TripleRange<TimeSpan>(
			TimeSpan.FromMilliseconds(50),
			TimeSpan.FromMilliseconds(500),
			TimeSpan.FromSeconds(1)
		);

		[ConstantsRange]
		public static readonly TripleRange<int> clipboardSaveCount = new TripleRange<int>(
			0,
			1024,
			1024 * 10
		);

		[ConstantsRange]
		public static readonly TripleRange<int> clipboardDuplicationCount = new TripleRange<int>(
			-1,
			50,
			256
		);
		public const double clipboardItemsListWidth = 220;
		public static readonly Size clipboardDefaultWindowSize = new Size(580, 380);
		[ConstantsRange]
		public static readonly TripleRange<double> clipboardFontSize = new TripleRange<double>(
			defaultFontSize.minimum,
			defaultFontSize.median,
			defaultFontSize.maximum
		);

		public const double templateItemsListWidth = 180;
		public const double templateReplaceListWidth = 100;
		public static readonly Size templateDefaultWindowSize = new Size(580, 380);
		[ConstantsRange]
		public static readonly TripleRange<double> templateFontSize = new TripleRange<double>(
			defaultFontSize.minimum,
			defaultFontSize.median,
			defaultFontSize.maximum
		);

		[ConstantsRange]
		public static readonly TripleRange<TimeSpan> windowSaveIntervalTime = new TripleRange<TimeSpan>(
			TimeSpan.FromSeconds(5),
			TimeSpan.FromMinutes(10),
			TimeSpan.FromHours(1)
		);
		[ConstantsRange]
		public static readonly TripleRange<int> windowSaveCount = new TripleRange<int>(
			3,
			10,
			20
		);

		public static readonly Thickness noteCaptionPadding = new Thickness(2);
		public const double noteCaptionHeight = 20;
		public static readonly ColorPairItemModel noteColor = new ColorPairItemModel(
			Colors.Black,
			Color.FromRgb(250, 250, 180)
		);

		[ConstantsRange]
		public static readonly TripleRange<double> noteFontSize = new TripleRange<double>(
			defaultFontSize.minimum,
			defaultFontSize.median,
			defaultFontSize.maximum
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

		#region app.config

		/// <summary>
		/// 文字列リテラルを書式で変換。
		/// 
		/// {...} を置き換える。
		/// * TIMESTAMP: そんとき
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		private static string ReplaceAppConfig(string src)
		{
			var map = new Dictionary<string, string>() {
				{ "TIMESTAMP", DateTime.Now.ToBinary().ToString() },
			};
			var replacedText = src.ReplaceRangeFromDictionary("{", "}", map);

			return replacedText;
		}

		public static string UriAbout { get { return ConfigurationManager.AppSettings["uri-about"]; } }
		public static string MailAbout { get { return ConfigurationManager.AppSettings["mail-about"]; } }
		public static string UriDevelopment { get { return ConfigurationManager.AppSettings["uri-development"]; } }
		public static string UriUpdate { get { return ReplaceAppConfig(ConfigurationManager.AppSettings["uri-update"]); } }
		public static string UriChangelogRelease { get { return ReplaceAppConfig(ConfigurationManager.AppSettings["uri-changelog-release"]); } }
		public static string UriChangelogRc { get { return ReplaceAppConfig(ConfigurationManager.AppSettings["uri-changelog-rc"]); } }
		public static string UriForum { get { return ConfigurationManager.AppSettings["uri-forum"]; } }
		public static string UriHelp { get { return ConfigurationManager.AppSettings["uri-help"]; } }
		public static string UriFeedback { get { return ConfigurationManager.AppSettings["uri-feedback"]; } }

		public static int CacheIndexNote { get { return int.Parse(ConfigurationManager.AppSettings["cache-index-note"]); } }
		public static int CacheIndexTemplate { get { return int.Parse(ConfigurationManager.AppSettings["cache-index-template"]); } }
		public static int CacheIndexClipboard { get { return int.Parse(ConfigurationManager.AppSettings["cache-index-clipboard"]); } }

		public static int BackupSettingCount { get { return int.Parse(ConfigurationManager.AppSettings["backup-setting"]); } }
		public static int BackupArchiveCount { get { return int.Parse(ConfigurationManager.AppSettings["backup-archive"]); } }

		#endregion

		#region property

		public static string ApplicationName { get { return programName; } }
		public static string BuildType { get { return buildType; } }
		public static string BuildProcess { get { return buildProcess; } }
		public static string ApplicationRevision { get { return applicationRevision; } }
		public static string ApplicationVersion { get { return applicationVersion; } }

		/// <summary>
		/// bin/
		/// </summary>
		public static string ApplicationBinDirectoryPath { get { return Path.Combine(applicationRootDirectoryPath, binDirectoryName); } }
		/// <summary>
		/// sbin/
		/// </summary>
		public static string ApplicationSBinDirectoryPath { get { return Path.Combine(applicationRootDirectoryPath, sbinDirectoryName); } }
		/// <summary>
		/// lib/
		/// </summary>
		public static string ApplicationLibraryDirectoryPath { get { return Path.Combine(applicationRootDirectoryPath, libraryDirectoryName); } }
		/// <summary>
		/// etc/
		/// </summary>
		public static string ApplicationEtcDirectoryPath { get { return Path.Combine(applicationRootDirectoryPath, etcDirectoryName); } }
		/// <summary>
		/// etc/lang
		/// </summary>
		public static string ApplicationLanguageDirectoryPath { get { return Path.Combine(ApplicationEtcDirectoryPath, languageDirectoryName); } }
		/// <summary>
		/// etc/style
		/// </summary>
		public static string ApplicationStyleDirectoryPath { get { return Path.Combine(ApplicationEtcDirectoryPath, styleDirectoryName); } }
		/// <summary>
		/// etc/script
		/// </summary>
		public static string ApplicationScriptDirectoryPath { get { return Path.Combine(ApplicationEtcDirectoryPath, scriptDirectoryName); } }
		/// <summary>
		/// doc/
		/// </summary>
		public static string ApplicationDocumentDirectoryPath { get { return Path.Combine(applicationRootDirectoryPath, documentDirectoryName); } }

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
