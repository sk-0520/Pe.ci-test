using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherGroupsSettingEditorElement : SettingEditorElementBase
    {
        public LauncherGroupsSettingEditorElement(INotifyManager notifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, idFactory, dispatcherWrapper, loggerFactory)
        {
            NotifyManager = notifyManager;
        }

        #region property

        INotifyManager NotifyManager { get; }
        public ObservableCollection<LauncherGroupElement> GroupItems { get; } = new ObservableCollection<LauncherGroupElement>();
        public ObservableCollection<LauncherElementWithIconElement<CommonLauncherItemElement>> LauncherItems { get; } = new ObservableCollection<LauncherElementWithIconElement<CommonLauncherItemElement>>();

        #endregion

        #region function
        #endregion

        #region SettingEditorElementBase

        public override void Load()
        {
            ThrowIfDisposed();

            IReadOnlyList<Guid> launcherItemIds;
            IReadOnlyList<Guid> groupIds;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                launcherItemIds = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();

                var launcherGroupsEntityDao = new LauncherGroupsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                groupIds = launcherGroupsEntityDao.SelectAllLauncherGroupIds().ToList();
            }

            LauncherItems.Clear();
            foreach(var launcherItemId in launcherItemIds) {
                var launcherItem = new CommonLauncherItemElement(launcherItemId, MainDatabaseBarrier, StatementLoader, LoggerFactory);
                launcherItem.Initialize();

                var iconPack = LauncherIconLoaderPackFactory.CreatePack(launcherItemId, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, DispatcherWrapper, LoggerFactory);
                var launcherIconElement = new LauncherIconElement(launcherItemId, iconPack, LoggerFactory);
                launcherIconElement.Initialize();

                var item = LauncherItemWithIconElement.Create(launcherItem, launcherIconElement, LoggerFactory);
                LauncherItems.Add(item);
            }

            GroupItems.Clear();
            foreach(var groupId in groupIds) {
                var element = new LauncherGroupElement(groupId, NotifyManager, MainDatabaseBarrier, StatementLoader, IdFactory, LoggerFactory);
                element.Initialize();
                GroupItems.Add(element);
            }
        }

        public override void Save()
        {
        }

        #endregion
    }

}
