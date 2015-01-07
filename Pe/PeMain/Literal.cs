using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.PeMain
{
	/// <summary>
	/// 各種定数
	/// 
	/// ただしプロパティの値は定数じゃないかも
	/// </summary>
	public static class Literal
	{
		#if DEBUG
		private static bool _initialized = false;
		#endif
		
		public const string programName = "Pe";
		public const string updateProgramDir = "Updater";
		public const string updateProgramName = updateProgramDir + ".exe";
		#if DEBUG
		public const string shortcutName = "Pe(DEBUG).lnk";
		#else
		public const string shortcutName = "Pe.lnk";
		#endif
		
		/// <summary>
		/// 前回バージョンがこれ未満なら使用許諾を表示
		/// </summary>
		//public static readonly Tuple<ushort, ushort, ushort> AcceptVersion = new Tuple<ushort, ushort, ushort>(0, 19, 0);
		public static readonly Tuple<ushort, ushort, ushort> AcceptVersion = Functions.ConvertVersionTuple("0.19.0");

		/// <summary>
		/// このプログラムが使用するディレクトリ名
		/// </summary>
		private const string _dirRootName = programName;
		
		private static string _settingRootDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		private static string _logRootDirPath     = Path.Combine(UserSettingDirPath, "log");
		
		private const string _mainSettingFileName    = "mainsetting.xml";
		private const string _launcherItemsFileName  = "launcher-items.xml";
		//private const string _clipboardItemsFileName = "clipboard-items.xml";
		private const string _dbFileName             = "db.sqlite3";
		private const string _backupDirName          = "backup";

		private const string _applicationsFileName = "ApplicationSetting.xml";
		private const string _applicationsSettingBaseDirectoryName = "Applications";
		private const string _applicationsLogBaseDirectoryName = "Applications";

		/// <summary>
		/// デフォルトの言語名。
		/// </summary>
		public const string defaultLanguage = "default";
		
		#if DEBUG
		public static readonly TimeSpan updateWaitTime = TimeSpan.FromSeconds(1);
		#else
		public static readonly TimeSpan updateWaitTime = TimeSpan.FromSeconds(30);
		#endif
		public static readonly TimeSpan startHomeDialogWaitTime = TimeSpan.FromSeconds(1.5);

		public static readonly TripleRange<int> windowSaveCount = new TripleRange<int>(3, 10, 20);
		public static readonly TripleRange<TimeSpan> windowSaveTime = new TripleRange<TimeSpan>(
			TimeSpan.FromMinutes(1),
			TimeSpan.FromMinutes(10),
			TimeSpan.FromMinutes(30)
		);
		
		/// <summary>
		/// ツールバー フロート状態 設定サイズ
		/// </summary>
		public static readonly Size toolbarFloatSize = new Size(SystemInformation.WorkingArea.Width / 10, 0);
		public static readonly Size toolbarDesktopSize = new Size(0, 0);
		public static readonly TripleRange<int> toolbarTextWidth = new TripleRange<int>(40, 80, 200);
		public static readonly TripleRange<TimeSpan> toolbarHiddenTime = new TripleRange<TimeSpan>(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(10));
		public static readonly TripleRange<TimeSpan> toolbarAnimateTime = new TripleRange<TimeSpan>(TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(500), TimeSpan.FromSeconds(1000));
			
		public const int waitCountForGetScreenCount = 10;
		public static readonly TimeSpan screenCountWaitTime = TimeSpan.FromMilliseconds(250);
		
		public static readonly TripleRange<TimeSpan> commandHiddenTime = new TripleRange<TimeSpan>(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(20));

		public static readonly TripleRange<TimeSpan> clipboardSleepTime = new TripleRange<TimeSpan>(
			TimeSpan.FromMilliseconds(15),
			TimeSpan.FromMilliseconds(250),
			TimeSpan.FromMilliseconds(500)
		);
		public static readonly TripleRange<TimeSpan> clipboardWaitTime = new TripleRange<TimeSpan>(
			TimeSpan.FromMilliseconds(50),
			TimeSpan.FromMilliseconds(500),
			TimeSpan.FromSeconds(1)
		);
		public static readonly TripleRange<TimeSpan> clipboardThreadWaitTime = new TripleRange<TimeSpan>(
			TimeSpan.FromMilliseconds(50),
			TimeSpan.FromMilliseconds(200),
			TimeSpan.FromSeconds(1)
		);
		public static readonly TripleRange<int> clipboardLimit = new TripleRange<int>(8, 1024, 1024 * 5);


		#region NOTE
		
		/// <summary>
		/// ノートサイズ
		/// </summary>
		public static readonly Size noteSize = new Size(200, 200);
		
		public static readonly Color noteForeColorWhite = Color.FromArgb(255, 255, 255);
		public static readonly Color noteForeColorBlack = Color.FromArgb(0, 0, 0);
		public static readonly Color noteForeColorRed = Color.FromArgb(255, 0, 0);
		public static readonly Color noteForeColorGreen = Color.FromArgb(0, 255, 0);
		public static readonly Color noteForeColorBlue = Color.FromArgb(0, 0, 255);
		public static readonly Color noteForeColorYellow = Color.FromArgb(255, 255, 128);
		public static readonly Color noteForeColorOrange = Color.FromArgb(255, 128, 0);
		public static readonly Color noteForeColorPurple = Color.FromArgb(255, 128, 255);
		
		public static readonly Color noteBackColorWhite = Color.FromArgb(240, 240, 240);
		public static readonly Color noteBackColorBlack = Color.FromArgb(80, 80, 80);
		public static readonly Color noteBackColorRed = Color.FromArgb(240, 80, 80);
		public static readonly Color noteBackColorGreen = Color.FromArgb(170, 230, 170);
		public static readonly Color noteBackColorBlue = Color.FromArgb(170, 170, 230);
		public static readonly Color noteBackColorYellow = Color.FromArgb(250, 250, 180);
		public static readonly Color noteBackColorOrange = Color.FromArgb(230, 170, 80);
		public static readonly Color noteBackColorPurple = Color.FromArgb(230, 170, 230);
		
		public static readonly Color noteFore = noteForeColorBlack;
		public static readonly Color noteBack = noteBackColorYellow;
		
		public static IList<Color> GetNoteForeColorList()
		{
			return new [] {
				noteForeColorBlack,
				noteForeColorWhite,
				noteForeColorRed,
				noteForeColorGreen,
				noteForeColorBlue,
				noteForeColorYellow,
				noteForeColorOrange,
				noteForeColorPurple,
			};
		}
		public static IList<Color> GetNoteBackColorList()
		{
			return new [] {
				noteBackColorBlack,
				noteBackColorWhite,
				noteBackColorRed,
				noteBackColorGreen,
				noteBackColorBlue,
				noteBackColorYellow,
				noteBackColorOrange,
				noteBackColorPurple,
			};
		}
		
		#endregion
		
		public const string timestampFileName = "yyyy-MM-dd_HH-mm-ss";
		
#if DEBUG
		public const int backupCount = 3;
		public const int logListLimit = 5;
#else
		public const int backupCount = 20;
		public const int logListLimit = 1000;
#endif


		/// <summary>
		/// 実行パス
		/// </summary>
		public static string ApplicationExecutablePath
		{
			get
			{
				return Application.ExecutablePath;
			}
		}
		
		/// <summary>
		/// 起動ディレクトリ
		/// </summary>
		public static string ApplicationRootDirPath
		{
			get
			{
				return Path.GetDirectoryName(Literal.ApplicationExecutablePath);
			}
		}
		/// <summary>
		/// bin/
		/// </summary>
		public static string ApplicationBinDirPath
		{
			get
			{
				return Path.Combine(ApplicationRootDirPath, "bin");
			}
		}

		public static string ApplicationBinAppPath
		{
			get { return Path.Combine(ApplicationBinDirPath, _applicationsFileName); }
		}

		public static string ApplicationSettingBaseDirectoryPath
		{
			get { return Path.Combine(UserSettingDirPath, _applicationsSettingBaseDirectoryName); }
		}

		public static string ApplicationLogBaseDirectoryPath
		{
			get { return Path.Combine(LogFileDirPath, _applicationsLogBaseDirectoryName); }
		}

		/// <summary>
		/// sbin/
		/// </summary>
		public static string ApplicationSBinDirPath
		{
			get
			{
				return Path.Combine(ApplicationRootDirPath, "sbin");
			}
		}

		public static string ApplicationSBinAppPath
		{
			get { return Path.Combine(ApplicationSBinDirPath, _applicationsFileName); }
		}

		/// <summary>
		/// etc/
		/// </summary>
		public static string ApplicationEtcDirPath
		{
			get
			{
				return Path.Combine(ApplicationRootDirPath, "etc");
			}
		}
		
		/// <summary>
		/// etc/style
		/// </summary>
		public static string ApplicationStyleDirPath
		{
			get
			{
				return Path.Combine(ApplicationEtcDirPath, "style");
			}
		}
		/// <summary>
		/// etc/script
		/// </summary>
		public static string ApplicationScriptDirPath
		{
			get
			{
				return Path.Combine(ApplicationEtcDirPath, "script");
			}
		}
		
		/// <summary>
		/// デフォルトのの
		/// </summary>
		public static string ApplicationDefaultLauncherItemPath
		{
			get
			{
				return Path.Combine(ApplicationEtcDirPath, "default-launcher.xml");
			}
		}
		
		
		/// <summary>
		/// etc/lang/
		/// </summary>
		public static string ApplicationLanguageDirPath
		{
			get
			{
				return Path.Combine(ApplicationEtcDirPath, "lang");
			}
		}
		
		/// <summary>
		/// doc/
		/// </summary>
		public static string ApplicationDocumentDirPath
		{
			get
			{
				return Path.Combine(ApplicationRootDirPath, "doc");
			}
		}
		/// <summary>
		/// ユーザー設定ルートディレクトリ
		/// </summary>
		public static string UserSettingDirPath
		{
			get
			{
				var path = Path.Combine(_settingRootDirPath, _dirRootName);
				
				return path;
			}
		}
		/// <summary>
		/// ユーザー設定ファイル
		/// </summary>
		public static string UserMainSettingPath
		{
			get { return Path.Combine(UserSettingDirPath, _mainSettingFileName); }
		}
		
		public static string UserLauncherItemsPath
		{
			get { return Path.Combine(UserSettingDirPath, _launcherItemsFileName); }
		}

		/*
		public static string UserClipboardItemsPath
		{
			get { return Path.Combine(UserSettingDirPath, _clipboardItemsFileName); }
		}
		*/

		public static string UserDBPath
		{
			get { return Path.Combine(UserSettingDirPath, _dbFileName); }
		}
		
		public static string UserBackupDirPath
		{
			get { return Path.Combine(UserSettingDirPath, _backupDirName);}
		}
		
		public static string NowTimestampFileName
		{
			get { return DateTime.Now.ToString(timestampFileName); }
		}
		
		public static string UserDownloadDirPath
		{
			get { return Path.Combine(UserSettingDirPath, "archive");}
		}
		
		/// <summary>
		/// ログ保存ディレクトリ
		/// </summary>
		public static string LogFileDirPath
		{
			get { return _logRootDirPath; }
		}
		
		#region PAGE
		public static string AboutWebURL
		{
			get { return ReplaceLiteralText(ConfigurationManager.AppSettings["page-about"]); }
		}
		public static string AboutMailAddress
		{
			get { return ReplaceLiteralText(ConfigurationManager.AppSettings["mail-address"]); }
		}
		public static string AboutDevelopmentURL
		{
			get { return ReplaceLiteralText(ConfigurationManager.AppSettings["page-development"]); }
		}
		public static string UpdateURL
		{
			get { return ReplaceLiteralText(ConfigurationManager.AppSettings["page-update"]); }
		}
		public static string ChangeLogURL
		{
			get { return ReplaceLiteralText(ConfigurationManager.AppSettings["page-changelog-release"]); }
		}
		public static string ChangeLogRcURL
		{
			get { return ReplaceLiteralText(ConfigurationManager.AppSettings["page-changelog-rc"]); }
		}
		public static string DiscussionURL
		{
			get { return ReplaceLiteralText(ConfigurationManager.AppSettings["page-discussion"]); }
		}
		
		public static string HelpDocumentURI
		{
			get { return ReplaceLiteralText(ConfigurationManager.AppSettings["page-help"]); }
		}
		#endregion
		
		public static FileVersionInfo Version
		{
			get { return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location); }
		}
		public static string ApplicationVersion
		{
			get 
			{
				return string.Format("{0}-{1}", Version.FileVersion, Version.ProductVersion);
			}
		}
		
		public static string StartupShortcutPath
		{
			get
			{
				var startupDirPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
				var appLinkPath = Path.Combine(startupDirPath, Literal.shortcutName);

				return appLinkPath;
			}
		}
		
		public static void Initialize(CommandLine commandLine)
		{
			#if DEBUG
			Debug.Assert(_initialized == false);
			#endif
			
			if(commandLine.HasOption("setting-root")) {
				_settingRootDirPath = Environment.ExpandEnvironmentVariables(commandLine.GetValue("setting-root"));
			}
			
			if(commandLine.HasOption("log")) {
				if(commandLine.HasValue("log")) {
					_logRootDirPath = Environment.ExpandEnvironmentVariables(commandLine.GetValue("log"));
				}
			}
			
			#if DEBUG
			_initialized = true;
			#endif
		}
		
		/// <summary>
		/// 文字列リテラルを書式で変換。
		/// 
		/// {...} を置き換える。
		/// * TIMESTAMP: そんとき
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		private static string ReplaceLiteralText(string src)
		{
			var map = new Dictionary<string, string>() {
				{ "TIMESTAMP", NowTimestampFileName }
			};
			var replacedText = src.ReplaceRangeFromDictionary("{", "}", map);
			
			return replacedText;
		}
	}
	
	public enum HotKeyId: ushort
	{
		ShowCommand = 0x0001,
		HiddenFile,
		Extension,
		CreateNote,
		HiddenNote,
		CompactNote,
		ShowFrontNote,
	}
	
	public static class AppLanguageName
	{
		public const string application = "APPLICATION";
		public const string version     = "VER";
		
		public const string timestamp   = "TIMESTAMP";
		public const string year        = "Y";
		public const string year04      = "Y:04";
		public const string month       = "M";
		public const string month02     = "M:02";
		public const string monthShortName = "M:S";
		public const string monthLongName  = "M:L";
		public const string day         = "D";
		public const string day02       = "D:02";
		public const string hour        = "h";
		public const string hour02      = "h:02";
		public const string minute      = "m";
		public const string minute02    = "m:02";
		public const string second      = "s";
		public const string second02    = "s:02";
		
		public const string groupName   = "GROUP";
		public const string itemName    = "ITEM";
		
		public const string noteTitle   = "NOTE";
		
		public const string versionNow    = "NOW";
		public const string versionNext   = "NEXT";
		public const string versionType   = "TYPE";

		public const string imageType = "TYPE";
		public const string imageWidth = "WIDTH";
		public const string imageHeight = "HEIGHT";
		public const string fileType = "TYPE";
		public const string fileCount = "COUNT";

		public const string clipboardPrevTime = "TIME";
	}
	
	public static class DataTables
	{
		static DataTables()
		{
			map = new Dictionary<string, int>() {
				{ masterTableVersion,         1 },
				{ masterTableNote,            1 },
				{ transactionTableNote,       1 },
				{ transactionTableNoteStyle,  1 },
			};
		}
		public static readonly Dictionary<string, int> map;
		
		public const string masterTableVersion        = "M_VERSION";
		public const string masterTableNote           = "M_NOTE";
		public const string transactionTableNote      = "T_NOTE";
		public const string transactionTableNoteStyle = "T_NOTE_STYLE";
	}
}
