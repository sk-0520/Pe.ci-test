using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem
{
    public class LauncherAddonViewModel: LauncherDetailViewModelBase
    {
        public LauncherAddonViewModel(LauncherItemElement model, IScreen screen, IKeyGestureGuide keyGestureGuide, IDispatcherWrapper dispatcherWrapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, screen, keyGestureGuide, dispatcherWrapper, launcherToolbarTheme, loggerFactory)
        { }

        #region property
        private LauncherAddonDetailData? Detail { get; set; }
        private PropertyChangedHooker? ExtensionPropertyChangedHooker { get; set; }

        #endregion

        #region command

        private ICommand? _ExecuteSimpleCommand;
        public ICommand ExecuteSimpleCommand => this._ExecuteSimpleCommand ??= new DelegateCommand(
            () => {
                ExecuteMainAsync(CancellationToken.None).ConfigureAwait(false);
            },
            () => !NowLoading
        );

        #endregion

        #region LauncherDetailViewModelBase

        public override string? Name
        {
            get
            {
                if(Detail != null && Detail.IsEnabled && Detail.Extension != null) {
                    if(Detail.Extension.CustomDisplayText) {
                        return Detail.Extension.DisplayText;
                    }
                }

                return base.Name;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Detail?.Extension != null) {
                    Detail.Extension.PropertyChanged -= Extension_PropertyChanged;
                }
                ExtensionPropertyChangedHooker?.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override Task LoadImplAsync(CancellationToken cancellationToken)
        {
            Detail = Model.LoadAddonDetail(this);
            if(Detail.IsEnabled) {
                if(Detail.Extension == null) {
                    throw new InvalidOperationException(nameof(Detail.Extension));
                }

                ExtensionPropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
                Detail.Extension.PropertyChanged += Extension_PropertyChanged;

                if(Detail.Extension.CustomDisplayText) {
                    ExtensionPropertyChangedHooker.AddHook(nameof(Detail.Extension.DisplayText), nameof(Name));
                }

            }
            return Task.CompletedTask;
        }

        protected override Task UnloadImplAsync(CancellationToken cancellationToken)
        {
            if(Detail?.Extension != null) {
                Detail.Extension.ChangeDisplay(Bridge.Plugin.Addon.LauncherItemIconMode.Toolbar, false, this);
            }

            return Task.CompletedTask;
        }

        protected override Task ExecuteMainImplAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => {
                Model.ExecuteAsync(Screen, cancellationToken);
            }, cancellationToken);
        }

        protected override object GetIcon(IconKind iconKind)
        {
            var factory = Model.CreateLauncherIconFactory();
            var iconSource = factory.CreateIconSource(DispatcherWrapper);
            return factory.CreateView(iconSource, false, DispatcherWrapper);
        }

        #endregion

        private void Extension_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ExtensionPropertyChangedHooker!.Execute(e, RaisePropertyChanged);
        }


    }
}
