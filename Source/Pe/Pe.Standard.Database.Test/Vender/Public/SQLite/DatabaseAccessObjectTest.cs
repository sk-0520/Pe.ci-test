using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using ContentTypeTextNet.Pe.Standard.Base;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using ContentTypeTextNet.Pe.Test;

namespace ContentTypeTextNet.Pe.Standard.Database.Test.Vender.Public.SQLite
{
    public class DatabaseAccessObjectTest
    {
        #region define

        private class SqliteDatabaseAccessObject: DatabaseAccessObjectBase
        {
            public SqliteDatabaseAccessObject()
                : base(default!, default!, new Pe.Core.Models.Database.Vender.Public.SQLite.SqliteImplementation(), NullLoggerFactory.Instance)
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

        [Fact]
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
            var actual1 = dao.ProcessStatement2(input, map);
            var expected1 = @"
select
*
from
    1
order by
COL3

";
            ExAssert.AreMultiLineTextEqualWithoutNewline(expected1, actual1);

            map.Clear();
            map["KEY1"] = "VALUE2";
            map["KEY2"] = "VALUE2";
            var actual2 = dao.ProcessStatement2(input, map);
            var expected2 = @"
select
*
from
    2
    22
order by
    COL2

";
            ExAssert.AreMultiLineTextEqualWithoutNewline(expected2, actual2);

            //LOADは諸々の事情でテストなし
        }



        #endregion
    }
}
