using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao
{
    public abstract class ApplicationDatabaseObjectBase : DatabaseAccessObjectBase
    {
        public ApplicationDatabaseObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILogger logger)
            : base(commander, statementLoader, logger)
        { }

        public ApplicationDatabaseObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILogFactory logFactory)
            : base(commander, statementLoader, logFactory)
        { }
    }
}
