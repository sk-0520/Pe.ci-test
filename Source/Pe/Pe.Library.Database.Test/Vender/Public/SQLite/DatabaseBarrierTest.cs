using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Database.Test.Vender.Public.SQLite
{
    public class DatabaseBarrierTest
    {
        public DatabaseBarrierTest()
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
        public void WaitWrite_default_Test()
        {
            using var readerWriterLocker = new ReadWriteLockHelper();
            var test = new DatabaseBarrier(DatabaseAccessor, readerWriterLocker);
            using(var transaction = test.WaitWrite()) {
                transaction.Insert("insert into TestTable1(ColKey, ColVal) values (5, 'E')");
                var value = transaction.QueryFirst<string>("select ColVal from TestTable1 where ColKey = 5");
                Assert.Equal("E", value);
            }
            Assert.Null(DatabaseAccessor.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = 5"));
        }

        [Fact]
        public void WaitWrite_timeout_Test()
        {
            using var readerWriterLocker = new ReadWriteLockHelper();
            var test = new DatabaseBarrier(DatabaseAccessor, readerWriterLocker);
            using(var transaction = test.WaitWrite(Timeout.InfiniteTimeSpan)) {
                transaction.Insert("insert into TestTable1(ColKey, ColVal) values (5, 'E')");
                var value = transaction.QueryFirst<string>("select ColVal from TestTable1 where ColKey = 5");
                Assert.Equal("E", value);
            }
            Assert.Null(DatabaseAccessor.QueryFirstOrDefault<string>("select ColVal from TestTable1 where ColKey = 5"));
        }

        [Fact]
        public void WaitRead_default_Test()
        {
            using var readerWriterLocker = new ReadWriteLockHelper();
            var test = new DatabaseBarrier(DatabaseAccessor, readerWriterLocker);
            using var transaction = test.WaitRead();
            var value = transaction.QueryFirst<string>("select ColVal from TestTable1 where ColKey = 4");
            Assert.Equal("D", value);
        }

        [Fact]
        public void WaitRead_timeout_Test()
        {
            using var readerWriterLocker = new ReadWriteLockHelper();
            var test = new DatabaseBarrier(DatabaseAccessor, readerWriterLocker);
            using var transaction = test.WaitRead(Timeout.InfiniteTimeSpan);
            var value = transaction.QueryFirst<string>("select ColVal from TestTable1 where ColKey = 4");
            Assert.Equal("D", value);
        }


        #endregion
    }
}
