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
    public class DatabaseContextsTest
    {
        #region function

        [Fact]
        public void Test()
        {
            var factory = new InMemorySqliteFactory();
            var databaseAccessor = new SqliteAccessor(factory, NullLoggerFactory.Instance);
            var databaseImplementation = databaseAccessor.DatabaseFactory.CreateImplementation();

            var contexts = new DatabaseContexts(databaseAccessor, databaseImplementation);
            Assert.Equal(databaseAccessor, contexts.Context);
            Assert.Equal(databaseImplementation, contexts.Implementation);
        }

        #endregion
    }
}
