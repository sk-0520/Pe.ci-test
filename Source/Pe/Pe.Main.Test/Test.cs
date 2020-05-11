using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test
{
    [TestClass]
    internal class Test
    {
        #region property

        public static ILoggerFactory LoggerFactory { get; set; } = new LoggerFactory();

        public static IDiContainer DiContainer { get; set; } = null!;

        #endregion

        #region function

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            var logger = LoggerFactory.CreateLogger(nameof(Test));
            logger.LogInformation("START Pe.Main.Test");

            var factoryPack = new ApplicationDatabaseFactoryPack(
                new ApplicationDatabaseFactory(true, false),
                new ApplicationDatabaseFactory(true, false),
                new ApplicationDatabaseFactory(true, false)
            );
            var lazyWriterWaitTimePack = new LazyWriterWaitTimePack(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));

            var testAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var sqlRootDirPath = Path.Combine(Path.GetDirectoryName(testAssemblyPath)!, "etc", "sql", "ContentTypeTextNet.Pe.Main");

            var diContainer = new ApplicationDiContainer();
            diContainer
                .Register<ILoggerFactory, ILoggerFactory>(LoggerFactory)
                .RegisterDatabase(factoryPack, lazyWriterWaitTimePack, LoggerFactory)
                .Register<IDatabaseStatementLoader, ApplicationDatabaseStatementLoader>(new ApplicationDatabaseStatementLoader(new DirectoryInfo(sqlRootDirPath), TimeSpan.FromMinutes(6), null, false, LoggerFactory))
                .Register<IIdFactory, IdFactory>(DiLifecycle.Transient)
            ;

            DiContainer = diContainer;

            var databaseAccessorPack = DiContainer.Build<IDatabaseAccessorPack>();

            var databaseSetupper = DiContainer.Build<DatabaseSetupper>();
            databaseSetupper.Initialize(databaseAccessorPack);
            var initVersion = databaseSetupper.GetLastVersion(databaseAccessorPack.Main);
            foreach(var accessor in databaseAccessorPack.Items) {
                accessor.Execute("pragma foreign_keys = false");
            }
            databaseSetupper.Migrate(databaseAccessorPack, initVersion!);
            foreach(var accessor in databaseAccessorPack.Items) {
                databaseSetupper.CheckForeignKey(accessor);
                accessor.Execute("pragma foreign_keys = true");
            }
            var lastVersion = databaseSetupper.GetLastVersion(databaseAccessorPack.Main);
            // 3桁バージョンってこうなってんのかよ
            Assert.AreEqual(BuildStatus.Version.Major, lastVersion!.Major);
            Assert.AreEqual(BuildStatus.Version.Minor, lastVersion!.Minor);
            Assert.AreEqual(BuildStatus.Version.Build, lastVersion!.Build);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            var logger = LoggerFactory.CreateLogger(nameof(Test));
            logger.LogInformation("END Pe.Main.Test");
        }
        #endregion
    }
}
