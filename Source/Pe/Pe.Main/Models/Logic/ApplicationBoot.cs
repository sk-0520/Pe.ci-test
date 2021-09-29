using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
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
        ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        ILogger Logger { get; }

        /// <summary>
        /// 本体起動コマンドパス。
        /// </summary>
        public static string CommandPath { get; } = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "exe");

        #endregion

        #region function

        public bool TryExecuteIpc(ApplicationSpecialExecuteIpcMode ipcMode, IEnumerable<string> arguments, ExecuteIpcDelegate action)
        {
            try {
                using var pipeServerStream = new AnonymousPipeServerStream(PipeDirection.In);

                var argument = new Dictionary<string, string> {
                    ["_mode"] = "IPC",
                    ["ipc-handle"] = pipeServerStream.GetClientHandleAsString(),
                    ["ipc-mode"] = ipcMode.ToString(),
                }.ToCommandLineArguments().Concat(
                    arguments.Select(i => CommandLine.Escape(i))
                ).JoinString(" ");

                using var process = new Process();
                process.StartInfo.FileName = CommandPath;
                process.StartInfo.Arguments = argument;

                using var pipeServerReader = new StreamReader(pipeServerStream);

                var commandLine = new CommandLine(arguments, false);

                process.Start();

                var output = pipeServerReader.ReadToEnd();

                action(commandLine, output);

                return true;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
            }

            return false;
        }

        #endregion
    }
}
