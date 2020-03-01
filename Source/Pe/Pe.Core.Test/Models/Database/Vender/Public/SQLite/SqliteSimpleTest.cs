using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Database.Vender.Public.SQLite
{
    [TestClass]
    public class SqliteSimpleTest
    {
        #region define

        #endregion
        public SqliteSimpleTest()
        {
            var factory = new TestSqliteFactory();
            DatabaseAccessor = new SqliteAccessor(factory, Test.LoggerFactory);
        }
        #region property

        IDatabaseAccessor DatabaseAccessor { get; }

        #endregion

        #region function

        [TestInitialize]
        public void TestInitialize()
        {
            var logger = Test.LoggerFactory.CreateLogger(nameof(SqliteSimpleTest));

            DatabaseAccessor.Batch(c => {
                var sqls = new[] {
@"
create table TestTable1 (
    ColKey integer,
    ColVal text
)
",
@"
insert into
    TestTable1(ColKey, ColVal)
values
    (1, 'A'),
    (2, 'B'),
    (3, 'C'),
    (4, 'D')
"
                };

                foreach(var sql in sqls) {
                    c.Execute(sql);
                }

                return true;
            });
        }

        [TestMethod]
        public void QueryTest()
        {
            var resultAsc = new[] { "A", "B", "C", "D", };
            var actualAsc = DatabaseAccessor.Query<string>("select ColVal from TestTable1 order by ColKey").ToList();
            CollectionAssert.AreEqual(resultAsc, actualAsc);

            var resultDesc = new[] { "D", "C", "B", "A", };
            var actualDesc = DatabaseAccessor.Query<string>("select ColVal from TestTable1 order by ColKey desc").ToList();
            CollectionAssert.AreEqual(resultDesc, actualDesc);
        }

        [TestMethod]
        public void QueryFirstTest()
        {
            var actual = DatabaseAccessor.QueryFirst<string>("select ColVal from TestTable1 where ColKey = 2");
            Assert.AreEqual("B", actual);

            Assert.ThrowsException<InvalidOperationException>(() => DatabaseAccessor.QueryFirst<string>("select ColVal from TestTable1 where ColKey = -1"));
        }

        [TestMethod]
        public void QueryFirstOrDefaultTest()
        {
            var actual = DatabaseAccessor.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = 2");
            Assert.AreEqual("B", actual);

            var actualDefault = DatabaseAccessor.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = -1");
            Assert.AreEqual(default(string), actualDefault);
        }

        [TestMethod]
        public void QuerySingleTest()
        {
            var actual = DatabaseAccessor.QuerySingle<string>("select ColVal from TestTable1 where ColKey = 2");
            Assert.AreEqual("B", actual);

            Assert.ThrowsException<InvalidOperationException>(() => DatabaseAccessor.QuerySingle<string>("select ColVal from TestTable1 where ColKey = -1"));
            Assert.ThrowsException<InvalidOperationException>(() => DatabaseAccessor.QuerySingle<string>("select ColVal from TestTable1 where ColKey in (1, 2)"));
        }

        #endregion
    }
}
