using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Element.Toolbar;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.ViewModel.Manager;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
{
    public class ApplicationManager : DisposerBase
    {
        public ApplicationManager()
        { }

        #region property

        ApplicationLogger ApplicationLogger { get; set; }
        ApplicationDiContainer ApplicationDiContainer { get; set; }

        ILogger Logger { get; set; }

        WindowManager WindowManager { get; set; }
        NotifyManager NotifyManager { get; set; }

        IList<LauncherToolbarElement> LauncherToolbars { get; } = new ObservableCollection<LauncherToolbarElement>();

        #endregion

        #region function

        void ShowStartupView()
        {
            using(var diContainer = ApplicationDiContainer.CreateChildContainer()) {
                diContainer
                    .RegisterLogger(Logger)
                    .RegisterMvvm<Element.Startup.StartupElement, ViewModel.Startup.StartupViewModel, View.Startup.StartupWindow>()
                ;
                var startupModel = diContainer.New<Element.Startup.StartupElement>();
                var view = diContainer.Make<View.Startup.StartupWindow>();

                var windowManager = diContainer.Get<IWindowManager>();
                windowManager.Register(new WindowItem(WindowKind.Startup, view));

                view.ShowDialog();

            }
        }

        void RegisterManagers()
        {
            Debug.Assert(ApplicationDiContainer != null);

            ApplicationDiContainer.Register<IWindowManager, WindowManager>(WindowManager);
            ApplicationDiContainer.Register<INotifyManager, NotifyManager>(NotifyManager);

        }

        public bool Startup(App app, StartupEventArgs e)
        {
            var initializer = new ApplicationInitializer();
            if(!initializer.Initialize(e.Args)) {
                return false;
            }

            ApplicationLogger = initializer.Logger;
            ApplicationDiContainer = initializer.DiContainer;
            WindowManager = initializer.WindowManager;
            NotifyManager = initializer.NotifyManager;

            RegisterManagers();

            Logger = ApplicationLogger.Factory.CreateCurrentClass();
            Logger.Debug("初期化完了");

            if(initializer.IsFirstStartup) {
                // 初期登録の画面を表示
                ShowStartupView();
            }

            return true;
        }

        public ManagerViewModel CreateViewModel()
        {
            var viewModel = new ManagerViewModel(this, Logger.Factory);
            return viewModel;
        }

        LauncherToolbarElement CreateLauncherToolbarElement(Screen screen)
        {
            var element = ApplicationDiContainer.Make<LauncherToolbarElement>();
            return element;
        }

        void BuildLauncherToolbars()
        {
            var screens = Screen.AllScreens;
            foreach(var screen in screens) {
                var element = CreateLauncherToolbarElement(screen);
            }
        }

        public void Execute()
        {
            Logger.Information("がんばる！");

            // ツールバーの生成
            BuildLauncherToolbars();

            // ノートの生成
        }

        public void Exit()
        {
            Logger.Information("おわる！");

            Application.Current.Shutdown();
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    //...
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
