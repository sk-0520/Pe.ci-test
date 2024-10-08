using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.CommonTest;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database
{
    public class PersistenceHelperTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void WaitWritePack_rollback_Test()
        {
            var creator = () => PersistenceHelper.WaitWritePack(
                Test.DiContainer.New<IMainDatabaseBarrier>(),
                Test.DiContainer.New<ILargeDatabaseBarrier>(),
                Test.DiContainer.New<ITemporaryDatabaseBarrier>(),
                DatabaseCommonStatus.CreateCurrentAccount()
            );
            using(var test = creator()) {
                Debug.Assert(test.Items.All(a => a.Implementation.SupportedTransactionDDL));
            }

            using(var test = creator()) {
                Assert.Equal(0, test.Main.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'M'"));
                Assert.Equal(0, test.Large.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'L'"));
                Assert.Equal(0, test.Temporary.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'T'"));
            }

            using(var test = creator()) {
                test.Main.Context.Execute("create table M(id integer, value text)");
                test.Large.Context.Execute("create table L(id integer, value text)");
                test.Temporary.Context.Execute("create table T(id integer, value text)");

                Assert.Equal(1, test.Main.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'M'"));
                Assert.Equal(1, test.Large.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'L'"));
                Assert.Equal(1, test.Temporary.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'T'"));

                test.Rollback();
            }

            using(var test = creator()) {
                Assert.Equal(0, test.Main.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'M'"));
                Assert.Equal(0, test.Large.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'L'"));
                Assert.Equal(0, test.Temporary.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'T'"));
            }
        }

        [Fact]
        public void WaitWritePack_using_Test()
        {
            var creator = () => PersistenceHelper.WaitWritePack(
                Test.DiContainer.New<IMainDatabaseBarrier>(),
                Test.DiContainer.New<ILargeDatabaseBarrier>(),
                Test.DiContainer.New<ITemporaryDatabaseBarrier>(),
                DatabaseCommonStatus.CreateCurrentAccount()
            );
            using(var test = creator()) {
                Debug.Assert(test.Items.All(a => a.Implementation.SupportedTransactionDDL));
            }

            using(var test = creator()) {
                Assert.Equal(0, test.Main.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'M'"));
                Assert.Equal(0, test.Large.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'L'"));
                Assert.Equal(0, test.Temporary.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'T'"));
            }

            using(var test = creator()) {
                test.Main.Context.Execute("create table M(id integer, value text)");
                test.Large.Context.Execute("create table L(id integer, value text)");
                test.Temporary.Context.Execute("create table T(id integer, value text)");

                Assert.Equal(1, test.Main.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'M'"));
                Assert.Equal(1, test.Large.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'L'"));
                Assert.Equal(1, test.Temporary.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'T'"));
            }

            using(var test = creator()) {
                Assert.Equal(0, test.Main.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'M'"));
                Assert.Equal(0, test.Large.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'L'"));
                Assert.Equal(0, test.Temporary.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'T'"));
            }
        }

        [Fact]
        public void WaitWritePack_commit_Test()
        {
            var creator = () => PersistenceHelper.WaitWritePack(
                Test.DiContainer.New<IMainDatabaseBarrier>(),
                Test.DiContainer.New<ILargeDatabaseBarrier>(),
                Test.DiContainer.New<ITemporaryDatabaseBarrier>(),
                DatabaseCommonStatus.CreateCurrentAccount()
            );
            using(var test = creator()) {
                Debug.Assert(test.Items.All(a => a.Implementation.SupportedTransactionDDL));
            }

            using(var test = creator()) {
                Assert.Equal(0, test.Main.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'M'"));
                Assert.Equal(0, test.Large.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'L'"));
                Assert.Equal(0, test.Temporary.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'T'"));
            }

            using(var test = creator()) {
                test.Main.Context.Execute("create table M(id integer, value text)");
                test.Large.Context.Execute("create table L(id integer, value text)");
                test.Temporary.Context.Execute("create table T(id integer, value text)");

                Assert.Equal(1, test.Main.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'M'"));
                Assert.Equal(1, test.Large.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'L'"));
                Assert.Equal(1, test.Temporary.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'T'"));

                test.Commit();
            }

            using(var test = creator()) {
                Assert.Equal(1, test.Main.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'M'"));
                Assert.Equal(1, test.Large.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'L'"));
                Assert.Equal(1, test.Temporary.Context.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'T'"));
            }
        }

        [Fact]
        public void WaitReadPackTest()
        {
            var creator = () => PersistenceHelper.WaitReadPack(
                Test.DiContainer.New<IMainDatabaseBarrier>(),
                Test.DiContainer.New<ILargeDatabaseBarrier>(),
                Test.DiContainer.New<ITemporaryDatabaseBarrier>(),
                DatabaseCommonStatus.CreateCurrentAccount()
            );
            using(var test = creator()) {
                Debug.Assert(test.Items.All(a => a.Implementation.SupportedTransactionDDL));
            }

            using(var test = creator()) {
                Assert.Throws<NotSupportedException>(() => test.Main.Context.Execute("create table M(id integer, value text)"));
                Assert.Throws<NotSupportedException>(() => test.Large.Context.Execute("create table L(id integer, value text)"));
                Assert.Throws<NotSupportedException>(() => test.Temporary.Context.Execute("create table T(id integer, value text)"));

                Assert.Throws<NotSupportedException>(() => test.Commit());
                var exception = Record.Exception(() => test.Rollback());
                Assert.Null(exception);
            }
        }

        [Fact]
        public void CopyTest()
        {
            var dir = TestIO.InitializeMethod(this);
            var srcPath = Path.Combine(dir.FullName, "src.sqlite3");
            var dstPath = Path.Combine(dir.FullName, "dst.sqlite3");

            using(var src = new SqliteAccessor(
                    new ApplicationDatabaseFactory(new FileInfo(srcPath), true, false),
                    NullLoggerFactory.Instance
            )) {
                src.Execute("create table MAIN1(id integer, value text)");
                Assert.Equal(1, src.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'MAIN1'"));
                Assert.Equal(0, src.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'MAIN2'"));

                using var dst = new SqliteAccessor(
                    new ApplicationDatabaseFactory(new FileInfo(dstPath), true, false),
                    NullLoggerFactory.Instance
                );
                dst.Execute("create table MAIN2(id integer, value text)");
                Assert.Equal(0, dst.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'MAIN1'"));
                Assert.Equal(1, dst.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'MAIN2'"));

                PersistenceHelper.Copy(src, dst);
            }

            using(var src = new SqliteAccessor(
                new ApplicationDatabaseFactory(new FileInfo(srcPath), true, false),
                NullLoggerFactory.Instance
            )) {
                using var dst = new SqliteAccessor(
                    new ApplicationDatabaseFactory(new FileInfo(dstPath), true, false),
                    NullLoggerFactory.Instance
                );

                Assert.Equal(1, src.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'MAIN1'"));
                Assert.Equal(0, src.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'MAIN2'"));

                Assert.Equal(1, dst.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'MAIN1'"));
                Assert.Equal(0, dst.SelectSingleCount("select count(*) from sqlite_master where type = 'table' and name = 'MAIN2'"));
            }
        }

        [Fact]
        public void Copy_throw_arg1_Test()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PersistenceHelper.Copy(null!, new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance)));
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void Copy_throw_arg2_Test()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PersistenceHelper.Copy(new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance), null!));
            Assert.Equal("destination", exception.ParamName);
        }

        [Fact]
        public void Copy_throw_arg1_eq_2_Test()
        {
            var accessor = new SqliteAccessor(new InMemorySqliteFactory(), NullLoggerFactory.Instance);
            var exception = Assert.Throws<ArgumentException>(() => PersistenceHelper.Copy(accessor, accessor));
            Assert.Equal("source == destination", exception.Message);
        }

        #endregion
    }
}
