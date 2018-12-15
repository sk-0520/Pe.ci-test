using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Database;

namespace ContentTypeTextNet.Pe.Main.Model
{
    public class ApplicationInitializer
    {
        #region property

        string CommandLineKeyLog { get; } = "log";

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

            commandLine.Execute();

            return commandLine;
        }

        EnvironmentParameters InitializeEnvironment(CommandLine commandLine)
        {
            Debug.Assert(commandLine.IsParsed);

            var applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return EnvironmentParameters.Initialize(new DirectoryInfo(applicationDirectory), commandLine);
        }

        bool IsFirstStartup(EnvironmentParameters environmentParameters, ILogger logger)
        {
            var file = environmentParameters.SettingFile;
            file.Refresh();
            return !file.Exists;
        }

        bool ShowAcceptView(IDiScopeContainerCreator scopeContainerCreator, ILogger logger)
        {
            using(var diContainer = scopeContainerCreator.Scope()) {
                diContainer
                    .Register<ILogger, ILogger>(DiLifecycle.Singleton, () => logger)
                    .Register<ILoggerFactory, ILoggerFactory>(DiLifecycle.Singleton, () => logger)
                    .Register<ViewElement.Accept.AcceptViewElement, ViewElement.Accept.AcceptViewElement>(DiLifecycle.Singleton)
                    .Register<ViewModel.Accept.AcceptViewModel, ViewModel.Accept.AcceptViewModel>(DiLifecycle.Transient)
                    .DirtyRegister<View.Accept.AcceptWindow, ViewModel.Accept.AcceptViewModel>(nameof(System.Windows.FrameworkElement.DataContext))
                ;

                var acceptModel = diContainer.New<ViewElement.Accept.AcceptViewElement>();
                var view = diContainer.Make<View.Accept.AcceptWindow>();
                view.ShowDialog();

                return acceptModel.Accepted;
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
                logger.Debug($"create {dir.FullName}");
                try {
                    dir.Create();
                } catch(Exception ex) {
                    logger.Error(ex);
                    throw;
                }
            }
        }

        ApplicationLogger CreateLogger(string outputPath, [System.Runtime.CompilerServices.CallerFilePath] string callerFilePath = default(string))
        {
            if(LogItem.ShortFileIndex == 0) {
                var ignoreLoggerFilePath = Path.Combine(Constants.ProjectName, "Source");
                var ignoreLoggerFilePathIndex = callerFilePath.IndexOf(ignoreLoggerFilePath, StringComparison.OrdinalIgnoreCase);
                LogItem.ShortFileIndex = ignoreLoggerFilePathIndex + ignoreLoggerFilePath.Length + 1/* \ の分も引いておく */;
            }

            var logger = new ApplicationLogger();
            var logKinds = LogKind.Information | LogKind.Error | LogKind.Fatal;
#if DEBUG
            logKinds = LogKind.All;
#elif BETA
            logKinds |= LogKind.Debug;
#endif
            logger.SetEnabled(logKinds);

            // ログ出力(ファイル・ディレクトリが存在しなければ終了で構わない)
            if(!string.IsNullOrWhiteSpace(outputPath)) {
                var expandedOutputPath = Environment.ExpandEnvironmentVariables(outputPath);
                // ディレクトリ指定であればタイムスタンプ付きでファイル生成
                string filePath = expandedOutputPath;
                if(Directory.Exists(expandedOutputPath)) {
                    var fileName = PathUtility.AppendExtension(DateTime.Now.ToString("yyyy-MM-dd_HHmmss"), "log");
                    filePath = Path.Combine(expandedOutputPath, fileName);
                }
                var writer = new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read), Encoding.Unicode) {
                    AutoFlush = true,
                };
                logger.AttachWriter(writer);
            }

            return logger;
        }

        private void OutputStartupLog(ILogger logger)
        {
            logger.Information("!!START!!");
        }

        DatabaseFactoryPack CreateDatabaseFactoryPack(EnvironmentParameters environmentParameters, ILogger logger)
        {
            return new DatabaseFactoryPack(
                new ApplicationDatabaseFactory(environmentParameters.SettingFile),
                new ApplicationDatabaseFactory(environmentParameters.FileFile),
                new ApplicationDatabaseFactory()
            );
        }

        void FirstSetup(EnvironmentParameters environmentParameters, ILogger logger)
        {
            logger.Information("初回セットアップ");

            // 初回セットアップに来ている場合既に存在するデータファイルは狂っている可能性があるので破棄する
            var deleteTartgetFiles = new[] {
                environmentParameters.SettingFile,
                environmentParameters.FileFile,
            };
            foreach(var file in deleteTartgetFiles) {
                logger.Trace($"delete: {file.FullName}");
                file.Refresh();
                try {
                    file.Delete();
                } catch(IOException ex) {
                    logger.Error(ex);
                }
            }

            using(var factoryPack = CreateDatabaseFactoryPack(environmentParameters, logger))
            using(var accessorPack = DatabaseAccessorPack.Create(factoryPack, logger.Factory)) {

                var databaseSetup = new DatabaseSetup(environmentParameters.MainSqlDirectory, logger.Factory);
                databaseSetup.Initialize(accessorPack);
            }
        }

        bool NormalSetup(out (DatabaseFactoryPack factory, DatabaseAccessorPack accessor) pack,EnvironmentParameters environmentParameters, ILogger logger)
        {
            logger.Information("DBセットアップ");

            var factoryPack = CreateDatabaseFactoryPack(environmentParameters, logger);
            var accessorPack = DatabaseAccessorPack.Create(factoryPack, logger.Factory);

            var databaseSetup = new DatabaseSetup(environmentParameters.MainSqlDirectory, logger.Factory);

            //前回実行バージョンの取得と取得失敗時に再セットアップ処理
            var lastVersion = databaseSetup.GetLastVersion(accessorPack.Main);
            if(lastVersion == null) {
                logger.Error("last version is null");
                logger.Warning("restart initialize");

                accessorPack.Dispose();
                factoryPack.Dispose();
                pack = default((DatabaseFactoryPack factory, DatabaseAccessorPack accessor));
                return false;
            }

            databaseSetup.Migrate(accessorPack, lastVersion);
            pack.factory = factoryPack;
            pack.accessor = accessorPack;
            return true;
        }

        void SetupContainer(EnvironmentParameters environmentParameters, ApplicationLogger logger)
        {
            var container = new DiContainer();

            container
                .Register<ILoggerFactory, ILoggerFactory>(DiLifecycle.Singleton, () => logger)
                .Register<ILogger, ApplicationLogger>(DiLifecycle.Singleton, () => logger)
            ;

        }

        public bool Initialize(IEnumerable<string> arguments)
        {
            InitializeEnvironmentVariable();

            var commandLine = CreateCommandLine(arguments);
            var environmentParameters = InitializeEnvironment(commandLine);
            var logger = CreateLogger(commandLine.GetValue(CommandLineKeyLog, string.Empty));
            OutputStartupLog(logger);

            var isFirstStartup = IsFirstStartup(environmentParameters, logger);
            if(isFirstStartup) {
                logger.Information("初回実行");
                // 設定ファイルやらなんやらを構築する前に完全初回の使用許諾を取る
                var dialogResult = ShowAcceptView(new DiContainer(), logger);
                if(!dialogResult) {
                    // 初回の使用許諾を得られなかったのでばいちゃ
                    logger.Information("使用許諾得られず");
                    return false;
                }
            }
            InitializeFileSystem(environmentParameters, logger);

            if(isFirstStartup) {
                FirstSetup(environmentParameters, logger);
            }

            (DatabaseFactoryPack factory, DatabaseAccessorPack accessor) pack;
            if(!NormalSetup(out pack, environmentParameters, logger)) {
                // データぶっ壊れてる系
                FirstSetup(environmentParameters, logger);
                var retryResult = NormalSetup(out pack, environmentParameters, logger);
                if(!retryResult) {
                    throw new ApplicationException();
                }
            }

            SetupContainer(environmentParameters, logger);

            return false;
        }

        #endregion
    }
}
