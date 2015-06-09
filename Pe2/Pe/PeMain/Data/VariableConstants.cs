namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
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
		string _logRootDirectoryPath = Path.Combine(_baseDirPath, Constants.logDirectoryName);

		string _mainSettingFileName = Constants.mainSettingFileName;
		string _launcherItemSettingFileName = Constants.launcherItemSettingFileName;
		string _launcherGroupItemSettingFileName = Constants.launcherGroupItemSettingFileName;

		string _languageCode = CultureInfo.CurrentCulture.Name;

		#endregion

		public VariableConstants()
		{
			FileLogging = false;
		}

		public VariableConstants(CommandLine commandLine)
			: this()
		{
			if (commandLine.HasOption("setting-root")) {
				this._settingRootDirectoryPath = Environment.ExpandEnvironmentVariables(commandLine.GetValue("setting-root"));
			}
			if (commandLine.HasOption("log")) {
				if (commandLine.HasValue("log")) {
					this._logRootDirectoryPath = Environment.ExpandEnvironmentVariables(commandLine.GetValue("log"));
				}
				FileLogging = true;
			}
		}

		#region property

		/// <summary>
		/// bin/
		/// </summary>
		public string ApplicationBinDirectoryPath { get { return Path.Combine(Constants.applicationRootDirectoryPath, Constants.binDirectoryName); } }
		/// <summary>
		/// sbin/
		/// </summary>
		public string ApplicationSBinDirectoryPath { get { return Path.Combine(Constants.applicationRootDirectoryPath, Constants.sbinDirectoryName); } }
		/// <summary>
		/// lib/
		/// </summary>
		public string ApplicationLibraryDirectoryPath { get { return Path.Combine(Constants.applicationRootDirectoryPath, Constants.libraryDirectoryName); } }
		/// <summary>
		/// etc/
		/// </summary>
		public string ApplicationEtcDirectoryPath { get { return Path.Combine(Constants.applicationRootDirectoryPath, Constants.etcDirectoryName); } }
		/// <summary>
		/// etc/lang
		/// </summary>
		public string ApplicationLanguageDirectoryPath { get { return Path.Combine(ApplicationEtcDirectoryPath, Constants.languageDirectoryName); } }
		/// <summary>
		/// etc/style
		/// </summary>
		public string ApplicationStyleDirectoryPath { get { return Path.Combine(ApplicationEtcDirectoryPath, Constants.styleDirectoryName); } }
		/// <summary>
		/// etc/script
		/// </summary>
		public string ApplicationScriptDirectoryPath { get { return Path.Combine(ApplicationEtcDirectoryPath, Constants.scriptDirectoryName); } }
		/// <summary>
		/// doc/
		/// </summary>
		public string ApplicationDocumentDirectoryPath { get { return Path.Combine(Constants.applicationRootDirectoryPath, Constants.documentDirectoryName); } }

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
		public string LogDirectoryPath { get { return _logRootDirectoryPath; } }
		/// <summary>
		/// バックアップディレクトリパス。
		/// </summary>
		public string UserBackupDirectoryPath { get { return Path.Combine(UserDirectoryPath, Constants.backupDirectoryName); } }

		public string UserSettingFileMainSettingPath { get { return Path.Combine(UserSettingDirectoryPath, this._mainSettingFileName); } }
		public string UserSettingFileLauncherItemSettingPath { get { return Path.Combine(UserSettingDirectoryPath, this._launcherItemSettingFileName); } }
		public string UserSettingFileLauncherGroupItemSetting { get { return Path.Combine(UserSettingDirectoryPath, this._launcherGroupItemSettingFileName); } }

		public string LanguageCode { get { return this._languageCode; } }

		public bool FileLogging { get; private set; }

		#endregion
	}
}
