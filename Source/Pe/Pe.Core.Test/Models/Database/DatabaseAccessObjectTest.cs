using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Database
{
    class DatabaseAccessObjectTest
    {
        #region define

        private class DatabaseAccessObject: DatabaseAccessObjectBase
        {
            public DatabaseAccessObject(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
                : base(context, statementLoader, implementation, loggerFactory)
            { }
        }

        #endregion
    }
}
