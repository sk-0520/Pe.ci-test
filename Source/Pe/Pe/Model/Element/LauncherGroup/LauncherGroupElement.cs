using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.Model.Manager;

namespace ContentTypeTextNet.Pe.Main.Model.Element.LauncherGroup
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
                var dao = new LauncherGroupsDao(commander, StatementLoader, this);
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
                var dao = new LauncherGroupItemsDao(commander, StatementLoader, this);
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
