using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Standard.Base;
using System.IO;
using ContentTypeTextNet.Pe.Main.Models.Data;
using System.Windows;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem
{
    public class LauncherSeparatorViewModel: LauncherDetailViewModelBase
    {
        public LauncherSeparatorViewModel(LauncherItemElement model, IScreen screen, IKeyGestureGuide keyGestureGuide, IDispatcherWrapper dispatcherWrapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, screen, keyGestureGuide, dispatcherWrapper, launcherToolbarTheme, loggerFactory)
        { }

        #region property

        public LauncherSeparatorData? Separator { get; set; }

        public DependencyObject HorizontalSeparator => LauncherToolbarTheme.GetLauncherSeparator(true, Separator?.Kind ?? LauncherSeparatorKind.None, Separator?.Width ?? 0);
        public DependencyObject VerticalSeparator => LauncherToolbarTheme.GetLauncherSeparator(false, Separator?.Kind ?? LauncherSeparatorKind.None, Separator?.Width ?? 0);

        #endregion

        #region LauncherItemViewModelBase

        protected override Task ExecuteMainImplAsync()
        {
            return Task.CompletedTask;
        }

        protected override bool CanExecuteMain => false;


        protected override Task LoadImplAsync()
        {
            return Task.Run(() => {
                Separator = Model.LoadSeparator();
                RaisePropertyChanged(nameof(HorizontalSeparator));
                RaisePropertyChanged(nameof(VerticalSeparator));
            });
        }

        protected override Task UnloadImplAsync()
        {
            return Task.CompletedTask;
        }

        protected override object GetIcon(IconKind iconKind)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
