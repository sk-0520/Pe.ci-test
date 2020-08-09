using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
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

        public LauncherItemCustomizeEditorElement(Guid launcherItemId, ILauncherItemAddonFinder launcherItemAddonFinder, LauncherItemAddonContextFactory launcherItemAddonContextFactory, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
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
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
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

        #region file

        public LauncherFileData? File { get; private set; }
        public ObservableCollection<LauncherEnvironmentVariableData>? EnvironmentVariableItems { get; private set; }
        public LauncherRedoData? Redo { get; private set; }
        #endregion

        #region storeapp

        public LauncherStoreAppData? StoreApp { get; private set; }

        #endregion

        #region addon

        ILauncherItemExtension? LauncherItemExtension { get; set; }
        public ILauncherItemPreferences? LauncherItemPreferences { get; private set; }

        #endregion

        #endregion


        #region function

        void LoadFileCore(IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            Debug.Assert(Kind == LauncherItemKind.File);

            var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);
            File = launcherFilesEntityDao.SelectFile(LauncherItemId);

            var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);
            var environmentVariableItems = launcherEnvVarsEntityDao.SelectEnvVarItems(LauncherItemId).ToList();
            EnvironmentVariableItems = new ObservableCollection<LauncherEnvironmentVariableData>(environmentVariableItems);

            var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);
            if(launcherRedoItemsEntityDao.SelectExistsLauncherRedoItem(LauncherItemId)) {
                var redoData = launcherRedoItemsEntityDao.SelectLauncherRedoItem(LauncherItemId);
                var launcherRedoSuccessExitCodesEntityDao = new LauncherRedoSuccessExitCodesEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);
                var exitCodes = launcherRedoSuccessExitCodesEntityDao.SelectRedoSuccessExitCodes(LauncherItemId);
                redoData.SuccessExitCodes.SetRange(exitCodes);
                Redo = redoData;
            } else {
                Redo = LauncherRedoData.GetDisable();
            }
        }

        protected void LoadFile()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                LoadFileCore(commander, commander.Implementation);
            }
        }

        void LoadAddonCore(IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            Debug.Assert(Kind == LauncherItemKind.Addon);

            var launcherAddonsEntityDao = new LauncherAddonsEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);
            var pluginId = launcherAddonsEntityDao.SelectAddonPluginId(LauncherItemId);

            if(LauncherItemAddonFinder.Exists(pluginId)) {
                Logger.LogError("ランチャーアイテムアドオンが存在しない: {0}", pluginId);
                return;
            }
            var plugin = LauncherItemAddonFinder.GetPlugin(pluginId);

            //LauncherItemAddonContextFactory.CreateContext(plugin.PluginInformations, LauncherItemId, , true);
        }

        protected void LoadAddon()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                LoadAddonCore(commander, commander.Implementation);
            }
        }

        void LoadLauncherItem()
        {
            ThrowIfDisposed();

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsDao = new LauncherItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                var launcherItemData = launcherItemsDao.SelectLauncherItem(LauncherItemId);

                Name = launcherItemData.Name;
                Code = launcherItemData.Code;
                Kind = launcherItemData.Kind;
                IconData = launcherItemData.Icon;
                IsEnabledCommandLauncher = launcherItemData.IsEnabledCommandLauncher;
                Comment = launcherItemData.Comment;

                var launcherTagsEntityDao = new LauncherTagsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                var tagItems = launcherTagsEntityDao.SelectTags(LauncherItemId);
                TagItems.SetRange(tagItems);

                switch(Kind) {
                    case LauncherItemKind.File: {
                            LoadFileCore(commander, commander.Implementation);
                        }
                        break;

                    case LauncherItemKind.StoreApp: {
                            var launcherStoreAppsEntityDao = new LauncherStoreAppsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                            StoreApp = launcherStoreAppsEntityDao.SelectStoreApp(LauncherItemId);
                        }
                        break;

                    case LauncherItemKind.Addon: {
                            LoadAddonCore(commander, commander.Implementation);
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        //public LauncherFileData LoadFileData()
        //{
        //    ThrowIfDisposed();

        //    using(var commander = MainDatabaseBarrier.WaitRead()) {
        //        var dao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
        //        return dao.SelectFile(LauncherItemId);
        //    }
        //}

        //public IReadOnlyCollection<LauncherEnvironmentVariableData> LoadEnvironmentVariableItems()
        //{
        //    ThrowIfDisposed();

        //    using(var commander = MainDatabaseBarrier.WaitRead()) {
        //        var dao = new LauncherEnvVarsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
        //        return dao.SelectEnvVarItems(LauncherItemId).ToList();
        //    }
        //}


        //public IReadOnlyCollection<string> LoadTags()
        //{
        //    ThrowIfDisposed();

        //    using(var commander = MainDatabaseBarrier.WaitRead()) {
        //        var dao = new LauncherTagsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
        //        return dao.SelectTags(LauncherItemId).ToList();
        //    }
        //}

        /// <summary>
        /// アイコンが変更されたか。
        /// </summary>
        /// <param name="iconData"></param>
        /// <param name="kindIconValue">種別により異なる生アイコン情報。</param>
        /// <param name="commander"></param>
        /// <param name="implementation"></param>
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
        public bool SaveItem(IDatabaseCommandsPack commandsPack)
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

            var launcherItemsEntityDao = new LauncherItemsEntityDao(commandsPack.Main.Commander, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);
            var launcherTagsEntityDao = new LauncherTagsEntityDao(commandsPack.Main.Commander, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);

            var launcherItemDomainDao = new LauncherItemDomainDao(commandsPack.Main.Commander, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);
            var currentFileIcon = launcherItemDomainDao.SelectFileIcon(LauncherItemId);

            launcherItemsEntityDao.UpdateCustomizeLauncherItem(itemData, commandsPack.CommonStatus);
            switch(Kind) {
                case LauncherItemKind.File: {
                        Debug.Assert(File != null);
                        Debug.Assert(EnvironmentVariableItems != null);
                        Debug.Assert(Redo != null);

                        iconChangedResult = CheckIconChanged(currentFileIcon, itemData.Icon, File.Path);

                        var launcherFilesEntityDao = new LauncherFilesEntityDao(commandsPack.Main.Commander, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);
                        var launcherMergeEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commandsPack.Main.Commander, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);
                        var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(commandsPack.Main.Commander, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);
                        var launcherRedoSuccessExitCodesEntityDao = new LauncherRedoSuccessExitCodesEntityDao(commandsPack.Main.Commander, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);

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

                        var launcherStoreAppsEntityDao = new LauncherStoreAppsEntityDao(commandsPack.Main.Commander, DatabaseStatementLoader, commandsPack.Main.Implementation, LoggerFactory);
                        launcherStoreAppsEntityDao.UpdateStoreApp(itemData.LauncherItemId, StoreApp, commandsPack.CommonStatus);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            launcherTagsEntityDao.DeleteTagByLauncherItemId(itemData.LauncherItemId);
            launcherTagsEntityDao.InsertTags(itemData.LauncherItemId, TagItems, commandsPack.CommonStatus);

            return iconChangedResult;
        }

        public void ClearIcon(IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            ThrowIfDisposed();

            var launcherItemIconsEntityDao = new LauncherItemIconsEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);
            var launcherItemIconStatusEntityDao = new LauncherItemIconStatusEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);

            launcherItemIconsEntityDao.DeleteAllSizeImageBinary(LauncherItemId);
            launcherItemIconStatusEntityDao.DeleteAllSizeLauncherItemIconState(LauncherItemId);
        }

        public void Save()
        {
            ThrowIfDisposed();

            bool needToIconClear;

            //            var pack = new ApplicationDatabaseCommandsPack(
            //    new DatabaseCommands(mainDatabaseCommander, mainDatabaseCommander.Implementation),
            //    new DatabaseCommands(fileDatabaseCommander, fileDatabaseCommander.Implementation),
            //    new DatabaseCommands(tempDatabaseCommander, tempDatabaseCommander.Implementation),
            //    DatabaseCommonStatus.CreateCurrentAccount()
            //);
            using(var pack = PersistentHelper.WaitWritePack(MainDatabaseBarrier, FileDatabaseBarrier, TemporaryDatabaseBarrier, DatabaseCommonStatus.CreateCurrentAccount())) {
                needToIconClear = SaveItem(pack);
                pack.Commit();
            }
            if(needToIconClear) {
                using(var commander = FileDatabaseBarrier.WaitWrite()) {
                    ClearIcon(commander, commander.Implementation);
                    commander.Commit();
                }
            }
        }


        [Obsolete]
        public void SaveFile(LauncherItemData launcherItemData, LauncherFileData launcherFileData, IEnumerable<LauncherEnvironmentVariableData> environmentVariableItems, IEnumerable<string> tags)
        {
            ThrowIfDisposed();

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                var launcherMergeEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                var launcherTagsEntityDao = new LauncherTagsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);

                launcherItemsEntityDao.UpdateCustomizeLauncherItem(launcherItemData, DatabaseCommonStatus.CreateCurrentAccount());
                launcherFilesEntityDao.UpdateCustomizeLauncherFile(launcherItemData.LauncherItemId, launcherFileData, launcherFileData, DatabaseCommonStatus.CreateCurrentAccount());

                launcherMergeEnvVarsEntityDao.DeleteEnvVarItemsByLauncherItemId(launcherItemData.LauncherItemId);
                launcherMergeEnvVarsEntityDao.InsertEnvVarItems(launcherItemData.LauncherItemId, environmentVariableItems, DatabaseCommonStatus.CreateCurrentAccount());

                launcherTagsEntityDao.DeleteTagByLauncherItemId(launcherItemData.LauncherItemId);
                launcherTagsEntityDao.InsertTags(launcherItemData.LauncherItemId, tags, DatabaseCommonStatus.CreateCurrentAccount());

                commander.Commit();
            }
            using(var commander = FileDatabaseBarrier.WaitWrite()) {
                var launcherItemIconsEntityDao = new LauncherItemIconsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                launcherItemIconsEntityDao.DeleteAllSizeImageBinary(launcherItemData.LauncherItemId);

                commander.Commit();
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
