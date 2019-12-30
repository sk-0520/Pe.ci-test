using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Theme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class ApplicationInitializer
    {
        #region property

        string CommandLineKeyLog { get; } = "log";
        string CommandLineSwitchForceLog { get; } = "force-log";

        string CommandLineSwitchAcceptSkip { get; } = "skip-accept";

        public bool IsFirstStartup { get; private set; }

        public ApplicationDiContainer? DiContainer { get; private set; }
        public ILoggerFactory? LoggerFactory { get; private set; }
        public WindowManager? WindowManager { get; private set; }
        //public OrderManager OrderManager { get; private set; }
        public NotifyManager? NotifyManager { get; private set; }
        public StatusManager? StatusManager { get; private set; }
        public ClipboardManager? ClipboardManager { get; private set; }

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
            commandLine.Add(longKey: CommandLineKeyLog, hasValue: true);
            commandLine.Add(longKey: CommandLineSwitchForceLog, hasValue: false);
            commandLine.Add(longKey: CommandLineSwitchAcceptSkip, hasValue: false);

            commandLine.Parse();

            return commandLine;
        }

        EnvironmentParameters InitializeEnvironment(CommandLine commandLine)
        {
            Debug.Assert(commandLine.IsParsed);

            var applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var rootDirectoryPath = Path.GetDirectoryName(applicationDirectory);

            return new EnvironmentParameters(new DirectoryInfo(rootDirectoryPath), commandLine);
        }

        ILoggerFactory CreateLoggerFactory(string logginConfigFilePath, string outputPath, bool createDirectory, [CallerFilePath] string callerFilePath = "")
        {
            var loggerFactory = new LoggerFactory();
            NLog.LogManager.LoadConfiguration(logginConfigFilePath);

            var op = new NLog.Extensions.Logging.NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true };
            var prov = new NLog.Extensions.Logging.NLogLoggerProvider(op, NLog.LogManager.LogFactory);
            loggerFactory.AddProvider(prov);

            var logger = loggerFactory.CreateLogger(GetType());
            logger.LogInformation("開発用ログ出力開始");

            // ログ出力(ファイル・ディレクトリが存在しなければ終了で構わない)
            if(!string.IsNullOrWhiteSpace(outputPath)) {
                var expandedOutputPath = Environment.ExpandEnvironmentVariables(outputPath);
                if(createDirectory) {
                    var fileName = Path.GetFileName(expandedOutputPath);
                    if(!string.IsNullOrEmpty(fileName) && fileName.IndexOf('.') == -1) {
                        // 拡張子がなければディレクトリ指定と決めつけ
                        Directory.CreateDirectory(expandedOutputPath);
                    } else {
                        var parentDir = Path.GetDirectoryName(expandedOutputPath);
                        if(!string.IsNullOrEmpty(parentDir)) {
                            Directory.CreateDirectory(parentDir);
                        }
                    }
                }

                // ディレクトリ指定であればタイムスタンプ付きでファイル生成
                var filePath = expandedOutputPath;
                if(Directory.Exists(expandedOutputPath)) {
                    var fileName = PathUtility.AppendExtension(DateTime.Now.ToString("yyyy-MM-dd_HHmmss"), "log");
                    filePath = Path.Combine(expandedOutputPath, fileName);
                }
                NLog.LogManager.LogFactory.Configuration.Variables.Add("logfile", filePath);
                logger.LogInformation("ファイルログ出力開始");
            } else {
                NLog.LogManager.LogFactory.Configuration.Variables.Add("logfile", "nul");
            }

            return loggerFactory;
        }

        bool CheckFirstStartup(EnvironmentParameters environmentParameters, ILogger logger)
        {
            var file = environmentParameters.MainFile;
            file.Refresh();
            return !file.Exists;
        }

        bool ShowAcceptView(IDiScopeContainerFactory scopeContainerCreator, ILoggerFactory loggerFactory)
        {
            using(var diContainer = scopeContainerCreator.CreateChildContainer()) {
                diContainer
                    .Register<ILoggerFactory, ILoggerFactory>(loggerFactory)
                    .Register<IDispatcherWrapper, ApplicationDispatcherWrapper>(DiLifecycle.Transient)
                    .RegisterMvvm<Element.Accept.AcceptElement, ViewModels.Accept.AcceptViewModel, Views.Accept.AcceptWindow>()
                ;
                using(var windowManager = new WindowManager(diContainer, loggerFactory)) {
                    var acceptModel = diContainer.Build<Element.Accept.AcceptElement>();
                    var view = diContainer.Build<Views.Accept.AcceptWindow>();
                    windowManager.Register(new WindowItem(WindowKind.Accept, view));
                    view.ShowDialog();

                    return acceptModel.Accepted;
                }
            }
        }

        void InitializeFileSystem(EnvironmentParameters environmentParameters, ILogger logger)
        {
            var dirs = new[] {
                environmentParameters.UserRoamingDirectory,
                environmentParameters.UserBackupDirectory,
                environmentParameters.UserSettingDirectory,
                environmentParameters.MachineDirectory,
                environmentParameters.MachineArchiveDirectory,
                environmentParameters.MachineUpdateDirectory,
                environmentParameters.TemporaryDirectory,
            };

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
                new ApplicationDatabaseFactory(environmentParameters.MainFile),
                new ApplicationDatabaseFactory(environmentParameters.FileFile),
                new ApplicationDatabaseFactory()
            );
        }
        IDatabaseStatementLoader GetStatementLoader(EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
        {
            return new ApplicationDatabaseStatementLoader(environmentParameters.MainSqlDirectory, TimeSpan.Zero, loggerFactory);
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

            var tuner = new DatabaseTuner(new IdFactory(loggerFactory), accessorPack, statementLoader, loggerFactory);
            tuner.Tune();

            pack.factory = factoryPack;
            pack.accessor = accessorPack;
            return true;
        }

        ApplicationDiContainer SetupContainer(EnvironmentParameters environmentParameters, ApplicationDatabaseFactoryPack factory, ILoggerFactory loggerFactory)
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

            container
                .Register<ILoggerFactory, ILoggerFactory>(loggerFactory)
                .Register<IDiContainer, ApplicationDiContainer>(container)
                .Register<EnvironmentParameters, EnvironmentParameters>(environmentParameters)

                .Register<IDatabaseStatementLoader, ApplicationDatabaseStatementLoader>(new ApplicationDatabaseStatementLoader(environmentParameters.MainSqlDirectory, TimeSpan.FromSeconds(30), loggerFactory))
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

                .Register<IIdFactory, IdFactory>(DiLifecycle.Transient)

                .Register<ILauncherToolbarTheme, LauncherToolbarTheme>(DiLifecycle.Transient)
                .Register<ILauncherGroupTheme, LauncherGroupTheme>(DiLifecycle.Transient)
                .Register<INoteTheme, NoteTheme>(DiLifecycle.Transient)
                .Register<IFontTheme, FontTheme>(DiLifecycle.Transient)
            ;

            //ApplicationDiContainer.Initialize(() => container);

            return container;
        }

        WindowManager SetupWindowManager(IDiRegisterContainer diContainer)
        {
            var manager = diContainer.Make<WindowManager>();

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

        public bool Initialize(App app, StartupEventArgs e)
        {
            InitializeEnvironmentVariable();
            InitializeClr();

            var commandLine = CreateCommandLine(e.Args);
            var environmentParameters = InitializeEnvironment(commandLine);

            var logginConfigFilePath = Path.Combine(environmentParameters.EtcDirectory.FullName, environmentParameters.Configuration.General.LoggingConfigFileName);
            var loggerFactory = CreateLoggerFactory(logginConfigFilePath, commandLine.GetValue(CommandLineKeyLog, string.Empty), commandLine.ExistsSwitch(CommandLineSwitchForceLog));
            var logger = loggerFactory.CreateLogger(GetType());

            var skipAccept = commandLine.ExistsSwitch(CommandLineSwitchAcceptSkip);
            if(skipAccept) {
                logger.LogInformation("使用許諾はコマンドライン設定によりスキップ");
            }


            IsFirstStartup = CheckFirstStartup(environmentParameters, logger);
            if(IsFirstStartup) {
                logger.LogInformation("初回実行");
                if(!skipAccept) {
                    // 設定ファイルやらなんやらを構築する前に完全初回の使用許諾を取る
                    var dialogResult = ShowAcceptView(new DiContainer(), loggerFactory);
                    if(!dialogResult) {
                        // 初回の使用許諾を得られなかったのでばいちゃ
                        logger.LogInformation("使用許諾得られず");
                        return false;
                    }
                }
            }

            InitializeFileSystem(environmentParameters, logger);
            if(IsFirstStartup) {
                FirstSetup(environmentParameters, loggerFactory, logger);
            }

            (ApplicationDatabaseFactoryPack factory, ApplicationDatabaseAccessorPack accessor) pack;
            if(!NormalSetup(out pack, environmentParameters, loggerFactory, logger)) {
                // データぶっ壊れてる系
                FirstSetup(environmentParameters, loggerFactory, logger);
                var retryResult = NormalSetup(out pack, environmentParameters, loggerFactory, logger);
                if(!retryResult) {
                    throw new ApplicationException();
                }
            }

            //TODO: バージョンアップに伴う使用許諾
            if(!IsFirstStartup && !skipAccept) {
            }

            var factory = pack.factory;
            pack.accessor.Dispose();

            LoggerFactory = loggerFactory;
            DiContainer = SetupContainer(environmentParameters, factory, loggerFactory);
            WindowManager = SetupWindowManager(DiContainer);
            //OrderManager = SetupOrderManager(DiContainer);
            NotifyManager = SetupNotifyManager(DiContainer);
            StatusManager = SetupStatusManager(DiContainer);
            ClipboardManager = SetupClipboardManager(DiContainer);


            return true;
        }

        #endregion
    }
}
