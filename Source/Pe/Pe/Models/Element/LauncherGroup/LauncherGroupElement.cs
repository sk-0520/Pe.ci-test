using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Applications;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup
{
    public class LauncherGroupElement : ElementBase
    {
        public LauncherGroupElement(Guid launcherGroupId, IOrderManager orderManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherGroupId = launcherGroupId;

            OrderManager = orderManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            IdFactory = idFactory;
        }

        #region property
        IOrderManager OrderManager { get; }
        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IIdFactory IdFactory { get; }

        public Guid LauncherGroupId { get; }

        public string Name { get; private set; }
        public LauncherGroupKind Kind { get; private set; }
        public LauncherGroupImageName ImageName { get; private set; }
        public Color ImageColor { get; private set; }
        public long Sort { get; private set; }

        List<Guid> LauncherItemIds { get; } = new List<Guid>();

        #endregion

        #region function

        public IReadOnlyList<Guid> GetLauncherItemIds() => LauncherItemIds.ToList();

        void LoadGroup()
        {
            LauncherGroupData data;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherGroupsEntityDao(commander, StatementLoader, commander.Implementation, this);
                data = dao.SelectLauncherGroup(LauncherGroupId);
            }

            Name = data.Name;
            Kind = data.Kind;
            ImageName = data.ImageName;
            ImageColor = data.ImageColor;
            Sort = data.Sort;
        }

        IEnumerable<Guid> GetLauncherItemsForNormal()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherGroupItemsEntityDao(commander, StatementLoader, commander.Implementation, this);
                return dao.SelectLauncherItemIds(LauncherGroupId);
            }
        }

        IEnumerable<Guid> GetLauncherItems()
        {
            switch(Kind) {
                case LauncherGroupKind.Normal:
                    return GetLauncherItemsForNormal();

                default:
                    throw new NotImplementedException();
            }
        }

        void LoadLauncherItems()
        {
            var items = GetLauncherItems();
            LauncherItemIds.Clear();
            LauncherItemIds.AddRange(items);
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            LoadGroup();
            LoadLauncherItems();
        }

        #endregion
    }
}
