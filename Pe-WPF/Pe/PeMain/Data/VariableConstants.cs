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

		//const string _rootDirectoryName = Constants.applicationName;

		static readonly string _baseDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		#endregion

		#region variable

		string _settingRootDirectoryPath = _baseDirPath;
		string _logRootDirectoryPath = Path.Combine(_baseDirPath, Constants.logDirectoryName);

		string _mainSettingFileName = Constants.mainSettingFileName;
		string _launcherItemSettingFileName = Constants.launcherItemSettingFileName;
		string _launcherGroupItemSettingFileName = Constants.launcherGroupItemSettingFileName;

		string _noteDirectoryFileName = Constants.noteSaveDirectoryName;
		string _noteIndexFileName = Constants.noteIndexFileName;

		string _clipboardDirectoryFileName = Constants.clipboardSaveDirectoryName;
		string _clipboardIndexFileName = Constants.clipboardIndexFileName;

		string _templateDirectoryFileName = Constants.templateSaveDirectoryName;
		string _templateIndexFileName = Constants.templateIndexFileName;

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
		/// ユーザールートディレクトリ。
		/// </summary>
		public string UserDirectoryPath { get { return Path.Combine(_settingRootDirectoryPath, Constants.ApplicationName); } }
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
		/// <summary>
		/// アーカイブディレクトリ。
		/// </summary>
		public string UserArchiveDirectoryPath { get { return Path.Combine(UserDirectoryPath, Constants.archiveDirectoryName); } }

		public string UserSettingMainSettingFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._mainSettingFileName); } }

		public string UserSettingLauncherItemSettingFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._launcherItemSettingFileName); } }
		public string UserSettingLauncherGroupItemSettingFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._launcherGroupItemSettingFileName); } }

		public string UserSettingNoteDirectoryPath { get { return Path.Combine(UserSettingDirectoryPath, this._noteDirectoryFileName); } }
		public string UserSettingNoteIndexFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._noteIndexFileName); } }

		public string UserSettingClipboardDirectoryPath { get { return Path.Combine(UserSettingDirectoryPath, this._clipboardDirectoryFileName); } }
		public string UserSettingClipboardIndexFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._clipboardIndexFileName); } }

		public string UserSettingTemplateDirectoryPath { get { return Path.Combine(UserSettingDirectoryPath, this._templateDirectoryFileName); } }
		public string UserSettingTemplateIndexFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._templateIndexFileName); } }

		public string LanguageCode { get { return this._languageCode; } }

		public bool FileLogging { get; private set; }

		#endregion
	}
}
