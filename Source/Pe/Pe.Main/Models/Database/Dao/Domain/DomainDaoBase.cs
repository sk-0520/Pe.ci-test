using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
    public abstract class DomainDaoBase : ApplicationDatabaseObjectBase
    {
        protected DomainDaoBase(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property
        #endregion
    }
}
