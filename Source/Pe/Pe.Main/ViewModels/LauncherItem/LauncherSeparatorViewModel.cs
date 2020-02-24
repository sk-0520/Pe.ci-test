using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem
{
    public class LauncherSeparatorViewModel : LauncherDetailViewModelBase
    {
        public LauncherSeparatorViewModel(LauncherItemElement model, IScreen screen, IDispatcherWrapper dispatcherWrapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, screen, dispatcherWrapper, launcherToolbarTheme, loggerFactory)
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
