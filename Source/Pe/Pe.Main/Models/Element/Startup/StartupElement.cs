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
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;

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
        public bool IsRegisteredLauncher { get; private set; }

        #endregion

        #region function

        public bool ExistsStartup()
        {
            ThrowIfDisposed();

            var startupRegister = new StartupRegister(LoggerFactory);
            return startupRegister.Exists();
        }

        public bool RegisterStartup()
        {
            ThrowIfDisposed();

            var startupRegister = new StartupRegister(LoggerFactory);
            return startupRegister.Register(new StartupParameter());
        }

        public void ShowImportProgramsView()
        {
            ThrowIfDisposed();

            using(var diContainer = UsingChildServiceLocator()) {
                diContainer
                    .RegisterMvvm<ImportProgramsElement, ImportProgramsViewModel, ImportProgramsWindow>()
                ;
                var importProgramsModel = diContainer.New<ImportProgramsElement>();
                var view = diContainer.Make<ImportProgramsWindow>();

                WindowManager.Register(new WindowItem(WindowKind.ImportPrograms, importProgramsModel, view));

                view.ShowDialog();
                if(importProgramsModel.IsRegisteredLauncher) {
                    IsRegisteredLauncher = true;
                }
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
