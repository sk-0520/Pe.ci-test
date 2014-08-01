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
		/// <summary>
		/// このプログラムが使用するディレクトリ名
		/// </summary>
		private static string _dirRootName = programName;
		
		private static string _settingRootDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		
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
		
		public const string timestampFileName = "yyyy-MM-dd_HH-mm-ss";
		
#if DEBUG
		public const int backupCount = 3;
#else
		public const int backupCount = 20;
#endif
		
		/// <summary>
		/// 起動ディレクトリ
		/// </summary>
		public static string PeRootDirPath
		{
			get
			{
				return Path.GetDirectoryName(Application.ExecutablePath);
			}
		}
		
		/// <summary>
		/// etc/
		/// </summary>
		public static string PeEtcDirPath
		{
			get
			{
				return Path.Combine(PeRootDirPath, "etc");
			}
		}
		
		/// <summary>
		/// etc/lang/
		/// </summary>
		public static string PeLanguageDirPath
		{
			get
			{
				return Path.Combine(PeEtcDirPath, "lang");
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
		
		public static void Initialize(CommandLine commandLine)
		{
			#if DEBUG
			Debug.Assert(_initialized == false);
			#endif
			
			if(commandLine.HasOption("setting-root")) {
				_settingRootDirPath = Environment.ExpandEnvironmentVariables(commandLine.GetValue("setting-root"));
			}
			
			#if DEBUG
			_initialized = true;
			#endif
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
		public const string application = "PE";
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
		
	}
	
	public static class DataTables
	{
		static DataTables()
		{
			map = new Dictionary<string, int>() {
				{ masterTableVersion,         1},
				{ masterTableNote,            1},
				{ masterTableNoteGroup,       1},
				{ transactionTableNote,       1},
				{ transactionTableNoteStyle,  1},
				{ transactionTableNoteGroup,  1},
			};
		}
		public static readonly Dictionary<string, int> map;
		
		public const string masterTableVersion        = "M_VERSION";
		public const string masterTableNote           = "M_NOTE";
		public const string masterTableNoteGroup      = "M_NOTE_GROUP";
		public const string transactionTableNote      = "T_NOTE";
		public const string transactionTableNoteStyle = "T_NOTE_STYLE";
		public const string transactionTableNoteGroup = "T_NOTE_GROUP";
	}
}
