using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.View.Startup;
using ContentTypeTextNet.Pe.Main.ViewModel.Startup;

namespace ContentTypeTextNet.Pe.Main.Model.ViewElement.Startup
{
    public class StartupViewElement : ViewElementBase
    {
        public StartupViewElement(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region function

        public bool ExistsStartup()
        {
            var startupRegister = new StartupRegister(Logger.Factory);
            return startupRegister.Exists();
        }

        public bool RegisterStartup()
        {
            var startupRegister = new StartupRegister(Logger.Factory);
            return startupRegister.Register();
        }

        public void ShowImportProgramsView()
        {
            using(var diContainer = DiContainer.Instance.Scope()) {
                var childLogger = Logger.Factory.CreateCurrentMethod();
                diContainer
                    .Register<ImportProgramsViewElement>(DiLifecycle.Singleton)
                    .Register<ImportProgramsViewModel>(DiLifecycle.Transient)
                    .Register<ILogger, ILogger>(childLogger)
                    .Register<ILoggerFactory, ILoggerFactory>(childLogger.Factory)
                    .DirtyRegister<ImportProgramsWindow, ImportProgramsViewModel>(nameof(System.Windows.FrameworkElement.DataContext))
                ;

                var importProgramsModel = diContainer.New<ImportProgramsViewElement>();
                var view = diContainer.Make<ImportProgramsWindow>();
                view.ShowDialog();
            }
        }

        #endregion
    }
}
