using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging.Abstractions;
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

    public class SqliteBooleanHandlerTest
    {
        #region function

        [Theory]
        [InlineData(true, "1", true)]
        [InlineData(false, "0", false)]
        public void Test(bool expectedValue, string expectedRaw, bool input)
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            accessor.Execute("create table TestTable(Id integer, Value boolean)");
            accessor.Execute("insert into TestTable(Id, Value) values(1, @Value)", new { Value = input });

            var actualRaw = accessor.QueryFirst<string>("select cast(Value as text) from TestTable where Id = 1");
            Assert.Equal(expectedRaw, actualRaw);

            var actualValue = accessor.QueryFirst<bool>("select Value from TestTable where Id = 1");
            Assert.Equal(expectedValue, actualValue);
        }

        #endregion
    }

    public class SqliteVersionHandlerTest
    {
        #region function

        public static TheoryData<Version, string, Version> Data => new() {
            {
                new Version(1,2,3),
                "1.2.3",
                new Version(1,2,3,4)
            }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void Test(Version expectedValue, string expectedRaw, Version input)
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            accessor.Execute("create table TestTable(Id integer, Value text)");
            accessor.Execute("insert into TestTable(Id, Value) values(1, @Value)", new { Value = input });

            var actualRaw = accessor.QueryFirst<string>("select cast(Value as text) from TestTable where Id = 1");
            Assert.Equal(expectedRaw, actualRaw);

            var actualValue = accessor.QueryFirst<Version>("select Value from TestTable where Id = 1");
            Assert.Equal(expectedValue, actualValue);
        }

        #endregion
    }

    public class SqliteGuidHandlerTest
    {
        #region function

        public static IEnumerable<object[]> Data()
        {
            return new[] {
                Guid.Empty,
            }.Select(a => new object[] { a, a.ToString("D"), a });
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Test(Guid expectedValue, string expectedRaw, Guid input)
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            accessor.Execute("create table TestTable(Id integer, Value text)");
            accessor.Execute("insert into TestTable(Id, Value) values(1, @Value)", new { Value = input });

            var actualRaw = accessor.QueryFirst<string>("select cast(Value as text) from TestTable where Id = 1");
            Assert.Equal(expectedRaw, actualRaw);

            var actualValue = accessor.QueryFirst<Guid>("select Value from TestTable where Id = 1");
            Assert.Equal(expectedValue, actualValue);
        }

        #endregion
    }

    public class SqliteTimeSpanHandlerTest
    {
        #region function

        public static IEnumerable<object[]> Data()
        {
            return new[] {
                TimeSpan.Zero,
            }.Select(a => new object[] { a, a.ToString("c"), a });
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Test(TimeSpan expectedValue, string expectedRaw, TimeSpan input)
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            accessor.Execute("create table TestTable(Id integer, Value text)");
            accessor.Execute("insert into TestTable(Id, Value) values(1, @Value)", new { Value = input });

            var actualRaw = accessor.QueryFirst<string>("select cast(Value as text) from TestTable where Id = 1");
            Assert.Equal(expectedRaw, actualRaw);

            var actualValue = accessor.QueryFirst<TimeSpan>("select Value from TestTable where Id = 1");
            Assert.Equal(expectedValue, actualValue);
        }

        #endregion
    }
}
