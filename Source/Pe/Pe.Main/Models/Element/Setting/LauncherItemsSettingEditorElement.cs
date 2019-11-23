using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
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

        public ObservableCollection<LauncherItemWithIconElement<LauncherItemCustomizeEditorElement>> Items { get; } = new ObservableCollection<LauncherItemWithIconElement<LauncherItemCustomizeEditorElement>>();

        #endregion

        #region function

        public void CreateNewItem(LauncherItemKind kind)
        {
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
