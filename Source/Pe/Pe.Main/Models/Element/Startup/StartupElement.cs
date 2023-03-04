using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.ViewModels.Startup;
using ContentTypeTextNet.Pe.Main.Views.Startup;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Startup
{
    public class StartupElement: ServiceLocatorElementBase
    {
        public StartupElement(IWindowManager windowManager, IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            WindowManager = windowManager;
        }

        #region property

        private IWindowManager WindowManager { get; }
        public bool IsRegisteredLauncher { get; private set; }
        public bool IsRegisteredStartup { get; private set; }

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
                importProgramsModel.Initialize();
                var view = diContainer.Build<ImportProgramsWindow>();

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
