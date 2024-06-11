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

        public LauncherSeparatorKind Kind => Separator?.Kind ?? LauncherSeparatorKind.Line;
        public int Width => Separator?.Width ?? 1;

        public DependencyObject HorizontalSeparator => LauncherToolbarTheme.GetLauncherSeparator(true, Width);
        public DependencyObject VerticalSeparator => LauncherToolbarTheme.GetLauncherSeparator(false, Width);

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
                RaisePropertyChanged(nameof(Kind));
                RaisePropertyChanged(nameof(Width));
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
            return default!;
        }

        #endregion
    }
}
