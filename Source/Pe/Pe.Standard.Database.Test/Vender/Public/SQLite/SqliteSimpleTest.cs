using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using ContentTypeTextNet.Pe.Standard.Database.Test.TestImpl.Vender.Public.SQLite;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Database.Test.Vender.Public.SQLite
{
    [TestClass]
    public class SqliteSimpleTest
    {
        #region define

        #endregion
        public SqliteSimpleTest()
        {
            var factory = new TestSqliteFactory();
            DatabaseAccessor = new SqliteAccessor(factory, NullLoggerFactory.Instance);
        }
        #region property

        IDatabaseAccessor DatabaseAccessor { get; }

        #endregion

        #region function

        [TestInitialize]
        public void TestInitialize()
        {
            var logger = NullLoggerFactory.Instance.CreateLogger(nameof(SqliteSimpleTest));

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
        public void GetDataReaderTest()
        {
            using var reader = DatabaseAccessor.GetDataReader("select * from TestTable1 order by ColKey");

            int rowNumber = 0;
            while(reader.Read()) {
                Assert.AreEqual("ColKey", reader.GetName(0));
                Assert.AreEqual("ColVal", reader.GetName(1));

                Assert.AreEqual(rowNumber + 1, reader.GetInt32(0));
                Assert.AreEqual(new string((char)('A' + rowNumber), 1), reader.GetString(1));

                rowNumber += 1;
            }
        }

        [TestMethod]
        public async Task GetDataReaderTestAsync()
        {
            using var reader = await DatabaseAccessor.GetDataReaderAsync("select * from TestTable1 order by ColKey");

            int rowNumber = 0;
            while(reader.Read()) {
                Assert.AreEqual("ColKey", reader.GetName(0));
                Assert.AreEqual("ColVal", reader.GetName(1));

                Assert.AreEqual(rowNumber + 1, reader.GetInt32(0));
                Assert.AreEqual(new string((char)('A' + rowNumber), 1), reader.GetString(1));

                rowNumber += 1;
            }
        }

        [TestMethod]
        public void GetDataTableTest()
        {
            using var actual = DatabaseAccessor.GetDataTable("select * from TestTable1 order by ColKey");

            Assert.AreEqual(1L, actual.Rows[0]["ColKey"]);
            Assert.AreEqual("A", actual.Rows[0]["ColVal"]);

            Assert.AreEqual(4L, actual.Rows[3]["ColKey"]);
            Assert.AreEqual("D", actual.Rows[3]["ColVal"]);
        }

        [TestMethod]
        public async Task GetDataTableAsyncTest()
        {
            using var actual = await DatabaseAccessor.GetDataTableAsync("select * from TestTable1 order by ColKey");

            Assert.AreEqual(1L, actual.Rows[0]["ColKey"]);
            Assert.AreEqual("A", actual.Rows[0]["ColVal"]);

            Assert.AreEqual(4L, actual.Rows[3]["ColKey"]);
            Assert.AreEqual("D", actual.Rows[3]["ColVal"]);
        }


        [TestMethod]
        public void ExecuteScalarTest()
        {
            var actual1 = DatabaseAccessor.GetScalar<string>("select ColVal from TestTable1 order by ColKey");
            Assert.AreEqual("A", actual1);

            var actual2 = DatabaseAccessor.GetScalar<string>("select ColVal from TestTable1 where ColKey <> 1 order by ColKey");
            Assert.AreEqual("B", actual2);
        }

        [TestMethod]
        public async Task GetScalarAsyncTest()
        {
            var actual1 = await DatabaseAccessor.GetScalarAsync<string>("select ColVal from TestTable1 order by ColKey");
            Assert.AreEqual("A", actual1);

            var actual2 = await DatabaseAccessor.GetScalarAsync<string>("select ColVal from TestTable1 where ColKey <> 1 order by ColKey");
            Assert.AreEqual("B", actual2);
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
        public async Task QueryFirstAsyncTest()
        {
            var actual = await DatabaseAccessor.QueryFirstAsync<string>("select ColVal from TestTable1 where ColKey = 2");
            Assert.AreEqual("B", actual);

            try {
                await DatabaseAccessor.QueryFirstAsync<string>("select ColVal from TestTable1 where ColKey = -1");
                Assert.Fail();
            } catch(InvalidOperationException) {
            }
        }

        [TestMethod]
        public void QueryFirstOrDefaultTest()
        {
            var actual = DatabaseAccessor.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = 2");
            Assert.AreEqual("B", actual);

            var actualDefault = DatabaseAccessor.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = -1");
            Assert.AreEqual(default, actualDefault);
        }

        [TestMethod]
        public async Task QueryFirstOrDefaultAsyncTest()
        {
            var actual = await DatabaseAccessor.QueryFirstOrDefaultAsync<string>("select ColVal from TestTable1 where ColKey = 2");
            Assert.AreEqual("B", actual);

            var actualDefault = await DatabaseAccessor.QueryFirstOrDefaultAsync<string>("select ColVal from TestTable1 where ColKey = -1");
            Assert.AreEqual(default, actualDefault);
        }

        [TestMethod]
        public void QuerySingleTest()
        {
            var actual = DatabaseAccessor.QuerySingle<string>("select ColVal from TestTable1 where ColKey = 2");
            Assert.AreEqual("B", actual);

            Assert.ThrowsException<InvalidOperationException>(() => DatabaseAccessor.QuerySingle<string>("select ColVal from TestTable1 where ColKey = -1"));
            Assert.ThrowsException<InvalidOperationException>(() => DatabaseAccessor.QuerySingle<string>("select ColVal from TestTable1 where ColKey in (1, 2)"));
        }

        [TestMethod]
        public async Task QuerySingleAsyncTest()
        {
            var actual = await DatabaseAccessor.QuerySingleAsync<string>("select ColVal from TestTable1 where ColKey = 2");
            Assert.AreEqual("B", actual);

            try {
                await DatabaseAccessor.QuerySingleAsync<string>("select ColVal from TestTable1 where ColKey = -1");
                Assert.Fail();
            } catch(InvalidOperationException) {
                Assert.IsTrue(true);
            }

            try {
                await DatabaseAccessor.QuerySingleAsync<string>("select ColVal from TestTable1 where ColKey in (1, 2)");
                Assert.Fail();
            } catch(InvalidOperationException) {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void QuerySingleOrDefaultTest()
        {
            var actual = DatabaseAccessor.QuerySingleOrDefault<string>("select ColVal from TestTable1 where ColKey = 2");
            Assert.AreEqual("B", actual);

            var actualNull = DatabaseAccessor.QuerySingleOrDefault<string>("select ColVal from TestTable1 where ColKey = -1");
            Assert.IsNull(actualNull);
        }

        [TestMethod]
        public async Task QuerySingleOrDefaultAsyncTest()
        {
            var actual = await DatabaseAccessor.QuerySingleOrDefaultAsync<string>("select ColVal from TestTable1 where ColKey = 2");
            Assert.AreEqual("B", actual);

            var actualNull = await DatabaseAccessor.QuerySingleOrDefaultAsync<string>("select ColVal from TestTable1 where ColKey = -1");
            Assert.IsNull(actualNull);
        }

        #endregion
    }
}
