using System.Threading;
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

        private ICommand? _ImportProgramsCommand;
        public ICommand ImportProgramsCommand => this._ImportProgramsCommand ??= new DelegateCommand(
        async () => {
                await Model.ShowImportProgramsViewAsync(CancellationToken.None);
                if(Model.IsRegisteredLauncher) {
                    RaisePropertyChanged(nameof(IsRegisteredLauncher));
                }
            }
        );

        private ICommand? _RegisterStartupCommand;
        public ICommand RegisterStartupCommand => this._RegisterStartupCommand ??= new DelegateCommand(
            () => {
                if(!Model.ExistsStartup()) {
                    Model.RegisterStartup();
                }
                // 状況によらずマークだけつける
                IsRegisteredStartup = true;
            }
        );

        private ICommand? _ShowNotificationAreaCommand;
        public ICommand ShowNotificationAreaCommand => this._ShowNotificationAreaCommand ??= new DelegateCommand(
            () => {
                var systemExecutor = new SystemExecutor();
                systemExecutor.OpenNotificationAreaHistory();
            }
        );

        #endregion
    }
}
