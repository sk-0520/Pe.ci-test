using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem
{
    public static class LauncherItemViewModelFactory
    {
        #region function

        public static LauncherDetailViewModelBase Create(LauncherItemElement model, IScreen screen, IKeyGestureGuide keyGestureGuide, IDispatcherWrapper dispatcherWrapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
        {
            switch(model.Kind) {
                case LauncherItemKind.File:
                    return new LauncherFileViewModel(model, screen, keyGestureGuide, dispatcherWrapper, launcherToolbarTheme, loggerFactory);

                //case LauncherItemKind.StoreApp:
                //    return new LauncherStoreAppViewModel(model, screen, keyGestureGuide, dispatcherWrapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.Addon:
                    return new LauncherAddonViewModel(model, screen, keyGestureGuide, dispatcherWrapper, launcherToolbarTheme, loggerFactory);

                case LauncherItemKind.Separator:
                    return new LauncherSeparatorViewModel(model, screen, keyGestureGuide, dispatcherWrapper, launcherToolbarTheme, loggerFactory);

                default:
                    throw new NotImplementedException();
            }

        }


        #endregion
    }
}
