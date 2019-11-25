using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup
{
    public class LauncherGroupElement : ElementBase, ILauncherGroupId
    {
        public LauncherGroupElement(Guid launcherGroupId, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherGroupId = launcherGroupId;

            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            IdFactory = idFactory;
        }

        #region property
        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IIdFactory IdFactory { get; }

        public string Name { get; private set; } = string.Empty;
        public LauncherGroupKind Kind { get; private set; }
        public LauncherGroupImageName ImageName { get; private set; }
        public Color ImageColor { get; private set; }
        public long Sequence { get; private set; }

        public List<Guid> LauncherItemIds { get; } = new List<Guid>();

        #endregion

        #region function

        public IReadOnlyList<Guid> GetLauncherItemIds() => LauncherItemIds.ToList();

        void LoadGroup()
        {
            ThrowIfDisposed();

            LauncherGroupData data;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherGroupsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                data = dao.SelectLauncherGroup(LauncherGroupId);
            }

            Name = data.Name;
            Kind = data.Kind;
            ImageName = data.ImageName;
            ImageColor = data.ImageColor;
            Sequence = data.Sequence;
        }

        IEnumerable<Guid> GetLauncherItemsForNormal()
        {
            ThrowIfDisposed();

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherGroupItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectLauncherItemIds(LauncherGroupId);
            }
        }

        IEnumerable<Guid> GetLauncherItems()
        {
            ThrowIfDisposed();

            switch(Kind) {
                case LauncherGroupKind.Normal:
                    return GetLauncherItemsForNormal();

                default:
                    throw new NotImplementedException();
            }
        }

        void LoadLauncherItems()
        {
            ThrowIfDisposed();

            var items = GetLauncherItems();
            LauncherItemIds.Clear();
            LauncherItemIds.AddRange(items);
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            NotifyManager.LauncherItemRegistered += NotifyManager_LauncherItemRegistered;

            LoadGroup();
            LoadLauncherItems();
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                NotifyManager.LauncherItemRegistered -= NotifyManager_LauncherItemRegistered;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherGroupId

        public Guid LauncherGroupId { get; }

        #endregion

        private void NotifyManager_LauncherItemRegistered(object? sender, LauncherItemRegisteredEventArgs e)
        {
            if(e.GroupId == LauncherGroupId) {
                // 自グループにアイテムが登録されたので放り込んでおく
                LauncherItemIds.Add(e.LauncherItemId);
            }
        }

    }
}
