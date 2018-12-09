using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database
{
    public abstract class DatabaseAccessObjectBase
    {
        public DatabaseAccessObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILogger logger)
        {
            Commander = commander;
            StatementLoader = statementLoader;
            Logger = logger;
        }

        public DatabaseAccessObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILogFactory logFactory)
            : this(commander, statementLoader, logFactory.CreateCurrentClass())
        { }

        #region property

        protected IDatabaseCommander Commander { get; }
        protected IDatabaseStatementLoader StatementLoader { get; }
        protected ILogger Logger { get; }

        #endregion

        #region function
        #endregion
    }
}
