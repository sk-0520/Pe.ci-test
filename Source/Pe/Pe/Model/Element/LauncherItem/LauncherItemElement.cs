using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Manager;

namespace ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem
{
    public class LauncherItemElement: ElementBase
    {
        public LauncherItemElement(Guid launcherItemId, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;

            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        public Guid LauncherItemId { get; }

        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        #endregion

        #region ElementBase

        override protected void InitializeImpl()
        {
        }

        #endregion
    }
}
