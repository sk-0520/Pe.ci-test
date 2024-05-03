using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using ContentTypeTextNet.Pe.Standard.Database;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Database.Vender.Public.SQLite
{
    public class SqliteImplementationTest
    {
        #region function

        [Fact]
        public void ToStatementColumnNameTest()
        {
            var test = new SqliteImplementation();
            var actual = test.ToStatementColumnName("column");
            Assert.Equal("[column]", actual);
        }

        [Fact]
        public void ToStatementTableNameTest()
        {
            var test = new SqliteImplementation();
            var actual = test.ToStatementTableName("table");
            Assert.Equal("[table]", actual);
        }

        [Theory]
        [InlineData("@param", "param", 0)]
        [InlineData("@param", "param", 1)]
        public void ToStatementParameterNameTest(string expected, string parameterName, int index)
        {
            var test = new SqliteImplementation();
            var actual = test.ToStatementParameterName(parameterName, index);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
