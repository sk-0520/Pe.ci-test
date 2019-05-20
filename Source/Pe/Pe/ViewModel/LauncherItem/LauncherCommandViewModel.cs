using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Theme;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherItem
{
    public class LauncherCommandViewModel : LauncherDetailViewModelBase
    {
        public LauncherCommandViewModel(LauncherItemElement model, Screen screen, IDispatcherWapper dispatcherWapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, screen, dispatcherWapper, launcherToolbarTheme, loggerFactory)
        { }

        #region property
        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region LauncherItemViewModelBase

        protected override Task InitializeImplAsync() => throw new NotImplementedException();

        protected override Task ExecuteMainImplAsync() => throw new NotImplementedException();

        #endregion
    }
}
