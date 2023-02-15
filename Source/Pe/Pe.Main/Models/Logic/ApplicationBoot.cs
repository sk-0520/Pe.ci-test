using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public delegate void ExecuteIpcDelegate(CommandLine commandLine, string output);

    /// <summary>
    /// 本体アプリケーション起動処理。
    /// </summary>
    public class ApplicationBoot
    {
        public ApplicationBoot(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        /// <summary>
        /// 本体起動コマンドパス。
        /// </summary>
        public static string CommandPath { get; } = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "exe");

        #endregion

        #region function

        public bool TryExecuteIpc(IpcMode ipcMode, IEnumerable<string> arguments, ExecuteIpcDelegate action)
        {
            try {
                using var pipeServerStream = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable);

                var args = new Dictionary<string, string> {
                    [ApplicationInitializer.CommandLineKeyRunMode] = RunModeUtility.ToString(RunMode.InterProcessCommunication),
                    [InterProcessCommunicationManager.CommandLineKeyIpcHandle] = pipeServerStream.GetClientHandleAsString(),
                    [InterProcessCommunicationManager.CommandLineKeyIpcMode] = ipcMode.ToString(),
#if DEBUG
                    ["log"] = @"x:\a.log",
#endif
                }.ToCommandLineArguments().Concat(
                    arguments.Select(i => CommandLine.Escape(i))
                );
                var argument = args.JoinString(" ");

                using var process = new Process() {
                    EnableRaisingEvents = true,
                    StartInfo = new ProcessStartInfo() {
                        FileName = CommandPath,
                        Arguments = argument,
                        UseShellExecute = false,
                    },
                };
                process.Exited += Process_Exited;

                using var pipeServerReader = new StreamReader(pipeServerStream);

                var commandLine = new CommandLine(args, false);

                process.Start();
                pipeServerStream.DisposeLocalCopyOfClientHandle();
                var output = pipeServerReader.ReadToEnd();

                action(commandLine, output);

                return true;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
            }

            return false;
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            var process = (Process?)sender;
            Logger.LogInformation("プロセス終了: {0}", process);
        }

        #endregion
    }
}
