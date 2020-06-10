using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Widget;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.Manager;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Widget
{
    public class WidgetNotifyAreaViewModel: ElementViewModelBase<WidgetElement>, INotifyArea, IViewLifecycleReceiver
    {
        public WidgetNotifyAreaViewModel(WidgetElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        { }

        #region property


        #endregion

        #region command

        #endregion

        #region function

        #endregion

        #region INotifyArea

        public string MenuHeader => Model.GetMenuHeader();
        public bool MenuHeaderHasAccessKey { get; } = false;
        public KeyGesture? MenuKeyGesture { get; }
        public DependencyObject? MenuIcon => DispatcherWrapper.Get(() => Model.GetMenuIcon());
        public bool MenuHasIcon { get; } = true;
        public bool MenuIsEnabled { get; } = true;
        public bool MenuIsChecked => Model.ViewCreated;

        public ICommand MenuCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                if(Model.ViewCreated) {
                    Model.HideView(this);
                } else {
                    Model.ShowView(this);
                }
            },
            () => MenuIsEnabled
        ));

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
        }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }

        public void ReceiveViewClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed(Window window, bool isUserOperation)
        {
            Model.ReceiveViewClosed(isUserOperation);
        }

        #endregion

    }
}
