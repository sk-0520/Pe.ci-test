using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Command
{
    public class CommandElement : ElementBase, IViewShowStarter
    {
        public CommandElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IOrderManager orderManager, IWindowManager windowManager, INotifyManager notifyManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
            OrderManager = orderManager;
            WindowManager = windowManager;
            NotifyManager = notifyManager;
        }

        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IOrderManager OrderManager { get; }
        IWindowManager WindowManager { get; }
        INotifyManager NotifyManager { get; }
        bool ViewCreated { get; set; }

        public ObservableCollection<LauncherItemElement> LauncherItemElements { get; } = new ObservableCollection<LauncherItemElement>();

        #endregion

        #region function

        public void Hide()
        {
            Debug.Assert(ViewCreated);

            WindowManager.GetWindowItems(WindowKind.Command).First().Window.Hide();
        }

        private void RefreshLauncherItems()
        {
            IReadOnlyList<Guid> ids;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                ids = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();
            }

            var launcherItemElements = ids
                .Select(i => OrderManager.GetOrCreateLauncherItemElement(i))
                .ToList();
            ;
            LauncherItemElements.SetRange(launcherItemElements);
        }

        public void Refresh()
        {
            // アイテム一覧とったりなんかしたりあれこれしたり
            RefreshLauncherItems();
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            Refresh();
        }

        #endregion

        #region IViewShowStarter

        public bool CanStartShowView
        {
            get
            {
                if(ViewCreated) {
                    return false;
                }

                return true;
            }
        }

        public void StartView()
        {
            if(!ViewCreated) {
                var windowItem = OrderManager.CreateCommandWindow(this);
                windowItem.Window.Show();
                ViewCreated = true;
            } else {
                var windoItem = WindowManager.GetWindowItems(WindowKind.Command).First();
                if(windoItem.Window.IsVisible) {
                    windoItem.Window.Activate();
                } else {
                    windoItem.Window.Show();
                }
            }
        }

        #endregion

        #region IViewCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            return false;
        }
        public bool ReceiveViewClosing()
        {
            return true;
        }

        public void ReceiveViewClosed()
        {
            ViewCreated = false;
        }


        #endregion

    }
}
