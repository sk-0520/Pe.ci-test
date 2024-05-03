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
using System.Runtime.CompilerServices;

namespace ContentTypeTextNet.Pe.Standard.Database.Test.Vender.Public.SQLite
{
    public class DatabaseAccessObjectTest
    {
        #region define

        private class TestStatementLoader: IDatabaseStatementLoader
        {
            #region IDatabaseStatementLoader

            /// <summary>
            /// キーからデータベース実行文を取得。
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public string LoadStatement(string key)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// 呼び出しクラス・メンバ名の完全名からデータベース実行文を取得する。
            /// </summary>
            /// <returns></returns>
            public string LoadStatementByCurrent(Type callerType, [CallerMemberName] string callerMemberName = "")
            {
                var current = TestIO.InitializeMethod(this);
                var file = TestIO.CreateTextFile(current, "MEMBER!NAME.sql", "file-sql1\nfile-sql2");
                using var reader = file.OpenText();
                return reader.ReadToEnd();
            }

            #endregion
        }

        private class SqliteDatabaseAccessObject: DatabaseAccessObjectBase
        {
            public SqliteDatabaseAccessObject()
                : base(default!, new TestStatementLoader(), new Pe.Core.Models.Database.Vender.Public.SQLite.SqliteImplementation(), NullLoggerFactory.Instance)
            { }

            #region function

            internal string ProcessStatement2(string statement, IReadOnlyDictionary<string, string> blocks, string callerMemberName = "")
            {
                return ProcessStatement(statement, blocks, callerMemberName);
            }

            #endregion
        }

        #endregion

        #region property

        private string Sql { get; } = @"
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
    NAME.sql
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

        #endregion

        #region function

        SqliteDatabaseAccessObject CreateDao()
        {
            return new SqliteDatabaseAccessObject();
        }

        [Fact]
        public void ProcessStatementTest_1()
        {
            var dao = CreateDao();
            var map = new Dictionary<string, string>();
            map["KEY1"] = "VALUE1";
            var actual1 = dao.ProcessStatement2(Sql, map);
            var expected1 = @"
select
*
from
    1
order by
COL3

";
            ExAssert.AreMultiLineTextEqualWithoutNewline(expected1, actual1);
        }

        [Fact]
        public void ProcessStatementTest_2()
        {
            var dao = CreateDao();
            var map = new Dictionary<string, string>();

            map["KEY1"] = "VALUE2";
            map["KEY2"] = "VALUE2";
            var actual2 = dao.ProcessStatement2(Sql, map);
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
        }

        [Fact]
        public void ProcessStatementTest_3()
        {
            var dao = CreateDao();
            var map = new Dictionary<string, string>();

            map["KEY1"] = "VALUE3";
            var actual3 = dao.ProcessStatement2(Sql, map, "MEMBER");
            var expected3 = @"
select
*
from
file-sql1
file-sql2
order by
COL3

";
            ExAssert.AreMultiLineTextEqualWithoutNewline(expected3, actual3);
        }

        #endregion
    }
}
