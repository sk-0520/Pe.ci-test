using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Views.Startup;
using ContentTypeTextNet.Pe.Main.ViewModels.Startup;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Startup
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
            var startupRegister = new StartupRegister(LoggerFactory);
            return startupRegister.Exists();
        }

        public bool RegisterStartup()
        {
            var startupRegister = new StartupRegister(LoggerFactory);
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

        #region ContextElementBase

        protected override void InitializeImpl()
        {
            Logger.LogTrace("not impl");
        }

        #endregion

    }
}
