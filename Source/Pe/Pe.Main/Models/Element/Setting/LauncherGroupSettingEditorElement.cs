using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherGroupSettingEditorElement : ElementBase, ILauncherGroupId
    {
        public LauncherGroupSettingEditorElement(Guid launcherGroupId, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherGroupId = launcherGroupId;

            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
            IdFactory = idFactory;
        }


        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IIdFactory IdFactory { get; }

        public string Name { get; set; } = string.Empty;
        public LauncherGroupKind Kind { get; set; }
        public LauncherGroupImageName ImageName { get; set; }
        public Color ImageColor { get; set; }
        public long Sequence { get; set; }

        public ObservableCollection<Guid> LauncherItemIds { get; } = new ObservableCollection<Guid>();

        #endregion

        #region function
        #endregion

        #region ElementBase
        protected override void InitializeImpl()
        {
            LauncherGroupData data;
            IEnumerable<Guid> launcherItemIds;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherGroupsEntityDao = new LauncherGroupsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                data = launcherGroupsEntityDao.SelectLauncherGroup(LauncherGroupId);

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

        #region ILauncherGroupId

        public Guid LauncherGroupId { get; }

        #endregion
    }
}
