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
        #region variable

        private bool _isRegisteredStartup;

        #endregion

        public StartupViewModel(StartupElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public bool IsRegisteredLauncher => Model.IsRegisteredLauncher;
        public bool IsRegisteredStartup
        {
            get => this._isRegisteredStartup;
            private set => SetProperty(ref this._isRegisteredStartup, value);
        }

        #endregion

        #region command

        public ICommand ImportProgramsCommand => GetOrCreateCommand(() => new DelegateCommand(
            async () => {
                await Model.ShowImportProgramsViewAsync();
                if(Model.IsRegisteredLauncher) {
                    RaisePropertyChanged(nameof(IsRegisteredLauncher));
                }
            }
        ));

        public ICommand RegisterStartupCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                if(!Model.ExistsStartup()) {
                    Model.RegisterStartup();
                }
                // 状況によらずマークだけつける
                IsRegisteredStartup = true;
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
