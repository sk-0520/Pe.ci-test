using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model
{
    public class ApplicationManager
    {
        public ApplicationManager()
        { }

        #region property

        ILogger Logger { get; set; }

        #endregion

        #region function

        void ShowStartupView()
        {
            using(var diContainer = DiContainer.Instance.Scope()) {
                var childLogger = Logger.Factory.CreateCurrentMethod();
                diContainer
                    .Register<ViewElement.Startup.StartupViewElement>(DiLifecycle.Singleton)
                    .Register<ViewModel.Startup.StartupViewModel>(DiLifecycle.Transient)
                    .Register<ILogger, ILogger>(childLogger)
                    .Register<ILoggerFactory, ILoggerFactory>(childLogger.Factory)
                    .DirtyRegister<View.Startup.StartupWindow, ViewModel.Startup.StartupViewModel>(nameof(System.Windows.FrameworkElement.DataContext))
                ;

                var startupModel = diContainer.New<ViewElement.Startup.StartupViewElement>();
                var view = diContainer.Make<View.Startup.StartupWindow>();
                view.ShowDialog();

            }
        }

        public bool Startup(App app, StartupEventArgs e)
        {
            var initializer = new ApplicationInitializer();
            if(!initializer.Initialize(e.Args)) {
                return false;
            }

            Logger = DiContainer.Instance.Get<ILoggerFactory>().CreateCurrentClass();
            Logger.Debug("初期化完了");

            if(initializer.IsFirstStartup||true) {
                // 初期登録の画面を表示
                ShowStartupView();
            }

            return true;
        }

        #endregion
    }
}
