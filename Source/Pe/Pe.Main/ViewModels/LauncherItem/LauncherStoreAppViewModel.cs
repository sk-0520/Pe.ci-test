using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem
{
    public abstract class LauncherStoreAppViewModel: LauncherDetailViewModelBase
    {
        protected LauncherStoreAppViewModel(LauncherItemElement model, IScreen screen, IKeyGestureGuide keyGestureGuide, IDispatcherWrapper dispatcherWrapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, screen, keyGestureGuide, dispatcherWrapper, launcherToolbarTheme, loggerFactory)
        { }

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

        protected override Task LoadImplAsync()
        {
            return Task.Run(() => {

            });
        }

        #endregion
    }
}
