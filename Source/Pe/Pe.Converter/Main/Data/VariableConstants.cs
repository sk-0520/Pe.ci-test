/*
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

namespace ContentTypeTextNet.Pe.PeMain.Data
{
    /// <summary>
    /// ほぼほぼ定数扱いだけど初回時にのみ変更かける。
    /// </summary>
    public sealed class VariableConstants
    {
        #region define

        //const string _rootDirectoryName = Constants.applicationName;

        //static readonly string _baseDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        #endregion

        #region variable

        string _settingRootDirectoryPath;
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


        public VariableConstants(string settingRootpath)
        {
            this._settingRootDirectoryPath = settingRootpath;
        }

        #region property

        internal string MutexName { get; private set; }

        /// <summary>
        /// ユーザールートディレクトリ。
        /// </summary>
        public string UserDirectoryPath { get { return Path.Combine(this._settingRootDirectoryPath, Constants.ApplicationName); } }
        /// <summary>
        /// ユーザー設定ディレクトリ。
        /// </summary>
        public string UserSettingDirectoryPath { get { return Path.Combine(UserDirectoryPath, Constants.settingDirectoryName); } }

        /// <summary>
        /// ログ保存ディレクトリ。
        /// </summary>
        internal string LogDirectoryPath { get { return this._logRootDirectoryPath; } }
        /// <summary>
        /// バックアップディレクトリパス。
        /// </summary>
        internal string UserBackupDirectoryPath { get { return Path.Combine(UserDirectoryPath, Constants.backupDirectoryName); } }
        /// <summary>
        /// アーカイブディレクトリ。
        /// </summary>
        internal string UserArchiveDirectoryPath { get { return Path.Combine(UserDirectoryPath, Constants.archiveDirectoryName); } }

        public string UserSettingMainSettingFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._mainSettingFileName); } }

        public string UserSettingLauncherItemSettingFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._launcherItemSettingFileName); } }
        public string UserSettingLauncherGroupItemSettingFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._launcherGroupItemSettingFileName); } }

        public string UserSettingNoteDirectoryPath { get { return Path.Combine(UserSettingDirectoryPath, this._noteDirectoryFileName); } }
        internal string UserSettingNoteBodyArchivePath { get { return Path.Combine(UserSettingNoteDirectoryPath, Constants.BodyArchiveFileName); } }
        public string UserSettingNoteIndexFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._noteIndexFileName); } }

        public string UserSettingClipboardDirectoryPath { get { return Path.Combine(UserSettingDirectoryPath, this._clipboardDirectoryFileName); } }
        internal string UserSettingClipboardBodyArchivePath { get { return Path.Combine(UserSettingClipboardDirectoryPath, Constants.BodyArchiveFileName); } }
        public string UserSettingClipboardIndexFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._clipboardIndexFileName); } }

        public string UserSettingTemplateDirectoryPath { get { return Path.Combine(UserSettingDirectoryPath, this._templateDirectoryFileName); } }
        internal string UserSettingTemplateBodyArchivePath { get { return Path.Combine(UserSettingTemplateDirectoryPath, Constants.BodyArchiveFileName); } }
        public string UserSettingTemplateIndexFilePath { get { return Path.Combine(UserSettingDirectoryPath, this._templateIndexFileName); } }

        internal string LanguageCode { get { return this._languageCode; } }

        internal bool FileLogging { get; private set; }
        public bool ForceAccept { get; private set; }

        internal bool IsQuickExecute
        {
            get
            {
                return ForceAccept;
            }
        }

        #endregion
    }
}
