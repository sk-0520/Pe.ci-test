using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.IconViewer
{
    public class CustomLauncherIconViewerViewModel: ViewModelBase, ILauncherItemId
    {
        #region variable

        object? _icon;

        #endregion

        internal CustomLauncherIconViewerViewModel(Guid launcherItemId, ILauncherItemExtension launcherItemExtension, LauncherItemAddonContextFactory launcherItemAddonContextFactory, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;
            LauncherItemExtension = launcherItemExtension;
            LauncherItemAddonContextFactory = launcherItemAddonContextFactory;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        ILauncherItemExtension LauncherItemExtension { get; }
        LauncherItemAddonContextFactory LauncherItemAddonContextFactory { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        public object? Icon
        {
            get => this._icon;
            private set => SetProperty(ref this._icon, value);
        }

        #endregion

        #region function

        //public async Task LoadAsync(IconScale iconScale, LauncherItemIconMode launcherItemIconMode, CancellationToken cancellationToken)
        //{
        //    var context = new LauncherItemAddonContextFactory()
        //    Icon = LauncherItemExtension.GetIcon(launcherItemIconMode, iconScale, )
        //}

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion
    }
}
