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
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public delegate void ExecuteIpcDelegate(CommandLine commandLine, string output);

    /// <summary>
    /// 本体アプリケーション起動処理。
    /// </summary>
    /// <remarks>
    /// <para>プロセス間通信処理しかしない。</para>
    /// </remarks>
    public class ApplicationBoot
    {
        public ApplicationBoot(EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
        {
            EnvironmentParameters = environmentParameters;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        private EnvironmentParameters EnvironmentParameters { get; }

        /// <summary>
        /// 本体起動コマンドパス。
        /// </summary>
        /// <remarks>
        /// <para>Pe.Main.exe と話せればいいので上位階層の Pe.exe を指定する必要なし。</para>
        /// </remarks>
        public static string CommandPath { get; } = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "exe");

        #endregion

        #region function

        /// <summary>
        /// プロセス間通信起動。
        /// </summary>
        /// <remarks>
        /// <para>現在のユーザー・端末・一時ディレクトリ情報が引き継がれ、強制ログ出力(<see cref="EnvironmentParameters.TemporaryIpcLogFile"/>)が行われる。</para>
        /// </remarks>
        /// <param name="ipcMode">プロセス間通信内容。</param>
        /// <param name="keyValueArguments">コマンドライン引数(キー:値)</param>
        /// <param name="switchArguments">コマンドライン引数(スイッチ, -- は不要)</param>
        /// <param name="action">結果データ受領。</param>
        /// <returns></returns>
        public bool TryExecuteIpc(IpcMode ipcMode, IEnumerable<KeyValuePair<string,string>> keyValueArguments, IEnumerable<string> switchArguments, ExecuteIpcDelegate action)
        {
            try {
                using var pipeServerStream = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable);

                var ipcArgs = new Dictionary<string, string> {
                    [ApplicationInitializer.CommandLineKeyRunMode] = RunModeUtility.ToString(RunMode.InterProcessCommunication),
                    [EnvironmentParameters.CommandLineKeyUserDirectory] = EnvironmentParameters.UserRoamingDirectory.FullName,
                    [EnvironmentParameters.CommandLineKeyMachineDirectory] = EnvironmentParameters.MachineDirectory.FullName,
                    [EnvironmentParameters.CommandLineKeyTemporaryDirectory] = EnvironmentParameters.TemporaryDirectory.FullName,
                    [ApplicationInitializer.CommandLineKeyLog] = EnvironmentParameters.TemporaryIpcLogFile.FullName,
                    [InterProcessCommunicationManager.CommandLineKeyIpcHandle] = pipeServerStream.GetClientHandleAsString(),
                    [InterProcessCommunicationManager.CommandLineKeyIpcMode] = ipcMode.ToString(),
                };

                var ipcSwitchArgs = new List<string>() {
                    ApplicationInitializer.CommandLineSwitchForceLog,
                };
                ipcSwitchArgs.AddRange(switchArguments);

                foreach(var keyValue in keyValueArguments) {
                    ipcArgs[keyValue.Key] = keyValue.Value;
                }

                var args = CommandLine.ToCommandLineArguments(ipcArgs)
                    .Concat(ipcSwitchArgs.Select(i => "--" + i))
                ;
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
                if(!process.WaitForExit(EnvironmentParameters.ApplicationConfiguration.General.IpcWait)) {
                    throw new TimeoutException();
                }
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
            if(process is not null) {
                process.Exited -= Process_Exited;
            }
        }

        #endregion
    }
}
