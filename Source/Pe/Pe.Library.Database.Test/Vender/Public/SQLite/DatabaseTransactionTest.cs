using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Database.Test.Vender.Public.SQLite
{
    public class DatabaseTransactionTest
    {
        public DatabaseTransactionTest()
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

        private IDatabaseAccessor DatabaseAccessor { get; }

        #endregion

        #region function

        [Fact]
        public void GetDataReaderTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");
                using var reader = context.GetDataReader("select * from TestTable1 order by ColKey");

                int rowNumber = 0;
                while(reader.Read()) {
                    Assert.Equal("ColKey", reader.GetName(0));
                    Assert.Equal("ColVal", reader.GetName(1));

                    Assert.Equal(rowNumber + 1, reader.GetInt32(0));
                    Assert.Equal(new string((char)('A' + rowNumber), 1), reader.GetString(1));

                    rowNumber += 1;
                }
                Assert.Equal(5, rowNumber);
            }
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
        }

        [Fact]
        public async Task GetDataReaderAsyncTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                using var reader = await context.GetDataReaderAsync("select * from TestTable1 order by ColKey");

                int rowNumber = 0;
                while(reader.Read()) {
                    Assert.Equal("ColKey", reader.GetName(0));
                    Assert.Equal("ColVal", reader.GetName(1));

                    Assert.Equal(rowNumber + 1, reader.GetInt32(0));
                    Assert.Equal(new string((char)('A' + rowNumber), 1), reader.GetString(1));

                    rowNumber += 1;
                }
                Assert.Equal(5, rowNumber);
            }

            Assert.Equal(4, DatabaseAccessor.QueryFirst<long>("select count(*) from TestTable1"));
        }

        [Fact]
        public void GetDataTableTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                using var actual = context.GetDataTable("select * from TestTable1 order by ColKey");

                Assert.Equal(1L, actual.Rows[0]["ColKey"]);
                Assert.Equal("A", actual.Rows[0]["ColVal"]);

                Assert.Equal(5L, actual.Rows[4]["ColKey"]);
                Assert.Equal("E", actual.Rows[4]["ColVal"]);
            }
        }

        [Fact]
        public async Task GetDataTableAsyncTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");
                using var actual = await context.GetDataTableAsync("select * from TestTable1 order by ColKey");

                Assert.Equal(1L, actual.Rows[0]["ColKey"]);
                Assert.Equal("A", actual.Rows[0]["ColVal"]);

                Assert.Equal(5L, actual.Rows[4]["ColKey"]);
                Assert.Equal("E", actual.Rows[4]["ColVal"]);
            }
        }

        [Fact]
        public void ExecuteScalarTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                var actual1 = context.GetScalar<string>("select ColVal from TestTable1 order by ColKey");
                Assert.Equal("A", actual1);

                var actual2 = context.GetScalar<string>("select ColVal from TestTable1 order by ColKey desc");
                Assert.Equal("E", actual2);

                var actual3 = context.GetScalar<string>("select ColVal from TestTable1 where ColKey = -1");
                Assert.Null(actual3);
            }
        }

        [Fact]
        public async Task GetScalarAsyncTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                var actual1 = await context.GetScalarAsync<string>("select ColVal from TestTable1 order by ColKey");
                Assert.Equal("A", actual1);

                var actual2 = await context.GetScalarAsync<string>("select ColVal from TestTable1 order by ColKey desc");
                Assert.Equal("E", actual2);

                var actual3 = await context.GetScalarAsync<string>("select ColVal from TestTable1 where ColKey = -1");
                Assert.Null(actual3);
            }
        }

        [Fact]
        public void QueryTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                var expectedAsc = new[] { "A", "B", "C", "D", "E" };
                var actualAsc = context.Query<string>("select ColVal from TestTable1 order by ColKey");
                Assert.Equal(expectedAsc, actualAsc);

                var expectedDesc = new[] { "E", "D", "C", "B", "A", };
                var actualDesc = context.Query<string>("select ColVal from TestTable1 order by ColKey desc");
                Assert.Equal(expectedDesc, actualDesc);
            }
        }

        [Fact]
        public void Query_Dynamic_Test()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                var expectedAsc = new[] { "A", "B", "C", "D", "E" };
                var actualAsc = context.Query("select * from TestTable1 order by ColKey");
                Assert.Equal(expectedAsc, actualAsc.Select(i => i.ColVal).ToList());

                var expectedDesc = new[] { "E", "D", "C", "B", "A", };
                var actualDesc = context.Query("select * from TestTable1 order by ColKey desc");
                Assert.Equal(expectedDesc, actualDesc.Select(i => i.ColVal).ToList());
            }
        }

        [Fact]
        public async Task QueryAsyncTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                var expectedAsc = new[] { "A", "B", "C", "D", "E" };
                var actualAsc = await context.QueryAsync<string>("select ColVal from TestTable1 order by ColKey");
                Assert.Equal(expectedAsc, actualAsc.ToList());

                var expectedDesc = new[] { "E", "D", "C", "B", "A", };
                var actualDesc = await context.QueryAsync<string>("select ColVal from TestTable1 order by ColKey desc");
                Assert.Equal(expectedDesc, actualDesc.ToList());
            }
        }

        [Fact]
        public async Task QueryAsync_Dynamic_Test()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                var expectedAsc = new[] { "A", "B", "C", "D", "E" };
                var actualAsc = await context.QueryAsync("select * from TestTable1 order by ColKey");
                Assert.Equal(expectedAsc, actualAsc.Select(i => i.ColVal).ToList());

                var expectedDesc = new[] { "E", "D", "C", "B", "A", };
                var actualDesc = await context.QueryAsync("select * from TestTable1 order by ColKey desc");
                Assert.Equal(expectedDesc, actualDesc.Select(i => i.ColVal).ToList());
            }
        }


        [Fact]
        public void QueryFirstTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");
                var actual = context.QueryFirst<string>("select ColVal from TestTable1 where ColKey = 5");
                Assert.Equal("E", actual);

                Assert.Throws<InvalidOperationException>(() => context.QueryFirst<string>("select ColVal from TestTable1 where ColKey = -1"));
            }
        }

        [Fact]
        public async Task QueryFirstAsyncTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");
                var actual = await context.QueryFirstAsync<string>("select ColVal from TestTable1 where ColKey = 5");
                Assert.Equal("E", actual);

                try {
                    await context.QueryFirstAsync<string>("select ColVal from TestTable1 where ColKey = -1");
                    Assert.Fail();
                } catch(InvalidOperationException) {
                }
            }
        }

        [Fact]
        public void QueryFirstOrDefaultTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                var actual = context.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = 5");
                Assert.Equal("E", actual);

                var actualDefault = context.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = -1");
                Assert.Equal(default, actualDefault);
            }
        }

        [Fact]
        public async Task QueryFirstOrDefaultAsyncTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                var actual = await context.QueryFirstOrDefaultAsync<string>("select ColVal from TestTable1 where ColKey = 5");
                Assert.Equal("E", actual);

                var actualDefault = await context.QueryFirstOrDefaultAsync<string>("select ColVal from TestTable1 where ColKey = -1");
                Assert.Equal(default, actualDefault);
            }
        }

        [Fact]
        public void QuerySingleTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                var actual = context.QuerySingle<string>("select ColVal from TestTable1 where ColKey = 5");
                Assert.Equal("E", actual);

                Assert.Throws<InvalidOperationException>(() => context.QuerySingle<string>("select ColVal from TestTable1 where ColKey = -1"));
                Assert.Throws<InvalidOperationException>(() => context.QuerySingle<string>("select ColVal from TestTable1 where ColKey in (1, 2)"));
            }
        }

        [Fact]
        public async Task QuerySingleAsyncTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                var actual = await context.QuerySingleAsync<string>("select ColVal from TestTable1 where ColKey = 5");
                Assert.Equal("E", actual);

                try {
                    await context.QuerySingleAsync<string>("select ColVal from TestTable1 where ColKey = -1");
                    Assert.Fail();
                } catch(InvalidOperationException) {
                    Assert.True(true);
                }

                try {
                    await context.QuerySingleAsync<string>("select ColVal from TestTable1 where ColKey in (1, 2)");
                    Assert.Fail();
                } catch(InvalidOperationException) {
                    Assert.True(true);
                }
            }
        }

        [Fact]
        public void QuerySingleOrDefaultTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                var actual = context.QuerySingleOrDefault<string>("select ColVal from TestTable1 where ColKey = 5");
                Assert.Equal("E", actual);

                var actualNull = context.QuerySingleOrDefault<string>("select ColVal from TestTable1 where ColKey = -1");
                Assert.Null(actualNull);
            }
        }

        [Fact]
        public async Task QuerySingleOrDefaultAsyncTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                context.Execute("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                var actual = await context.QuerySingleOrDefaultAsync<string>("select ColVal from TestTable1 where ColKey = 5");
                Assert.Equal("E", actual);

                var actualNull = await context.QuerySingleOrDefaultAsync<string>("select ColVal from TestTable1 where ColKey = -1");
                Assert.Null(actualNull);
            }
        }

        [Fact]
        public async Task ExecuteAsyncTest()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                await context.ExecuteAsync("insert into TestTable1(ColKey, ColVal) values (5, 'E')");

                var actual = context.QueryFirst<long>("select count(*) from TestTable1");
                Assert.Equal(5, actual);
            }
        }


        #endregion
    }
}
