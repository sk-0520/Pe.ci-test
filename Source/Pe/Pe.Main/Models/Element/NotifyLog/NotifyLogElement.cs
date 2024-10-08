using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog
{
    public class NotifyLogElement: ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        #region variable

        private NotifyLogPosition _position;
        private HorizontalAlignment _cursorHorizontalAlignment = HorizontalAlignment.Left;
        private VerticalAlignment _cursorVerticalAlignment = VerticalAlignment.Top;

        #endregion
        public NotifyLogElement(IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, INotifyManager notifyManager, IOrderManager orderManager, IWindowManager windowManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            NotifyManager = notifyManager;
            OrderManager = orderManager;
            WindowManager = windowManager;

            NotifyManager.NotifyLogChanged += NotifyManager_NotifyLogChanged;
        }

        #region property

        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }

        private INotifyManager NotifyManager { get; }
        private IOrderManager OrderManager { get; }
        private IWindowManager WindowManager { get; }

        public ReadOnlyObservableCollection<NotifyLogItemElement> TopmostNotifyLogs => NotifyManager.TopmostNotifyLogs;
        public ReadOnlyObservableCollection<NotifyLogItemElement> StreamNotifyLogs => NotifyManager.StreamNotifyLogs;
        public bool ViewCreated { get; private set; }

        private bool IsVisible { get; set; }
        public NotifyLogPosition Position
        {
            get => this._position;
            private set => SetProperty(ref this._position, value);
        }

        private bool NowSilent { get; set; }

        public HorizontalAlignment CursorHorizontalAlignment
        {
            get => this._cursorHorizontalAlignment;
            set => SetProperty(ref this._cursorHorizontalAlignment, value);
        }
        public VerticalAlignment CursorVerticalAlignment
        {
            get => this._cursorVerticalAlignment;
            set => SetProperty(ref this._cursorVerticalAlignment, value);
        }

        public bool NowShowing { get; private set; }

        #endregion

        #region function

        public void HideView(bool force)
        {
            Debug.Assert(ViewCreated);
            NowShowing = false;

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

        private Task RefreshSettingAsync(CancellationToken cancellationToken)
        {
            var setting = MainDatabaseBarrier.ReadData(c => {
                var dao = new AppNotifyLogSettingEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                return dao.SelectSettingNotifyLogSetting();
            });
            IsVisible = setting.IsVisible;
            Position = setting.Position;

            return Task.CompletedTask;
        }

        public Task RefreshAsync(CancellationToken cancellationToken)
        {
            return RefreshSettingAsync(cancellationToken);
        }

        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            return RefreshAsync(cancellationToken);
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
                case NotifyLogPosition.Cursor:
                    deviceWindowLocation.X = Math.Clamp(podDevicePoint.X, deviceArea.X, deviceArea.Right - deviceWindowRect.Width);
                    deviceWindowLocation.Y = Math.Clamp(podDevicePoint.Y, deviceArea.Y, deviceArea.Bottom - deviceWindowRect.Height);

                    CursorHorizontalAlignment = podDevicePoint.X <= deviceWindowLocation.X
                        ? HorizontalAlignment.Left
                        : HorizontalAlignment.Right
                    ;
                    CursorVerticalAlignment = podDevicePoint.Y <= deviceWindowLocation.Y
                        ? VerticalAlignment.Top
                        : VerticalAlignment.Bottom
                    ;
                    break;

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

                default:
                    throw new NotImplementedException();
            }
            NativeMethods.SetWindowPos(HandleUtility.GetWindowHandle(windowItem.Window), IntPtr.Zero, (int)deviceWindowLocation.X, (int)deviceWindowLocation.Y, 0, 0, SWP.SWP_NOSIZE | SWP.SWP_NOACTIVATE);
        }

        public void ExecuteLogById(NotifyLogId notifyLogId)
        {
            var logItem = StreamNotifyLogs.FirstOrDefault(i => i.NotifyLogId == notifyLogId);
            if(logItem == null) {
                // タイミングによっては可能性あり
                Logger.LogWarning("指定ログなし: {0}", notifyLogId);
                return;
            }

            if(!(logItem.Kind == NotifyLogKind.Command || logItem.Kind == NotifyLogKind.Undo)) {
                Logger.LogError("指定ログは実行不可: {0}, {1}", notifyLogId, logItem.Kind);
                return;
            }

            try {
                logItem.Callback();
                NotifyManager.ClearLog(logItem.NotifyLogId);
            } catch(Exception ex) {
                Logger.LogError(ex, "ログ実行失敗: [{Message}] {NotifyLogId}, {Kind}", ex.Message, notifyLogId, logItem.Kind);
            }
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
            if(!IsVisible) {
                Logger.LogTrace("通知ログは非表示設定");
                return;
            }

            NowShowing = true;

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

        /// <inheritdoc cref="IViewCloseReceiver.ReceiveViewClosedAsync(bool, CancellationToken)"/>
        public Task ReceiveViewClosedAsync(bool isUserOperation, CancellationToken cancellationToken)
        {
            ViewCreated = false;
            return Task.CompletedTask;
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
                    if(ViewCreated) {
                        if(TopmostNotifyLogs.Count == 0 && StreamNotifyLogs.Count == 0) {
                            HideView(false);
                        } else {
                            var windowItem = WindowManager.GetWindowItems(WindowKind.NotifyLog).First();
                            MoveView(windowItem);
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
