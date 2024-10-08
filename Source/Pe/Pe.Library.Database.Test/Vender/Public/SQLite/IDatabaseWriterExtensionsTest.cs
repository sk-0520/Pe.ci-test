using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Database.Test.Vender.Public.SQLite
{
    public class IDatabaseWriterExtensionsTest
    {
        public IDatabaseWriterExtensionsTest()
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

        [Theory]
        [InlineData(4, "update TestTable1 set ColVal = ColVal + ColVal")]
        [InlineData(2, "UPDATE TestTable1 set ColVal = ColVal + ColVal where ColKey in (1, 2)")]
        [InlineData(0, @"
            update
            TestTable1
            set
            ColVal = ColVal + ColVal
            where
            ColKey = 5
        ")]
        public void UpdateTest(int expected, string sql)
        {
            var actual = DatabaseAccessor.Update(sql);
            Assert.Equal(expected, actual);
        }

#if DEBUG
        [Theory]
        [InlineData("")]
        [InlineData("select * from table")]
        [InlineData("updatee TestTable1 set ColVal = ColVal + ColVal")]
        public void Update_throw_Test(string sql)
        {
            var exception = Assert.Throws<DatabaseStatementException>(() => DatabaseAccessor.Update(sql));
            Assert.Equal("update", exception.Message);
        }
#endif

        [Theory]
        [InlineData(4, "update TestTable1 set ColVal = ColVal + ColVal")]
        [InlineData(2, "UPDATE TestTable1 set ColVal = ColVal + ColVal where ColKey in (1, 2)")]
        [InlineData(0, @"
            update
            TestTable1
            set
            ColVal = ColVal + ColVal
            where
            ColKey = 5
        ")]
        public async Task UpdateAsyncTest(int expected, string sql)
        {
            var actual = await DatabaseAccessor.UpdateAsync(sql);
            Assert.Equal(expected, actual);
        }

#if DEBUG
        [Theory]
        [InlineData("")]
        [InlineData("select * from table")]
        [InlineData("updatee TestTable1 set ColVal = ColVal + ColVal")]
        public async Task UpdateAsync_throw_Test(string sql)
        {
            var exception = await Assert.ThrowsAsync<DatabaseStatementException>(() => DatabaseAccessor.UpdateAsync(sql));
            Assert.Equal("update", exception.Message);
        }
#endif

        [Fact]
        public void UpdateByKeyTest()
        {
            var exception = Record.Exception(() => DatabaseAccessor.UpdateByKey("update TestTable1 set ColVal = ColVal + ColVal where ColKey = 1"));
            Assert.Null(exception);
        }

#if DEBUG
        [Fact]
        public void UpdateByKey_throw_Test()
        {
            var exception = Assert.Throws<DatabaseStatementException>(() => DatabaseAccessor.UpdateByKey("updatee TestTable1 set ColVal = ColVal + ColVal"));
            Assert.Equal("update", exception.Message);
        }
#endif

        [Fact]
        public void UpdateByKey_throw_0_Test()
        {
            var exception = Assert.Throws<DatabaseManipulationException>(() => DatabaseAccessor.UpdateByKey("update TestTable1 set ColVal = ColVal + ColVal where ColKey = 0"));
            Assert.Equal("update -> 0", exception.Message);
        }

        [Fact]
        public void UpdateByKey_throw_2_Test()
        {
            var exception = Assert.Throws<DatabaseManipulationException>(() => DatabaseAccessor.UpdateByKey("update TestTable1 set ColVal = ColVal + ColVal where ColKey < 3"));
            Assert.Equal("update -> 2", exception.Message);
        }

        [Fact]
        public async Task UpdateByKeyAsyncTest()
        {
            var exception = await Record.ExceptionAsync(() => DatabaseAccessor.UpdateByKeyAsync("update TestTable1 set ColVal = ColVal + ColVal where ColKey = 1"));
            Assert.Null(exception);
        }

#if DEBUG
        [Fact]
        public async Task UpdateByKeyAsync_throw_Test()
        {
            var exception = await Assert.ThrowsAsync<DatabaseStatementException>(() => DatabaseAccessor.UpdateByKeyAsync("updatee TestTable1 set ColVal = ColVal + ColVal"));
            Assert.Equal("update", exception.Message);
        }
#endif

        [Fact]
        public async Task UpdateByKeyAsync_throw_0_Test()
        {
            var exception = await Assert.ThrowsAsync<DatabaseManipulationException>(() => DatabaseAccessor.UpdateByKeyAsync("update TestTable1 set ColVal = ColVal + ColVal where ColKey = 0"));
            Assert.Equal("update -> 0", exception.Message);
        }

        [Fact]
        public async Task UpdateByKeyAsync_throw_2_Test()
        {
            var exception = await Assert.ThrowsAsync<DatabaseManipulationException>(() => DatabaseAccessor.UpdateByKeyAsync("update TestTable1 set ColVal = ColVal + ColVal where ColKey < 3"));
            Assert.Equal("update -> 2", exception.Message);
        }

        [Fact]
        public void UpdateByKeyOrNothing_update_Test()
        {
            var actual = DatabaseAccessor.UpdateByKeyOrNothing("update TestTable1 set ColVal = ColVal + ColVal where ColKey = 1");
            Assert.True(actual);
        }

        [Fact]
        public void UpdateByKeyOrNothing_nothing_Test()
        {
            var actual = DatabaseAccessor.UpdateByKeyOrNothing("update TestTable1 set ColVal = ColVal + ColVal where ColKey = 0");
            Assert.False(actual);
        }

#if DEBUG
        [Fact]
        public void UpdateByKeyOrNothing_throw_Test()
        {
            var exception = Assert.Throws<DatabaseStatementException>(() => DatabaseAccessor.UpdateByKeyOrNothing("updatee TestTable1 set ColVal = ColVal + ColVal"));
            Assert.Equal("update", exception.Message);
        }
#endif

        [Fact]
        public void UpdateByKeyOrNothing_throw_2_Test()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                var exception = Assert.Throws<DatabaseManipulationException>(() => DatabaseAccessor.UpdateByKeyOrNothing("update TestTable1 set ColVal = ColVal + ColVal where ColKey < 3"));
                Assert.Equal("update -> 2", exception.Message);
            }
        }

        [Fact]
        public async Task UpdateByKeyOrNothingAsync_update_Test()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                var actual = await DatabaseAccessor.UpdateByKeyOrNothingAsync("update TestTable1 set ColVal = ColVal + ColVal where ColKey = 1");
                Assert.True(actual);
            }
        }

        [Fact]
        public async Task UpdateByKeyOrNothingAsync_nothing_Test()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                var actual = await DatabaseAccessor.UpdateByKeyOrNothingAsync("update TestTable1 set ColVal = ColVal + ColVal where ColKey = 0");
                Assert.False(actual);
            }
        }

#if DEBUG
        [Fact]
        public async Task UpdateByKeyOrNothingAsync_throw_Test()
        {
            var exception = await Assert.ThrowsAsync<DatabaseStatementException>(() => DatabaseAccessor.UpdateByKeyOrNothingAsync("updatee TestTable1 set ColVal = ColVal + ColVal"));
            Assert.Equal("update", exception.Message);
        }
#endif

        [Fact]
        public async Task UpdateByKeyOrNothingAsync_throw_2_Test()
        {
            using(var context = DatabaseAccessor.BeginTransaction()) {
                var exception = await Assert.ThrowsAsync<DatabaseManipulationException>(() => DatabaseAccessor.UpdateByKeyOrNothingAsync("update TestTable1 set ColVal = ColVal + ColVal where ColKey < 3"));
                Assert.Equal("update -> 2", exception.Message);
            }
        }

        [Theory]
        [InlineData(1, "insert into TestTable1(ColKey, ColVal) values (5, 'E')")]
        [InlineData(2, "INSERT into TestTable1(ColKey, ColVal) values (5, 'E'),(6, 'F')")]
        [InlineData(0, @"
            insert
            into
            TestTable1
            (
            ColKey,
            ColVal
            )
            select
            *
            from
            TestTable1
            where
            ColKey < 0
        ")]
        public void InsertTest(int expected, string sql)
        {
            var actual = DatabaseAccessor.Insert(sql);
            Assert.Equal(expected, actual);
        }

#if DEBUG
        [Theory]
        [InlineData("")]
        [InlineData("select * from table")]
        [InlineData("insertt into TestTable1(ColKey, ColVal) values (5, 'E')")]
        public void Insert_throw_Test(string sql)
        {
            var exception = Assert.Throws<DatabaseStatementException>(() => DatabaseAccessor.Insert(sql));
            Assert.Equal("insert", exception.Message);
        }
#endif

        [Theory]
        [InlineData(1, "insert into TestTable1(ColKey, ColVal) values (5, 'E')")]
        [InlineData(2, "INSERT into TestTable1(ColKey, ColVal) values (5, 'E'),(6, 'F')")]
        [InlineData(0, @"
            insert
            into
            TestTable1
            (
            ColKey,
            ColVal
            )
            select
            *
            from
            TestTable1
            where
            ColKey < 0
        ")]
        public async Task InsertAsyncTest(int expected, string sql)
        {
            var actual = await DatabaseAccessor.InsertAsync(sql);
            Assert.Equal(expected, actual);
        }

#if DEBUG
        [Theory]
        [InlineData("")]
        [InlineData("select * from table")]
        [InlineData("insertt into TestTable1(ColKey, ColVal) values (5, 'E')")]
        public async Task InsertAsync_throw_Test(string sql)
        {
            var exception = await Assert.ThrowsAsync<DatabaseStatementException>(() => DatabaseAccessor.InsertAsync(sql));
            Assert.Equal("insert", exception.Message);
        }
#endif

        [Fact]
        public void InsertSingleTest()
        {
            var exception = Record.Exception(() => DatabaseAccessor.InsertSingle("insert into TestTable1(ColKey, ColVal) values (5, 'E')"));
            Assert.Null(exception);
        }

#if DEBUG
        [Fact]
        public void InsertSingle_throw_Test()
        {
            Assert.Throws<DatabaseStatementException>(() => DatabaseAccessor.InsertSingle("iinsert into TestTable1(ColKey, ColVal) values (5, 'E')"));
        }
#endif

        [Fact]
        public void InsertSingle_throw_0_Test()
        {
            var exception = Assert.Throws<DatabaseManipulationException>(() => DatabaseAccessor.InsertSingle("insert into TestTable1(ColKey, ColVal) select * from TestTable1 where ColKey < 0"));
            Assert.Equal("insert -> 0", exception.Message);
        }

        [Fact]
        public void InsertSingle_throw_2_Test()
        {
            var exception = Assert.Throws<DatabaseManipulationException>(() => DatabaseAccessor.InsertSingle("insert into TestTable1(ColKey, ColVal) values (5, 'E'),(6, 'F')"));
            Assert.Equal("insert -> 2", exception.Message);
        }


        [Fact]
        public async Task InsertSingleAsyncTest()
        {
            var exception = await Record.ExceptionAsync(() => DatabaseAccessor.InsertSingleAsync("insert into TestTable1(ColKey, ColVal) values (5, 'E')"));
            Assert.Null(exception);
        }

#if DEBUG
        [Fact]
        public async Task InsertSingleAsync_throw_Test()
        {
            await Assert.ThrowsAsync<DatabaseStatementException>(() => DatabaseAccessor.InsertSingleAsync("iinsert into TestTable1(ColKey, ColVal) values (5, 'E')"));
        }
#endif

        [Fact]
        public async Task InsertSingleAsync_throw_0_Test()
        {
            var exception = await Assert.ThrowsAsync<DatabaseManipulationException>(() => DatabaseAccessor.InsertSingleAsync("insert into TestTable1(ColKey, ColVal) select * from TestTable1 where ColKey < 0"));
            Assert.Equal("insert -> 0", exception.Message);
        }

        [Fact]
        public async Task InsertSingleAsync_throw_2_Test()
        {
            var exception = await Assert.ThrowsAsync<DatabaseManipulationException>(() => DatabaseAccessor.InsertSingleAsync("insert into TestTable1(ColKey, ColVal) values (5, 'E'),(6, 'F')"));
            Assert.Equal("insert -> 2", exception.Message);
        }

        [Theory]
        [InlineData(1, "delete from TestTable1 where ColKey = 1")]
        [InlineData(2, "DELETE from TestTable1 where ColKey < 3")]
        [InlineData(0, @"
            delete
            from
            TestTable1
            where
            ColKey < 0
        ")]
        public void DeleteTest(int expected, string sql)
        {
            var actual = DatabaseAccessor.Delete(sql);
            Assert.Equal(expected, actual);
        }

#if DEBUG
        [Theory]
        [InlineData("")]
        [InlineData("select * from table")]
        [InlineData("deletee from TestTable1 where ColKey = 1")]
        public void Delete_throw_Test(string sql)
        {
            Assert.Throws<DatabaseStatementException>(() => DatabaseAccessor.Delete(sql));
        }
#endif

        [Theory]
        [InlineData(1, "delete from TestTable1 where ColKey = 1")]
        [InlineData(2, "DELETE from TestTable1 where ColKey < 3")]
        [InlineData(0, @"
            delete
            from
            TestTable1
            where
            ColKey < 0
        ")]
        public async Task DeleteAsyncTest(int expected, string sql)
        {
            var actual = await DatabaseAccessor.DeleteAsync(sql);
            Assert.Equal(expected, actual);
        }

#if DEBUG
        [Theory]
        [InlineData("")]
        [InlineData("select * from table")]
        [InlineData("deletee from TestTable1 where ColKey = 1")]
        public async Task DeleteAsync_throw_Test(string sql)
        {
            await Assert.ThrowsAsync<DatabaseStatementException>(() => DatabaseAccessor.DeleteAsync(sql));
        }
#endif

        [Fact]
        public void DeleteByKeyTest()
        {
            var exception = Record.Exception(() => DatabaseAccessor.DeleteByKey("delete from TestTable1 where ColKey = 1"));
            Assert.Null(exception);
        }

#if DEBUG
        [Fact]
        public void DeleteByKey_throw_Test()
        {
            var exception = Assert.Throws<DatabaseStatementException>(() => DatabaseAccessor.DeleteByKey("deletee from TestTable1 where ColKey = 1"));
            Assert.Equal("delete", exception.Message);
        }
#endif

        [Fact]
        public void DeleteByKey_throw_0_Test()
        {
            var exception = Assert.Throws<DatabaseManipulationException>(() => DatabaseAccessor.DeleteByKey("delete from TestTable1 where ColKey = 0"));
            Assert.Equal("delete -> 0", exception.Message);
        }

        [Fact]
        public void DeleteByKey_throw_2_Test()
        {
            var exception = Assert.Throws<DatabaseManipulationException>(() => DatabaseAccessor.DeleteByKey("delete from TestTable1 where ColKey < 3"));
            Assert.Equal("delete -> 2", exception.Message);
        }

        [Fact]
        public async Task DeleteByKeyAsyncTest()
        {
            var exception = await Record.ExceptionAsync(() => DatabaseAccessor.DeleteByKeyAsync("delete from TestTable1 where ColKey = 1"));
            Assert.Null(exception);
        }

#if DEBUG
        [Fact]
        public async Task DeleteByKeyAsync_throw_Test()
        {
            var exception = await Assert.ThrowsAsync<DatabaseStatementException>(() => DatabaseAccessor.DeleteByKeyAsync("deletee from TestTable1 where ColKey = 1"));
            Assert.Equal("delete", exception.Message);
        }
#endif

        [Fact]
        public async Task DeleteByKeyAsync_throw_0_Test()
        {
            var exception = await Assert.ThrowsAsync<DatabaseManipulationException>(() => DatabaseAccessor.DeleteByKeyAsync("delete from TestTable1 where ColKey = 0"));
            Assert.Equal("delete -> 0", exception.Message);
        }

        [Fact]
        public async Task DeleteByKeyAsync_throw_2_Test()
        {
            var exception = await Assert.ThrowsAsync<DatabaseManipulationException>(() => DatabaseAccessor.DeleteByKeyAsync("delete from TestTable1 where ColKey < 3"));
            Assert.Equal("delete -> 2", exception.Message);
        }

        [Fact]
        public void DeleteByKeyOrNothing_update_Test()
        {
            var actual = DatabaseAccessor.DeleteByKeyOrNothing("delete from TestTable1 where ColKey = 1");
            Assert.True(actual);
        }

        [Fact]
        public void DeleteByKeyOrNothing_nothing_Test()
        {
            var actual = DatabaseAccessor.DeleteByKeyOrNothing("delete from TestTable1 where ColKey = 0");
            Assert.False(actual);
        }

#if DEBUG
        [Fact]
        public void DeleteByKeyOrNothing_throw_Test()
        {
            var exception = Assert.Throws<DatabaseStatementException>(() => DatabaseAccessor.DeleteByKeyOrNothing("deletee from TestTable1 where ColKey = 1"));
            Assert.Equal("delete", exception.Message);
        }
#endif

        [Fact]
        public void DeleteByKeyOrNothing_2_Test()
        {
            var exception = Assert.Throws<DatabaseManipulationException>(() => DatabaseAccessor.DeleteByKeyOrNothing("delete from TestTable1 where ColKey < 3"));
            Assert.Equal("delete -> 2", exception.Message);
        }

        [Fact]
        public async Task DeleteByKeyOrNothingAsync_update_Test()
        {
            var actual = await DatabaseAccessor.DeleteByKeyOrNothingAsync("delete from TestTable1 where ColKey = 1");
            Assert.True(actual);
        }

        [Fact]
        public async Task DeleteByKeyOrNothingAsync_nothing_Test()
        {
            var actual = await DatabaseAccessor.DeleteByKeyOrNothingAsync("delete from TestTable1 where ColKey = 0");
            Assert.False(actual);
        }

#if DEBUG
        [Fact]
        public async Task DeleteByKeyOrNothingAsync_throw_Test()
        {
            var exception = await Assert.ThrowsAsync<DatabaseStatementException>(() => DatabaseAccessor.DeleteByKeyOrNothingAsync("deletee from TestTable1 where ColKey = 1"));
            Assert.Equal("delete", exception.Message);
        }
#endif
        [Fact]
        public async Task DeleteByKeyOrNothingAsync_2_Test()
        {
            var exception = await Assert.ThrowsAsync<DatabaseManipulationException>(() => DatabaseAccessor.DeleteByKeyOrNothingAsync("delete from TestTable1 where ColKey < 3"));
            Assert.Equal("delete -> 2", exception.Message);
        }
        #endregion
    }
}
