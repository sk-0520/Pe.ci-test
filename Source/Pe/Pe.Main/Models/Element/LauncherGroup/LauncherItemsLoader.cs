using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup
{
    public class LauncherItemsLoader
    {
        public LauncherItemsLoader(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
        {
            Commander = commander;
            StatementLoader = statementLoader;
            Implementation = implementation;
            LoggerFactory = loggerFactory;
        }

        #region property

        IDatabaseCommander Commander { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IDatabaseImplementation Implementation { get; }
        ILoggerFactory LoggerFactory { get; }
        #endregion

        #region function

        IEnumerable<Guid> LoadNormalIds(Guid launcherGroupId)
        {
                var dao = new LauncherGroupItemsEntityDao(Commander, StatementLoader, Implementation, LoggerFactory);
                return dao.SelectLauncherItemIds(launcherGroupId);
        }

        public IEnumerable<Guid> LoadLauncherItemIds(Guid launcherGroupId, LauncherGroupKind kind)
        {
            switch(kind) {
                case LauncherGroupKind.Normal:
                    return LoadNormalIds(launcherGroupId);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
