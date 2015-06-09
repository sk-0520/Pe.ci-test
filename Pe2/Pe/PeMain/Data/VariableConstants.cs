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

	/// <summary>
	/// ほぼほぼ定数扱いだけど初回時にのみ変更かける。
	/// </summary>
	public sealed class VariableConstants
	{
		#region define

		const string _rootDirectoryName = Constants.programName;

		static readonly string _baseDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		#endregion

		#region variable

		string _settingRootDirectoryPath = _baseDirPath;
		string _logRootDirPath = Path.Combine(_baseDirPath, Constants.logDirectoryName);

		string _mainSettingFileName = Constants.mainSettingFileName;
		string _launcherItemSettingFileName = Constants.launcherItemSettingFileName;
		string _launcherGroupItemSettingFileName = Constants.launcherGroupItemSettingFileName;

		#endregion

		public VariableConstants()
		{ }

		public VariableConstants(CommandLine commandLine)
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
		public string ApplicationExecutablePath { get { return Assembly.GetExecutingAssembly().Location; } }
		/// <summary>
		/// 起動ディレクトリ
		/// </summary>
		public string ApplicationRootDirectoryPath { get { return Path.GetDirectoryName(ApplicationExecutablePath); } }

		/// <summary>
		/// bin/
		/// </summary>
		public string ApplicationBinDirectoryPath { get { return Path.Combine(ApplicationRootDirectoryPath, Constants.binDirectoryName); } }
		/// <summary>
		/// sbin/
		/// </summary>
		public string ApplicationSBinDirectoryPath { get { return Path.Combine(ApplicationRootDirectoryPath, Constants.sbinDirectoryName); } }
		/// <summary>
		/// lib/
		/// </summary>
		public string ApplicationLibraryDirectoryPath { get { return Path.Combine(ApplicationRootDirectoryPath, Constants.libDirectoryName); } }
		/// <summary>
		/// etc/
		/// </summary>
		public string ApplicationEtcDirectoryPath { get { return Path.Combine(ApplicationRootDirectoryPath, Constants.etcDirectoryName); } }
		/// <summary>
		/// etc/lang
		/// </summary>
		public string ApplicationLanguageDirectoryPath { get { return Path.Combine(ApplicationEtcDirectoryPath, Constants.langDirectoryName); } }
		/// <summary>
		/// doc/
		/// </summary>
		public string ApplicationDocumentDirectoryPath { get { return Path.Combine(ApplicationRootDirectoryPath, Constants.docDirectoryName); } }

		/// <summary>
		/// ユーザールートディレクトリ。
		/// </summary>
		public string UserDirectoryPath { get { return Path.Combine(_settingRootDirectoryPath, _rootDirectoryName); } }
		/// <summary>
		/// ユーザー設定ディレクトリ。
		/// </summary>
		public string UserSettingDirectoryPath { get { return Path.Combine(UserDirectoryPath, Constants.settingDirectoryName); } }
		/// <summary>
		/// ログ保存ディレクトリ。
		/// </summary>
		public string LogDirectoryPath { get { return _logRootDirPath; } }
		/// <summary>
		/// バックアップディレクトリパス。
		/// </summary>
		public string UserBackupDirectoryPath { get { return Path.Combine(UserDirectoryPath, Constants.backupDirectoryName); } }

		public string UserSettingFileMainSettingPath { get { return Path.Combine(UserSettingDirectoryPath, this._mainSettingFileName); } }
		public string UserSettingFileLauncherItemSettingPath { get { return Path.Combine(UserSettingDirectoryPath, this._launcherItemSettingFileName); } }
		public string UserSettingFileLauncherGroupItemSetting { get { return Path.Combine(UserSettingDirectoryPath, this._launcherGroupItemSettingFileName); } }

		#endregion
	}
}
