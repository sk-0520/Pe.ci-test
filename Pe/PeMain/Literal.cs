/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 17:06
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PeUtility;

namespace PeMain
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
		public const string updateProgramName = "PeUpdater.exe";
		
		/// <summary>
		/// 前回バージョンがこれ未満なら使用許諾を表示
		/// </summary>
		//public static readonly Tuple<ushort, ushort, ushort> AcceptVersion = new Tuple<ushort, ushort, ushort>(0, 19, 0);
		public static readonly Tuple<ushort, ushort, ushort> AcceptVersion = Functions.ConvertVersionTuple("0.19.0");

		/// <summary>
		/// このプログラムが使用するディレクトリ名
		/// </summary>
		private static string _dirRootName = programName;
		
		private static string _settingRootDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		private static string _logRootDirPath     = Path.Combine(UserSettingDirPath, "log");
		
		private static string _mainSettingFileName   = "mainsetting.xml";
		private static string _launcherItemsFileName = "launcher-items.xml";
		private static string _dbFileName            = "db.sqlite3";
		private static string _backupDirName         = "backup";
		
		/// <summary>
		/// ツールバー フロート状態 設定サイズ
		/// </summary>
		public static readonly Size toolbarFloatSize = new Size(SystemInformation.WorkingArea.Width / 10, 0);
		public static readonly Size toolbarDesktopSize = new Size(0, 0);
		public const int toolbarTextWidth = 80;
		
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
		
		public const string timestampFileName = "yyyy-MM-dd_HH-mm-ss";
		
		#if DEBUG
		public const int backupCount = 3;
		public const int logListLimit = 20;
		#else
		public const int backupCount = 20;
		public const int logListLimit = 1000;
		#endif
		
		/// <summary>
		/// 起動ディレクトリ
		/// </summary>
		public static string ApplicationRootDirPath
		{
			get
			{
				return Path.GetDirectoryName(Application.ExecutablePath);
			}
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
		
		public static string AboutWebPage
		{
			get { return ConfigurationManager.AppSettings["web-page"]; }
		}
		public static string AboutMailAddress
		{
			get { return ConfigurationManager.AppSettings["mail-address"]; }
		}
		public static string AboutDevelopPage
		{
			get { return ConfigurationManager.AppSettings["dev-page"]; }
		}
		public static string UpdateURL
		{
			get { return ReplaceLiteralText(ConfigurationManager.AppSettings["update-page"]); }
		}
		public static string ChangeLogURL
		{
			get { return ReplaceLiteralText(ConfigurationManager.AppSettings["changelog-release-page"]); }
		}
		public static string ChangeLogRcURL
		{
			get { return ReplaceLiteralText(ConfigurationManager.AppSettings["changelog-rc-page"]); }
		}
		
		public static string HelpDocumentURI
		{
			get { return ReplaceLiteralText(ConfigurationManager.AppSettings["help-document"]); }
		}
		
		public static FileVersionInfo Version
		{
			get { return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location); }
		}
		public static string ApplicationVersion
		{
			get { return Version.ProductVersion; }
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
	
	public static class AppEnv
	{
		public static string AppFilePath { get { return "PE_APP_EXE"; } }
		public static string AppDirPath { get { return "PE_APP_DIR"; } }
		public static string AppUserDir { get { return "PE_USER_DIR"; } }
	}
	
	public enum HotKeyId: ushort
	{
		ShowCommand = 0x0001,
		HiddenFile,
		Extension,
		CreateNote,
		HiddenNote,
		CompactNote,
	}
	
	public static class SystemLanguageName
	{
		public const string application = "APPLICATION";
		public const string version     = "VER";
		
		public const string year        = "Y";
		public const string year04      = "Y:04";
		public const string month       = "M";
		public const string month02     = "M:02";
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
