using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using Forms = System.Windows.Forms;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Base;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting.Factory;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ApplicationInitializerException: Exception
    {
        public ApplicationInitializerException()
        { }
        public ApplicationInitializerException(string message)
            : base(message)
        { }

        public ApplicationInitializerException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    internal class ApplicationInitializer
    {
        #region define

        private const int AppLogLimit = 128;

        #endregion

        #region property

        private bool BetaMode { get; } =
#if BETA
            true
#else
    false
#endif
;

        public static string CommandLineKeyRunMode { get; } = "run-mode";
        private string CommandLineKeyAppLogLimit { get; } = "app-log-limit";
        public static string CommandLineKeyLog { get; } = "log";
        private string CommandLineKeyWithLog { get; } = "with-log";
        private string CommandLineSwitchFullTraceLog { get; } = "full-trace-log";
        public static string CommandLineSwitchForceLog { get; } = "force-log";

        private string CommandLineSwitchAcceptSkip { get; } = "skip-accept";
        private string CommandLineSwitchBetaVersion { get; } = "beta-version";

#if DEBUG
        private string CommandLineSwitchDebugDevelopMode { get; } = "debug-dev-mode";
        public bool IsDebugDevelopMode { get; private set; }
#endif
        /// <summary>
        /// テストプラグイン格納ディレクトリパス。
        /// </summary>
        private string CommandLineTestPluginDirectoryPath { get; } = "test-plugin-dir";
        /// <summary>
        /// テストプラグイン名。
        /// </summary>
        /// <remarks>
        /// <para>通常は<see cref="CommandLineTestPluginDirectoryPath"/>のディレクトリ名を使用するが、デバッグ実行時などのプラグインディレクトリ名とプラグイン名が異なる場合に指定する。</para>
        /// <para>なお指定の際には拡張子を除外すること(plugin-name.dll -> plugin-name)</para>
        /// </remarks>
        private string CommandLineTestPluginName { get; } = "test-plugin-name";

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

        private void InitializeEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("PE_DESKTOP", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
        }

        private void InitializeClr()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private CommandLine CreateCommandLine(IEnumerable<string> arguments)
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

        private bool ShowCommandLineBetaMessageIfUnspecified(CommandLine commandLine)
        {
            var knownBetaVersion = commandLine.ExistsSwitch(CommandLineSwitchBetaVersion);
            if(knownBetaVersion) {
                return true;
            }

            // この段階で ApplicationEnvironmentParameters は未構築なのでルートディレクトリを取得する重複コード
            var rootDirectory = GetRootDirectory();
            var batchFilePath = Path.Combine(rootDirectory.FullName, "bat", "beta.bat");

            var message = TextUtility.ReplaceFromDictionary(
                Properties.Resources.String_BetaVersion_Unknown_Message_Format,
                new Dictionary<string, string>() {
                    ["BETA-BAT"] = batchFilePath,
                }
            );

            var result = Forms.TaskDialog.ShowDialog(
                new Forms.TaskDialogPage() {
                    Caption = Properties.Resources.String_BetaVersion_Unknown_Caption,
                    Heading = Properties.Resources.String_BetaVersion_Unknown_Heading,
                    Text = message,
                    Buttons = [
                        Forms.TaskDialogButton.Continue,
                        Forms.TaskDialogButton.Abort,
                    ],
                    DefaultButton = Forms.TaskDialogButton.Abort,
                    Icon = Forms.TaskDialogIcon.ShieldWarningYellowBar,
                    SizeToContent = true,
                },
                Forms.TaskDialogStartupLocation.CenterScreen
            );

            return result == Forms.TaskDialogButton.Continue;
        }

        private bool ShowCommandLineTestPlugin(CommandLine commandLine, EnvironmentParameters environmentParameters)
        {
            var testPluginDirectoryPath = commandLine.GetValue(CommandLineTestPluginDirectoryPath, string.Empty);
            if(string.IsNullOrWhiteSpace(testPluginDirectoryPath)) {
                return true;
            }

            var expandedTestPluginDirectoryPath = Environment.ExpandEnvironmentVariables(testPluginDirectoryPath);

            if(!Directory.Exists(expandedTestPluginDirectoryPath)) {
                Forms.TaskDialog.ShowDialog(
                    new Forms.TaskDialogPage() {
                        Caption = Properties.Resources.String_TestPlugin_NotFound_Caption,
                        Text = TextUtility.ReplaceFromDictionary(
                            Properties.Resources.String_TestPlugin_NotFound_Message_Format,
                            new Dictionary<string, string>() {
                                ["PATH"] = testPluginDirectoryPath,
                            }
                        ),
                        Buttons = [
                            Forms.TaskDialogButton.OK
                        ],
                        Icon = Forms.TaskDialogIcon.Error,
                    },
                    Forms.TaskDialogStartupLocation.CenterScreen
                );

                return false;
            }

            var userDirKey = commandLine.GetValue(EnvironmentParameters.CommandLineKeyUserDirectory, string.Empty);
            var machineDirKey = commandLine.GetValue(EnvironmentParameters.CommandLineKeyMachineDirectory, string.Empty);
            var tempDirKey = commandLine.GetValue(EnvironmentParameters.CommandLineKeyTemporaryDirectory, string.Empty);

            var hasEmpty = new[] { userDirKey, machineDirKey, tempDirKey, }.Any(i => string.IsNullOrWhiteSpace(i));
            if(hasEmpty) {
                var result = Forms.TaskDialog.ShowDialog(
                    new Forms.TaskDialogPage() {
                        Caption = Properties.Resources.String_TestPlugin_Data_Caption,
                        Text = TextUtility.ReplaceFromDictionary(
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
                        Buttons = [
                            Forms.TaskDialogButton.Continue,
                            Forms.TaskDialogButton.Abort,
                        ],
                        DefaultButton = Forms.TaskDialogButton.Abort,
                        Icon = Forms.TaskDialogIcon.Warning,
                    },
                    Forms.TaskDialogStartupLocation.CenterScreen
                );

                if(result != Forms.TaskDialogButton.Continue) {
                    return false;
                }
            }

            TestPluginDirectoryPath = expandedTestPluginDirectoryPath;
            TestPluginName = commandLine.GetValue(CommandLineTestPluginName, string.Empty);

            return true;
        }

        private DirectoryInfo GetRootDirectory()
        {
            var applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var rootDirectoryPath = Path.GetDirectoryName(applicationDirectory)!;

            return new DirectoryInfo(rootDirectoryPath);
        }

        private ApplicationEnvironmentParameters InitializeEnvironment(CommandLine commandLine)
        {
            Debug.Assert(commandLine.IsParsed);

            var rootDirectory = GetRootDirectory();

            return new ApplicationEnvironmentParameters(rootDirectory, commandLine);
        }

        private bool CheckFirstStartup(EnvironmentParameters environmentParameters, ILogger logger)
        {
            var file = environmentParameters.MainFile;
            file.Refresh();
            return !file.Exists;
        }

        private async Task<AcceptResult> ShowAcceptViewAsync(IDiScopeContainerFactory scopeContainerCreator, EnvironmentParameters environmentParameters, ICultureService cultureService, ILoggerFactory? loggerFactory, CancellationToken cancellationToken)
        {
            using(var diContainer = scopeContainerCreator.CreateChildContainer()) {
                if(loggerFactory != null) {
                    diContainer.Register<ILoggerFactory, ILoggerFactory>(loggerFactory);
                }

                diContainer
                    .Register<IDispatcherWrapper, ApplicationDispatcherWrapper>(DiLifecycle.Transient, () => new ApplicationDispatcherWrapper(TimeSpan.FromSeconds(10)))
                    .Register<EnvironmentParameters, EnvironmentParameters>(environmentParameters)
                    .Register<ApplicationConfiguration, ApplicationConfiguration>(environmentParameters.ApplicationConfiguration)
                    .RegisterMvvm<Element.Accept.AcceptElement, ViewModels.Accept.AcceptViewModel, Views.Accept.AcceptWindow>()
                ;
                using(var windowManager = new WindowManager(diContainer, cultureService, diContainer.Get<ILoggerFactory>())) {
                    using var acceptModel = diContainer.Build<Element.Accept.AcceptElement>();
                    await acceptModel.InitializeAsync(cancellationToken);
                    var view = diContainer.Build<Views.Accept.AcceptWindow>();
                    windowManager.Register(new WindowItem(Manager.WindowKind.Accept, acceptModel, view));
                    view.ShowDialog();

                    return new AcceptResult(
                        acceptModel.Accepted,
                        acceptModel.UpdateKind,
                        acceptModel.IsEnabledTelemetry
                    );
                }
            }
        }

        private void InitializeFileSystem(EnvironmentParameters environmentParameters, ILogger logger)
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

        private ApplicationDatabaseFactoryPack CreateDatabaseFactoryPack(EnvironmentParameters environmentParameters, bool foreignKeys, ILogger logger)
        {
            return new ApplicationDatabaseFactoryPack(
                new ApplicationDatabaseFactory(environmentParameters.MainFile, foreignKeys, false),
                new ApplicationDatabaseFactory(environmentParameters.LargeFile, foreignKeys, false),
                new ApplicationDatabaseFactory(true, false)
            );
        }
        private IDatabaseStatementLoader GetDatabaseStatementLoader(EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
        {
            DatabaseAccessor? statementAccessor = null;
            environmentParameters.SqlStatementAccessorFile.Refresh();
            if(environmentParameters.SqlStatementAccessorFile.Exists) {
                statementAccessor = new ApplicationDatabaseAccessor(new ApplicationDatabaseFactory(environmentParameters.SqlStatementAccessorFile, true, true), loggerFactory);
            }

            return new ApplicationDatabaseStatementLoader(environmentParameters.MainSqlDirectory, TimeSpan.Zero, statementAccessor, environmentParameters.ApplicationConfiguration.File.GivePriorityToFile, loggerFactory);
        }

        private void FirstSetup(EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory, ILogger logger)
        {
            logger.LogInformation("初回セットアップ");

            // 初回セットアップに来ている場合既に存在するデータファイルは狂っている可能性があるので破棄する
            var deleteTargetFiles = new[] {
                environmentParameters.MainFile,
                environmentParameters.LargeFile,
            };
            foreach(var file in deleteTargetFiles) {
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

        private bool NormalSetup(out (ApplicationDatabaseFactoryPack factory, ApplicationDatabaseAccessorPack accessor) pack, EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory, ILogger logger)
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

            databaseSetupper.Migrate(accessorPack, lastVersion);

            pack.factory = CreateDatabaseFactoryPack(environmentParameters, true, logger);
            pack.accessor = ApplicationDatabaseAccessorPack.Create(factoryPack, loggerFactory);

            foreach(var accessor in pack.accessor.Items) {
                databaseSetupper.CheckForeignKey(accessor);
            }

            return true;
        }

        private ApplicationDiContainer SetupContainer(EnvironmentParameters environmentParameters, ApplicationDatabaseFactoryPack? factory, CultureService cultureService, ILoggerFactory loggerFactory)
        {
            var container = new ApplicationDiContainer();

            var delayWriterWaitTimePack = new DelayWriterWaitTimePack(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));

            DatabaseAccessor? statementAccessor = null;
            environmentParameters.SqlStatementAccessorFile.Refresh();
            if(environmentParameters.SqlStatementAccessorFile.Exists) {
                statementAccessor = new ApplicationDatabaseAccessor(new ApplicationDatabaseFactory(environmentParameters.SqlStatementAccessorFile, true, true), loggerFactory);
            }

            // DIコンテナ登録(なんかいろいろ)
            container
                .Register<ILoggerFactory, ILoggerFactory>(loggerFactory)
                .Register<IDiContainer, ApplicationDiContainer>(container)

                .Register(environmentParameters)
                .Register(environmentParameters.ApplicationConfiguration)
                .Register(environmentParameters.ApplicationConfiguration.General)
                .Register(environmentParameters.ApplicationConfiguration.Web)
                .Register(environmentParameters.ApplicationConfiguration.Api)
                .Register(environmentParameters.ApplicationConfiguration.Backup)
                .Register(environmentParameters.ApplicationConfiguration.File)
                .Register(environmentParameters.ApplicationConfiguration.Display)
                .Register(environmentParameters.ApplicationConfiguration.Hook)
                .Register(environmentParameters.ApplicationConfiguration.NotifyLog)
                .Register(environmentParameters.ApplicationConfiguration.LauncherItem)
                .Register(environmentParameters.ApplicationConfiguration.LauncherToolbar)
                .Register(environmentParameters.ApplicationConfiguration.LauncherGroup)
                .Register(environmentParameters.ApplicationConfiguration.Note)
                .Register(environmentParameters.ApplicationConfiguration.Command)
                .Register(environmentParameters.ApplicationConfiguration.Platform)
                .Register(environmentParameters.ApplicationConfiguration.Schedule)
                .Register<IApplicationInformation>(new ApplicationInformation(BuildStatus.Version, ProcessArchitecture.ApplicationArchitecture))

                .Register<PluginContextFactory>(DiLifecycle.Transient)
                .Register<PreferencesContextFactory>(DiLifecycle.Transient)
                .Register<LauncherItemAddonContextFactory>(DiLifecycle.Transient)
                .Register<WidgetAddonContextFactory>(DiLifecycle.Transient)
                .Register<BackgroundAddonContextFactory>(DiLifecycle.Transient)

                .Register<IDispatcherWrapper, IDispatcherWrapper>(DiLifecycle.Transient, () => new ApplicationDispatcherWrapper(environmentParameters.ApplicationConfiguration.General.DispatcherWait))
                .Register(cultureService)
                .Register<ICultureService>(cultureService)
                .Register<IViewManager, ViewManager>(DiLifecycle.Transient)
                .Register<IImageLoader, ImageLoader>(DiLifecycle.Transient)
                .Register<IMediaConverter, MediaConverter>(DiLifecycle.Transient)
                .Register<IPolicy, Policy>(DiLifecycle.Transient)
                .Register<IPluginArguments, PluginArguments>(DiLifecycle.Transient)

                .Register<IIdFactory, IdFactory>(DiLifecycle.Transient)
                .Register<IKeyGestureGuide, KeyGestureGuide>(DiLifecycle.Transient)

                .Register<ISettingElementFactory, SettingElementFactory>(DiLifecycle.Transient)
            ;

            if(factory is not null) {
                container.Register<IDatabaseStatementLoader, ApplicationDatabaseStatementLoader>(new ApplicationDatabaseStatementLoader(environmentParameters.MainSqlDirectory, TimeSpan.FromMinutes(6), statementAccessor, environmentParameters.ApplicationConfiguration.File.GivePriorityToFile, loggerFactory));
                container.RegisterDatabase(factory, delayWriterWaitTimePack, loggerFactory);
            }


            return container;
        }

        private WindowManager SetupWindowManager(IDiRegisterContainer diContainer, ICultureService cultureService)
        {
            var manager = diContainer.Build<WindowManager>(cultureService);

            return manager;
        }

        /*
        OrderManager SetupOrderManager(IDiRegisterContainer diContainer)
        {
            var manager = diContainer.Make<OrderManager>();

            return manager;
        }

        */
        private NotifyManager SetupNotifyManager(IDiRegisterContainer diContainer)
        {
            var manager = diContainer.Build<NotifyManager>();

            return manager;
        }

        private StatusManager SetupStatusManager(IDiRegisterContainer diContainer)
        {
            var manager = diContainer.Build<StatusManager>();

            return manager;
        }

        private ClipboardManager SetupClipboardManager(IDiRegisterContainer diContainer)
        {
            var manager = diContainer.Build<ClipboardManager>();

            return manager;
        }

        private UserAgentManager SetupUserAgentManager(IDiRegisterContainer diContainer)
        {
            var manager = diContainer.Build<UserAgentManager>();

            return manager;
        }

        private void InitializeDirectory(EnvironmentParameters environmentParameters, ILogger logger, ILoggerFactory loggerFactory)
        {
            // 限定的に一時ディレクトリ内きれいにする
            var dirs = new[] {
                environmentParameters.TemporaryApplicationExtractDirectory,
                environmentParameters.TemporaryPluginAutomaticExtractDirectory,
                environmentParameters.TemporaryPluginManualExtractDirectory,
                environmentParameters.TemporarySettingDirectory,
            };
            foreach(var dir in dirs) {
                logger.LogInformation("cleanup: {0}", dir.FullName);
                try {
                    var directoryCleaner = new DirectoryCleaner(dir, environmentParameters.ApplicationConfiguration.File.DirectoryRemoveWaitCount, environmentParameters.ApplicationConfiguration.File.DirectoryRemoveWaitTime, loggerFactory);
                    directoryCleaner.Clear(false);
                } catch(Exception ex) {
                    logger.LogError(ex, ex.Message);
                }
            }
        }

        public async Task<bool> InitializeAsync(App app, StartupEventArgs e, CancellationToken cancellationToken)
        {
            InitializeEnvironmentVariable();
            InitializeClr();

            var commandLine = CreateCommandLine(e.Args);
            RunMode = RunModeUtility.Parse(commandLine.GetValue(CommandLineKeyRunMode, string.Empty));

            if(BetaMode) {
                if(RunModeUtility.CheckBetaModeAlert(RunMode)) {
                    if(!ShowCommandLineBetaMessageIfUnspecified(commandLine)) {
                        return false;
                    }
                }
            }

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

            var loggingConfigFilePath = Path.Combine(environmentParameters.EtcDirectory.FullName, environmentParameters.ApplicationConfiguration.General.LogConfigFileName);
            Logging = new ApplicationLogging(
                appLogLimit,
                loggingConfigFilePath,
                commandLine.GetValue(CommandLineKeyLog, string.Empty),
                commandLine.GetValue(CommandLineKeyWithLog, string.Empty),
                commandLine.ExistsSwitch(CommandLineSwitchForceLog),
                commandLine.ExistsSwitch(CommandLineSwitchFullTraceLog)
            );
            var loggerFactory = Logging.Factory;
            var logger = Logging.Factory.CreateLogger(GetType());

            if(RunModeUtility.IsSingleProcessOnly(RunMode)) {
                var mutexName = environmentParameters.ApplicationConfiguration.General.MutexName;
                logger.LogInformation("ミューテックス名: {0}", mutexName);
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

            if(!RunModeUtility.IsBuildInfrastructure(RunMode)) {
                // DI/その他 の重要インフラは構築しない
                logger.LogInformation("インフラ構築スキップのため初期化処理 途中終了");
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
            if(RunModeUtility.NeedFirstSetup(RunMode)) {
                IsFirstStartup = CheckFirstStartup(environmentParameters, logger);
                if(IsFirstStartup) {
                    logger.LogInformation("初回実行");
                    if(!skipAccept) {
                        // 設定ファイルやらなんやらを構築する前に完全初回の使用許諾を取る
                        acceptResult = await ShowAcceptViewAsync(new DiContainer(false), environmentParameters, cultureService, loggerFactory, cancellationToken);
                        if(!acceptResult.Accepted) {
                            // 初回の使用許諾を得られなかったのでばいちゃ
                            logger.LogInformation("使用許諾得られず");
                            return false;
                        }
                    }
                }
            }

            if(RunModeUtility.CanTestPluginInstall(RunMode)) {
                if(!ShowCommandLineTestPlugin(commandLine, environmentParameters)) {
                    return false;
                }
            }

            if(RunModeUtility.IsBuildFileSystem(RunMode)) {
                InitializeFileSystem(environmentParameters, logger);
            }
            environmentParameters.SetFileSystemInitialized();

            if(RunModeUtility.IsBuildPersistence(RunMode)) {
                if(IsFirstStartup) {
                    FirstSetup(environmentParameters, loggerFactory, logger);
                }
            }

            if(RunModeUtility.IsBuildFileSystem(RunMode)) {
                InitializeDirectory(environmentParameters, logger, loggerFactory);
            }

            ApplicationDatabaseFactoryPack? factory = null;
            if(RunModeUtility.IsBuildPersistence(RunMode)) {
                (ApplicationDatabaseFactoryPack factory, ApplicationDatabaseAccessorPack accessor) pack;
                if(!NormalSetup(out pack, environmentParameters, loggerFactory, logger)) {
                    logger.LogWarning("再初期化処理実施");
                    // データぶっ壊れてる系
                    FirstSetup(environmentParameters, loggerFactory, logger);
                    var retryResult = NormalSetup(out pack, environmentParameters, loggerFactory, logger);
                    if(!retryResult) {
                        logger.LogWarning("あかんかったわ");
                        throw new ApplicationInitializerException();
                    }
                }

                factory = pack.factory;
                pack.accessor.Dispose();
            } else {
                // セットアップしないにしてもDB使用可能にしておく(TODO: クラッシュレポートの場合はもう少し異常系を試した方がいいかも)
                factory = CreateDatabaseFactoryPack(environmentParameters, true, logger);
            }

            DiContainer = SetupContainer(environmentParameters, factory, cultureService, loggerFactory);
            if(RunModeUtility.IsBuildPersistence(RunMode)) {
                var databaseSetupper = DiContainer.Build<DatabaseSetupper>();
                var lastVersion = databaseSetupper.GetLastVersion(DiContainer.Build<IDatabaseAccessorPack>().Main)!;
                databaseSetupper.Adjust(DiContainer.Build<IDatabaseAccessorPack>(), lastVersion);
            }

            WindowManager = SetupWindowManager(DiContainer, cultureService);
            //OrderManager = SetupOrderManager(DiContainer);
            NotifyManager = SetupNotifyManager(DiContainer);
            StatusManager = SetupStatusManager(DiContainer);
            ClipboardManager = SetupClipboardManager(DiContainer);
            UserAgentManager = SetupUserAgentManager(DiContainer);

            if(RunModeUtility.IsBuildPersistence(RunMode)) {
                var cultureServiceChanger = DiContainer.Build<CultureServiceChanger>(CultureService.Instance, WindowManager);
                cultureServiceChanger.ChangeCulture();
            }
            if(RunModeUtility.NeedFirstSetup(RunMode)) {
                if(acceptResult != null) {
                    //Debug.Assert(IsFirstStartup);
                    var mainDatabaseBarrier = DiContainer.Build<IMainDatabaseBarrier>();
                    var userIdManager = DiContainer.Build<UserIdManager>();
                    var userId = acceptResult.IsEnabledTelemetry
                        ? userIdManager.CreateFromEnvironment()
                        : string.Empty
                    ;
                    using(var context = mainDatabaseBarrier.WaitWrite()) {
                        var appExecuteSettingEntityDao = DiContainer.Build<AppExecuteSettingEntityDao>(context, context.Implementation);
                        appExecuteSettingEntityDao.UpdateExecuteSettingAcceptInput(userId, acceptResult.IsEnabledTelemetry, DatabaseCommonStatus.CreateCurrentAccount());

                        var appUpdateSettingEntityDao = DiContainer.Build<AppUpdateSettingEntityDao>(context, context.Implementation);
                        appUpdateSettingEntityDao.UpdateReleaseVersion(acceptResult.UpdateKind, DatabaseCommonStatus.CreateCurrentAccount());

                        context.Commit();
                    }
                } else {
                    var mainDatabaseBarrier = DiContainer.Build<IMainDatabaseBarrier>();
                    using(var context = mainDatabaseBarrier.WaitWrite()) {
                        var appExecuteSettingEntityDao = DiContainer.Build<AppExecuteSettingEntityDao>(context, context.Implementation);
                        var setting = appExecuteSettingEntityDao.SelectSettingExecuteSetting();

                        if(setting.IsEnabledTelemetry) {
                            var userIdManager = DiContainer.Build<UserIdManager>();
                            if(!userIdManager.IsValidUserId(setting.UserId)) {
                                logger.LogInformation("統計情報送信は有効だがユーザーIDが不正のため無効化: {0}", setting.UserId);
                                appExecuteSettingEntityDao.UpdateExecuteSettingAcceptInput(string.Empty, false, DatabaseCommonStatus.CreateCurrentAccount());
                                context.Commit();
                            }
                        }

                        context.Commit();
                    }
                }
            }

            return true;
        }

        #endregion
    }
}
