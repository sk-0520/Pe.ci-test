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
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    internal class ApplicationInitializer
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

#if DEBUG
        string CommandLineSwitchDebugDevelopMode { get; } = "debug-dev-mode";
        public bool IsDebugDevelopMode { get; private set; }
#endif
        /// <summary>
        /// テストプラグイン格納ディレクトリパス。
        /// </summary>
        string CommandLineTestPluginDirectoryPath { get; } = "test-plugin-dir";
        /// <summary>
        /// テストプラグイン名。
        /// <para>通常は<see cref="CommandLineTestPluginDirectoryPath"/>のディレクトリ名を使用するが、デバッグ実行時などのプラグインディレクトリ名とプラグイン名が異なる場合に指定する。</para>
        /// <para>なお指定の際には拡張子を除外すること(plugin-name.dll -> plugin-name)</para>
        /// </summary>
        string CommandLineTestPluginName { get; } = "test-plugin-name";

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

        public string TestPluginDirectoryPath { get; private set; } = string.Empty;
        public string TestPluginName { get; private set; } = string.Empty;
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
#if DEBUG
            commandLine.Add(longKey: CommandLineSwitchDebugDevelopMode, hasValue: false);
#endif
            commandLine.Add(longKey: CommandLineTestPluginDirectoryPath, hasValue: true);
            commandLine.Add(longKey: CommandLineTestPluginName, hasValue: true);

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
                Properties.Resources.String_BetaVersion_Unknown_Message,
                Properties.Resources.String_BetaVersion_Unknown_Caption,
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning,
                MessageBoxResult.Cancel
            );

            return result == MessageBoxResult.OK;
        }
#endif
        bool ShowCommandLineTestPlugin(CommandLine commandLine, EnvironmentParameters environmentParameters)
        {
            var testPluginDirectoryPath = commandLine.GetValue(CommandLineTestPluginDirectoryPath, string.Empty);
            if(string.IsNullOrWhiteSpace(testPluginDirectoryPath)) {
                return true;
            }

            var expandedTestPluginDirectoryPath = Environment.ExpandEnvironmentVariables(testPluginDirectoryPath);

            if(!Directory.Exists(expandedTestPluginDirectoryPath)) {
                MessageBox.Show(
                    TextUtility.ReplaceFromDictionary(
                        Properties.Resources.String_TestPlugin_NotFound_Message_Format,
                        new Dictionary<string, string>() {
                            ["PATH"] = testPluginDirectoryPath,
                        }
                    ),
                    Properties.Resources.String_TestPlugin_NotFound_Caption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return false;
            }

            var userDirKey = commandLine.GetValue(EnvironmentParameters.CommandLineKeyUserDirectory, string.Empty);
            var machineDirKey = commandLine.GetValue(EnvironmentParameters.CommandLineKeyMachineDirectory, string.Empty);
            var tempDirKey = commandLine.GetValue(EnvironmentParameters.CommandLineKeyTemporaryDirectory, string.Empty);

            var hasEmpty = new[] { userDirKey, machineDirKey, tempDirKey, }.Any(i => string.IsNullOrWhiteSpace(i));
            if(hasEmpty) {
                var result = MessageBox.Show(
                    TextUtility.ReplaceFromDictionary(
                        Properties.Resources.String_TestPlugin_Data_Message_Format,
                        new Dictionary<string, string>() {
                            ["COMMAND-USER-KEY"] = EnvironmentParameters.CommandLineKeyUserDirectory,
                            ["COMMAND-MACHINE-KEY"] = EnvironmentParameters.CommandLineKeyMachineDirectory,
                            ["COMMAND-TEMP-KEY"] = EnvironmentParameters.CommandLineKeyTemporaryDirectory,
                            ["USER-DIR"] = userDirKey,
                            ["MACHINE-DIR"] = machineDirKey,
                            ["TEMP-DIR"] = tempDirKey,
                        }
                    ),
                    Properties.Resources.String_TestPlugin_Data_Caption,
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning,
                    MessageBoxResult.Cancel
                );

                if(result != MessageBoxResult.OK) {
                    return false;
                }
            }

            TestPluginDirectoryPath = expandedTestPluginDirectoryPath;
            TestPluginName = commandLine.GetValue(CommandLineTestPluginName, string.Empty);

            return true;
        }

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
                    .Register<IDispatcherWrapper, ApplicationDispatcherWrapper>(DiLifecycle.Transient, () => new ApplicationDispatcherWrapper(TimeSpan.FromSeconds(10)))
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

        ApplicationDatabaseFactoryPack CreateDatabaseFactoryPack(EnvironmentParameters environmentParameters, bool foreignKeys, ILogger logger)
        {
            return new ApplicationDatabaseFactoryPack(
                new ApplicationDatabaseFactory(environmentParameters.MainFile, foreignKeys, false),
                new ApplicationDatabaseFactory(environmentParameters.FileFile, foreignKeys, false),
                new ApplicationDatabaseFactory(true, false)
            );
        }
        IDatabaseStatementLoader GetDatabaseStatementLoader(EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
        {
            DatabaseAccessor? statementAccessor = null;
            environmentParameters.SqlStatementAccessorFile.Refresh();
            if(environmentParameters.SqlStatementAccessorFile.Exists) {
                statementAccessor = new ApplicationDatabaseAccessor(new ApplicationDatabaseFactory(environmentParameters.SqlStatementAccessorFile, true, true), loggerFactory);
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
            using(var factoryPack = CreateDatabaseFactoryPack(environmentParameters, false, logger))
            using(var accessorPack = ApplicationDatabaseAccessorPack.Create(factoryPack, loggerFactory)) {
                var statementLoader = GetDatabaseStatementLoader(environmentParameters, loggerFactory);
                using var statementLoaderDisposer = statementLoader as IDisposable;
                var databaseSetupper = new DatabaseSetupper(idFactory, statementLoader, loggerFactory);
                databaseSetupper.Initialize(accessorPack);
            }
        }

        bool NormalSetup(out (ApplicationDatabaseFactoryPack factory, ApplicationDatabaseAccessorPack accessor) pack, EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory, ILogger logger)
        {
            logger.LogInformation("DBセットアップ");

            using var factoryPack = CreateDatabaseFactoryPack(environmentParameters, false, logger);
            using var accessorPack = ApplicationDatabaseAccessorPack.Create(factoryPack, loggerFactory);

            var idFactory = new IdFactory(loggerFactory);

            var statementLoader = GetDatabaseStatementLoader(environmentParameters, loggerFactory);
            using var statementLoaderDisposer = statementLoader as IDisposable;

            var databaseSetupper = new DatabaseSetupper(idFactory, statementLoader, loggerFactory);

            //前回実行バージョンの取得と取得失敗時に再セットアップ処理
            var lastVersion = databaseSetupper.GetLastVersion(accessorPack.Main);
            if(lastVersion == null) {
                logger.LogError("最終使用バージョンが取得できず");

                pack = default((ApplicationDatabaseFactoryPack factory, ApplicationDatabaseAccessorPack accessor));
                return false;
            }

            databaseSetupper.Migrating(accessorPack, lastVersion);

            pack.factory = CreateDatabaseFactoryPack(environmentParameters, true, logger);
            pack.accessor = ApplicationDatabaseAccessorPack.Create(factoryPack, loggerFactory);

            foreach(var accessor in pack.accessor.Items) {
                databaseSetupper.CheckForeignKey(accessor);
            }

            return true;
        }

        ApplicationDiContainer SetupContainer(EnvironmentParameters environmentParameters, ApplicationDatabaseFactoryPack factory, CultureService cultureService, ILoggerFactory loggerFactory)
        {
            var container = new ApplicationDiContainer();

            var lazyWriterWaitTimePack = new LazyWriterWaitTimePack(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));

            DatabaseAccessor? statementAccessor = null;
            environmentParameters.SqlStatementAccessorFile.Refresh();
            if(environmentParameters.SqlStatementAccessorFile.Exists) {
                statementAccessor = new ApplicationDatabaseAccessor(new ApplicationDatabaseFactory(environmentParameters.SqlStatementAccessorFile, true, true), loggerFactory);
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
                .Register<HookConfiguration, HookConfiguration>(environmentParameters.Configuration.Hook)
                .Register<NotifyLogConfiguration, NotifyLogConfiguration>(environmentParameters.Configuration.NotifyLog)
                .Register<LauncherItemConfiguration, LauncherItemConfiguration>(environmentParameters.Configuration.LauncherItem)
                .Register<LauncherToolbarConfiguration, LauncherToolbarConfiguration>(environmentParameters.Configuration.LauncherToobar)
                .Register<LauncherGroupConfiguration, LauncherGroupConfiguration>(environmentParameters.Configuration.LauncherGroup)
                .Register<NoteConfiguration, NoteConfiguration>(environmentParameters.Configuration.Note)
                .Register<CommandConfiguration, CommandConfiguration>(environmentParameters.Configuration.Command)
                .Register<PlatformConfiguration, PlatformConfiguration>(environmentParameters.Configuration.Platform)
                .Register<ScheduleConfiguration, ScheduleConfiguration>(environmentParameters.Configuration.Schedule)

                .Register<IDatabaseStatementLoader, ApplicationDatabaseStatementLoader>(new ApplicationDatabaseStatementLoader(environmentParameters.MainSqlDirectory, TimeSpan.FromMinutes(6), statementAccessor, environmentParameters.Configuration.File.GivePriorityToFile, loggerFactory))

                .RegisterDatabase(factory, lazyWriterWaitTimePack, loggerFactory)

                .Register<PluginContextFactory, PluginContextFactory>(DiLifecycle.Transient)
                .Register<PreferencesContextFactory, PreferencesContextFactory>(DiLifecycle.Transient)
                .Register<LauncherItemAddonContextFactory, LauncherItemAddonContextFactory>(DiLifecycle.Transient)
                .Register<WidgetAddonContextFactory, WidgetAddonContextFactory>(DiLifecycle.Transient)
                .Register<BackgroundAddonContextFactory, BackgroundAddonContextFactory>(DiLifecycle.Transient)

                .Register<IDispatcherWrapper, IDispatcherWrapper>(DiLifecycle.Transient, () => new ApplicationDispatcherWrapper(environmentParameters.Configuration.General.DispatcherWait))
                .Register<CultureService, CultureService>(cultureService)
                .Register<IImageLoader, ImageLoader>(DiLifecycle.Transient)

                .Register<IIdFactory, IdFactory>(DiLifecycle.Transient)
            ;

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
            var manager = diContainer.Build<NotifyManager>();

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

#if DEBUG
            IsDebugDevelopMode = commandLine.ExistsSwitch(CommandLineSwitchDebugDevelopMode);
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

#if DEBUG
            if(IsDebugDevelopMode) {
                if(!skipAccept) {
                    skipAccept = true;
                    logger.LogWarning("使用許諾はデバッグ時開発者モードとしてスキップ");
                }
            }
#endif

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

            if(RunMode != RunMode.CrashReport) {
                if(!ShowCommandLineTestPlugin(commandLine, environmentParameters)) {
                    return false;
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
                logger.LogWarning("再初期化処理実施");
                // データぶっ壊れてる系
                FirstSetup(environmentParameters, loggerFactory, logger);
                var retryResult = NormalSetup(out pack, environmentParameters, loggerFactory, logger);
                if(!retryResult) {
                    logger.LogWarning("あかんかったわ");
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
