using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Database.Test.Vender.Public.SQLite
{
    public class IDatabaseBarrierExtensionsTest
    {
        public IDatabaseBarrierExtensionsTest()
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
        public void ReadDataTest()
        {
            using var readerWriterLocker = new ReadWriteLockHelper();
            var test = new DatabaseBarrier(DatabaseAccessor, readerWriterLocker);
            var actual = test.ReadData(transaction => {
                return transaction.QueryFirst<string>("select ColVal from TestTable1 where ColKey = 4");
            });
            Assert.Equal("D", actual);
        }

        [Fact]
        public void ReadData_param_Test()
        {
            using var readerWriterLocker = new ReadWriteLockHelper();
            var test = new DatabaseBarrier(DatabaseAccessor, readerWriterLocker);
            var actual = test.ReadData((transaction, arg) => {
                return transaction.QueryFirst<string>("select ColVal from TestTable1 where ColKey = :ColKey", arg);
            }, new { ColKey = 4 });
            Assert.Equal("D", actual);
        }

        #endregion
    }
}
