using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Model;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Model.Applications
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

        ILogger CreateLogger(string outputPath, bool createDirectory, [CallerFilePath] string callerFilePath = "")
        {
            var loggerFactory = new LoggerFactory();
            //loggerFactory.AddProvider(new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider());
            //loggerFactory.AddProvider(new Microsoft.Extensions.Logging.)
            //TODO: 場所
            NLog.LogManager.LoadConfiguration("etc/nlog.config");
            //new NLog.Config.LoggingConfiguration()
            var s = NLog.LogManager.Configuration.AllTargets;

            var op = new NLog.Extensions.Logging.NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true };
            var prov = new NLog.Extensions.Logging.NLogLoggerProvider(op, NLog.LogManager.LogFactory);
            loggerFactory.AddProvider(prov);

            //NLog.Extensions.Logging.ConfigSettingLayoutRenderer.DefaultConfiguration

            var logger = loggerFactory.CreateLogger(GetType());
            logger.LogTrace("うんち");
            logger.LogDebug("うんち");
            logger.LogInformation("うんち");
            logger.LogWarning("うんち");
            logger.LogError("うんち");
            return logger;
#if false
            // ログ出力(ファイル・ディレクトリが存在しなければ終了で構わない)
            if(!string.IsNullOrWhiteSpace(outputPath)) {
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
                var expandedOutputPath = Environment.ExpandEnvironmentVariables(outputPath);
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。

                if(createDirectory) {
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
                    var fileName = Path.GetFileName(expandedOutputPath)!;
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。
                    if(fileName.IndexOf('.') == -1) {
                              // 拡張子がなければディレクトリ指定と決めつけ
                        Directory.CreateDirectory(expandedOutputPath);
                    } else {
                        var parentDir = Path.GetDirectoryName(expandedOutputPath);
                        Directory.CreateDirectory(parentDir);
                    }
                }

                // ディレクトリ指定であればタイムスタンプ付きでファイル生成
                string filePath = expandedOutputPath;
                if(Directory.Exists(expandedOutputPath)) {
                    var fileName = PathUtility.AppendExtension(DateTime.Now.ToString("yyyy-MM-dd_HHmmss"), "log");
                    filePath = Path.Combine(expandedOutputPath, fileName);
                }
                var writer = new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read), Encoding.Unicode) {
                    AutoFlush = true,
                };
            }

            return logger;
#endif
        }

        public void Initialize(App app, StartupEventArgs e)
        {
            InitializeEnvironmentVariable();

            var commandLine = CreateCommandLine(e.Args);
            var environmentParameters = InitializeEnvironment(commandLine);

            var logger = CreateLogger(commandLine.GetValue(CommandLineKeyLog, string.Empty), commandLine.ExistsSwitch(CommandLineSwitchForceLog));

        }

        #endregion
    }
}
