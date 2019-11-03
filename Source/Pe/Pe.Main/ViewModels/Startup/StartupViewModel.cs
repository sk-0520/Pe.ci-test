using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Startup;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Main.Models.Platform;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Startup
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
                var systemExecutor = new SystemExecutor(LoggerFactory);
                systemExecutor.OpenNotificationAreaHistory();
            }
        ));



        #endregion
    }
}
