using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog
{
    public class NotifyLogElement : ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        public NotifyLogElement(INotifyManager notifyManager, IOrderManager orderManager, IWindowManager windowManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NotifyManager = notifyManager;
            OrderManager = orderManager;
            WindowManager = windowManager;

            NotifyManager.NotifyLogChanged += NotifyManager_NotifyLogChanged;
        }

        #region property

        private INotifyManager NotifyManager { get; }
        private IOrderManager OrderManager { get; }
        private IWindowManager WindowManager { get; }

        public ReadOnlyObservableCollection<NotifyLogItemElement> TopmostNotifyLogs => NotifyManager.TopmostNotifyLogs;
        public ReadOnlyObservableCollection<NotifyLogItemElement> StreamNotifyLogs => NotifyManager.StreamNotifyLogs;
        public bool ViewCreated { get; private set; }

        #endregion

        #region function

        public void HideView(bool force)
        {
            Debug.Assert(ViewCreated);
            WindowManager.GetWindowItems(WindowKind.NotifyLog).First().Window.Hide();
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        { }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    NotifyManager.NotifyLogChanged -= NotifyManager_NotifyLogChanged;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IViewShowStarter

        public bool CanStartShowView
        {
            get
            {
                if(ViewCreated) {
                    return false;
                }

                return true;
            }
        }

        public void StartView()
        {
            var windowItem = OrderManager.CreateNotifyLogWindow(this);
            windowItem.Window.Show();
            ViewCreated = true;
        }

        #endregion

        #region IViewCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            return false;
        }
        public bool ReceiveViewClosing()
        {
            return true;
        }

        public void ReceiveViewClosed()
        {
            ViewCreated = false;
        }


        #endregion


        private void NotifyManager_NotifyLogChanged(object? sender, NotifyLogEventArgs e)
        {
            switch(e.Kind) {
                case Data.NotifyEventKind.Add:
                case Data.NotifyEventKind.Change:
                    if(CanStartShowView) {
                        StartView();
                    }
                    break;

                case Data.NotifyEventKind.Clear:
                    if(ViewCreated && TopmostNotifyLogs.Count == 0 && StreamNotifyLogs.Count == 0) {
                        HideView(false);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }


    }
}
