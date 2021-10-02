using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    internal class InterProcessCommunicationManager: DisposerBase
    {
        public InterProcessCommunicationManager(ApplicationInitializer initializer, StartupEventArgs e)
        {
            Logging = initializer.Logging ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.Logging));
            Logger = Logging.Factory.CreateLogger(GetType());
            Logger.LogInformation("START IPC!");

            ApplicationDiContainer = initializer.DiContainer ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.DiContainer));

            ClipboardManager = initializer.ClipboardManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.ClipboardManager));
            UserAgentManager = initializer.UserAgentManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.UserAgentManager));

            ApplicationDiContainer.Register<IClipboardManager, ClipboardManager>(ClipboardManager);
            ApplicationDiContainer.Register<IUserAgentManager, UserAgentManager>(UserAgentManager);
            ApplicationDiContainer.Register<IHttpUserAgentFactory, IHttpUserAgentFactory>(UserAgentManager);

            CommandLine = new CommandLine(e.Args, false);
            var commandLineIpcHandle = CommandLine.Add(longKey: CommandLineKeyIpcHandle, hasValue: true);
            var commandLineIpcMode = CommandLine.Add(longKey: CommandLineKeyIpcMode, hasValue: true);
            CommandLineIpcFile = CommandLine.Add(longKey: CommandLineKeyIpcFile, hasValue: true);

            Logger.LogInformation("コマンドライン解析開始: {0}", e.Args.JoinString(" "));
            if(!CommandLine.Parse()) {
                throw new ArgumentException("parse error: " + e.Args.JoinString(" "), nameof(e) + "." + nameof(e.Args));
            }

            Logger.LogInformation("プロセス間通信処理方法取得開始");
            if(!CommandLine.Values.TryGetValue(commandLineIpcMode, out var ipcModeValue)) {
                throw new ArgumentException(commandLineIpcMode.ToString(), nameof(e) + "." + nameof(e.Args));
            }
            if(!Enum.TryParse<IpcMode>(ipcModeValue.First, out var ipcMode)) {
                throw new ArgumentException($"{nameof(ipcModeValue)}: {ipcModeValue} not defined {nameof(IpcMode)}", nameof(e) + "." + nameof(e.Args));
            }
            IpcMode = ipcMode;
            Logger.LogInformation("プロセス間通信処理方法: {0}", IpcMode);

            Logger.LogInformation("パイプハンドル取得開始");
            if(!CommandLine.Values.TryGetValue(commandLineIpcHandle, out var ipcHandleValue)) {
                throw new ArgumentException(commandLineIpcHandle.ToString(), nameof(e) + "." + nameof(e.Args));
            }
            Logger.LogInformation("パイプハンドル: {0}", ipcHandleValue.First);
            IpcPipeHandle = ipcHandleValue.First;
        }

        #region property
        public static string CommandLineKeyIpcHandle { get; } = "ipc-handle";
        public static string CommandLineKeyIpcMode { get; } = "ipc-mode";
        public static string CommandLineKeyIpcFile { get; } = "ipc-file";

        ApplicationLogging Logging { get; set; }
        ILogger Logger { get; set; }
        ILoggerFactory LoggerFactory => Logging.Factory;
        ApplicationDiContainer ApplicationDiContainer { get; set; }

        ClipboardManager ClipboardManager { get; set; }
        UserAgentManager UserAgentManager { get; set; }

        CommandLine CommandLine { get; }

        IpcMode IpcMode { get; }
        string IpcPipeHandle { get; }

        #region 各処理ごとのコマンドライン

        public CommandLineKey CommandLineIpcFile { get; }
        #endregion


        #endregion

        #region function

        private void ExecutePluginStatus(TextWriter writer)
        {
            //JsonNetCoreSerializer
            throw new NotImplementedException();
        }

        public void Execute()
        {
            using var pipeClientStream = new AnonymousPipeClientStream(PipeDirection.Out, IpcPipeHandle);
            using var writer = new StreamWriter(pipeClientStream);

            switch(IpcMode) {
                case IpcMode.PluginStatus:
                    ExecutePluginStatus(writer);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
            }

            base.Dispose(disposing);

            Logger.LogInformation("END IPC!");
        }

        #endregion
    }
}
