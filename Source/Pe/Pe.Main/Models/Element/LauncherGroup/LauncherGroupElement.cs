using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base.Linq;
using System.Threading;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup
{
    public class LauncherGroupElement: ElementBase, ILauncherGroupId
    {
        public LauncherGroupElement(LauncherGroupId launcherGroupId, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherGroupId = launcherGroupId;

            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            IdFactory = idFactory;
        }

        #region property
        private INotifyManager NotifyManager { get; }
        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private IIdFactory IdFactory { get; }

        public string Name { get; private set; } = string.Empty;
        public LauncherGroupKind Kind { get; private set; }
        public LauncherGroupImageName ImageName { get; private set; }
        public Color ImageColor { get; private set; }
        public long Sequence { get; private set; }

        public List<LauncherItemId> LauncherItemIds { get; } = new List<LauncherItemId>();

        #endregion

        #region function

        public IReadOnlyList<LauncherItemId> GetLauncherItemIds() => LauncherItemIds.ToList();

        private void LoadGroup()
        {
            ThrowIfDisposed();

            LauncherGroupData data;
            IEnumerable<LauncherItemId> launcherItemIds;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherGroupsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                data = dao.SelectLauncherGroup(LauncherGroupId);

                var launcherItemsLoader = new LauncherItemsLoader(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                launcherItemIds = launcherItemsLoader.LoadLauncherItemIds(LauncherGroupId, data.Kind);
            }

            Name = data.Name;
            Kind = data.Kind;
            ImageName = data.ImageName;
            ImageColor = data.ImageColor;
            Sequence = data.Sequence;

            LauncherItemIds.SetRange(launcherItemIds);
        }

        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            NotifyManager.LauncherItemRegistered += NotifyManager_LauncherItemRegistered;
            NotifyManager.LauncherItemRemovedInLauncherGroup += NotifyManager_LauncherItemRemovedInLauncherGroup;

            LoadGroup();

            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                NotifyManager.LauncherItemRegistered -= NotifyManager_LauncherItemRegistered;
                NotifyManager.LauncherItemRemovedInLauncherGroup -= NotifyManager_LauncherItemRemovedInLauncherGroup;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherGroupId

        public LauncherGroupId LauncherGroupId { get; }

        #endregion

        private void NotifyManager_LauncherItemRegistered(object? sender, LauncherItemRegisteredEventArgs e)
        {
            if(e.LauncherGroupId == LauncherGroupId) {
                // 自グループにアイテムが登録されたので放り込んでおく
                LauncherItemIds.Add(e.LauncherItemId);
            }
        }

        private void NotifyManager_LauncherItemRemovedInLauncherGroup(object? sender, LauncherItemRemoveInLauncherGroupEventArgs e)
        {
            if(e.LauncherGroupId == LauncherGroupId) {
                var removedItemIndex = LauncherItemIds
                    .Counting()
                    .Where(i => i.Value == e.LauncherItemId)
                    .Counting()
                    .First(i => i.Number == e.Index)
                    .Value.Number
                ;
                LauncherItemIds.RemoveAt(removedItemIndex);
            }
        }
    }
}
