using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Applications
{
    public class ApplicationDatabaseFactoryTest
    {
        #region function

        [Fact]
        public void Constructor_InMemory_Editable()
        {
            var test = new ApplicationDatabaseFactory(true, false);
            using var connection = test.CreateConnection();
            connection.Open();
            var exception = Record.Exception(() => connection.Execute("create table T (value text)"));
            Assert.Null(exception);
        }

        [Fact]
        public void Constructor_InMemory_Readonly()
        {
            var test = new ApplicationDatabaseFactory(true, true);
            using var connection = test.CreateConnection();
            connection.Open();
            var exception = Assert.Throws<SQLiteException>(() => connection.Execute("create table T (value text)"));
            Assert.Contains("readonly", exception.Message);
        }

        [Fact]
        public void Constructor_File_Editable()
        {
            var dir = TestIO.InitializeMethod(this);
            var file = new FileInfo(Path.Combine(dir.FullName, "test.sqlite3"));

            var test = new ApplicationDatabaseFactory(file, true, false);
            using var connection = test.CreateConnection();
            connection.Open();
            var exception = Record.Exception(() => connection.Execute("create table T (value text)"));
            Assert.Null(exception);
        }

        [Fact]
        public void Constructor_File_Readonly()
        {
            var dir = TestIO.InitializeMethod(this);
            var file = new FileInfo(Path.Combine(dir.FullName, "test.sqlite3"));

            // 読み込み専用で開くために作っておく
            {
                var setup = new ApplicationDatabaseFactory(file, true, false);
                using var setupConnection = setup.CreateConnection();
                setupConnection.Open();
            }

            var test = new ApplicationDatabaseFactory(file, true, true);
            using var connection = test.CreateConnection();
            connection.Open();
            var exception = Assert.Throws<SQLiteException>(() => connection.Execute("create table T (value text)"));
            Assert.Contains("readonly", exception.Message);
        }

        #endregion
    }

    public class ApplicationDatabaseAccessorTest()
    {
        #region function

        private (ApplicationDatabaseAccessor accessor, MockLog log) CreateAppDB(LogLevel logLevel)
        {
            var factory = new ApplicationDatabaseFactory(true, false);
            var mockLog = MockLog.Create(logLevel);
            var accessor = new ApplicationDatabaseAccessor(factory, mockLog.Factory.Object);
            return (accessor, mockLog);
        }

        [Fact]
        public void LoggingStatement_Disabled_Test()
        {
            var test = CreateAppDB(LogLevel.Debug);
            var actual = test.accessor.GetScalar<long>("select 1 + 2");
            test.log.VerifyLogNever();
            Assert.Equal(3, actual);
        }

        [Fact]
        public void LoggingStatement_Enabled_WithoutParameter_Test()
        {
            var test = CreateAppDB(LogLevel.Trace);
            var actual = test.accessor.GetScalar<long>("select 1 + 2");
            test.log.VerifyMessageContains(LogLevel.Trace, "[SQL]", Moq.Times.Once());
            test.log.VerifyMessageContains(LogLevel.Trace, "[PARAM]", Moq.Times.Never());
            Assert.Equal(3, actual);
        }

        [Fact]
        public void LoggingStatement_Enabled_WithParameter_Test()
        {
            var test = CreateAppDB(LogLevel.Trace);
            var actual = test.accessor.GetScalar<long>("select 1 + 2 + @Value", new { Value = 3 });
            test.log.VerifyMessageContains(LogLevel.Trace, "[SQL]", Moq.Times.Once());
            test.log.VerifyMessageContains(LogLevel.Trace, "[PARAM]", Moq.Times.Once());
            Assert.Equal(6, actual);
        }

        #endregion
    }
}
