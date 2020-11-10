using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize
{
    /// <summary>
    /// ランチャーアイテム編集処理。
    /// </summary>
    public class LauncherItemCustomizeEditorElement: ElementBase, ILauncherItemId
    {
        #region variable

        bool _isLazyLoad;

        #endregion

        public LauncherItemCustomizeEditorElement(Guid launcherItemId, ILauncherItemAddonFinder launcherItemAddonFinder, LauncherItemAddonContextFactory launcherItemAddonContextFactory, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier fileDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;
            LauncherItemAddonFinder = launcherItemAddonFinder;
            LauncherItemAddonContextFactory = launcherItemAddonContextFactory;
            ClipboardManager = clipboardManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            TemporaryDatabaseBarrier = temporaryDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        ILauncherItemAddonFinder LauncherItemAddonFinder { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        ILargeDatabaseBarrier FileDatabaseBarrier { get; }
        ITemporaryDatabaseBarrier TemporaryDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }
        LauncherItemAddonContextFactory LauncherItemAddonContextFactory { get; }
        IClipboardManager ClipboardManager { get; }

        /// <summary>
        /// 一括用遅延読み込みモードか。
        /// <para>設定画面が遅い(#634)対応。</para>
        /// </summary>
        internal bool IsLazyLoad
        {
            get => this._isLazyLoad;
            protected private set => SetProperty(ref this._isLazyLoad, value);
        }

        public string Name { get; set; } = string.Empty;
        public string Code { get; protected private set; } = string.Empty;
        public LauncherItemKind Kind { get; protected private set; }
        public bool IsEnabledCommandLauncher { get; set; }

        public IconData IconData { get; set; } = new IconData();
        public string Comment { get; set; } = string.Empty;

        public ObservableCollection<string> TagItems { get; } = new ObservableCollection<string>();

        private bool IsSaved { get; set; }

        #region file

        public LauncherFileData? File { get; private set; }
        public ObservableCollection<LauncherEnvironmentVariableData>? EnvironmentVariableItems { get; private set; }
        public LauncherRedoData? Redo { get; private set; }
        #endregion

        #region storeapp

        public LauncherStoreAppData? StoreApp { get; private set; }

        #endregion

        #region addon

        public bool LauncherItemSupportedPreferences { get; private set; }
        IPlugin? LauncherItemPlugin { get; set; }
        ILauncherItemExtension? LauncherItemExtension { get; set; }
        ILauncherItemPreferences? LauncherItemPreferences { get; set; }

        #endregion

        #endregion


        #region function

        void LoadFileCore(IDatabaseContext context, IDatabaseImplementation implementation)
        {
            Debug.Assert(Kind == LauncherItemKind.File);

            var launcherFilesEntityDao = new LauncherFilesEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);
            File = launcherFilesEntityDao.SelectFile(LauncherItemId);

            var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);
            var environmentVariableItems = launcherEnvVarsEntityDao.SelectEnvVarItems(LauncherItemId).ToList();
            EnvironmentVariableItems = new ObservableCollection<LauncherEnvironmentVariableData>(environmentVariableItems);

            var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);
            if(launcherRedoItemsEntityDao.SelectExistsLauncherRedoItem(LauncherItemId)) {
                var redoData = launcherRedoItemsEntityDao.SelectLauncherRedoItem(LauncherItemId);
                var launcherRedoSuccessExitCodesEntityDao = new LauncherRedoSuccessExitCodesEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);
                var exitCodes = launcherRedoSuccessExitCodesEntityDao.SelectRedoSuccessExitCodes(LauncherItemId);
                redoData.SuccessExitCodes.SetRange(exitCodes);
                Redo = redoData;
            } else {
                Redo = LauncherRedoData.GetDisable();
            }
        }

        protected void LoadFile()
        {
            using(var context = MainDatabaseBarrier.WaitRead()) {
                LoadFileCore(context, context.Implementation);
            }
        }

        void LoadAddonCore(IDatabaseContextsPack databaseContextsPack)
        {
            Debug.Assert(Kind == LauncherItemKind.Addon);
            Debug.Assert(LauncherItemPlugin == null);
            Debug.Assert(LauncherItemExtension == null);
            Debug.Assert(LauncherItemPreferences == null);

            var launcherAddonsEntityDao = new LauncherAddonsEntityDao(databaseContextsPack.Main.Context, DatabaseStatementLoader, databaseContextsPack.Main.Implementation, LoggerFactory);
            var pluginId = launcherAddonsEntityDao.SelectAddonPluginId(LauncherItemId);

            if(!LauncherItemAddonFinder.Exists(pluginId)) {
                Logger.LogError("ランチャーアイテムアドオンが存在しない: {0}", pluginId);
                return;
            }
            LauncherItemPlugin = LauncherItemAddonFinder.GetPlugin(pluginId);

            LauncherItemExtension = LauncherItemAddonFinder.Find(LauncherItemId, pluginId);
            LauncherItemSupportedPreferences = LauncherItemExtension.SupportedPreferences;
            if(!LauncherItemSupportedPreferences) {
                Logger.LogInformation("{0} はアドオン設定をサポートしていない", LauncherItemPlugin.PluginInformations.PluginIdentifiers);
                return;
            }

            using(var context = LauncherItemAddonContextFactory.CreateContext(LauncherItemPlugin.PluginInformations, LauncherItemId, databaseContextsPack, true)) {
                LauncherItemPreferences = LauncherItemExtension.CreatePreferences(context);
            }
        }

        protected void LoadAddon()
        {
            using var pack = PersistentHelper.WaitReadPack(MainDatabaseBarrier, FileDatabaseBarrier, TemporaryDatabaseBarrier, DatabaseCommonStatus.CreateCurrentAccount());
            LoadAddonCore(pack);
        }

        internal UserControl BeginLauncherItemPreferences()
        {
            Debug.Assert(LauncherItemSupportedPreferences);
            Debug.Assert(LauncherItemPlugin != null);
            Debug.Assert(LauncherItemPreferences != null);

            using var pack = PersistentHelper.WaitReadPack(MainDatabaseBarrier, FileDatabaseBarrier, TemporaryDatabaseBarrier, DatabaseCommonStatus.CreateCurrentAccount());
            using(var context = LauncherItemAddonContextFactory.CreatePreferencesLoadContext(LauncherItemPlugin.PluginInformations, LauncherItemId, pack)) {
                return LauncherItemPreferences.BeginPreferences(context);
            }
        }

        internal bool CheckLauncherItemPreferences()
        {
            Debug.Assert(LauncherItemSupportedPreferences);
            Debug.Assert(LauncherItemPlugin != null);
            Debug.Assert(LauncherItemPreferences != null);

            using var pack = PersistentHelper.WaitReadPack(MainDatabaseBarrier, FileDatabaseBarrier, TemporaryDatabaseBarrier, DatabaseCommonStatus.CreateCurrentAccount());
            using(var context = LauncherItemAddonContextFactory.CreatePreferencesCheckContext(LauncherItemPlugin.PluginInformations, LauncherItemId, pack)) {
                LauncherItemPreferences.CheckPreferences(context);
                return context.HasError;
            }
        }

        internal void SaveLauncherPreferences(IDatabaseContextsPack databaseContextsPack)
        {
            Debug.Assert(LauncherItemSupportedPreferences);
            Debug.Assert(LauncherItemPlugin != null);
            Debug.Assert(LauncherItemPreferences != null);

            using(var context = LauncherItemAddonContextFactory.CreatePreferencesSaveContext(LauncherItemPlugin.PluginInformations, LauncherItemId, databaseContextsPack)) {
                LauncherItemPreferences.SavePreferences(context);
                IsSaved = true;
            }
        }

        internal void EndLauncherPreferences()
        {
            Debug.Assert(LauncherItemSupportedPreferences);
            Debug.Assert(LauncherItemPlugin != null);
            Debug.Assert(LauncherItemPreferences != null);

            using(var context = LauncherItemAddonContextFactory.CreatePreferencesEndContext(LauncherItemPlugin.PluginInformations, LauncherItemId)) {
                context.IsSaved = IsSaved;
                LauncherItemPreferences.EndPreferences(context);
            }
        }

        internal string GetLauncherItemPluginHeader()
        {
            if(LauncherItemPlugin == null) {
                return Properties.Resources.String_LauncherItemCustomizeControl_UnloadedPlugin_Header;
            }

            Debug.Assert(LauncherItemPlugin != null);
            Debug.Assert(LauncherItemSupportedPreferences);
            Debug.Assert(LauncherItemPreferences != null);

            //TODO: なんかこう、ヘッダ名のために仰々しいなぁ XAML でプラグインID薄くしたいし。。。
            return LauncherItemPlugin.PluginInformations.PluginIdentifiers.ToString()!;
        }

        void LoadLauncherItem()
        {
            ThrowIfDisposed();

            using var pack = PersistentHelper.WaitReadPack(MainDatabaseBarrier, FileDatabaseBarrier, TemporaryDatabaseBarrier, DatabaseCommonStatus.CreateCurrentAccount());

            var launcherItemsDao = new LauncherItemsEntityDao(pack.Main.Context, DatabaseStatementLoader, pack.Main.Implementation, LoggerFactory);
            var launcherItemData = launcherItemsDao.SelectLauncherItem(LauncherItemId);

            Name = launcherItemData.Name;
            Code = launcherItemData.Code;
            Kind = launcherItemData.Kind;
            IconData = launcherItemData.Icon;
            IsEnabledCommandLauncher = launcherItemData.IsEnabledCommandLauncher;
            Comment = launcherItemData.Comment;

            var launcherTagsEntityDao = new LauncherTagsEntityDao(pack.Main.Context, DatabaseStatementLoader, pack.Main.Implementation, LoggerFactory);
            var tagItems = launcherTagsEntityDao.SelectTags(LauncherItemId);
            TagItems.SetRange(tagItems);

            switch(Kind) {
                case LauncherItemKind.File: {
                        LoadFileCore(pack.Main.Context, pack.Main.Implementation);
                    }
                    break;

                case LauncherItemKind.StoreApp: {
                        var launcherStoreAppsEntityDao = new LauncherStoreAppsEntityDao(pack.Main.Context, DatabaseStatementLoader, pack.Main.Implementation, LoggerFactory);
                        StoreApp = launcherStoreAppsEntityDao.SelectStoreApp(LauncherItemId);
                    }
                    break;

                case LauncherItemKind.Addon: {
                        LoadAddonCore(pack);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// アイコンが変更されたか。
        /// </summary>
        /// <param name="iconData"></param>
        /// <param name="kindIconValue">種別により異なる生アイコン情報。</param>
        /// <returns>真: アイコンが変更された。</returns>
        private bool CheckIconChanged(LauncherIconData currentFileIcon, IReadOnlyIconData iconData, string kindIconValue)
        {
            switch(Kind) {
                case LauncherItemKind.File: {
                        if(!PathUtility.IsEqual(currentFileIcon.Icon.Path, iconData.Path)) {
                            return true;
                        }
                        if(currentFileIcon.Icon.Index != iconData.Index) {
                            return true;
                        }

                        if(!PathUtility.IsEqual(currentFileIcon.Path.Path, kindIconValue)) {
                            return true;
                        }

                        return false;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// アイテム保存。
        /// </summary>
        /// <param name="commandsPack"></param>
        /// <returns>アイコンの削除が必要か。</returns>
        public bool SaveItem(IDatabaseContextsPack commandsPack)
        {
            ThrowIfDisposed();

            var itemData = new LauncherItemData() {
                LauncherItemId = LauncherItemId,
                Kind = Kind,
                Code = Code,
                Name = Name,
                IsEnabledCommandLauncher = IsEnabledCommandLauncher,
                Comment = Comment,
                Icon = new IconData() {
                    Path = IconData.Path,
                    Index = IconData.Index,
                },
            };

            var iconChangedResult = false;

            var launcherItemsEntityDao = new LauncherItemsEntityDao(commandsPack.Main.Context, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);
            var launcherTagsEntityDao = new LauncherTagsEntityDao(commandsPack.Main.Context, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);

            var launcherItemDomainDao = new LauncherItemDomainDao(commandsPack.Main.Context, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);
            var currentFileIcon = launcherItemDomainDao.SelectFileIcon(LauncherItemId);

            launcherItemsEntityDao.UpdateCustomizeLauncherItem(itemData, commandsPack.CommonStatus);
            switch(Kind) {
                case LauncherItemKind.File: {
                        Debug.Assert(File != null);
                        Debug.Assert(EnvironmentVariableItems != null);
                        Debug.Assert(Redo != null);

                        iconChangedResult = CheckIconChanged(currentFileIcon, itemData.Icon, File.Path);

                        var launcherFilesEntityDao = new LauncherFilesEntityDao(commandsPack.Main.Context, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);
                        var launcherMergeEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commandsPack.Main.Context, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);
                        var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(commandsPack.Main.Context, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);
                        var launcherRedoSuccessExitCodesEntityDao = new LauncherRedoSuccessExitCodesEntityDao(commandsPack.Main.Context, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);

                        launcherFilesEntityDao.UpdateCustomizeLauncherFile(itemData.LauncherItemId, File, File, commandsPack.CommonStatus);
                        launcherRedoItemsEntityDao.UpdateRedoItem(itemData.LauncherItemId, Redo, commandsPack.CommonStatus);

                        launcherRedoSuccessExitCodesEntityDao.DeleteSuccessExitCodes(itemData.LauncherItemId);
                        launcherRedoSuccessExitCodesEntityDao.InsertSuccessExitCodes(itemData.LauncherItemId, Redo.SuccessExitCodes, commandsPack.CommonStatus);

                        launcherMergeEnvVarsEntityDao.DeleteEnvVarItemsByLauncherItemId(itemData.LauncherItemId);
                        launcherMergeEnvVarsEntityDao.InsertEnvVarItems(itemData.LauncherItemId, EnvironmentVariableItems, commandsPack.CommonStatus);
                    }
                    break;

                case LauncherItemKind.StoreApp: {
                        Debug.Assert(StoreApp != null);

                        var launcherStoreAppsEntityDao = new LauncherStoreAppsEntityDao(commandsPack.Main.Context, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);
                        launcherStoreAppsEntityDao.UpdateStoreApp(itemData.LauncherItemId, StoreApp, commandsPack.CommonStatus);
                    }
                    break;

                case LauncherItemKind.Addon: {
                        if(LauncherItemPlugin == null) {
                            Logger.LogWarning("読み込めてないプラグインはプラグイン設定箇所スキップ");
                            break;
                        }
                        Debug.Assert(LauncherItemPlugin != null);
                        Debug.Assert(LauncherItemSupportedPreferences);
                        Debug.Assert(LauncherItemExtension != null);
                        Debug.Assert(LauncherItemPreferences != null);

                        SaveLauncherPreferences(commandsPack);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            launcherTagsEntityDao.DeleteTagByLauncherItemId(itemData.LauncherItemId);
            launcherTagsEntityDao.InsertTags(itemData.LauncherItemId, TagItems, commandsPack.CommonStatus);

            return iconChangedResult;
        }

        public void ClearIcon(IDatabaseContext context, IDatabaseImplementation implementation)
        {
            ThrowIfDisposed();

            var launcherItemIconsEntityDao = new LauncherItemIconsEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);
            var launcherItemIconStatusEntityDao = new LauncherItemIconStatusEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);

            launcherItemIconsEntityDao.DeleteAllSizeImageBinary(LauncherItemId);
            launcherItemIconStatusEntityDao.DeleteAllSizeLauncherItemIconState(LauncherItemId);
        }

        public void Save()
        {
            ThrowIfDisposed();

            bool needToIconClear;

            using(var pack = PersistentHelper.WaitWritePack(MainDatabaseBarrier, FileDatabaseBarrier, TemporaryDatabaseBarrier, DatabaseCommonStatus.CreateCurrentAccount())) {
                needToIconClear = SaveItem(pack);
                pack.Commit();
            }
            if(needToIconClear) {
                using(var context = FileDatabaseBarrier.WaitWrite()) {
                    ClearIcon(context, context.Implementation);
                    context.Commit();
                }
            }
        }

        public LauncherIconFactory CreateLauncherIconFactory()
        {
            return new LauncherIconFactory(LauncherItemId, Kind, LauncherItemAddonFinder, MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
        }


        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            if(IsLazyLoad) {
                return;
            }

            LoadLauncherItem();
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    TagItems.Clear();

                    if(LauncherItemPreferences != null) {
                        EndLauncherPreferences();
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherItemId
        public Guid LauncherItemId { get; }
        #endregion
    }
}
