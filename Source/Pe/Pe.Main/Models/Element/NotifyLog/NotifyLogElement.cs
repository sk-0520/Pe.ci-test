using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog
{
    public class NotifyLogElement : ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        public NotifyLogElement(IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, INotifyManager notifyManager, IOrderManager orderManager, IWindowManager windowManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            NotifyManager = notifyManager;
            OrderManager = orderManager;
            WindowManager = windowManager;

            NotifyManager.NotifyLogChanged += NotifyManager_NotifyLogChanged;
        }

        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        private INotifyManager NotifyManager { get; }
        private IOrderManager OrderManager { get; }
        private IWindowManager WindowManager { get; }

        public ReadOnlyObservableCollection<NotifyLogItemElement> TopmostNotifyLogs => NotifyManager.TopmostNotifyLogs;
        public ReadOnlyObservableCollection<NotifyLogItemElement> StreamNotifyLogs => NotifyManager.StreamNotifyLogs;
        public bool ViewCreated { get; private set; }

        public NotifyLogPosition Position { get; private set; }

        private bool NowSilent { get; set; }

        #endregion

        #region function

        public void HideView(bool force)
        {
            Debug.Assert(ViewCreated);
            WindowManager.GetWindowItems(WindowKind.NotifyLog).First().Window.Hide();
        }

        public IDisposable ToSilent()
        {
            if(NowSilent) {
                throw new InvalidOperationException(nameof(NowSilent));
            }

            NowSilent = true;
            return new ActionDisposer(d => {
                NowSilent = false;

                if(TopmostNotifyLogs.Any() || StreamNotifyLogs.Any()) {
                    StartView();
                }
            });
        }

        private void RefreshSetting()
        {
            var setting = MainDatabaseBarrier.ReadData(c => {
                var dao = new AppNotifyLogSettingEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                return dao.SelectSettingNotifyLogSetting();
            });
            Position = setting.Position;
        }

        public void Refresh()
        {
            RefreshSetting();
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    NotifyManager.NotifyLogChanged -= NotifyManager_NotifyLogChanged;
                }
            }

            base.Dispose(disposing);
        }

        void MoveView(WindowItem windowItem)
        {
            Debug.Assert(ViewCreated);

            NativeMethods.GetCursorPos(out var podDevicePoint);
            var screen = Screen.FromDevicePoint(new Point(podDevicePoint.X, podDevicePoint.Y));
            var deviceArea = screen.DeviceWorkingArea;
            NativeMethods.GetWindowRect(HandleUtility.GetWindowHandle(windowItem.Window), out var deviceWindowRect);
            var deviceWindowLocation = new Point();
            switch(Position) {
                case NotifyLogPosition.Center:
                    deviceWindowLocation.X = deviceArea.X + (deviceArea.Width / 2) - (deviceWindowRect.Width / 2);
                    deviceWindowLocation.Y = deviceArea.Y + (deviceArea.Height / 2) - (deviceWindowRect.Height / 2);
                    break;

                case NotifyLogPosition.LeftTop:
                    deviceWindowLocation.X = deviceArea.X;
                    deviceWindowLocation.Y = deviceArea.Y;
                    break;

                case NotifyLogPosition.RightTop:
                    deviceWindowLocation.X = deviceArea.X + deviceArea.Width - deviceWindowRect.Width;
                    deviceWindowLocation.Y = deviceArea.Y;
                    break;

                case NotifyLogPosition.LeftBottom:
                    deviceWindowLocation.X = deviceArea.X;
                    deviceWindowLocation.Y = deviceArea.Y + deviceArea.Height - deviceWindowRect.Height;
                    break;

                case NotifyLogPosition.RightBottom:
                    deviceWindowLocation.X = deviceArea.X + deviceArea.Width - deviceWindowRect.Width;
                    deviceWindowLocation.Y = deviceArea.Y + deviceArea.Height - deviceWindowRect.Height;
                    break;

            }
            NativeMethods.SetWindowPos(HandleUtility.GetWindowHandle(windowItem.Window), IntPtr.Zero, (int)deviceWindowLocation.X, (int)deviceWindowLocation.Y, 0, 0, SWP.SWP_NOSIZE);
        }

        #endregion

        #region IViewShowStarter

        public bool CanStartShowView
        {
            get
            {
                if(NowSilent) {
                    return false;
                }

                if(ViewCreated) {
                    return false;
                }

                return true;
            }
        }

        public void StartView()
        {
            if(!ViewCreated) {
                var windowItem = OrderManager.CreateNotifyLogWindow(this);
                windowItem.Window.Show();
                ViewCreated = true;
                MoveView(windowItem);
            } else {
                var windowItem = WindowManager.GetWindowItems(WindowKind.NotifyLog).First();
                if(windowItem.Window.IsVisible) {
                    MoveView(windowItem);
                } else {
                    MoveView(windowItem);
                    windowItem.Window.Show();
                }
            }
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
            if(NowSilent) {
                return;
            }

            switch(e.Kind) {
                case Data.NotifyEventKind.Add:
                case Data.NotifyEventKind.Change:
                    StartView();
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
