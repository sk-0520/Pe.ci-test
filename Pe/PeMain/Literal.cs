/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 17:06
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PeMain
{
	/// <summary>
	/// 各種定数
	/// 
	/// ただしプロパティの値は定数じゃないかも
	/// </summary>
	public static class Literal
	{
		public const string programName = "Pe";
		/// <summary>
		/// このプログラムが使用するディレクトリ名
		/// </summary>
		private const string dirRootName = programName;
		
		public const string mainSettingFileName = "mainsetting.xml";
		public const string launcherItemsName   = "launcher-items.xml";
		public const string backupDirName       = "backup";
		
		/// <summary>
		/// ツールバー フロート状態 設定サイズ
		/// </summary>
		public static readonly Size toolbarFloatSize = new Size(SystemInformation.WorkingArea.Width / 10, 0);
		public static readonly Size toolbarDesktopSize = new Size(0, 0);
		public const int toolbarTextWidth = 80;
		
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
#if DEBUG
				var path = Path.Combine(@"Z:\", Environment.ExpandEnvironmentVariables("%USERNAME%"));
#else
				var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), dirRootName);
#endif
				return path;
			}
		}
		/// <summary>
		/// ユーザー設定ファイル
		/// </summary>
		public static string UserMainSettingPath
		{
			get { return Path.Combine(UserSettingDirPath, mainSettingFileName); } 
		}
		
		public static string UserLauncherItemsPath
		{
			get { return Path.Combine(UserSettingDirPath, launcherItemsName);}
		}
		
		public static string UserBackupDirPath
		{
			get { return Path.Combine(UserSettingDirPath, backupDirName);}
		}
			
	}
	
	public struct AppEnv
	{
		public string AppFilePath { get { return "PE_APP_EXE"; } }
		public string AppDirPath { get { return "PE_APP_DIR"; } }
		public string AppUserDir { get { return "PE_USER_DIR"; } }
	}
	
	public enum HotKeyId: ushort
	{
		ShowCommand = 0x0001,
		HiddenFile,
		Extension,
	}
	
	public static class SystemLanguageName
	{
		const string application = "PE";
		const string year = "Y";
		const string month = "M";
		const string day = "D";
		const string hour = "h";
		const string minute = "m";
		const string second = "s";
		
	}
}
