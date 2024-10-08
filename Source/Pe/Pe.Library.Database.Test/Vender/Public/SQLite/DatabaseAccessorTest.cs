using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Database.Test.Vender.Public.SQLite
{
    public class DatabaseAccessorTest
    {
        #region define

        #endregion

        public DatabaseAccessorTest()
        {
            var factory = new InMemorySqliteFactory();
            DatabaseAccessor = new SqliteAccessor(factory, NullLoggerFactory.Instance);

            var logger = NullLoggerFactory.Instance.CreateLogger(nameof(DatabaseAccessorTest));

            var statements = new[] {
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
            foreach(var statement in statements) {
                c.Execute(statement);
            }
            c.Commit();
        }

        #region property

        private DatabaseAccessor DatabaseAccessor { get; }

        #endregion

        #region function

        [Fact]
        public void ConnectionTest()
        {
            var factory = new InMemorySqliteFactory();
            var databaseAccessor = new SqliteAccessor(factory, NullLoggerFactory.Instance.CreateLogger(GetType().ToString()));
            Assert.Equal(databaseAccessor.BaseConnection, databaseAccessor.Connection);
        }

        [Fact]
        public void PauseConnectionTest()
        {
            var databaseAccessor = DatabaseAccessor;
            Assert.True(databaseAccessor.IsOpened);
            Assert.False(databaseAccessor.ConnectionPausing);

            // テスト初期化のテーブル 1 + ダミーテーブル 1のみ
            databaseAccessor.Execute("create table X (col integer)");
            var actual1 = databaseAccessor.QueryFirst<long>("select count(*) from sqlite_master where type = 'table'");
            Assert.Equal(2, actual1);

            using(DatabaseAccessor.PauseConnection()) {
                Assert.False(databaseAccessor.IsOpened);
                Assert.True(databaseAccessor.ConnectionPausing);

                // つながってないのでアクセスできない(処理正常終了は sqlite connection に依存している)
                databaseAccessor.Execute("create table X (col integer)");
                var actual2 = databaseAccessor.QueryFirst<long>("select count(*) from sqlite_master where type = 'table'");
                Assert.Equal(0, actual2);

                // 非オープン時(=ネスト)した状態は何も行われない
                using(DatabaseAccessor.PauseConnection()) {
                    databaseAccessor.Execute("create table X (col integer)");
                    var actual3 = databaseAccessor.QueryFirst<long>("select count(*) from sqlite_master where type = 'table'");
                    Assert.Equal(0, actual3);
                }

                // ネストから復帰しても大本がまだ閉じているのでアクセスできない
                databaseAccessor.Execute("create table X (col integer)");
                var actual4 = databaseAccessor.QueryFirst<long>("select count(*) from sqlite_master where type = 'table'");
                Assert.Equal(0, actual4);
            }

            // 再接続によりテスト初期化データが破棄されている(in memory 前提)
            databaseAccessor.Execute("create table X (col integer)");
            var actual5 = databaseAccessor.QueryFirst<long>("select count(*) from sqlite_master where type = 'table'");
            Assert.Equal(1, actual5);
        }

        [Fact]
        public void GetDataReaderTest()
        {
            using var reader = DatabaseAccessor.GetDataReader("select * from TestTable1 order by ColKey");

            int rowNumber = 0;
            while(reader.Read()) {
                Assert.Equal("ColKey", reader.GetName(0));
                Assert.Equal("ColVal", reader.GetName(1));

                Assert.Equal(rowNumber + 1, reader.GetInt32(0));
                Assert.Equal(new string((char)('A' + rowNumber), 1), reader.GetString(1));

                rowNumber += 1;
            }
            Assert.Equal(4, rowNumber);
        }

        [Fact]
        public async Task GetDataReaderAsyncTest()
        {
            using var reader = await DatabaseAccessor.GetDataReaderAsync("select * from TestTable1 order by ColKey");

            int rowNumber = 0;
            while(reader.Read()) {
                Assert.Equal("ColKey", reader.GetName(0));
                Assert.Equal("ColVal", reader.GetName(1));

                Assert.Equal(rowNumber + 1, reader.GetInt32(0));
                Assert.Equal(new string((char)('A' + rowNumber), 1), reader.GetString(1));

                rowNumber += 1;
            }
            Assert.Equal(4, rowNumber);
        }

        [Fact]
        public void GetDataTableTest()
        {
            using var actual = DatabaseAccessor.GetDataTable("select * from TestTable1 order by ColKey");

            Assert.Equal(1L, actual.Rows[0]["ColKey"]);
            Assert.Equal("A", actual.Rows[0]["ColVal"]);

            Assert.Equal(4L, actual.Rows[3]["ColKey"]);
            Assert.Equal("D", actual.Rows[3]["ColVal"]);
        }

        [Fact]
        public async Task GetDataTableAsyncTest()
        {
            using var actual = await DatabaseAccessor.GetDataTableAsync("select * from TestTable1 order by ColKey");

            Assert.Equal(1L, actual.Rows[0]["ColKey"]);
            Assert.Equal("A", actual.Rows[0]["ColVal"]);

            Assert.Equal(4L, actual.Rows[3]["ColKey"]);
            Assert.Equal("D", actual.Rows[3]["ColVal"]);
        }


        [Fact]
        public void ExecuteScalarTest()
        {
            var actual1 = DatabaseAccessor.GetScalar<string>("select ColVal from TestTable1 order by ColKey");
            Assert.Equal("A", actual1);

            var actual2 = DatabaseAccessor.GetScalar<string>("select ColVal from TestTable1 order by ColKey desc");
            Assert.Equal("D", actual2);

            var actual3 = DatabaseAccessor.GetScalar<string>("select ColVal from TestTable1 where ColKey = -1");
            Assert.Null(actual3);
        }

        [Fact]
        public async Task GetScalarAsyncTest()
        {
            var actual1 = await DatabaseAccessor.GetScalarAsync<string>("select ColVal from TestTable1 order by ColKey");
            Assert.Equal("A", actual1);

            var actual2 = await DatabaseAccessor.GetScalarAsync<string>("select ColVal from TestTable1 order by ColKey desc");
            Assert.Equal("D", actual2);

            var actual3 = await DatabaseAccessor.GetScalarAsync<string>("select ColVal from TestTable1 where ColKey = -1");
            Assert.Null(actual3);
        }

        [Fact]
        public void QueryTest()
        {
            var expectedAsc = new[] { "A", "B", "C", "D", };
            var actualAsc = DatabaseAccessor.Query<string>("select ColVal from TestTable1 order by ColKey");
            Assert.Equal(expectedAsc, actualAsc);

            var expectedDesc = new[] { "D", "C", "B", "A", };
            var actualDesc = DatabaseAccessor.Query<string>("select ColVal from TestTable1 order by ColKey desc");
            Assert.Equal(expectedDesc, actualDesc);
        }

        [Fact]
        public void Query_Dynamic_Test()
        {
            var expectedAsc = new[] { "A", "B", "C", "D", };
            var actualAsc = DatabaseAccessor.Query("select * from TestTable1 order by ColKey");
            Assert.Equal(expectedAsc, actualAsc.Select(i => i.ColVal).ToList());

            var expectedDesc = new[] { "D", "C", "B", "A", };
            var actualDesc = DatabaseAccessor.Query("select * from TestTable1 order by ColKey desc");
            Assert.Equal(expectedDesc, actualDesc.Select(i => i.ColVal).ToList());
        }

        [Fact]
        public async Task QueryAsyncTest()
        {
            var expectedAsc = new[] { "A", "B", "C", "D", };
            var actualAsc = await DatabaseAccessor.QueryAsync<string>("select ColVal from TestTable1 order by ColKey");
            Assert.Equal(expectedAsc, actualAsc.ToList());

            var expectedDesc = new[] { "D", "C", "B", "A", };
            var actualDesc = await DatabaseAccessor.QueryAsync<string>("select ColVal from TestTable1 order by ColKey desc");
            Assert.Equal(expectedDesc, actualDesc.ToList());
        }

        [Fact]
        public async Task QueryAsync_Dynamic_Test()
        {
            var expectedAsc = new[] { "A", "B", "C", "D", };
            var actualAsc = await DatabaseAccessor.QueryAsync("select * from TestTable1 order by ColKey");
            Assert.Equal(expectedAsc, actualAsc.Select(i => i.ColVal).ToList());

            var expectedDesc = new[] { "D", "C", "B", "A", };
            var actualDesc = await DatabaseAccessor.QueryAsync("select * from TestTable1 order by ColKey desc");
            Assert.Equal(expectedDesc, actualDesc.Select(i => i.ColVal).ToList());
        }

        [Fact]
        public void QueryFirstTest()
        {
            var actual = DatabaseAccessor.QueryFirst<string>("select ColVal from TestTable1 where ColKey = 4");
            Assert.Equal("D", actual);

            Assert.Throws<InvalidOperationException>(() => DatabaseAccessor.QueryFirst<string>("select ColVal from TestTable1 where ColKey = -1"));
        }

        [Fact]
        public async Task QueryFirstAsyncTest()
        {
            var actual = await DatabaseAccessor.QueryFirstAsync<string>("select ColVal from TestTable1 where ColKey = 4");
            Assert.Equal("D", actual);

            try {
                await DatabaseAccessor.QueryFirstAsync<string>("select ColVal from TestTable1 where ColKey = -1");
                Assert.Fail();
            } catch(InvalidOperationException) {
            }
        }

        [Fact]
        public void QueryFirstOrDefaultTest()
        {
            var actual = DatabaseAccessor.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = 4");
            Assert.Equal("D", actual);

            var actualDefault = DatabaseAccessor.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = -1");
            Assert.Equal(default, actualDefault);
        }

        [Fact]
        public async Task QueryFirstOrDefaultAsyncTest()
        {
            var actual = await DatabaseAccessor.QueryFirstOrDefaultAsync<string>("select ColVal from TestTable1 where ColKey = 4");
            Assert.Equal("D", actual);

            var actualDefault = await DatabaseAccessor.QueryFirstOrDefaultAsync<string>("select ColVal from TestTable1 where ColKey = -1");
            Assert.Equal(default, actualDefault);
        }

        [Fact]
        public void QuerySingleTest()
        {
            var actual = DatabaseAccessor.QuerySingle<string>("select ColVal from TestTable1 where ColKey = 4");
            Assert.Equal("D", actual);

            Assert.Throws<InvalidOperationException>(() => DatabaseAccessor.QuerySingle<string>("select ColVal from TestTable1 where ColKey = -1"));
            Assert.Throws<InvalidOperationException>(() => DatabaseAccessor.QuerySingle<string>("select ColVal from TestTable1 where ColKey in (1, 2)"));
        }

        [Fact]
        public async Task QuerySingleAsyncTest()
        {
            var actual = await DatabaseAccessor.QuerySingleAsync<string>("select ColVal from TestTable1 where ColKey = 4");
            Assert.Equal("D", actual);

            try {
                await DatabaseAccessor.QuerySingleAsync<string>("select ColVal from TestTable1 where ColKey = -1");
                Assert.Fail();
            } catch(InvalidOperationException) {
                Assert.True(true);
            }

            try {
                await DatabaseAccessor.QuerySingleAsync<string>("select ColVal from TestTable1 where ColKey in (1, 2)");
                Assert.Fail();
            } catch(InvalidOperationException) {
                Assert.True(true);
            }
        }

        [Fact]
        public void QuerySingleOrDefaultTest()
        {
            var actual = DatabaseAccessor.QuerySingleOrDefault<string>("select ColVal from TestTable1 where ColKey = 4");
            Assert.Equal("D", actual);

            var actualNull = DatabaseAccessor.QuerySingleOrDefault<string>("select ColVal from TestTable1 where ColKey = -1");
            Assert.Null(actualNull);
        }

        [Fact]
        public async Task QuerySingleOrDefaultAsyncTest()
        {
            var actual = await DatabaseAccessor.QuerySingleOrDefaultAsync<string>("select ColVal from TestTable1 where ColKey = 4");
            Assert.Equal("D", actual);

            var actualNull = await DatabaseAccessor.QuerySingleOrDefaultAsync<string>("select ColVal from TestTable1 where ColKey = -1");
            Assert.Null(actualNull);
        }

        [Fact]
        public void ExecuteTest()
        {
            var actual = DatabaseAccessor.Execute("create table TestTable2 as select * from TestTable1");
            Assert.Equal(4, actual);
        }

        [Fact]
        public async Task ExecuteAsyncTest()
        {
            var actual = await DatabaseAccessor.ExecuteAsync("create table TestTable2 as select * from TestTable1");
            Assert.Equal(4, actual);
        }

        [Fact]
        public void TransactionTest()
        {
            using(var transaction = DatabaseAccessor.BeginTransaction()) {
                var actualNone = transaction.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = 0");
                Assert.Null(actualNone);

                transaction.Execute("insert into TestTable1(ColKey, ColVal) values (0, 'Z')");
                var actualZ = transaction.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = 0");
                Assert.NotNull(actualZ);
                Assert.Equal("Z", actualZ);
            }

            var actualNone2 = DatabaseAccessor.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = 0");
            Assert.Null(actualNone2);
        }

        [Fact]
        public async Task ReadonlyTransactionTest()
        {
            using(var transaction = DatabaseAccessor.BeginReadOnlyTransaction()) {
                Assert.Null(transaction.Transaction);

                var actualNone = transaction.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = 0");
                Assert.Null(actualNone);

                Assert.Throws<NotSupportedException>(() => transaction.Execute("insert into TestTable1(ColKey, ColVal) values (0, 'Z')"));
                await Assert.ThrowsAsync<NotSupportedException>(() => transaction.ExecuteAsync("insert into TestTable1(ColKey, ColVal) values (0, 'Z')"));

                Assert.Throws<NotSupportedException>(() => transaction.Commit());
            }

            var actualNone2 = DatabaseAccessor.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = 0");
            Assert.Null(actualNone2);
        }

        #endregion
    }
}
