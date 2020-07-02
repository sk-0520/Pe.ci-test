using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize
{
    public class LauncherItemCustomizeEditorElement: ElementBase, ILauncherItemId
    {
        public LauncherItemCustomizeEditorElement(Guid launcherItemId, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;
            ClipboardManager = clipboardManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }
        IClipboardManager ClipboardManager { get; }

        public string Name { get; set; } = string.Empty;
        public string Code { get; protected set; } = string.Empty;
        public LauncherItemKind Kind { get; private set; }
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

        #endregion


        #region function

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
                            var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                            File = launcherFilesEntityDao.SelectFile(LauncherItemId);

                            var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                            var environmentVariableItems = launcherEnvVarsEntityDao.SelectEnvVarItems(LauncherItemId).ToList();
                            EnvironmentVariableItems = new ObservableCollection<LauncherEnvironmentVariableData>(environmentVariableItems);

                            var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                            if(launcherRedoItemsEntityDao.SelectExistsLauncherRedoItem(LauncherItemId)) {
                                var redoData = launcherRedoItemsEntityDao.SelectLauncherRedoItem(LauncherItemId);
                                var launcherRedoSuccessExitCodesEntityDao = new LauncherRedoSuccessExitCodesEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                                var exitCodes = launcherRedoSuccessExitCodesEntityDao.SelectRedoSuccessExitCodes(LauncherItemId);
                                redoData.SuccessExitCodes.SetRange(exitCodes);
                                Redo = redoData;
                            } else {
                                Redo = LauncherRedoData.GetDisable();
                            }
                        }
                        break;

                    case LauncherItemKind.StoreApp: {
                            var launcherStoreAppsEntityDao = new LauncherStoreAppsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                            StoreApp = launcherStoreAppsEntityDao.SelectStoreApp(LauncherItemId);
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
        /// <param name="commander"></param>
        /// <param name="implementation"></param>
        /// <param name="databaseCommonStatus"></param>
        /// <returns>アイコンの削除が必要か。</returns>
        public bool SaveItem(IDatabaseCommander commander, IDatabaseImplementation implementation, IDatabaseCommonStatus databaseCommonStatus)
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

            var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);
            var launcherTagsEntityDao = new LauncherTagsEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);

            var launcherItemDomainDao = new LauncherItemDomainDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);
            var currentFileIcon = launcherItemDomainDao.SelectFileIcon(LauncherItemId);

            launcherItemsEntityDao.UpdateCustomizeLauncherItem(itemData, databaseCommonStatus);
            switch(Kind) {
                case LauncherItemKind.File: {
                        Debug.Assert(File != null);
                        Debug.Assert(EnvironmentVariableItems != null);
                        Debug.Assert(Redo != null);

                        iconChangedResult = CheckIconChanged(currentFileIcon, itemData.Icon, File.Path);

                        var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);
                        var launcherMergeEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);
                        var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);
                        var launcherRedoSuccessExitCodesEntityDao = new LauncherRedoSuccessExitCodesEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);

                        launcherFilesEntityDao.UpdateCustomizeLauncherFile(itemData.LauncherItemId, File, File, databaseCommonStatus);
                        launcherRedoItemsEntityDao.UpdateRedoItem(itemData.LauncherItemId, Redo, databaseCommonStatus);

                        launcherRedoSuccessExitCodesEntityDao.DeleteSuccessExitCodes(itemData.LauncherItemId);
                        launcherRedoSuccessExitCodesEntityDao.InsertSuccessExitCodes(itemData.LauncherItemId, Redo.SuccessExitCodes, databaseCommonStatus);

                        launcherMergeEnvVarsEntityDao.DeleteEnvVarItemsByLauncherItemId(itemData.LauncherItemId);
                        launcherMergeEnvVarsEntityDao.InsertEnvVarItems(itemData.LauncherItemId, EnvironmentVariableItems, databaseCommonStatus);
                    }
                    break;

                case LauncherItemKind.StoreApp: {
                        Debug.Assert(StoreApp != null);

                        var launcherStoreAppsEntityDao = new LauncherStoreAppsEntityDao(commander, DatabaseStatementLoader, implementation, LoggerFactory);
                        launcherStoreAppsEntityDao.UpdateStoreApp(itemData.LauncherItemId, StoreApp, databaseCommonStatus);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            launcherTagsEntityDao.DeleteTagByLauncherItemId(itemData.LauncherItemId);
            launcherTagsEntityDao.InsertTags(itemData.LauncherItemId, TagItems, databaseCommonStatus);

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
            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                needToIconClear = SaveItem(commander, commander.Implementation, DatabaseCommonStatus.CreateCurrentAccount());
                commander.Commit();
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

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
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
