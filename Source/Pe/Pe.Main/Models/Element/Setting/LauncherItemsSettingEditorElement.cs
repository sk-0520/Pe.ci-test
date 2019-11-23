using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherItemsSettingEditorElement : SettingEditorElementBase
    {
        public LauncherItemsSettingEditorElement(IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, idFactory, dispatcherWrapper, loggerFactory)
        {
        }

        #region property

        public ObservableCollection<LauncherElementWithIconElement<LauncherItemCustomizeEditorElement>> Items { get; } = new ObservableCollection<LauncherElementWithIconElement<LauncherItemCustomizeEditorElement>>();

        #endregion

        #region function

        public void RemoveItem(Guid launcherItemId)
        {

        }

        public Guid CreateNewItem(LauncherItemKind kind)
        {
            var newLauncherItemId = IdFactory.CreateLauncherItemId();

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var launcherItemsDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherFilesDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);

                //TODO: 名前の自動設定
                var item = new LauncherItemData() {
                    LauncherItemId = newLauncherItemId,
                    Kind = kind,
                };

                switch(kind) {
                    case LauncherItemKind.File: {
                            var file = new LauncherFileData();
                            launcherItemsDao.InsertLauncherItem(item, DatabaseCommonStatus.CreateCurrentAccount());
                            launcherFilesDao.InsertFile(item.LauncherItemId, file, DatabaseCommonStatus.CreateCurrentAccount());
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }

                commander.Commit();
            }

            var customizeEditor = new LauncherItemCustomizeEditorElement(newLauncherItemId, ClipboardManager, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, LoggerFactory);
            customizeEditor.Initialize();

            var iconPack = LauncherIconLoaderPackFactory.CreatePack(newLauncherItemId, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, DispatcherWrapper, LoggerFactory);
            var launcherIconElement = new LauncherIconElement(newLauncherItemId, iconPack, LoggerFactory);
            launcherIconElement.Initialize();

            var newItem = LauncherItemWithIconElement.Create(customizeEditor, launcherIconElement, LoggerFactory);
            Items.Add(newItem);

            return newLauncherItemId;
        }

        /// <summary>
        /// ファイルを現在のグループに登録する。
        /// </summary>
        /// <param name="filePath">対象ファイルパス。</param>
        /// <param name="expandShortcut"><paramref name="filePath"/>がショートカットの場合にショートカットの内容を登録するか</param>
        public Guid RegisterFile(string filePath, bool expandShortcut)
        {
            //TODO: LauncherToolbarElement.RegisterFile と重複
            var file = new FileInfo(filePath);
            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);
            var data = launcherFactory.FromFile(file, expandShortcut);
            var tags = launcherFactory.GetTags(file).ToList();

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var launcherItemsDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherTagsDao = new LauncherTagsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherFilesDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherGroupItemsDao = new LauncherGroupItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);

                var codes = launcherItemsDao.SelectFuzzyCodes(data.Item.Code).ToList();
                data.Item.Code = launcherFactory.GetUniqueCode(data.Item.Code, codes);

                launcherItemsDao.InsertLauncherItem(data.Item, DatabaseCommonStatus.CreateCurrentAccount());
                launcherFilesDao.InsertFile(data.Item.LauncherItemId, data.File, DatabaseCommonStatus.CreateCurrentAccount());
                launcherTagsDao.InsertTags(data.Item.LauncherItemId, tags, DatabaseCommonStatus.CreateCurrentAccount());

                commander.Commit();
            }

            var customizeEditor = new LauncherItemCustomizeEditorElement(data.Item.LauncherItemId, ClipboardManager, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, LoggerFactory);
            customizeEditor.Initialize();

            var iconPack = LauncherIconLoaderPackFactory.CreatePack(data.Item.LauncherItemId, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, DispatcherWrapper, LoggerFactory);
            var launcherIconElement = new LauncherIconElement(data.Item.LauncherItemId, iconPack, LoggerFactory);
            launcherIconElement.Initialize();

            var newItem = LauncherItemWithIconElement.Create(customizeEditor, launcherIconElement, LoggerFactory);
            Items.Add(newItem);

            return data.Item.LauncherItemId;
        }

        #endregion

        #region SettingEditorElementBase

        public override void Load()
        {
            IReadOnlyList<Guid> launcherItemIds;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                launcherItemIds = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();
            }

            Items.Clear();
            foreach(var launcherItemId in launcherItemIds) {
                var customizeEditor = new LauncherItemCustomizeEditorElement(launcherItemId, ClipboardManager, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, LoggerFactory);
                customizeEditor.Initialize();

                var iconPack = LauncherIconLoaderPackFactory.CreatePack(launcherItemId, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, DispatcherWrapper, LoggerFactory);
                var launcherIconElement = new LauncherIconElement(launcherItemId, iconPack, LoggerFactory);
                launcherIconElement.Initialize();

                var item = LauncherItemWithIconElement.Create(customizeEditor, launcherIconElement, LoggerFactory);
                Items.Add(item);
            }
        }

        public override void Save()
        {
        }

        #endregion
    }
}
