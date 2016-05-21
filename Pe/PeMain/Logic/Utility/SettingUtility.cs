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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using Implement = ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore;
using ContentTypeTextNet.Pe.PeMain.Data;
using System.Security.Cryptography;
using System.IO;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    /// <summary>
    /// 設定データを上手いことなんやかんやする。
    /// </summary>
    public static class SettingUtility
    {
        #region define

        const int userIdLength = 32;

        #endregion

        #region check

        internal static bool IsIllegalPlusNumber(double number)
        {
            return double.IsNaN(number) || number <= 0;
        }

        internal static bool IsIllegalPlusNumber(int number)
        {
            return number <= 0;
        }

        internal static bool IsIllegalString(string s)
        {
            return s == null;
        }

        #endregion

        #region function

        public static bool CheckAccept(RunningInformationSettingModel model, INonProcess nonProcess)
        {
            if(!model.Accept) {
                // 完全に初回
                nonProcess.Logger.Debug("first");
                return false;
            }

            if(model.LastExecuteVersion == null) {
                // 何らかの理由で前回実行時のバージョン格納されていない
                nonProcess.Logger.Debug("last version == null");
                return false;
            }

            if(model.LastExecuteVersion < Constants.AcceptVersion) {
                // 前回バージョンから強制定期に使用許諾が必要
                nonProcess.Logger.Debug("last version < accept version");
                return false;
            }

            return true;
        }

        public static bool CheckUserId(RunningInformationSettingModel model)
        {
            var userId = model.UserId ?? string.Empty;
            if(userId.Length != userIdLength) {
                return false;
            }

            var hash = "0123456789abcdef";
            return userId.All(c => hash.Contains(c));
        }

        /// <summary>
        /// GUIDを一意なものに変更する。
        /// <para>関連する部分までは面倒見ない。</para>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list"></param>
        internal static int UpdateUniqueGuid(GuidModelBase model, IReadOnlyList<GuidModelBase> list, int maxRetryCount = 10000)
        {
            int count = 0;
            while(list.Any(l => l.Id == model.Id)) {
                if(maxRetryCount < count++) {
                    throw new Exception($"over {nameof(maxRetryCount)}({maxRetryCount})");
                }
                model.Id = Guid.NewGuid();
            }

            return count;
        }


        #region create

        /// <summary>
        /// 指定データからユーザー識別子を作成する。
        /// <para>ハッシュ処理統一のために存在している</para>
        /// </summary>
        /// <param name="buffer">データ</param>
        /// <returns>ユーザー識別子。</returns>
        static string CreateUserId(byte[] buffer)
        {
            using(var hash = MD5.Create()) {
                var binary = hash.ComputeHash(buffer);
                var result = new StringBuilder(binary.Length);
                foreach(var b in binary) {
                    result.AppendFormat("{0:x2}", b);
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// 実行環境からユーザー識別子を作成する。
        /// </summary>
        /// <returns></returns>
        public static string CreateUserIdFromEnvironment()
        {
            using(var info = new AppInformationCollection()) {
                var infoCpu = info.GetCPU();
                var infoMem = info.GetMemory();

                var user = CovertUtility.ToByteArray(Environment.UserName);
                var os = CovertUtility.ToByteArray(Environment.OSVersion);
                var cpu = CovertUtility.ToByteArray(infoCpu.Items["Name"]);
                var mem = CovertUtility.ToByteArray(infoMem.Items["TotalVisibleMemorySize"]);

                using(var stream = new MemoryStream()) {
                    stream.Write(user, 0, user.Length);
                    stream.Write(os, 0, os.Length);
                    stream.Write(cpu, 0, cpu.Length);
                    stream.Write(mem, 0, mem.Length);

                    return CreateUserId(stream.GetBuffer());
                }
            }
        }

        /// <summary>
        /// 時間からユーザー識別子を作成する。
        /// <para>ランダム作成用</para>
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string CreateUserIdFromDateTime(DateTime dateTime)
        {
            var timestamp = CovertUtility.ToByteArray(dateTime);
            return CreateUserId(timestamp);
        }

        static TModel CreateModelName<TModel>(IEnumerable<TModel> items, ILanguage language, string nameKey)
            where TModel : IName, new()
        {
            var newName = language[nameKey];

            var result = new TModel();
            if(items != null || items.Any()) {
                newName = TextUtility.ToUniqueDefault(newName, items.Select(g => g.Name));
            }
            result.Name = newName;

            return result;
        }

        public static LauncherItemModel CreateLauncherItem(LauncherItemCollectionModel items, INonProcess nonProcess)
        {
            var result = CreateModelName(items, nonProcess.Language, "new/launcher-name");
            InitializeLauncherItem(result, null, nonProcess);
            return result;
        }

        public static LauncherGroupItemModel CreateLauncherGroup(LauncherGroupItemCollectionModel group, INonProcess nonProcess)
        {
            var result = CreateModelName(group, nonProcess.Language, "new/group-name");
            InitializeLauncherGroupItem(result, null, nonProcess);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="nonProcess"></param>
        /// <returns></returns>
        public static TemplateIndexItemModel CreateTemplateIndexItem(TemplateIndexItemCollectionModel items, INonProcess nonProcess)
        {
            var result = CreateModelName(items, nonProcess.Language, "new/template-name");
            return result;
        }


        #endregion

        #region increment

        /// <summary>
        /// 実行情報の切り上げ。
        /// </summary>
        /// <param name="model"></param>
        public static void IncrementRunningInformation(RunningInformationSettingModel model)
        {
            CheckUtility.EnforceNotNull(model);

            model.LastExecuteVersion = Constants.applicationVersionNumber;
            model.ExecuteCount += 1;
        }

        /// <summary>
        /// リスト構造の整理。
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
        static void IncrementList(CollectionModel<string> list, string value)
        {
            if(!string.IsNullOrEmpty(value)) {
                var index = list.FindIndex(s => s == value);
                if(index != -1) {
                    list.RemoveAt(index);
                }
                list.Insert(0, value);
            }
        }

        public static void IncrementLauncherItem(LauncherItemModel launcherItem, string option, string workDirPath, INonProcess nonProcess)
        {
            CheckUtility.EnforceNotNull(launcherItem);

            var dateTime = DateTime.Now;

            IncrementList(launcherItem.History.Options, option);
            IncrementList(launcherItem.History.WorkDirectoryPaths, workDirPath);

            launcherItem.History.ExecuteTimestamp = dateTime;
            launcherItem.History.ExecuteCount = RangeUtility.Increment(launcherItem.History.ExecuteCount);

            launcherItem.History.Update(dateTime);
        }

        #endregion

        #endregion

        #region initialize

        public static void InitializeLoggingSetting(LoggingSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeLoggingSetting.Correction(setting, previousVersion, nonProcess);
        }

        private static void InitializeStreamSetting(StreamSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeStreamSetting.Correction(setting, previousVersion, nonProcess);
        }

        public static void InitializeToolbarSetting(ToolbarSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeToolbarSetting.Correction(setting, previousVersion, nonProcess);

            foreach(var toolbar in setting.Items) {
                InitializeToolbarItem(toolbar, previousVersion, nonProcess);
            }
        }

        public static void InitializeToolbarItem(ToolbarItemModel model, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeToolbarItem.Correction(model, previousVersion, nonProcess);
        }

        public static void InitializeWindowSaveSetting(WindowSaveSettingModel model, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeWindowSaveSetting.Correction(model, previousVersion, nonProcess);
        }

        public static void InitializeNoteSetting(NoteSettingModel model, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeNoteSetting.Correction(model, previousVersion, nonProcess);
        }

        public static void InitializeClipboardSetting(ClipboardSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            var init = new Implement.InitializeClipboardSetting(setting, previousVersion, nonProcess);
            init.Correction();
        }

        public static void InitializeTemplateSetting(TemplateSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeTemplateSetting.Correction(setting, previousVersion, nonProcess);
        }

        public static void InitializeCommandSetting(CommandSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            var init = new Implement.InitializeCommandSetting(setting, previousVersion, nonProcess);
            init.Correction();
        }

        private static void InitializeGeneralSetting(GeneralSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            var init = new Implement.InitializeGeneralSetting(setting, previousVersion, nonProcess);
            init.Correction();
        }


        /// <summary>
        /// 本体設定を補正。
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="previousVersion"></param>
        /// <param name="nonProcess"></param>
        public static void InitializeMainSetting(MainSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            CheckUtility.EnforceNotNull(setting);

            InitializeRunningInformationSetting(setting.RunningInformation, previousVersion, nonProcess);
            InitializeLoggingSetting(setting.Logging, previousVersion, nonProcess);
            InitializeStreamSetting(setting.Stream, previousVersion, nonProcess);
            InitializeToolbarSetting(setting.Toolbar, previousVersion, nonProcess);
            InitializeNoteSetting(setting.Note, previousVersion, nonProcess);
            InitializeWindowSaveSetting(setting.WindowSave, previousVersion, nonProcess);
            InitializeClipboardSetting(setting.Clipboard, previousVersion, nonProcess);
            InitializeTemplateSetting(setting.Template, previousVersion, nonProcess);
            InitializeCommandSetting(setting.Command, previousVersion, nonProcess);
            InitializeGeneralSetting(setting.General, previousVersion, nonProcess);
        }

        private static void InitializeRunningInformationSetting(RunningInformationSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeRunningInformationSetting.Correction(setting, previousVersion, nonProcess);
        }

        public static void InitializeLauncherItemSetting(LauncherItemSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeLauncherItemSetting.Correction(setting, previousVersion, nonProcess);

            foreach(var item in setting.Items) {
                InitializeLauncherItem(item, previousVersion, nonProcess);
            }
        }

        public static void InitializeLauncherItem(LauncherItemModel item, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeLauncherItem.Correction(item, previousVersion, nonProcess);
        }

        public static void InitializeLauncherGroupSetting(LauncherGroupSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeLauncherGroupSetting.Correction(setting, previousVersion, nonProcess);

            foreach(var item in setting.Groups) {
                InitializeLauncherGroupItem(item, previousVersion, nonProcess);
            }
        }

        public static void InitializeLauncherGroupItem(LauncherGroupItemModel item, Version previousVersion, INonProcess nonProcess)
        {
            var init = new Implement.InitializeLauncherGroupItem(item, previousVersion, nonProcess);
            init.Correction();
        }

        public static void InitializeNoteIndexSetting(NoteIndexSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeNoteIndexSetting.Correction(setting, previousVersion, nonProcess);

            CheckUtility.EnforceNotNull(setting);
            foreach(var noteIndex in setting.Items) {
                InitializeNoteIndexItem(noteIndex, previousVersion, nonProcess);
            }
        }

        public static void InitializeNoteIndexItem(NoteIndexItemModel indexItem, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeNoteIndexItem.Correction(indexItem, previousVersion, nonProcess);
        }

        public static void InitializeTemplateIndexSetting(TemplateIndexSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeTemplateIndexSetting.Correction(setting, previousVersion, nonProcess);
            foreach(var templateItem in setting.Items) {
                InitializeTemplateIndexSetting(templateItem, previousVersion, nonProcess);
            }
        }

        public static void InitializeTemplateIndexSetting(TemplateIndexItemModel model, Version previousVersion, INonProcess nonProcess)
        {
            Implement.InitializeTemplateIndexItem.Correction(model, previousVersion, nonProcess);
        }

        public static void InitializeClipboardIndexSetting(ClipboardIndexSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            var init = new Implement.InitializeClipboardIndexSetting(setting, previousVersion, nonProcess);
            init.Correction();
            foreach(var clipboardItem in setting.Items) {
                InitializeClipboardIndexItem(clipboardItem, previousVersion, nonProcess);
            }
        }

        public static void InitializeClipboardIndexItem(ClipboardIndexItemModel clipboardItend, Version previousVersion, INonProcess nonProcess)
        {
            var init = new Implement.InitializeClipboardIndexItem(clipboardItend, previousVersion, nonProcess);
            init.Correction();
        }

        #endregion
    }
}
