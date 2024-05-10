using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Widget;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Widget
{
    public class WidgetNotifyAreaViewModel: WidgetViewModelBase<WidgetElement>
    {
        public WidgetNotifyAreaViewModel(WidgetElement model, IUserTracker userTracker, IWindowManager windowManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, windowManager, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public DependencyObject? MenuIcon => DispatcherWrapper.Get(() => Model.GetMenuIcon());
        public string MenuHeader => Model.GetMenuHeader();
        public bool IsVisible => Model.ViewCreated;
        public bool IsTopmost => Model.IsTopmost;

        #endregion

        #region command

        private ICommand? _ToggleVisibleCommand;
        public ICommand ToggleVisibleCommand => this._ToggleVisibleCommand ??= new DelegateCommand(
            () => {
                if(Model.ViewCreated) {
                    Model.SaveStatus(false);
                    Model.HideView();
                } else {
                    Model.ShowView(this);
                    Model.SaveStatus(true);
                }
                RaisePropertyChanged(nameof(IsVisible));
            }
        );

        private ICommand? _ToggleTopmostCommand;
        public ICommand ToggleTopmostCommand => this._ToggleTopmostCommand ??= new DelegateCommand(
            () => {
                Model.ToggleTopmost();
                RaisePropertyChanged(nameof(IsTopmost));
            }
        );

        #endregion

        #region function

        #endregion
    }
}
