using System;
using System.Linq;
using System.Threading.Tasks;
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

            var c = DatabaseAccessor.BeginTransaction();
            foreach(var sql in sqls) {
                c.Execute(sql);
            }
            c.Commit();
        }

        [TestMethod]
        public void QueryTest()
        {
            var expectedAsc = new[] { "A", "B", "C", "D", };
            var actualAsc = DatabaseAccessor.Query<string>("select ColVal from TestTable1 order by ColKey");
            CollectionAssert.AreEqual(expectedAsc, actualAsc.ToList());

            var expectedDesc = new[] { "D", "C", "B", "A", };
            var actualDesc = DatabaseAccessor.Query<string>("select ColVal from TestTable1 order by ColKey desc");
            CollectionAssert.AreEqual(expectedDesc, actualDesc.ToList());
        }

        [TestMethod]
        public void Query_Dynamic_Test()
        {
            var expectedAsc = new[] { "A", "B", "C", "D", };
            var actualAsc = DatabaseAccessor.Query("select * from TestTable1 order by ColKey");
            CollectionAssert.AreEqual(expectedAsc, actualAsc.Select(i => i.ColVal).ToList());

            var expectedDesc = new[] { "D", "C", "B", "A", };
            var actualDesc = DatabaseAccessor.Query("select * from TestTable1 order by ColKey desc");
            CollectionAssert.AreEqual(expectedDesc, actualDesc.Select(i => i.ColVal).ToList());
        }

        [TestMethod]
        public async Task QueryAsyncTest()
        {
            var expectedAsc = new[] { "A", "B", "C", "D", };
            var actualAsc = await DatabaseAccessor.QueryAsync<string>("select ColVal from TestTable1 order by ColKey");
            CollectionAssert.AreEqual(expectedAsc, actualAsc.ToList());

            var expectedDesc = new[] { "D", "C", "B", "A", };
            var actualDesc = await DatabaseAccessor.QueryAsync<string>("select ColVal from TestTable1 order by ColKey desc");
            CollectionAssert.AreEqual(expectedDesc, actualDesc.ToList());
        }

        [TestMethod]
        public async Task QueryAsync_Dynamic_Test()
        {
            var expectedAsc = new[] { "A", "B", "C", "D", };
            var actualAsc = await DatabaseAccessor.QueryAsync("select * from TestTable1 order by ColKey");
            CollectionAssert.AreEqual(expectedAsc, actualAsc.Select(i => i.ColVal).ToList());

            var expectedDesc = new[] { "D", "C", "B", "A", };
            var actualDesc = await DatabaseAccessor.QueryAsync("select * from TestTable1 order by ColKey desc");
            CollectionAssert.AreEqual(expectedDesc, actualDesc.Select(i => i.ColVal).ToList());
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
