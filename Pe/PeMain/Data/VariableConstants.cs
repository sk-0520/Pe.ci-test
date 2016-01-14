/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
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
        string _logRootDirectoryPath = string.Empty;

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
            if(commandLine.HasOption("setting-root")) {
                this._settingRootDirectoryPath = Environment.ExpandEnvironmentVariables(commandLine.GetValue("setting-root"));
            }

            this._logRootDirectoryPath = Path.Combine(UserDirectoryPath, Constants.logDirectoryName);
            if(commandLine.HasOption("log")) {
                if(commandLine.HasValue("log")) {
                    this._logRootDirectoryPath = Environment.ExpandEnvironmentVariables(commandLine.GetValue("log"));
                }
                FileLogging = true;
            }

            string mutexName = string.Empty;
            if(commandLine.HasValue("mutex")) {
                mutexName = commandLine.GetValue("mutex");
            }
            if(string.IsNullOrWhiteSpace(mutexName)) {
                mutexName = Constants.ApplicationName;
#if DEBUG
                mutexName += "_debug";
                //mutexName += new Random().Next().ToString();
#endif
            }
            MutexName = mutexName;

            if(commandLine.HasOption("accept")) {
                var acceptValue = commandLine.GetValue("accept").Trim();
                ForceAccept = acceptValue == "force";
            }

        }

        #region property

        public string MutexName { get; private set; }

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
        public string UserSettingNoteBodyArchivePath { get { return Path.Combine(UserSettingNoteDirectoryPath, Constants.BodyArchiveFileName); } }
        public string UserSettingNoteIndexFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._noteIndexFileName); } }

        public string UserSettingClipboardDirectoryPath { get { return Path.Combine(UserSettingDirectoryPath, this._clipboardDirectoryFileName); } }
        public string UserSettingClipboardBodyArchivePath { get { return Path.Combine(UserSettingClipboardDirectoryPath, Constants.BodyArchiveFileName); } }
        public string UserSettingClipboardIndexFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._clipboardIndexFileName); } }

        public string UserSettingTemplateDirectoryPath { get { return Path.Combine(UserSettingDirectoryPath, this._templateDirectoryFileName); } }
        public string UserSettingTemplateBodyArchivePath { get { return Path.Combine(UserSettingTemplateDirectoryPath, Constants.BodyArchiveFileName); } }
        public string UserSettingTemplateIndexFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._templateIndexFileName); } }

        public string LanguageCode { get { return this._languageCode; } }

        public bool FileLogging { get; private set; }
        public bool ForceAccept { get; private set; }

        public bool IsQuickExecute
        {
            get
            {
                return ForceAccept;
            }
        }

        #endregion
    }
}
