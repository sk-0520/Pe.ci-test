using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.ViewElement.Startup;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Startup
{
    public class StartupViewModel : SingleModelViewModelBase<StartupViewElement>
    {
        public StartupViewModel(StartupViewElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }
    }
}
