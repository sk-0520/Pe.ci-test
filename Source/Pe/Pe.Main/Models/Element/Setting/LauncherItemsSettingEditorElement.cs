using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherItemsSettingEditorElement : SettingEditorElementBase
    {
        internal LauncherItemsSettingEditorElement(ObservableCollection<LauncherItemSettingEditorElement> allLauncherItems, PluginContainer pluginContainer, ISettingNotifyManager settingNotifyManager,IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IIdFactory idFactory, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, databaseStatementLoader, idFactory, imageLoader, dispatcherWrapper, loggerFactory)
        {
            AllLauncherItems = allLauncherItems;
            PluginContainer = pluginContainer;
        }

        #region property

        public ObservableCollection<LauncherItemSettingEditorElement> AllLauncherItems { get; }
        PluginContainer PluginContainer { get; }
        #endregion

        #region function

        public void RemoveItem(Guid launcherItemId)
        {
            ThrowIfDisposed();

            var item = AllLauncherItems.First(i => i.LauncherItemId == launcherItemId);
            AllLauncherItems.Remove(item);
            item.Dispose();

            // DBから物理削除
            using(var commander = FileDatabaseBarrier.WaitWrite()) {
                var launcherItemIconsEntityDao = new LauncherItemIconsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                launcherItemIconsEntityDao.DeleteAllSizeImageBinary(launcherItemId);

                commander.Commit();
            }
            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                launcherEnvVarsEntityDao.DeleteEnvVarItemsByLauncherItemId(launcherItemId);

                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                launcherFilesEntityDao.DeleteFileByLauncherItemId(launcherItemId);

                var launcherGroupItemsEntityDao = new LauncherGroupItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                launcherGroupItemsEntityDao.DeleteGroupItemsByLauncherItemId(launcherItemId);

                var launcherItemHistoriesEntityDao = new LauncherItemHistoriesEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                launcherItemHistoriesEntityDao.DeleteHistoriesByLauncherItemId(launcherItemId);

                var launcherTagsEntityDao = new LauncherTagsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                launcherTagsEntityDao.DeleteTagByLauncherItemId(launcherItemId);

                var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                launcherRedoItemsEntityDao.DeleteRedoItemByLauncherItemId(launcherItemId);

                var launcherRedoSuccessExitCodesEntityDao = new LauncherRedoSuccessExitCodesEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                launcherRedoSuccessExitCodesEntityDao.DeleteSuccessExitCodes(launcherItemId);

                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                launcherItemsEntityDao.DeleteLauncherItem(launcherItemId);

                commander.Commit();
            }

            SettingNotifyManager.SendLauncherItemRemove(launcherItemId);
        }

        public Guid AddNewItem(LauncherItemKind kind, Guid launcherItemId)
        {
            ThrowIfDisposed();

            var newLauncherItemId = IdFactory.CreateLauncherItemId();
            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var launcherItemsDao = new LauncherItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);

                //TODO: 名前の自動設定
                var item = new LauncherItemData() {
                    LauncherItemId = newLauncherItemId,
                    Kind = kind,
                    Name = TextUtility.ToUnique(Properties.Resources.String_LauncherItem_NewItem_Name, AllLauncherItems.Select(i => i.Name).ToList(), StringComparison.OrdinalIgnoreCase, (s, n) => $"{s}({n})"),
                };

                var newCode = kind.ToString().ToLower() + "-item-code";
                var codes = launcherItemsDao.SelectFuzzyCodes(newCode).ToList();
                item.Code = launcherFactory.GetUniqueCode(newCode, codes);
                item.IsEnabledCommandLauncher = true;

                switch(kind) {
                    case LauncherItemKind.File: {
                            var launcherFilesDao = new LauncherFilesEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                            var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);

                            var file = new LauncherFileData();
                            launcherItemsDao.InsertLauncherItem(item, DatabaseCommonStatus.CreateCurrentAccount());
                            launcherFilesDao.InsertFile(item.LauncherItemId, file, DatabaseCommonStatus.CreateCurrentAccount());
                            launcherRedoItemsEntityDao.InsertRedoItem(item.LauncherItemId, LauncherRedoData.GetDisable(), DatabaseCommonStatus.CreateCurrentAccount());
                        }
                        break;

                    case LauncherItemKind.StoreApp: {
                            var launcherStoreAppsEntityDao = new LauncherStoreAppsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);

                            var store = new LauncherStoreAppData();
                            launcherItemsDao.InsertLauncherItem(item, DatabaseCommonStatus.CreateCurrentAccount());
                            launcherStoreAppsEntityDao.InsertStoreApp(item.LauncherItemId, store, DatabaseCommonStatus.CreateCurrentAccount());
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }

                commander.Commit();
            }

            var customizeEditor = new LauncherItemSettingEditorElement(newLauncherItemId, ClipboardManager, MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            customizeEditor.Initialize();

            AllLauncherItems.Add(customizeEditor);

            return newLauncherItemId;
        }

        /// <summary>
        /// ファイルを登録する。
        /// <para>TODO: <see cref="LauncherToolbar.LauncherToolbarElement.RegisterFile"/>と重複</para>
        /// </summary>
        /// <param name="filePath">対象ファイルパス。</param>
        /// <param name="expandShortcut"><paramref name="filePath"/>がショートカットの場合にショートカットの内容を登録するか</param>
        public Guid RegisterFile(string filePath, bool expandShortcut)
        {
            ThrowIfDisposed();

            var file = new FileInfo(filePath);
            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);
            var data = launcherFactory.FromFile(file, expandShortcut);
            var tags = launcherFactory.GetTags(file).ToList();

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var launcherItemsDao = new LauncherItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                var launcherTagsDao = new LauncherTagsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                var launcherFilesDao = new LauncherFilesEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                var launcherGroupItemsDao = new LauncherGroupItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);

                var codes = launcherItemsDao.SelectFuzzyCodes(data.Item.Code).ToList();
                data.Item.Code = launcherFactory.GetUniqueCode(data.Item.Code, codes);

                launcherItemsDao.InsertLauncherItem(data.Item, DatabaseCommonStatus.CreateCurrentAccount());
                launcherFilesDao.InsertFile(data.Item.LauncherItemId, data.File, DatabaseCommonStatus.CreateCurrentAccount());
                launcherTagsDao.InsertTags(data.Item.LauncherItemId, tags, DatabaseCommonStatus.CreateCurrentAccount());
                launcherRedoItemsEntityDao.InsertRedoItem(data.Item.LauncherItemId, LauncherRedoData.GetDisable(), DatabaseCommonStatus.CreateCurrentAccount());

                commander.Commit();
            }

            var customizeEditor = new LauncherItemSettingEditorElement(data.Item.LauncherItemId, ClipboardManager, MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            customizeEditor.Initialize();

            AllLauncherItems.Add(customizeEditor);

            return data.Item.LauncherItemId;
        }

        #endregion

        #region SettingEditorElementBase

        protected override void LoadImpl()
        {
            ThrowIfDisposed();

            /* なにしたかったんだこれ
            IReadOnlyList<Guid> launcherItemIds;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                launcherItemIds = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();
            }
            */

            //Items.Clear();
            //foreach(var launcherItemId in launcherItemIds) {
            //    var customizeEditor = new LauncherItemCustomizeEditorElement(launcherItemId, ClipboardManager, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, LoggerFactory);
            //    customizeEditor.Initialize();

            //    var iconPack = LauncherIconLoaderPackFactory.CreatePack(launcherItemId, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, DispatcherWrapper, LoggerFactory);
            //    var launcherIconElement = new LauncherIconElement(launcherItemId, iconPack, LoggerFactory);
            //    launcherIconElement.Initialize();

            //    var item = LauncherItemWithIconElement.Create(customizeEditor, launcherIconElement, LoggerFactory);
            //    Items.Add(item);
            //}
        }

        protected override void SaveImpl(IDatabaseCommandsPack commandPack)
        {
            foreach(var item in AllLauncherItems.Where(i => !i.IsLazyLoad)) {
                var needIconClear = item.SaveItem(commandPack.Main.Commander, commandPack.Main.Implementation, commandPack.CommonStatus);
                if(needIconClear) {
                    item.ClearIcon(commandPack.File.Commander, commandPack.File.Implementation);
                }
            }
        }

        #endregion
    }
}
