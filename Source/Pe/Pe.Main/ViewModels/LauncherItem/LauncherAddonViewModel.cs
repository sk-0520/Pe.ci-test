using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem
{
    public class LauncherAddonViewModel: LauncherDetailViewModelBase
    {
        public LauncherAddonViewModel(LauncherItemElement model, IScreen screen, IDispatcherWrapper dispatcherWrapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, screen, dispatcherWrapper, launcherToolbarTheme, loggerFactory)
        { }

        #region property
        LauncherAddonDetailData? Detail { get; set; }

        #endregion

        #region command

        #endregion

        #region LauncherDetailViewModelBase

        protected override Task InitializeImplAsync()
        {
            Detail = Model.LoadAddonDetail();
            return Task.CompletedTask;
        }

        protected override Task ExecuteMainImplAsync()
        {
            return Task.Run(() => {
                Model.Execute(Screen);
            });
        }

        protected override object GetIcon(IconKind iconKind)
        {
            var factory = Model.CreateLauncherIconFactory();
            var iconSource = factory.CreateIconSource(DispatcherWrapper);
            return factory.CreateView(iconSource, false, DispatcherWrapper);
        }

        #endregion
    }
}
