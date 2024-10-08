//#define NOT_IPC
#if !DEBUG && NOT_IPC
#error  NOT_IPC
#endif

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
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Core.Models.Serialization;
using ContentTypeTextNet.Pe.Library.Base.Linq;

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

            ApplicationDiContainer.Register<IPlatformTheme, PlatformThemeLoader>(DiLifecycle.Singleton);
            var factory = new ApplicationDatabaseFactoryPack(
                new ApplicationDatabaseFactory(true, false),
                new ApplicationDatabaseFactory(true, false),
                new ApplicationDatabaseFactory(true, false)
            );
            var delayWriterWaitTimePack = new DelayWriterWaitTimePack(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();
            ApplicationDiContainer.Register<IDatabaseStatementLoader, ApplicationDatabaseStatementLoader>(new ApplicationDatabaseStatementLoader(environmentParameters.MainSqlDirectory, TimeSpan.FromMinutes(6), null, environmentParameters.ApplicationConfiguration.File.GivePriorityToFile, LoggerFactory));
            ApplicationDiContainer.RegisterDatabase(factory, delayWriterWaitTimePack, LoggerFactory);

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

#if !NOT_IPC
            Logger.LogInformation("パイプハンドル取得開始");
            if(!CommandLine.Values.TryGetValue(commandLineIpcHandle, out var ipcHandleValue)) {
                throw new ArgumentException(commandLineIpcHandle.ToString(), nameof(e) + "." + nameof(e.Args));
            }
            Logger.LogInformation("パイプハンドル: {0}", ipcHandleValue.First);
            IpcPipeHandle = ipcHandleValue.First;
#endif
        }

        #region property

        /// <summary>
        /// プロセス間通信 パイプハンドル。
        /// </summary>
        /// <remarks>
        /// <para>必須。</para>
        /// </remarks>
        public static string CommandLineKeyIpcHandle { get; } = "ipc-handle";
        /// <summary>
        /// プロセス間通信 処理モード。
        /// <para>必須。</para>
        /// </summary>
        public static string CommandLineKeyIpcMode { get; } = "ipc-mode";
        /// <summary>
        /// プロセス間通信 対象ファイル。
        /// </summary>
        /// <remarks>
        /// <para><see cref="CommandLineKeyIpcMode"/>依存。</para>
        /// </remarks>
        public static string CommandLineKeyIpcFile { get; } = "ipc-file";

        private ApplicationLogging Logging { get; set; }
        private ILogger Logger { get; set; }
        private ILoggerFactory LoggerFactory => Logging.Factory;
        private ApplicationDiContainer ApplicationDiContainer { get; set; }

        private ClipboardManager ClipboardManager { get; set; }
        private UserAgentManager UserAgentManager { get; set; }

        private CommandLine CommandLine { get; }

        private IpcMode IpcMode { get; }
#if !NOT_IPC
        private string IpcPipeHandle { get; }
#endif

        #region 各処理ごとのコマンドライン

        public CommandLineKey CommandLineIpcFile { get; }

        #endregion

        #endregion

        #region function

        private object ExecutePluginStatus()
        {
            Logger.LogInformation("プラグインステータス取得");
            //var serializer = new JsonTextSerializer();

            var addonContainer = ApplicationDiContainer.Build<AddonContainer>();
            var themeContainer = ApplicationDiContainer.Build<ThemeContainer>();
            var pluginContainer = ApplicationDiContainer.Build<PluginContainer>(addonContainer, themeContainer);
            var pluginInstaller = new PluginInstaller(
                pluginContainer,
                ApplicationDiContainer.Build<PluginConstructorContext>(),
                Logging.PauseReceiveLog,
                ApplicationDiContainer.Build<EnvironmentParameters>(),
                ApplicationDiContainer.Build<IDatabaseStatementLoader>(),
                ApplicationDiContainer.Build<LoggerFactory>()
            );


            var pluginInstallData = pluginInstaller.LoadPluginInfo(
                new FileInfo(CommandLine.Values[CommandLineIpcFile].First)
            );

            return pluginInstallData;
        }

        public void Execute()
        {
#if !NOT_IPC
            using var pipeClientStream = new AnonymousPipeClientStream(PipeDirection.Out, IpcPipeHandle);
#else
            using var pipeClientStream = new MemoryReleaseStream();
#endif
            var responseObject = IpcMode switch {
                IpcMode.GetPluginStatus => ExecutePluginStatus(),
                _ => throw new NotImplementedException(),
            };

            var serializer = new JsonTextSerializer();
            serializer.Save(responseObject, pipeClientStream);
#if NOT_IPC
            pipeClientStream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(pipeClientStream);
            var result = reader.ReadToEnd();
            Logger.LogInformation("result: {result}", result);
#endif
            //using var fs = new FileStream(@"x:\a.json", FileMode.Create);
            //serializer.Save(responseObject, fs);
        }

#endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Logger.LogInformation("END IPC!");
            }

            base.Dispose(disposing);

            Logger.LogInformation("END IPC!");
        }

        #endregion
    }
}
