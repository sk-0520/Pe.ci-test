using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup
{
    public class LauncherGroupElement: ElementBase, ILauncherGroupId
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
            IEnumerable<Guid> launcherItemIds;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherGroupsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                data = dao.SelectLauncherGroup(LauncherGroupId);

                var launcherItemsLoader = new LauncherItemsLoader(commander, StatementLoader, commander.Implementation, LoggerFactory);
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

        protected override void InitializeImpl()
        {
            NotifyManager.LauncherItemRegistered += NotifyManager_LauncherItemRegistered;
            NotifyManager.LauncherItemRemovedInLauncherGroup += NotifyManager_LauncherItemRemovedInLauncherGroup;

            LoadGroup();
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

        public Guid LauncherGroupId { get; }

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
