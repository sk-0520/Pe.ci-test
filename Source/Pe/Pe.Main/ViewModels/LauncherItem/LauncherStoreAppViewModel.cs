using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem
{
    public abstract class LauncherStoreAppViewModel : LauncherDetailViewModelBase
    {
        protected LauncherStoreAppViewModel(LauncherItemElement model, IScreen screen, IDispatcherWrapper dispatcherWrapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, screen, dispatcherWrapper, launcherToolbarTheme, loggerFactory)
        {
        }

        #region property

        #endregion

        #region command

        #endregion

        #region function

        #endregion

        #region LauncherDetailViewModelBase

        protected override Task ExecuteMainImplAsync()
        {
            return Task.Run(() => {

            });
        }

        protected override Task InitializeImplAsync()
        {
            return Task.Run(() => {

            });
        }

        #endregion
    }
}
