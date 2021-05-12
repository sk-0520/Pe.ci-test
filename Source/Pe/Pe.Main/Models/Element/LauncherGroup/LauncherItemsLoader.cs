using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup
{
    public class LauncherItemsLoader
    {
        public LauncherItemsLoader(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
        {
            Context = context;
            StatementLoader = statementLoader;
            Implementation = implementation;
            LoggerFactory = loggerFactory;
        }

        #region property

        IDatabaseContext Context { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IDatabaseImplementation Implementation { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        ILoggerFactory LoggerFactory { get; }
        #endregion

        #region function

        IEnumerable<Guid> LoadNormalIds(Guid launcherGroupId)
        {
            var dao = new LauncherGroupItemsEntityDao(Context, StatementLoader, Implementation, LoggerFactory);
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
