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
using ContentTypeTextNet.Pe.Main.Model.Element;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.View.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModel.Manager;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
{
    public partial class ApplicationManager : DisposerBase, IOrderManager, INotifyManager
    {
        public ApplicationManager()
        { }

        #region property

        ApplicationLogger ApplicationLogger { get; set; }
        ApplicationDiContainer ApplicationDiContainer { get; set; }

        ILogger Logger { get; set; }

        WindowManager WindowManager { get; set; }
        OrderManagerIml OrderManager { get; set; }
        NotifyManagerImpl NotifyManager { get; set; }

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
            ApplicationDiContainer.Register<IOrderManager, IOrderManager>(this);
            ApplicationDiContainer.Register<INotifyManager, INotifyManager>(this);

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
            OrderManager = ApplicationDiContainer.Make<OrderManagerIml>(); //initializer.OrderManager;
            NotifyManager = ApplicationDiContainer.Make<NotifyManagerImpl>();//initializer.NotifyManager;

            RegisterManagers();

            Logger = ApplicationLogger.Factory.CreateTartget(GetType());
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

        IReadOnlyList<IWindowShowStarter> CreateLauncherToolbars()
        {
            var screens = Screen.AllScreens;
            var result = new List<IWindowShowStarter>(screens.Length);

            foreach(var screen in screens) {
                var element = (LauncherToolbarElement)CreateElement(new OrderElementParameter<Screen>(ElementKind.LauncherToolbar, screen));

                LauncherToolbars.Add(element);

                result.Add(element);
            }

            return result;
        }

        public void Execute()
        {
            Logger.Information("がんばる！");

            // ツールバーの生成
            var toolbarWindowStaters = CreateLauncherToolbars();

            // ノートの生成

            var windowStaters = toolbarWindowStaters
                .Concat(Enumerable.Empty<IWindowShowStarter>())
                .Where(i => i.CanStartShowWindow)
                .ToList()
            ;
            foreach(var windowStater in windowStaters) {
                windowStater.StartShowWindow();
            }
        }

        public void Exit()
        {
            Logger.Information("おわる！");

            Application.Current.Shutdown();
        }

        #endregion

        #region IOrderManager

        public ElementBase CreateElement(OrderElementParameter parameter)
        {
            return OrderManager.CreateElement(parameter);
        }

        public WindowItem CreateWindow(OrderWindowParameter parameter)
        {
            var windowItem = OrderManager.CreateWindow(parameter);

            WindowManager.Register(windowItem);

            return windowItem;
        }

        #endregion

        #region INotifyManager
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
