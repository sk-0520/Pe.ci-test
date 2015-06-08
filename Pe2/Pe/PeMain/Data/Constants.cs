namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;

	public sealed class Constants
	{
		#region define

		public const string programName = "Pe2";
		const string _rootDirectoryName = programName;

		#endregion

		#region variable

		static string _settingRootDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		static string _logRootDirPath = Path.Combine(UserSettingDirectoryPath, "log");

		#endregion

		public Constants()
		{
		}

		public Constants(CommandLine commandLine)
		{
			if(commandLine.HasOption("setting-root")) {
				_settingRootDirectoryPath = Environment.ExpandEnvironmentVariables(commandLine.GetValue("setting-root"));
			}
			if(commandLine.HasOption("log")) {
				if(commandLine.HasValue("log")) {
					_logRootDirPath = Environment.ExpandEnvironmentVariables(commandLine.GetValue("log"));
				}
			}
		}

		#region property

		/// <summary>
		/// 実行パス
		/// </summary>
		public static string ApplicationExecutablePath { get { return Assembly.GetExecutingAssembly().Location; } }
		/// <summary>
		/// 起動ディレクトリ
		/// </summary>
		public static string ApplicationRootDirectoryPath { get { return Path.GetDirectoryName(ApplicationExecutablePath); } }

		/// <summary>
		/// bin/
		/// </summary>
		public static string ApplicationBinDirectoryPath { get { return Path.Combine(ApplicationRootDirectoryPath, "bin"); } }
		/// <summary>
		/// sbin/
		/// </summary>
		public static string ApplicationSBinDirPath { get { return Path.Combine(ApplicationRootDirectoryPath, "sbin"); } }
		/// <summary>
		/// lib/
		/// </summary>
		public static string ApplicationLibraryDirectoryPath { get { return Path.Combine(ApplicationRootDirectoryPath, "lib"); } }
		/// <summary>
		/// etc/
		/// </summary>
		public static string ApplicationEtcDirPath { get { return Path.Combine(ApplicationRootDirectoryPath, "etc"); } }
		/// <summary>
		/// doc/
		/// </summary>
		public static string ApplicationDocumentDirPath { get { return Path.Combine(ApplicationRootDirectoryPath, "doc"); } }

		/// <summary>
		/// ユーザールートディレクトリ。
		/// </summary>
		public static string UserDirectoryPath { get { return Path.Combine(_settingRootDirectoryPath, _rootDirectoryName); } }
		/// <summary>
		/// ユーザー設定ディレクトリ。
		/// </summary>
		public static string UserSettingDirectoryPath { get { return Path.Combine(UserDirectoryPath, "setting"); } }
		/// <summary>
		/// ログ保存ディレクトリ。
		/// </summary>
		public static string LogDirectoryPath { get { return _logRootDirPath; } }
		/// <summary>
		/// バックアップディレクトリパス。
		/// </summary>
		public static string UserBackupDirectoryPath { get { return Path.Combine(UserDirectoryPath, "backup"); } }

		public string UserSettingFileMainSettingPath { get { return Path.Combine(UserSettingDirectoryPath, "main-setting.xml"); } }
		public string UserSettingFileLauncherItemSettingPath { get { return Path.Combine(UserSettingDirectoryPath, "item-setting.xml"); } }
		public string UserSettingFileLauncherGroupItemSetting { get { return Path.Combine(UserSettingDirectoryPath, "group-item.xml"); } }

		#endregion
	}
}
