using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.Model.Manager;
using ContentTypeTextNet.Pe.Main.View.Startup;
using ContentTypeTextNet.Pe.Main.ViewModel.Startup;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Startup
{
    public class StartupElement : ContextElementBase
    {
        public StartupElement(IWindowManager windowManager, IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            WindowManager = windowManager;
        }

        #region property

        IWindowManager WindowManager { get; }

        #endregion

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
            using(var diContainer = UsingChildServiceLocator()) {
                diContainer
                    .RegisterMvvm<ImportProgramsElement, ImportProgramsViewModel, ImportProgramsWindow>()
                ;
                var importProgramsModel = diContainer.New<ImportProgramsElement>();
                var view = diContainer.Make<ImportProgramsWindow>();

                WindowManager.Register(new WindowItem(WindowKind.ImportPrograms, view));

                view.ShowDialog();
            }
        }

        #endregion
    }
}
