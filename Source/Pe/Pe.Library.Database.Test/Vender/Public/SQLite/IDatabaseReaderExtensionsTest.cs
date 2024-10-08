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
    public class IDatabaseReaderExtensionsTest
    {
        public IDatabaseReaderExtensionsTest()
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
        [InlineData("select ColVal from TestTable1 order by ColKey")]
        [InlineData("select ColVal from TestTable1 /*order by ColKey*/")] // しゃあない
        [InlineData("select ColVal from TestTable1 OrDeR By ColKey")]
        [InlineData("select ColVal from TestTable1 order by ColKey desc")]
        [InlineData(@"
            select
            ColVal
            from
            TestTable1
            order
            by
            ColKey
        ")]
        public void SelectOrderedTest(string sql)
        {
            var exception1 = Record.Exception(() => DatabaseAccessor.SelectOrdered<string>(sql));
            Assert.Null(exception1);

            var exception2 = Record.Exception(() => DatabaseAccessor.SelectOrdered(sql));
            Assert.Null(exception2);
        }

#if DEBUG
        [Theory]
        [InlineData("")]
        [InlineData("select ColVal from TestTable1 order ColKey")]
        [InlineData("select ColVal from TestTable1 by ColKey")]
        [InlineData("select ColVal from TestTable1 order /**/ by ColKey")]
        public void SelectOrdered_throw_Test(string sql)
        {
            Assert.Throws<DatabaseStatementException>(() => DatabaseAccessor.SelectOrdered<string>(sql));
            Assert.Throws<DatabaseStatementException>(() => DatabaseAccessor.SelectOrdered(sql));
        }
#endif

        [Theory]
        [InlineData("select ColVal from TestTable1 order by ColKey")]
        [InlineData("select ColVal from TestTable1 /*order by ColKey*/")] // しゃあない
        [InlineData("select ColVal from TestTable1 OrDeR By ColKey")]
        [InlineData("select ColVal from TestTable1 order by ColKey desc")]
        [InlineData(@"
            select
            ColVal
            from
            TestTable1
            order
            by
            ColKey
        ")]
        public async Task SelectOrderedAsyncTest(string sql)
        {
            var exception1 = await Record.ExceptionAsync(() => DatabaseAccessor.SelectOrderedAsync<string>(sql));
            Assert.Null(exception1);

            var exception2 = await Record.ExceptionAsync(() => DatabaseAccessor.SelectOrderedAsync(sql));
            Assert.Null(exception2);
        }

#if DEBUG
        [Theory]
        [InlineData("")]
        [InlineData("select ColVal from TestTable1 order ColKey")]
        [InlineData("select ColVal from TestTable1 by ColKey")]
        [InlineData("select ColVal from TestTable1 order /**/ by ColKey")]
        public async Task SelectOrderedAsync_throw_Test(string sql)
        {
            await Assert.ThrowsAsync<DatabaseStatementException>(() => DatabaseAccessor.SelectOrderedAsync<string>(sql));
            await Assert.ThrowsAsync<DatabaseStatementException>(() => DatabaseAccessor.SelectOrderedAsync(sql));
        }
#endif

        [Theory]
        [InlineData("select count(*) from TestTable1")]
        [InlineData("select count(1) from TestTable1")]
        [InlineData("select * from (select count(*) from TestTable1)")] // しゃあない
        [InlineData(@"
            select
            count
            (
                *
            )
            from
            TestTable1
        ")]
        public void SelectSingleCountTest(string sql)
        {
            var exception = Record.Exception(() => DatabaseAccessor.SelectSingleCount(sql));
            Assert.Null(exception);
        }

#if DEBUG
        [Theory]
        [InlineData("")]
        [InlineData("select * from TestTable1")]
        [InlineData("select count from TestTable1")]
        public void SelectSingleCount_throw_Test(string sql)
        {
            Assert.Throws<DatabaseStatementException>(() => DatabaseAccessor.SelectSingleCount(sql));
        }
#endif

        [Theory]
        [InlineData("select count(*) from TestTable1")]
        [InlineData("select count(1) from TestTable1")]
        [InlineData("select * from (select count(*) from TestTable1)")] // しゃあない
        [InlineData(@"
            select
            count
            (
                *
            )
            from
            TestTable1
        ")]
        public async Task SelectSingleCountAsyncTest(string sql)
        {
            var exception = await Record.ExceptionAsync(() => DatabaseAccessor.SelectSingleCountAsync(sql));
            Assert.Null(exception);
        }

#if DEBUG
        [Theory]
        [InlineData("")]
        [InlineData("select * from TestTable1")]
        [InlineData("select count from TestTable1")]
        public async Task SelectSingleCountAsync_throw_Test(string sql)
        {
            await Assert.ThrowsAsync<DatabaseStatementException>(() => DatabaseAccessor.SelectSingleCountAsync(sql));
        }
#endif

        #endregion
    }
}
