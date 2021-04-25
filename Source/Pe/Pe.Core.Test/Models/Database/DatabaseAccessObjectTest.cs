using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Database
{
    [TestClass]
    public class DatabaseAccessObjectTest
    {
        #region define

        private class SqliteDatabaseAccessObject: DatabaseAccessObjectBase
        {
            public SqliteDatabaseAccessObject()
                : base(default!, default!, new Pe.Core.Models.Database.Vender.Public.SQLite.SqliteImplementation(), Test.LoggerFactory)
            { }

            #region function

            internal string ProcessStatement2(string statement, IReadOnlyDictionary<string, string> blocks, string callerMemberName = "")
            {
                return ProcessStatement(statement, blocks, callerMemberName);
            }

            #endregion
        }

        #endregion

        #region function

        SqliteDatabaseAccessObject CreateDao()
        {
            return new SqliteDatabaseAccessObject();
        }

        [TestMethod]
        public void ProcessStatementTest_1()
        {
            var input = @"
select
*
from
/*/!*//*KEY1
VALUE1: {1}
VALUE2: {
    2
}
VALUE3: LOAD
VALUE3:
    LOAD
*/TABLE/*!/*/
";
            var dao = CreateDao();
            var map = new Dictionary<string, string>();
            map["KEY"] = "VALUE1";
            var output1 = dao.ProcessStatement2(input, map);
        }



        #endregion
    }
}
