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
using ContentTypeTextNet.Pe.Main.Model.Applications;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
{
    public class ApplicationManager
    {
        public ApplicationManager()
        { }

        #region property

        ApplicationLogger ApplicationLogger { get; set; }
        ApplicationDiContainer ApplicationDiContainer { get; set; }

        ILogger Logger { get; set; }

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
                view.ShowDialog();

            }
        }

        public bool Startup(App app, StartupEventArgs e)
        {
            var initializer = new ApplicationInitializer();
            if(!initializer.Initialize(e.Args)) {
                return false;
            }

            ApplicationLogger = initializer.Logger;
            ApplicationDiContainer = initializer.DiContainer;

            Logger = ApplicationLogger.Factory.CreateCurrentClass();
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
