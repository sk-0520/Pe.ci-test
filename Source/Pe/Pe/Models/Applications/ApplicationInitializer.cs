using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class ApplicationInitializer
    {
        #region property

        string CommandLineKeyLog { get; } = "log";
        string CommandLineSwitchForceLog { get; } = "force-log";

        public bool IsFirstStartup { get; private set; }

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

            return true;
        }

        #endregion
    }
}
