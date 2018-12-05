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

            commandLine.Execute();

            return commandLine;
        }

        void InitializeEnvironment(CommandLine commandLine)
        {
            Debug.Assert(commandLine.IsParsed);

            var applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            EnvironmentParameters.Initialize(new DirectoryInfo(applicationDirectory), commandLine);
        }

        bool IsFirstStartup()
        {
            var file = EnvironmentParameters.Current.SettingFile;
            file.Refresh();
            return !file.Exists;
        }

        bool ShowAcceptView(ILogger logger)
        {
            // ログがあったりなかったりするフワフワ状態なので一時的にDIコンテナ作成(嬉しがってめちゃくちゃ生成)
            using(var diContainer = DiContainer.Current?.Scope() ?? new DiContainer().Scope()) {
                diContainer.Register<ILogger, ILogger>(() => logger, DiLifecycle.Singleton);
                diContainer.Register<ILogFactory, ILogFactory>(() => logger, DiLifecycle.Singleton);
                diContainer.Register<ViewElement.Accept.AcceptViewElement, ViewElement.Accept.AcceptViewElement>(DiLifecycle.Singleton);
                diContainer.Register<ViewModel.Accept.AcceptViewModel, ViewModel.Accept.AcceptViewModel>(DiLifecycle.Transient);
                diContainer.DirtyRegister<View.Accept.AcceptWindow, ViewModel.Accept.AcceptViewModel>(nameof(System.Windows.FrameworkElement.DataContext));

                var acceptModel = diContainer.New<ViewElement.Accept.AcceptViewElement>();
                var view = diContainer.Make<View.Accept.AcceptWindow>();
                view.ShowDialog();

                return acceptModel.Accepted;
            }
        }

        void CreateLogger()
        {

        }

        void Startup()
        {
            var container = new DiContainer();

            container
                .Register<ILogger, ApplicationLogger>(DiLifecycle.Singleton)
            ;

        }

        void FirstSetup()
        {
        }

        void ExecuteSetup()
        {

        }

        public bool Initialize(IEnumerable<string> arguments)
        {
            InitializeEnvironmentVariable();

            var commandLine = CreateCommandLine(arguments);
            InitializeEnvironment(commandLine);

            var isFirstStartup = IsFirstStartup();
            if(isFirstStartup) {
                // 設定ファイルやらなんやらを構築する前に完全初回の使用許諾を取る
                var dialogResult = ShowAcceptView(new Library.Shared.Link.Model.NullLogger());
                if(!dialogResult) {
                    // 初回の使用許諾を得られなかったのでばいちゃ
                    return false;
                }
                Startup();
            }

            return false;
        }

        #endregion
    }
}
