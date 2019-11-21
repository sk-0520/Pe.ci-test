using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class SettingElement : ElementBase, IViewCloseReceiver
    {
        public SettingElement(IOrderManager orderManager, IClipboardManager clipboardManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IDispatcherWapper dispatcherWapper, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            OrderManager = orderManager;
            ClipboardManager = clipboardManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
            DispatcherWapper = dispatcherWapper;
            IdFactory  = idFactory;

            LauncherItemSetting = new LauncherItemSettingElement(LauncherItemIds, LauncherIconElements, OrderManager, ClipboardManager, NotifyManager, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, IdFactory, LoggerFactory);
        }

        #region property

        IOrderManager OrderManager { get; }
        IClipboardManager ClipboardManager { get; }
        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IDispatcherWapper DispatcherWapper { get; }
        IIdFactory IdFactory { get; }
        public bool IsSubmit { get; private set; }

        /// <summary>
        /// 設定開始時に存在するランチャーアイテムID
        /// </summary>
        ISet<Guid> InitialLauncherItemIds { get; } = new HashSet<Guid>();
        public ObservableCollection<Guid> LauncherItemIds { get; } = new ObservableCollection<Guid>();
        public ObservableCollection<LauncherIconElement> LauncherIconElements { get; } = new ObservableCollection<LauncherIconElement>();

        public LauncherItemSettingElement LauncherItemSetting { get; }

        #endregion

        #region function

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            Logger.LogTrace("todo");
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                LauncherItemIds.SetRange(launcherItemsEntityDao.SelectAllLauncherItemIds());
            }
            InitialLauncherItemIds.SetRange(LauncherItemIds);

            // アイコン自体は設定中でも書き込み・不明アイコン追加OK
            foreach(var launcherItemId in LauncherItemIds) {
                var launcherIconImageLoaders = EnumUtility.GetMembers<IconBox>()
                    .Select(i => new LauncherIconLoader(launcherItemId, i, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, DispatcherWapper, LoggerFactory))
                ;
                var iconImageLoaderPack = new IconImageLoaderPack(launcherIconImageLoaders);
                var iconElement = new LauncherIconElement(launcherItemId, iconImageLoaderPack, LoggerFactory);
                LauncherIconElements.Add(iconElement);
            }
            LauncherItemSetting.Initialize();
        }

        #endregion

        #region IViewCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            return true;
        }

        public bool ReceiveViewClosing()
        {
            return true;
        }

        public void ReceiveViewClosed()
        { }


        #endregion

    }
}
