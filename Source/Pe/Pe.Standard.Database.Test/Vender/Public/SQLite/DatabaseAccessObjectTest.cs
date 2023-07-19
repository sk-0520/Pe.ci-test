using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Database.Test.Vender.Public.SQLite
{
    [TestClass]
    public class DatabaseAccessObjectTest
    {
        #region define

        private class SqliteDatabaseAccessObject: DatabaseAccessObjectBase
        {
            public SqliteDatabaseAccessObject()
                : base(default!, default!, new Pe.Core.Models.Database.Vender.Public.SQLite.SqliteImplementation(), new NullLoggerFactory())
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
/*{{*//*KEY1
VALUE1:CODE
    1
VALUE2:CODE
    2
    22
VALUE3:LOAD
    NAME
*/

TABLE

/*}}*/
order by
/*{{*//*KEY2
VALUE1:CODE
    COL1
VALUE2:CODE
    COL2
*/COL3/*}}*/

";
            var dao = CreateDao();
            var map = new Dictionary<string, string>();
            map["KEY1"] = "VALUE1";
            var output1 = dao.ProcessStatement2(input, map);
            var actual1 = @"
select
*
from
    1
order by
COL3

";
            Assert.AreEqual(actual1, output1);

            map.Clear();
            map["KEY1"] = "VALUE2";
            map["KEY2"] = "VALUE2";
            var output2 = dao.ProcessStatement2(input, map);
            var actual2 = @"
select
*
from
    2
    22
order by
    COL2

";
            Assert.AreEqual(actual2, output2);

            //LOADは諸々の事情でテストなし
        }



        #endregion
    }
}
