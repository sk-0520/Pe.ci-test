using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Platform;
using ContentTypeTextNet.Pe.Main.Model.Element.Startup;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Startup
{
    public class StartupViewModel : SingleModelViewModelBase<StartupElement>
    {
        public StartupViewModel(StartupElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
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
