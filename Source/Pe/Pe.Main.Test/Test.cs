using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using ContentTypeTextNet.Pe.Bridge.Models;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;

namespace ContentTypeTextNet.Pe.Main.Test
{
    [Flags]
    public enum TestSetup
    {
        None = 0b0000,
        Http = 0b0001,
        Database = 0b0010,
    }

    public enum AccessorKind
    {
        Main,
        Large,
        Temporary,
    }

    public class Test
    {
        #region define

        public sealed class NotFoundApplicationConfigurationException: Exception
        {
            public NotFoundApplicationConfigurationException(string message) : base(message) { }
        }

        #endregion

        public Test(IDiContainer diContainer, IUserAgentManager userAgentManager, Mock<IHttpUserAgent> mockHttpUserAgent, ILoggerFactory loggerFactory)
        {
            DiContainer = diContainer;
            LoggerFactory = loggerFactory;
            UserAgentManager = userAgentManager;
            MockHttpUserAgent = mockHttpUserAgent;
        }

        #region property

        /// <summary>
        /// テスト用ロガー。
        /// </summary>
        public ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// テスト用DIコンテナ。
        /// </summary>
        public IDiContainer DiContainer { get; }

        public Mock<IHttpUserAgent> MockHttpUserAgent { get; }
        public IUserAgentManager UserAgentManager { get; }

        #endregion

        #region function

        public static DiContainer CreateDiContainer(ILoggerFactory loggerFactory)
        {
            var diContainer = new ApplicationDiContainer();
            diContainer
                .Register<ILoggerFactory, ILoggerFactory>(loggerFactory)
                .Register<IIdFactory, IdFactory>(DiLifecycle.Transient)
            ;

            return diContainer;
        }

        public static void SetupHttpAgent(IHttpUserAgent httpUserAgent)
        {
            var mockHttpUserAgent = new Mock<IHttpUserAgent>();
            var mockUserAgentManager = new Mock<IUserAgentManager>();
            mockUserAgentManager
                .Setup(a => a.CreateUserAgent())
                .Returns(mockHttpUserAgent.Object)
            ;
        }

        public static void SetupDatabase(IDiRegisterContainer diContainer)
        {
            var factoryPack = new ApplicationDatabaseFactoryPack(
                new ApplicationDatabaseFactory(true, false),
                new ApplicationDatabaseFactory(true, false),
                new ApplicationDatabaseFactory(true, false)
            );
            var delayWriterWaitTimePack = new DelayWriterWaitTimePack(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));

            var testAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var sqlRootDirPath = Path.Combine(Path.GetDirectoryName(testAssemblyPath)!, "etc", "sql", "ContentTypeTextNet.Pe.Main");

            diContainer
                .Register<IDatabaseStatementLoader, ApplicationDatabaseStatementLoader>(new ApplicationDatabaseStatementLoader(new DirectoryInfo(sqlRootDirPath), TimeSpan.FromMinutes(6), null, true, diContainer.Build<LoggerFactory>()))
                .Register<IDatabaseCommonStatus>(DatabaseCommonStatus.CreateCurrentAccount())
                .RegisterDatabase(factoryPack, delayWriterWaitTimePack, diContainer.Build<ILoggerFactory>())
            ;

            var databaseAccessorPack = diContainer.Build<IDatabaseAccessorPack>();

            var databaseSetupper = diContainer.Build<DatabaseSetupper>();
            databaseSetupper.Initialize(databaseAccessorPack);
            var initVersion = databaseSetupper.GetLastVersion(databaseAccessorPack.Main);
            foreach(var accessor in databaseAccessorPack.Items) {
                accessor.Execute("pragma foreign_keys = false");
            }
            databaseSetupper.Migrate(databaseAccessorPack, initVersion!);
            databaseSetupper.Adjust(databaseAccessorPack, initVersion!);
            foreach(var accessor in databaseAccessorPack.Items) {
                databaseSetupper.CheckForeignKey(accessor);
                accessor.Execute("pragma foreign_keys = true");
            }
            var lastVersion = databaseSetupper.GetLastVersion(databaseAccessorPack.Main);
            Assert.NotNull(lastVersion);

            //return new ApplicationDatabaseStatementLoader(environmentParameters.MainSqlDirectory, TimeSpan.Zero, statementAccessor, environmentParameters.ApplicationConfiguration.File.GivePriorityToFile, loggerFactory);

            //var db = new DataSet();
            //using(var c = mainDatabaseBarrier.WaitRead()) {
            //    var keys=c.Query<string>("select name from sqlite_master where type='table'");
            //    foreach(var key in keys) {
            //        var table = c.GetDataTable("select * from " + key);
            //        db.Tables.Add(table);
            //    }
            //}

            // 3桁バージョンってこうなってんのかよ
            var versionElements = new[] {
                new { Now = BuildStatus.Version.Major, Init = lastVersion.Major},
                new { Now = BuildStatus.Version.Minor, Init = lastVersion.Minor},
                new { Now = BuildStatus.Version.Build, Init = lastVersion.Build},
            };
            Assert.True(versionElements.All(i => i.Now == i.Init));

            var databaseAdjuster = diContainer.Build<DatabaseAdjuster>();
            databaseAdjuster.Adjust();
        }

        public static Test Create(TestSetup setups = TestSetup.None)
        {
            var loggerFactory = new LoggerFactory();
            var diContainer = CreateDiContainer(loggerFactory);

            if(setups.HasFlag(TestSetup.Database)) {
                SetupDatabase(diContainer);
            }

            var mockHttpUserAgent = new Mock<IHttpUserAgent>();
            IUserAgentManager userAgentManager;
            if(setups.HasFlag(TestSetup.Http)) {
                SetupHttpAgent(mockHttpUserAgent.Object);

                var mockUserAgentManager = new Mock<IUserAgentManager>();
                mockUserAgentManager
                    .Setup(a => a.CreateUserAgent())
                    .Returns(mockHttpUserAgent.Object)
                ;

                diContainer.Register<IHttpUserAgentFactory>(mockUserAgentManager.Object);
                diContainer.Register<IApplicationHttpUserAgentFactory>(mockUserAgentManager.Object);
                diContainer.Register<IUserAgentManager>(mockUserAgentManager.Object);
                userAgentManager = mockUserAgentManager.Object;
            } else {
                var mockUserAgentManager = new Mock<IUserAgentManager>();
                userAgentManager = mockUserAgentManager.Object;
            }

            return new Test(diContainer, userAgentManager, mockHttpUserAgent, loggerFactory);
        }

        public static ApplicationConfiguration GetApplicationConfiguration(object obj, [CallerMemberName] string configurationBaseName = "")
        {
            var projectTestPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var fullName = obj.GetType().FullName ?? throw new Exception(obj.ToString());
            var subName = fullName.Substring("ContentTypeTextNet.Pe.Main.Test.".Length);
            var configurationPath = Path.Combine(projectTestPath, subName.Replace('.', Path.DirectorySeparatorChar) + ".config", $"{configurationBaseName}.json");

            if(!File.Exists(configurationPath)) {
                throw new NotFoundApplicationConfigurationException(configurationPath);
            }

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(configurationPath);
            var configurationRoot = configurationBuilder.Build();
            return new ApplicationConfiguration(configurationRoot);
        }

        public TDao BuildDao<TDao>(AccessorKind kind)
            where TDao : DatabaseAccessObjectBase
        {
            IApplicationDatabaseAccessor databaseAccessor = kind switch {
                AccessorKind.Main => DiContainer.New<IMainDatabaseAccessor>(),
                AccessorKind.Large => DiContainer.New<ILargeDatabaseAccessor>(),
                AccessorKind.Temporary => DiContainer.New<ITemporaryDatabaseAccessor>(),
                _ => throw new NotImplementedException(),
            };
            var result = DiContainer.Build<TDao>(databaseAccessor, databaseAccessor.DatabaseFactory.CreateImplementation());
            return result;
        }

        #endregion
    }
}
