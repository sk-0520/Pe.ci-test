using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Theme;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherItem
{
    public class LauncherScriptViewModel : LauncherDetailViewModelBase
    {
        public LauncherScriptViewModel(LauncherItemElement model, IDispatcherWapper dispatcherWapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, dispatcherWapper, launcherToolbarTheme, loggerFactory)
        { }

        #region property
        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region LauncherItemViewModelBase
        protected override Task InitializeAsyncImpl() => throw new NotImplementedException();
        protected override Task ExecuteMainImplAsync() => throw new NotImplementedException();
        #endregion
    }

}
