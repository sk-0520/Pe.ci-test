using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Database.Vender.Public.SQLite
{
    public class SqliteManagementTest
    {
        #region function

        [Theory]
        [InlineData(typeof(long), "integer")]
        [InlineData(typeof(long), "numeric")]
        [InlineData(typeof(double), "real")]
        [InlineData(typeof(string), "text")]
        [InlineData(typeof(byte[]), "blob")]
        public void ToDatabaseColumnType_Simple_Test(Type expected, string rawType)
        {
            var actual = SqliteManagement.ToDatabaseColumnType(rawType);
            Assert.Equal(expected, actual.CliType);
        }

        [Theory]
        [InlineData(typeof(long), 10, 5, "numeric(10,5)")]
        [InlineData(typeof(long), 10, 5, "numeric (10,5)")]
        [InlineData(typeof(long), 10, 5, "numeric ( 10,5)")]
        [InlineData(typeof(long), 10, 5, "numeric ( 10, 5)")]
        [InlineData(typeof(long), 10, 5, "numeric ( 10 , 5 )")]
        [InlineData(typeof(long), 10, 5, "numeric ( 10 , 5 ) ")]
        [InlineData(typeof(long), 10, 0, "numeric(10)")]
        [InlineData(typeof(long), 10, 0, "numeric (10)")]
        [InlineData(typeof(long), 10, 0, "numeric ( 10)")]
        [InlineData(typeof(long), 10, 0, "numeric ( 10 )")]
        [InlineData(typeof(long), 10, 0, "numeric ( 10 ) ")]
        // 分からん系
        [InlineData(typeof(void), 0, 0, "unknown(x)")]
        [InlineData(typeof(void), 0, 0, "unknown(x,y)")]
        // 状況不明系
        [InlineData(typeof(string), 2, 0, "TEXT(2)")]
        [InlineData(typeof(string), 2, 1, "TEXT(2,1)")]
        public void ToDatabaseColumnType_Range_Test(Type expectedType, int expectedPrecision, int expectedScale, string rawType)
        {
            var actual = SqliteManagement.ToDatabaseColumnType(rawType);
            Assert.Equal(expectedType, actual.CliType);
            Assert.Equal(expectedPrecision, actual.Precision);
            Assert.Equal(expectedScale, actual.Scale);
        }

        [Fact]
        public void GetDatabaseNamesTest()
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            var test = accessor.DatabaseFactory.CreateImplementation().CreateManagement(accessor);
            var actual = test.GetDatabaseItems();

            Assert.Single(actual);
            Assert.Contains(new DatabaseInformationItem("main"), actual);
        }

        [Fact]
        public void GetDatabaseNames_WithTemp_Test()
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            var test = accessor.DatabaseFactory.CreateImplementation().CreateManagement(accessor);
            accessor.Execute("create temporary table T(value text)");
            var actual = test.GetDatabaseItems();

            Assert.Equal(2, actual.Count);
            Assert.Contains(new DatabaseInformationItem("main"), actual);
            Assert.Contains(new DatabaseInformationItem("temp"), actual);
        }

        [Fact]
        public void GetSchemaItems_Single_Test()
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            var test = accessor.DatabaseFactory.CreateImplementation().CreateManagement(accessor);
            var databaseItems = test.GetDatabaseItems();

            Assert.Single(databaseItems);
            var databaseItem = databaseItems.ElementAt(0);
            var actual = test.GetSchemaItems(databaseItem);
            Assert.Single(actual);
            Assert.True(actual.ElementAt(0).IsDefault);
            Assert.Equal("main", actual.ElementAt(0).Name);
        }

        [Fact]
        public void GetSchemaItems_WithTemp_Test()
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            var test = accessor.DatabaseFactory.CreateImplementation().CreateManagement(accessor);
            accessor.Execute("create temporary table T(value text)");
            var databaseItems = test.GetDatabaseItems();

            Assert.Equal(2, databaseItems.Count);

            var databaseItem1 = databaseItems.First(a => a.Name == "main");
            var actual1 = test.GetSchemaItems(databaseItem1);
            Assert.Single(actual1);
            Assert.True(actual1.ElementAt(0).IsDefault);

            var databaseItem2 = databaseItems.First(a => a.Name == "temp");
            var actual2 = test.GetSchemaItems(databaseItem2);
            Assert.Single(actual2);
            Assert.False(actual2.ElementAt(0).IsDefault);
        }

        [Fact]
        public void GetResourceItems_Empty_Test()
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            var test = accessor.DatabaseFactory.CreateImplementation().CreateManagement(accessor);

            var databaseItems = test.GetDatabaseItems();
            Assert.Single(databaseItems);
            var databaseItem = databaseItems.ElementAt(0);
            var schemaItems = test.GetSchemaItems(databaseItem);
            Assert.Single(schemaItems);
            var schemaItem = schemaItems.ElementAt(0);

            var resourceItems = test.GetResourceItems(schemaItem, DatabaseResourceKinds.All);
            Assert.Empty(resourceItems);
        }

        [Fact]
        public void GetResourceItems_SingleTable_Test()
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            var sql = "create table T(value text)";
            accessor.Execute(sql);
            var test = accessor.DatabaseFactory.CreateImplementation().CreateManagement(accessor);

            var databaseItems = test.GetDatabaseItems();
            Assert.Single(databaseItems);
            var databaseItem = databaseItems.ElementAt(0);
            var schemaItems = test.GetSchemaItems(databaseItem);
            Assert.Single(schemaItems);
            var schemaItem = schemaItems.ElementAt(0);

            var resourceItems = test.GetResourceItems(schemaItem, DatabaseResourceKinds.All);
            Assert.Single(resourceItems);
            var actual = resourceItems.ElementAt(0);
            Assert.Equal("T", actual.Name);
            Assert.Equal(DatabaseResourceKinds.Table, actual.Kind);
            Assert.Equal(sql, actual.Source, true);

        }

        [Fact]
        public void GetResourceItems_Analyze_Test()
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            accessor.Execute("create table T(value text)");
            accessor.Execute("analyze\n");
            var test = accessor.DatabaseFactory.CreateImplementation().CreateManagement(accessor);

            var databaseItems = test.GetDatabaseItems();
            Assert.Single(databaseItems);
            var databaseItem = databaseItems.ElementAt(0);
            var schemaItems = test.GetSchemaItems(databaseItem);
            Assert.Single(schemaItems);
            var schemaItem = schemaItems.ElementAt(0);

            var resourceItems = test.GetResourceItems(schemaItem, DatabaseResourceKinds.All);
            Assert.Equal(3, resourceItems.Count);
            var actual_1 = resourceItems.First(a => a.Name == "T");
            Assert.Equal("T", actual_1.Name);
            Assert.Equal(DatabaseResourceKinds.Table, actual_1.Kind);

            var actual_2 = resourceItems.First(a => a.Name == "sqlite_stat1");
            Assert.Equal("sqlite_stat1", actual_2.Name);
            Assert.Equal(DatabaseResourceKinds.Table, actual_2.Kind);

            var actual_3 = resourceItems.First(a => a.Name == "sqlite_stat4");
            Assert.Equal("sqlite_stat4", actual_3.Name);
            Assert.Equal(DatabaseResourceKinds.Table, actual_3.Kind);
        }

        [Fact]
        public void GetResourceItems_View_Test()
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            accessor.Execute("create table T(value text)");
            accessor.Execute("create view V as select * from T");
            var test = accessor.DatabaseFactory.CreateImplementation().CreateManagement(accessor);

            var databaseItems = test.GetDatabaseItems();
            Assert.Single(databaseItems);
            var databaseItem = databaseItems.ElementAt(0);
            var schemaItems = test.GetSchemaItems(databaseItem);
            Assert.Single(schemaItems);
            var schemaItem = schemaItems.ElementAt(0);

            var resourceItems = test.GetResourceItems(schemaItem, DatabaseResourceKinds.All);
            Assert.Equal(2, resourceItems.Count);
            var actual_1 = resourceItems.First(a => a.Name == "T");
            Assert.Equal("T", actual_1.Name);
            Assert.Equal(DatabaseResourceKinds.Table, actual_1.Kind);

            var actual_2 = resourceItems.First(a => a.Name == "V");
            Assert.Equal("V", actual_2.Name);
            Assert.Equal(DatabaseResourceKinds.View, actual_2.Kind);
        }

        [Fact]
        public void GetResourceItems_Index_Test()
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            accessor.Execute("create table T(value text)");
            accessor.Execute("create index I on T(value)");
            var test = accessor.DatabaseFactory.CreateImplementation().CreateManagement(accessor);

            var databaseItems = test.GetDatabaseItems();
            Assert.Single(databaseItems);
            var databaseItem = databaseItems.ElementAt(0);
            var schemaItems = test.GetSchemaItems(databaseItem);
            Assert.Single(schemaItems);
            var schemaItem = schemaItems.ElementAt(0);

            var resourceItems = test.GetResourceItems(schemaItem, DatabaseResourceKinds.All);
            Assert.Equal(2, resourceItems.Count);
            var actual_1 = resourceItems.First(a => a.Name == "T");
            Assert.Equal("T", actual_1.Name);
            Assert.Equal(DatabaseResourceKinds.Table, actual_1.Kind);

            var actual_2 = resourceItems.First(a => a.Name == "I");
            Assert.Equal("I", actual_2.Name);
            Assert.Equal(DatabaseResourceKinds.Index, actual_2.Kind);
        }

        [Fact]
        public void GetColumns_Throw_Test()
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            accessor.Execute("create table T(t text primary key, i integer, r real, b blob, nn integer not null, dv text default 'TEXT', n numeric(10,3))");
            var test = accessor.DatabaseFactory.CreateImplementation().CreateManagement(accessor);

            var databaseItems = test.GetDatabaseItems();
            Assert.Single(databaseItems);
            var databaseItem = databaseItems.ElementAt(0);
            var schemaItems = test.GetSchemaItems(databaseItem);
            Assert.Single(schemaItems);
            var schemaItem = schemaItems.ElementAt(0);

            var resourceItems = test.GetResourceItems(schemaItem, DatabaseResourceKinds.Table);
            Assert.Single(resourceItems);
            var resourceItem = resourceItems.ElementAt(0);
            Assert.Equal("T", resourceItem.Name);
            Assert.Equal(DatabaseResourceKinds.Table, resourceItem.Kind);

            var invalidResource = new DatabaseResourceItem(
                resourceItem.Schema,
                resourceItem.Name,
                DatabaseResourceKinds.Index,
                resourceItem.Source
            );
            var exception = Assert.Throws<ArgumentException>(() => test.GetColumns(invalidResource));
            Assert.Equal("tableResource", exception.ParamName);
        }

        [Fact]
        public void GetColumnsTest()
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            accessor.Execute("create table T(t text primary key, i integer, r real, b blob, nn integer not null, dv text default 'TEXT', n numeric(10,3))");
            var test = accessor.DatabaseFactory.CreateImplementation().CreateManagement(accessor);

            var databaseItems = test.GetDatabaseItems();
            Assert.Single(databaseItems);
            var databaseItem = databaseItems.ElementAt(0);
            var schemaItems = test.GetSchemaItems(databaseItem);
            Assert.Single(schemaItems);
            var schemaItem = schemaItems.ElementAt(0);

            var resourceItems = test.GetResourceItems(schemaItem, DatabaseResourceKinds.Table);
            Assert.Single(resourceItems);
            var resourceItem = resourceItems.ElementAt(0);
            Assert.Equal("T", resourceItem.Name);
            Assert.Equal(DatabaseResourceKinds.Table, resourceItem.Kind);

            var actual = test.GetColumns(resourceItem);
            Assert.Equal(7, actual.Count);

            {
                var actualTarget = actual[0];
                Assert.True(actualTarget.IsPrimary);
                Assert.True(actualTarget.IsNullable);
                Assert.Empty(actualTarget.DefaultValue);
                Assert.Equal(typeof(string), actualTarget.Type.CliType);
                Assert.Equal("TEXT", actualTarget.Type.RawType);
            }
            {
                var actualTarget = actual[1];
                Assert.False(actualTarget.IsPrimary);
                Assert.True(actualTarget.IsNullable);
                Assert.Empty(actualTarget.DefaultValue);
                Assert.Equal(typeof(long), actualTarget.Type.CliType);
                Assert.Equal("INTEGER", actualTarget.Type.RawType);
            }
            {
                var actualTarget = actual[2];
                Assert.False(actualTarget.IsPrimary);
                Assert.True(actualTarget.IsNullable);
                Assert.Empty(actualTarget.DefaultValue);
                Assert.Equal(typeof(double), actualTarget.Type.CliType);
                Assert.Equal("REAL", actualTarget.Type.RawType);
            }
            {
                var actualTarget = actual[3];
                Assert.False(actualTarget.IsPrimary);
                Assert.True(actualTarget.IsNullable);
                Assert.Empty(actualTarget.DefaultValue);
                Assert.Equal(typeof(byte[]), actualTarget.Type.CliType);
                Assert.Equal("BLOB", actualTarget.Type.RawType);
            }
            {
                var actualTarget = actual[4];
                Assert.False(actualTarget.IsPrimary);
                Assert.False(actualTarget.IsNullable);
                Assert.Empty(actualTarget.DefaultValue);
                Assert.Equal(typeof(long), actualTarget.Type.CliType);
                Assert.Equal("INTEGER", actualTarget.Type.RawType);
            }
            {
                var actualTarget = actual[5];
                Assert.False(actualTarget.IsPrimary);
                Assert.True(actualTarget.IsNullable);
                Assert.Equal("'TEXT'", actualTarget.DefaultValue);
                Assert.Equal(typeof(string), actualTarget.Type.CliType);
                Assert.Equal("TEXT", actualTarget.Type.RawType);
            }
            {
                var actualTarget = actual[6];
                Assert.False(actualTarget.IsPrimary);
                Assert.True(actualTarget.IsNullable);
                Assert.Empty(actualTarget.DefaultValue);
                Assert.Equal(typeof(long), actualTarget.Type.CliType);
                Assert.Equal("NUMERIC(10,3)", actualTarget.Type.RawType);
            }
        }

        #endregion
    }
}
