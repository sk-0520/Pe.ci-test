using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base.Linq;
using System.Threading;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherGroupSettingEditorElement: ElementBase, ILauncherGroupId
    {
        public LauncherGroupSettingEditorElement(LauncherGroupId launcherGroupId, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherGroupId = launcherGroupId;

            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = statementLoader;
            IdFactory = idFactory;
        }

        #region property

        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private IIdFactory IdFactory { get; }

        public string Name { get; set; } = string.Empty;
        public LauncherGroupKind Kind { get; set; }
        public LauncherGroupImageName ImageName { get; set; }
        public Color ImageColor { get; set; }
        public long Sequence { get; set; }

        public ObservableCollection<LauncherItemId> LauncherItems { get; } = new ObservableCollection<LauncherItemId>();

        #endregion

        #region function

        public void InsertLauncherItemId(int index, LauncherItemId launcherItemId)
        {
            LauncherItems.Insert(index, launcherItemId);
        }

        public void MoveLauncherItemId(int startIndex, int insertIndex)
        {
            var item = LauncherItems[startIndex];
            LauncherItems.RemoveAt(startIndex);
            LauncherItems.Insert(insertIndex, item);
        }

        public void RemoveLauncherItemAt(int index)
        {
            LauncherItems.RemoveAt(index);
        }

        public void Save(IDatabaseContextsPack pack)
        {
            ThrowIfDisposed();

            var launcherGroupData = new LauncherGroupData() {
                LauncherGroupId = LauncherGroupId,
                Kind = Kind,
                Name = Name,
                ImageName = ImageName,
                ImageColor = ImageColor,
                Sequence = Sequence
            };
            // 存在しないランチャーアイテムは保存対象外とする
            var launcherItemsEntityDao = new LauncherItemsEntityDao(pack.Main.Context, DatabaseStatementLoader, pack.Main.Implementation, LoggerFactory);
            var launcherItemIds = LauncherItems
                // こんなとこでSQL発行するとか業務じゃむり
                .Where(i => launcherItemsEntityDao.SelectExistsLauncherItem(i))
                .ToList()
            ;

            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);

            var launcherGroupsEntityDao = new LauncherGroupsEntityDao(pack.Main.Context, DatabaseStatementLoader, pack.Main.Implementation, LoggerFactory);
            launcherGroupsEntityDao.UpdateGroup(launcherGroupData, DatabaseCommonStatus.CreateCurrentAccount());

            var launcherGroupItemsDao = new LauncherGroupItemsEntityDao(pack.Main.Context, DatabaseStatementLoader, pack.Main.Implementation, LoggerFactory);
            launcherGroupItemsDao.DeleteGroupItemsByLauncherGroupId(LauncherGroupId);

            var currentMaxSequence = launcherGroupItemsDao.SelectMaxSequence(LauncherGroupId);
            launcherGroupItemsDao.InsertNewItems(LauncherGroupId, launcherItemIds, currentMaxSequence + launcherFactory.GroupItemsStep, launcherFactory.GroupItemsStep, DatabaseCommonStatus.CreateCurrentAccount());
        }

        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            LauncherGroupData data;
            IEnumerable<LauncherItemId> launcherItemIds;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var launcherGroupsEntityDao = new LauncherGroupsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                data = launcherGroupsEntityDao.SelectLauncherGroup(LauncherGroupId);

                var launcherItemsLoader = new LauncherItemsLoader(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                launcherItemIds = launcherItemsLoader.LoadLauncherItemIds(LauncherGroupId, data.Kind);
            }

            Name = data.Name;
            Kind = data.Kind;
            ImageName = data.ImageName;
            ImageColor = data.ImageColor;
            Sequence = data.Sequence;
            LauncherItems.SetRange(launcherItemIds);

            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    LauncherItems.Clear();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherGroupId

        public LauncherGroupId LauncherGroupId { get; }

        #endregion
    }
}
