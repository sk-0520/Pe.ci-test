using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class ApplicationInitializer
    {
        #region define

        const int AppLogLimit = 128;

        #endregion

        #region property

        string CommandLineKeyRunMode { get; } = "run-mode";
        string CommandLineKeyAppLogLimit { get; } = "app-log-limit";
        string CommandLineKeyLog { get; } = "log";
        string CommandLineKeyWithLog { get; } = "with-log";
        string CommandLineSwitchFullTraceLog { get; } = "full-trace-log";
        string CommandLineSwitchForceLog { get; } = "force-log";

        string CommandLineSwitchAcceptSkip { get; } = "skip-accept";
        string CommandLineSwitchBetaVersion { get; } = "beta-version";

        public bool IsFirstStartup { get; private set; }
        public RunMode RunMode { get; private set; }
        public ApplicationDiContainer? DiContainer { get; private set; }
        public ApplicationLogging? Logging { get; private set; }
        public WindowManager? WindowManager { get; private set; }
        //public OrderManager OrderManager { get; private set; }
        public NotifyManager? NotifyManager { get; private set; }
        public StatusManager? StatusManager { get; private set; }
        public ClipboardManager? ClipboardManager { get; private set; }
        public UserAgentManager? UserAgentManager { get; private set; }

        public Mutex? Mutex { get; private set; }

        #endregion

        #region function

        void InitializeEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("PE_DESKTOP", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
        }

        void InitializeClr()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        CommandLine CreateCommandLine(IEnumerable<string> arguments)
        {
            var commandLine = new CommandLine(arguments, false);

            commandLine.Add(longKey: EnvironmentParameters.CommandLineKeyUserDirectory, hasValue: true);
            commandLine.Add(longKey: EnvironmentParameters.CommandLineKeyMachineDirectory, hasValue: true);
            commandLine.Add(longKey: EnvironmentParameters.CommandLineKeyTemporaryDirectory, hasValue: true);
            commandLine.Add(longKey: CommandLineKeyRunMode, hasValue: true);
            commandLine.Add(longKey: CommandLineKeyAppLogLimit, hasValue: true);
            commandLine.Add(longKey: CommandLineKeyLog, hasValue: true);
            commandLine.Add(longKey: CommandLineKeyWithLog, hasValue: true);
            commandLine.Add(longKey: CommandLineSwitchFullTraceLog, hasValue: false);
            commandLine.Add(longKey: CommandLineSwitchForceLog, hasValue: false);
            commandLine.Add(longKey: CommandLineSwitchAcceptSkip, hasValue: false);
            commandLine.Add(longKey: CommandLineSwitchBetaVersion, hasValue: false);

            commandLine.Parse();

            return commandLine;
        }

#if BETA
        bool ShowCommandLineMessageIfUnspecified(CommandLine commandLine)
        {
            var knownBetaVersion = commandLine.ExistsSwitch(CommandLineSwitchBetaVersion);
            if(knownBetaVersion) {
                return true;
            }

            var result = MessageBox.Show(
                Properties.Resources.String_Unknown_BetaVersion_Message,
                Properties.Resources.String_Unknown_BetaVersion_Caption,
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning,
                MessageBoxResult.Cancel
            );

            return result == MessageBoxResult.OK;
        }
#endif

        ApplicationEnvironmentParameters InitializeEnvironment(CommandLine commandLine)
        {
            Debug.Assert(commandLine.IsParsed);

            var applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var rootDirectoryPath = Path.GetDirectoryName(applicationDirectory);

            return new ApplicationEnvironmentParameters(new DirectoryInfo(rootDirectoryPath), commandLine);
        }

        bool CheckFirstStartup(EnvironmentParameters environmentParameters, ILogger logger)
        {
            var file = environmentParameters.MainFile;
            file.Refresh();
            return !file.Exists;
        }

        AcceptResult ShowAcceptView(IDiScopeContainerFactory scopeContainerCreator, EnvironmentParameters environmentParameters, ILoggerFactory? loggerFactory)
        {
            using(var diContainer = scopeContainerCreator.CreateChildContainer()) {
                if(loggerFactory != null) {
                    diContainer.Register<ILoggerFactory, ILoggerFactory>(loggerFactory);
                }

                diContainer
                    .Register<IDispatcherWrapper, ApplicationDispatcherWrapper>(DiLifecycle.Transient)
                    .Register<EnvironmentParameters, EnvironmentParameters>(environmentParameters)
                    .Register<CustomConfiguration, CustomConfiguration>(environmentParameters.Configuration)
                    .RegisterMvvm<Element.Accept.AcceptElement, ViewModels.Accept.AcceptViewModel, Views.Accept.AcceptWindow>()
                ;
                using(var windowManager = new WindowManager(diContainer, CultureService.Instance, diContainer.Get<ILoggerFactory>())) {
                    using var acceptModel = diContainer.Build<Element.Accept.AcceptElement>();
                    acceptModel.Initialize();
                    var view = diContainer.Build<Views.Accept.AcceptWindow>();
                    windowManager.Register(new WindowItem(WindowKind.Accept, acceptModel, view));
                    view.ShowDialog();

                    return new AcceptResult(
                        acceptModel.Accepted,
                        acceptModel.UpdateKind,
                        acceptModel.IsEnabledTelemetry
                    );
                }
            }
        }

        void InitializeFileSystem(EnvironmentParameters environmentParameters, ILogger logger)
        {
            var dirs = environmentParameters.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(i => i.GetCustomAttribute<InitialDirectoryAttribute>() != null)
                .Select(i => i.GetValue(environmentParameters))
                .OfType<DirectoryInfo>()
                .ToList()
            ;

            foreach(var dir in dirs) {
                logger.LogDebug("create {0}", dir.FullName);
                try {
                    dir.Create();
                } catch(Exception ex) {
                    logger.LogError(ex, ex.Message);
                    throw;
                }
            }
        }

        ApplicationDatabaseFactoryPack CreateDatabaseFactoryPack(EnvironmentParameters environmentParameters, ILogger logger)
        {
            return new ApplicationDatabaseFactoryPack(
                new ApplicationDatabaseFactory(environmentParameters.MainFile, false),
                new ApplicationDatabaseFactory(environmentParameters.FileFile, false),
                new ApplicationDatabaseFactory()
            );
        }
        IDatabaseStatementLoader GetStatementLoader(EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
        {
            DatabaseAccessor? statementAccessor = null;
            environmentParameters.SqlStatementAccessorFile.Refresh();
            if(environmentParameters.SqlStatementAccessorFile.Exists) {
                statementAccessor = new ApplicationDatabaseAccessor(new ApplicationDatabaseFactory(environmentParameters.SqlStatementAccessorFile, true), loggerFactory);
            }

            return new ApplicationDatabaseStatementLoader(environmentParameters.MainSqlDirectory, TimeSpan.Zero, statementAccessor, environmentParameters.Configuration.File.GivePriorityToFile, loggerFactory);
        }

        void FirstSetup(EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory, ILogger logger)
        {
            logger.LogInformation("初回セットアップ");

            // 初回セットアップに来ている場合既に存在するデータファイルは狂っている可能性があるので破棄する
            var deleteTartgetFiles = new[] {
                environmentParameters.MainFile,
                environmentParameters.FileFile,
            };
            foreach(var file in deleteTartgetFiles) {
                logger.LogDebug("delete: {0}", file.FullName);
                file.Refresh();
                try {
                    file.Delete();
                } catch(IOException ex) {
                    logger.LogError(ex, ex.Message);
                }
            }

            var idFactory = new IdFactory(loggerFactory);
            using(var factoryPack = CreateDatabaseFactoryPack(environmentParameters, logger))
            using(var accessorPack = ApplicationDatabaseAccessorPack.Create(factoryPack, loggerFactory)) {
                var statementLoader = GetStatementLoader(environmentParameters, loggerFactory);
                using var statementLoaderDisposer = statementLoader as IDisposable;
                var databaseSetupper = new DatabaseSetupper(idFactory, statementLoader, loggerFactory);
                databaseSetupper.Initialize(accessorPack);
            }
        }

        bool NormalSetup(out (ApplicationDatabaseFactoryPack factory, ApplicationDatabaseAccessorPack accessor) pack, EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory, ILogger logger)
        {
            logger.LogInformation("DBセットアップ");

            var factoryPack = CreateDatabaseFactoryPack(environmentParameters, logger);
            var accessorPack = ApplicationDatabaseAccessorPack.Create(factoryPack, loggerFactory);

            var idFactory = new IdFactory(loggerFactory);

            var statementLoader = GetStatementLoader(environmentParameters, loggerFactory);
            using var statementLoaderDisposer = statementLoader as IDisposable;

            var databaseSetupper = new DatabaseSetupper(idFactory, statementLoader, loggerFactory);

            //前回実行バージョンの取得と取得失敗時に再セットアップ処理
            var lastVersion = databaseSetupper.GetLastVersion(accessorPack.Main);
            if(lastVersion == null) {
                logger.LogError("last version is null");
                logger.LogWarning("restart initialize");

                accessorPack.Dispose();
                factoryPack.Dispose();
                pack = default((ApplicationDatabaseFactoryPack factory, ApplicationDatabaseAccessorPack accessor));
                return false;
            }

            databaseSetupper.Migrate(accessorPack, lastVersion);

            pack.factory = factoryPack;
            pack.accessor = accessorPack;
            return true;
        }

        ApplicationDiContainer SetupContainer(EnvironmentParameters environmentParameters, ApplicationDatabaseFactoryPack factory, CultureService cultureService, ILoggerFactory loggerFactory)
        {
            var container = new ApplicationDiContainer();

            //var accessor = ApplicationDatabaseAccessorPack.Create(factory, loggerFactory);
            var lazyWriterWaitTimePack = new LazyWriterWaitTimePack(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
            /*
            var readerWriterLockerPack = new ApplicationReaderWriterLockerPack(
                new ApplicationMainReaderWriterLocker(),
                new ApplicationFileReaderWriterLocker(),
                new ApplicationTemporaryReaderWriterLocker()
            );
            var barrierPack = new ApplicationDatabaseBarrierPack(
                new ApplicationDatabaseBarrier(accessor.Main, readerWriterLockerPack.Main),
                new ApplicationDatabaseBarrier(accessor.File, readerWriterLockerPack.File),
                new ApplicationDatabaseBarrier(accessor.Temporary, readerWriterLockerPack.Temporary)
            );

            var lazyWriterPack = new ApplicationDatabaseLazyWriterPack(
                new ApplicationDatabaseLazyWriter(barrierPack.Main, TimeSpan.FromSeconds(3), loggerFactory),
                new ApplicationDatabaseLazyWriter(barrierPack.File, TimeSpan.FromSeconds(3), loggerFactory),
                new ApplicationDatabaseLazyWriter(barrierPack.Temporary, TimeSpan.FromSeconds(3), loggerFactory)
            );
            */
            DatabaseAccessor? statementAccessor = null;
            environmentParameters.SqlStatementAccessorFile.Refresh();
            if(environmentParameters.SqlStatementAccessorFile.Exists) {
                statementAccessor = new ApplicationDatabaseAccessor(new ApplicationDatabaseFactory(environmentParameters.SqlStatementAccessorFile, true), loggerFactory);
            }

            container
                .Register<ILoggerFactory, ILoggerFactory>(loggerFactory)
                .Register<IDiContainer, ApplicationDiContainer>(container)

                .Register<EnvironmentParameters, EnvironmentParameters>(environmentParameters)
                .Register<CustomConfiguration, CustomConfiguration>(environmentParameters.Configuration)
                .Register<GeneralConfiguration, GeneralConfiguration>(environmentParameters.Configuration.General)
                .Register<WebConfiguration, WebConfiguration>(environmentParameters.Configuration.Web)
                .Register<ApiConfiguration, ApiConfiguration>(environmentParameters.Configuration.Api)
                .Register<BackupConfiguration, BackupConfiguration>(environmentParameters.Configuration.Backup)
                .Register<FileConfiguration, FileConfiguration>(environmentParameters.Configuration.File)
                .Register<DisplayConfiguration, DisplayConfiguration>(environmentParameters.Configuration.Display)
                .Register<LauncherToolbarConfiguration, LauncherToolbarConfiguration>(environmentParameters.Configuration.LauncherToobar)
                .Register<LauncherGroupConfiguration, LauncherGroupConfiguration>(environmentParameters.Configuration.LauncherGroup)
                .Register<NoteConfiguration, NoteConfiguration>(environmentParameters.Configuration.Note)
                .Register<CommandConfiguration, CommandConfiguration>(environmentParameters.Configuration.Command)
                .Register<PlatformConfiguration, PlatformConfiguration>(environmentParameters.Configuration.Platform)

                .Register<IDatabaseStatementLoader, ApplicationDatabaseStatementLoader>(new ApplicationDatabaseStatementLoader(environmentParameters.MainSqlDirectory, TimeSpan.FromMinutes(6), statementAccessor, environmentParameters.Configuration.File.GivePriorityToFile, loggerFactory))
                /*
                .Register<IDatabaseFactoryPack, ApplicationDatabaseFactoryPack>(factory)
                .Register<IDatabaseAccessorPack, ApplicationDatabaseAccessorPack>(accessor)
                .Register<IDatabaseBarrierPack, ApplicationDatabaseBarrierPack>(barrierPack)
                .Register<IReaderWriterLockerPack, ApplicationReaderWriterLockerPack>(readerWriterLockerPack)
                .Register<IDatabaseLazyWriterPack, ApplicationDatabaseLazyWriterPack>(lazyWriterPack)

                .Register<IMainDatabaseBarrier, ApplicationDatabaseBarrier>(barrierPack.Main)
                .Register<IFileDatabaseBarrier, ApplicationDatabaseBarrier>(barrierPack.File)
                .Register<ITemporaryDatabaseBarrier, ApplicationDatabaseBarrier>(barrierPack.Temporary)

                .Register<IMainDatabaseLazyWriter, ApplicationDatabaseLazyWriter>(lazyWriterPack.Main)
                .Register<IFileDatabaseLazyWriter, ApplicationDatabaseLazyWriter>(lazyWriterPack.File)
                .Register<ITemporaryDatabaseLazyWriter, ApplicationDatabaseLazyWriter>(lazyWriterPack.Temporary)
                */
                .RegisterDatabase(factory, lazyWriterWaitTimePack, loggerFactory)
                .Register<IDispatcherWrapper, ApplicationDispatcherWrapper>(DiLifecycle.Transient)
                .Register<CultureService, CultureService>(cultureService)

                .Register<IIdFactory, IdFactory>(DiLifecycle.Transient)
            ;

            //ApplicationDiContainer.Initialize(() => container);

            return container;
        }

        WindowManager SetupWindowManager(IDiRegisterContainer diContainer)
        {
            var manager = diContainer.Build<WindowManager>(CultureService.Instance);

            return manager;
        }

        /*
        OrderManager SetupOrderManager(IDiRegisterContainer diContainer)
        {
            var manager = diContainer.Make<OrderManager>();

            return manager;
        }

        */
        NotifyManager SetupNotifyManager(IDiRegisterContainer diContainer)
        {
            var manager = diContainer.Make<NotifyManager>();

            return manager;
        }

        StatusManager SetupStatusManager(IDiRegisterContainer diContainer)
        {
            var manager = diContainer.Build<StatusManager>();

            return manager;
        }

        ClipboardManager SetupClipboardManager(IDiRegisterContainer diContainer)
        {
            var manager = diContainer.Build<ClipboardManager>();

            return manager;
        }

        UserAgentManager SetupUserAgentManager(IDiRegisterContainer diContainer)
        {
            var manager = diContainer.Build<UserAgentManager>();

            return manager;
        }


        public bool Initialize(App app, StartupEventArgs e)
        {
            InitializeEnvironmentVariable();
            InitializeClr();

            var commandLine = CreateCommandLine(e.Args);
            RunMode = RunModeUtility.Parse(commandLine.GetValue(CommandLineKeyRunMode, string.Empty));
#if BETA
            if(RunMode != RunMode.CrashReport) {
                if(!ShowCommandLineMessageIfUnspecified(commandLine)) {
                    return false;
                }
            }
#endif
            var environmentParameters = InitializeEnvironment(commandLine);

            if(!int.TryParse(commandLine.GetValue(CommandLineKeyAppLogLimit, string.Empty), out var appLogLimit)) {
                appLogLimit = AppLogLimit;
            }
            if(appLogLimit < 1) {
                appLogLimit = AppLogLimit;
            }

            var logginConfigFilePath = Path.Combine(environmentParameters.EtcDirectory.FullName, environmentParameters.Configuration.General.LoggingConfigFileName);
            Logging = new ApplicationLogging(
                appLogLimit,
                logginConfigFilePath,
                commandLine.GetValue(CommandLineKeyLog, string.Empty),
                commandLine.GetValue(CommandLineKeyWithLog, string.Empty),
                commandLine.ExistsSwitch(CommandLineSwitchForceLog),
                commandLine.ExistsSwitch(CommandLineSwitchFullTraceLog)
            );
            var loggerFactory = Logging.Factory;
            var logger = Logging.Factory.CreateLogger(GetType());

            if(RunMode == RunMode.Normal) {
                var mutexName = environmentParameters.Configuration.General.MutexName;
                logger.LogInformation("mutext: {0}", mutexName);
                var mutex = new Mutex(true, mutexName, out var createdNew);
                if(!createdNew) {
                    //NOTE: 起動中プロセスになんかするならここかなぁ
                    logger.LogWarning("二重起動: {0}", mutexName);
                    mutex.Dispose();
                    return false;
                }
                Mutex = mutex;
            }

            var cultureService = new CultureService(EnumResourceManagerFactory.Create());
            CultureService.Initialize(cultureService);

            if(RunMode == RunMode.CrashReport) {
                // DI/その他 の重要インフラは構築しない
                logger.LogInformation("クラッシュレポート起動のため初期化処理 途中終了");
                return true;
            }

            var skipAccept = commandLine.ExistsSwitch(CommandLineSwitchAcceptSkip);
            if(skipAccept) {
                logger.LogInformation("使用許諾はコマンドライン設定によりスキップ");
            }

            AcceptResult? acceptResult = null;
            IsFirstStartup = CheckFirstStartup(environmentParameters, logger);
            if(IsFirstStartup) {
                logger.LogInformation("初回実行");
                if(!skipAccept) {
                    // 設定ファイルやらなんやらを構築する前に完全初回の使用許諾を取る
                    acceptResult = ShowAcceptView(new DiContainer(false), environmentParameters, loggerFactory);
                    if(!acceptResult.Accepted) {
                        // 初回の使用許諾を得られなかったのでばいちゃ
                        logger.LogInformation("使用許諾得られず");
                        return false;
                    }
                }
            }

            InitializeFileSystem(environmentParameters, logger);
            environmentParameters.SetFileSystemInitialized();

            if(IsFirstStartup) {
                FirstSetup(environmentParameters, loggerFactory, logger);
            }

            var webViewinItializer = new WebViewinItializer(loggerFactory);
            webViewinItializer.Initialize(environmentParameters, cultureService);
            //try {
            //    webViewinItializer.Initialize(environmentParameters);
            //} catch(Exception ex) {
            //    logger.LogWarning(ex, ex.Message);
            //    webViewinItializer.AddVisualCppRuntimeRedist(environmentParameters);
            //}

            (ApplicationDatabaseFactoryPack factory, ApplicationDatabaseAccessorPack accessor) pack;
            if(!NormalSetup(out pack, environmentParameters, loggerFactory, logger)) {
                // データぶっ壊れてる系
                FirstSetup(environmentParameters, loggerFactory, logger);
                var retryResult = NormalSetup(out pack, environmentParameters, loggerFactory, logger);
                if(!retryResult) {
                    throw new ApplicationException();
                }
            }

            var factory = pack.factory;
            pack.accessor.Dispose();

            DiContainer = SetupContainer(environmentParameters, factory, cultureService, loggerFactory);
            WindowManager = SetupWindowManager(DiContainer);
            //OrderManager = SetupOrderManager(DiContainer);
            NotifyManager = SetupNotifyManager(DiContainer);
            StatusManager = SetupStatusManager(DiContainer);
            ClipboardManager = SetupClipboardManager(DiContainer);
            UserAgentManager = SetupUserAgentManager(DiContainer);

            var cultureServiceChanger = DiContainer.Build<CultureServiceChanger>(CultureService.Instance, WindowManager);
            cultureServiceChanger.ChangeCulture();

            if(acceptResult != null) {
                //Debug.Assert(IsFirstStartup);
                var mainDatabaseBarrier = DiContainer.Build<IMainDatabaseBarrier>();
                var userIdManager = DiContainer.Build<UserIdManager>();
                var userId = acceptResult.IsEnabledTelemetry
                    ? userIdManager.CreateFromEnvironment()
                    : string.Empty
                ;
                using(var commander = mainDatabaseBarrier.WaitWrite()) {
                    var appExecuteSettingEntityDao = DiContainer.Build<AppExecuteSettingEntityDao>(commander, commander.Implementation);
                    appExecuteSettingEntityDao.UpdateExecuteSettingAcceptInput(userId, acceptResult.IsEnabledTelemetry, DatabaseCommonStatus.CreateCurrentAccount());

                    var appUpdateSettingEntityDao = DiContainer.Build<AppUpdateSettingEntityDao>(commander, commander.Implementation);
                    appUpdateSettingEntityDao.UpdateReleaseVersion(acceptResult.UpdateKind, DatabaseCommonStatus.CreateCurrentAccount());

                    commander.Commit();
                }
            } else {
                var mainDatabaseBarrier = DiContainer.Build<IMainDatabaseBarrier>();
                using(var commander = mainDatabaseBarrier.WaitWrite()) {
                    var appExecuteSettingEntityDao = DiContainer.Build<AppExecuteSettingEntityDao>(commander, commander.Implementation);
                    var setting = appExecuteSettingEntityDao.SelectSettingExecuteSetting();

                    if(setting.IsEnabledTelemetry) {
                        var userIdManager = DiContainer.Build<UserIdManager>();
                        if(!userIdManager.IsValidUserId(setting.UserId)) {
                            logger.LogInformation("統計情報送信は有効だがユーザーIDが不正のため無効化: {0}", setting.UserId);
                            appExecuteSettingEntityDao.UpdateExecuteSettingAcceptInput(string.Empty, false, DatabaseCommonStatus.CreateCurrentAccount());
                            commander.Commit();
                        }
                    }

                    commander.Commit();
                }
            }

            return true;
        }


        #endregion
    }
}
