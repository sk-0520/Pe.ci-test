using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class EntityDaoBaseTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        private sealed class EntityNotDao: EntityDaoBase
        {
            public EntityNotDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
                : base(context, statementLoader, implementation, loggerFactory)
            { }
        }

        [Fact]
        public void TableName_Throw_Test()
        {
            var test = Test.BuildDao<EntityNotDao>(AccessorKind.Main);
            Assert.Throws<NotImplementedException>(() => test.TableName);
        }

        private sealed class EntityDao: EntityDaoBase
        {
            public EntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
                : base(context, statementLoader, implementation, loggerFactory)
            { }
        }

        [Fact]
        public void TableName_Empty_Test()
        {
            var test = Test.BuildDao<EntityDao>(AccessorKind.Main);
            Assert.Empty(test.TableName);
        }

        private sealed class TableNameEntityDao: EntityDaoBase
        {
            public TableNameEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
                : base(context, statementLoader, implementation, loggerFactory)
            { }
        }

        [Fact]
        public void TableNameTest()
        {
            var test = Test.BuildDao<TableNameEntityDao>(AccessorKind.Main);
            Assert.Equal("TableName", test.TableName);
        }

        #endregion
    }
}
