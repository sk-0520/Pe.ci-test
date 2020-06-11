using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Widget;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Widget
{
    public abstract class WidgetViewModelBase<TWidgetElement>: ElementViewModelBase<TWidgetElement>, IViewLifecycleReceiver
        where TWidgetElement : WidgetElement
    {
        protected WidgetViewModelBase(TWidgetElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        { }

        #region IViewLifecycleReceiver

        public virtual void ReceiveViewInitialized(Window window)
        {
            // ツールウィンドウを強制
            UIUtility.SetToolWindowStyle(window, false, false);
        }

        public virtual void ReceiveViewLoaded(Window window)
        { }

        public virtual void ReceiveViewUserClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }

        public virtual void ReceiveViewClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public virtual void ReceiveViewClosed(Window window, bool isUserOperation)
        {
            Model.ReceiveViewClosed(isUserOperation);
        }

        #endregion
    }
}
