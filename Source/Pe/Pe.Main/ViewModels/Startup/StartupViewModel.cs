using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Startup;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Startup
{
    public class StartupViewModel: ElementViewModelBase<StartupElement>
    {
        public StartupViewModel(StartupElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        { }

        #region property



        #endregion

        #region command

        public ICommand ImportProgramsCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.ShowImportProgramsView();
            }
        ));

        public ICommand RegisterStartupCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                if(!Model.ExistsStartup()) {
                    Model.RegisterStartup();
                }
            }
        ));

        public ICommand ShowNotificationAreaCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var systemExecutor = new SystemExecutor();
                systemExecutor.OpenNotificationAreaHistory();
            }
        ));



        #endregion
    }
}
