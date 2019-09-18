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

        public bool IsFirstStartup { get; private set; }

        public ApplicationDiContainer? DiContainer { get; private set; }
        public ILoggerFactory? LoggerFactory { get; private set; }

        #endregion

        #region function

        void InitializeEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("PE_DESKTOP", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
        }

        CommandLine CreateCommandLine(IEnumerable<string> arguments)
        {
            var commandLine = new CommandLine(arguments, false);

            commandLine.Add(longKey: EnvironmentParameters.CommandLineKeyUserDirectory, hasValue: true);
            commandLine.Add(longKey: EnvironmentParameters.CommandLineKeyMachineDirectory, hasValue: true);
            commandLine.Add(longKey: EnvironmentParameters.CommandLineKeyTemporaryDirectory, hasValue: true);
            commandLine.Add(longKey: CommandLineKeyLog, hasValue: true);
            commandLine.Add(longKey: CommandLineSwitchForceLog, hasValue: false);

            commandLine.Parse();

            return commandLine;
        }

        EnvironmentParameters InitializeEnvironment(CommandLine commandLine)
        {
            Debug.Assert(commandLine.IsParsed);

            var applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return EnvironmentParameters.Initialize(new DirectoryInfo(applicationDirectory), commandLine);
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
            var file = environmentParameters.SettingFile;
            file.Refresh();
            return !file.Exists;
        }

        bool ShowAcceptView(IDiScopeContainerFactory scopeContainerCreator, ILoggerFactory loggerFactory)
        {
            using(var diContainer = scopeContainerCreator.CreateChildContainer()) {
                diContainer
                    .Register<IDispatcherWapper, ApplicationDispatcherWapper>(DiLifecycle.Transient)
                    .RegisterLogger(loggerFactory)
                    .RegisterMvvm<Element.Accept.AcceptElement, ViewModel.Accept.AcceptViewModel, Views.Accept.AcceptWindow>()
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

        DatabaseFactoryPack CreateDatabaseFactoryPack(EnvironmentParameters environmentParameters, ILogger logger)
        {
            return new DatabaseFactoryPack(
                new ApplicationDatabaseFactory(environmentParameters.SettingFile),
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
                environmentParameters.SettingFile,
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

            using(var factoryPack = CreateDatabaseFactoryPack(environmentParameters, logger))
            using(var accessorPack = DatabaseAccessorPack.Create(factoryPack, loggerFactory)) {
                var statementLoader = GetStatementLoader(environmentParameters, loggerFactory);
                var databaseSetupper = new DatabaseSetupper(statementLoader, loggerFactory);
                databaseSetupper.Initialize(accessorPack);
            }
        }

        bool NormalSetup(out (DatabaseFactoryPack factory, DatabaseAccessorPack accessor) pack, EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory, ILogger logger)
        {
            logger.LogInformation("DBセットアップ");

            var factoryPack = CreateDatabaseFactoryPack(environmentParameters, logger);
            var accessorPack = DatabaseAccessorPack.Create(factoryPack, loggerFactory);

            var statementLoader = GetStatementLoader(environmentParameters, loggerFactory);
            var databaseSetupper = new DatabaseSetupper(statementLoader, loggerFactory);

            //前回実行バージョンの取得と取得失敗時に再セットアップ処理
            var lastVersion = databaseSetupper.GetLastVersion(accessorPack.Main);
            if(lastVersion == null) {
                logger.LogError("last version is null");
                logger.LogWarning("restart initialize");

                accessorPack.Dispose();
                factoryPack.Dispose();
                pack = default((DatabaseFactoryPack factory, DatabaseAccessorPack accessor));
                return false;
            }

            databaseSetupper.Migrate(accessorPack, lastVersion);

            var tuner = new DatabaseTuner(new IdFactory(loggerFactory), accessorPack, statementLoader, loggerFactory);
            tuner.Tune();

            pack.factory = factoryPack;
            pack.accessor = accessorPack;
            return true;
        }

        ApplicationDiContainer SetupContainer(EnvironmentParameters environmentParameters, DatabaseFactoryPack factory, DatabaseAccessorPack accessor, ILoggerFactory loggerFactory)
        {
            var container = new ApplicationDiContainer();

            var rwlp = new ReadWriteLockPack(
                new ApplicationMainReaderWriterLocker(),
                new ApplicationFileReaderWriterLocker(),
                new ApplicationTemporaryReaderWriterLocker()
            );

            container
                .Register<ILoggerFactory, ILoggerFactory>(loggerFactory)
                .Register<IDiContainer, ApplicationDiContainer>(container)
                .Register<IDatabaseStatementLoader, ApplicationDatabaseStatementLoader>(new ApplicationDatabaseStatementLoader(environmentParameters.MainSqlDirectory, TimeSpan.FromSeconds(30), loggerFactory))
                .Register<IDatabaseFactoryPack, DatabaseFactoryPack>(factory)
                .Register<IDatabaseAccessorPack, DatabaseAccessorPack>(accessor)
                .Register<IMainDatabaseBarrier, ApplicationDatabaseBarrier>(new ApplicationDatabaseBarrier(accessor.Main, rwlp.Main))
                .Register<IFileDatabaseBarrier, ApplicationDatabaseBarrier>(new ApplicationDatabaseBarrier(accessor.File, rwlp.File))
                .Register<ITemporaryDatabaseBarrier, ApplicationDatabaseBarrier>(new ApplicationDatabaseBarrier(accessor.Temporary, rwlp.Temporary))
                .Register<IReadWriteLockPack, ReadWriteLockPack>(rwlp)
                .Register<IDispatcherWapper, ApplicationDispatcherWapper>(DiLifecycle.Transient)
                .Register<IIdFactory, IdFactory>(DiLifecycle.Transient)
                .Register<ILauncherToolbarTheme, LauncherToolbarTheme>(DiLifecycle.Transient)
                .Register<ILauncherGroupTheme, LauncherGroupTheme>(DiLifecycle.Transient)
                .Register<INoteTheme, NoteTheme>(DiLifecycle.Transient)
                .Register<IFontTheme, FontTheme>(DiLifecycle.Transient)
            ;

            ApplicationDiContainer.Initialize(() => container);

            return container;
        }

        public bool Initialize(App app, StartupEventArgs e)
        {
            InitializeEnvironmentVariable();

            var commandLine = CreateCommandLine(e.Args);
            var environmentParameters = InitializeEnvironment(commandLine);

            var logginConfigFilePath = Path.Combine(environmentParameters.EtcDirectory.FullName, Constants.LoggingConfigFileName);
            var loggerFactory = CreateLoggerFactory(logginConfigFilePath, commandLine.GetValue(CommandLineKeyLog, string.Empty), commandLine.ExistsSwitch(CommandLineSwitchForceLog));
            var logger = loggerFactory.CreateLogger(GetType());

            IsFirstStartup = CheckFirstStartup(environmentParameters, logger);
            if(IsFirstStartup) {
                logger.LogInformation("初回実行");
                // 設定ファイルやらなんやらを構築する前に完全初回の使用許諾を取る
                var dialogResult = ShowAcceptView(new DiContainer(), loggerFactory);
                if(!dialogResult) {
                    // 初回の使用許諾を得られなかったのでばいちゃ
                    logger.LogInformation("使用許諾得られず");
                    return false;
                }
            }

            InitializeFileSystem(environmentParameters, logger);
            if(IsFirstStartup) {
                FirstSetup(environmentParameters, loggerFactory, logger);
            }

            (DatabaseFactoryPack factory, DatabaseAccessorPack accessor) pack;
            if(!NormalSetup(out pack, environmentParameters, loggerFactory, logger)) {
                // データぶっ壊れてる系
                FirstSetup(environmentParameters, loggerFactory, logger);
                var retryResult = NormalSetup(out pack, environmentParameters, loggerFactory, logger);
                if(!retryResult) {
                    throw new ApplicationException();
                }
            }

            DiContainer = SetupContainer(environmentParameters, pack.factory, pack.accessor, loggerFactory);

            return true;
        }

        #endregion
    }
}
