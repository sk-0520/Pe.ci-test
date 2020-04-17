using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.NotifyLog
{
    public class NotifyLogViewModel : ElementViewModelBase<NotifyLogElement>, IViewLifecycleReceiver
    {
        public NotifyLogViewModel(NotifyLogElement model, INotifyLogTheme notifyLogTheme, IPlatformTheme platformTheme, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            NotifyLogTheme = notifyLogTheme;
            PlatformTheme = platformTheme;

            ThemeProperties = new ThemeProperties(this);

            PlatformTheme.Changed += PlatformTheme_Changed;

            TopmostNotifyLogCollection = new ActionModelViewModelObservableCollectionManager<NotifyLogItemElement, NotifyLogItemViewModel>(Model.TopmostNotifyLogs) {
                ToViewModel = m => new NotifyLogItemViewModel(m, UserTracker, DispatcherWrapper, LoggerFactory)
            };
            TopmostNotifyLogItems = TopmostNotifyLogCollection.GetDefaultView();

            StreamNotifyLogCollection = new ActionModelViewModelObservableCollectionManager<NotifyLogItemElement, NotifyLogItemViewModel>(Model.StreamNotifyLogs) {
                ToViewModel = m => new NotifyLogItemViewModel(m, UserTracker, DispatcherWrapper, LoggerFactory)
            };
            StreamNotifyLogItems = StreamNotifyLogCollection.GetDefaultView();
        }

        #region property

        INotifyLogTheme NotifyLogTheme { get; }
        IPlatformTheme PlatformTheme { get; }

        IDpiScaleOutputor? DpiScaleOutputor { get; set; }

        ThemeProperties ThemeProperties { get; }

        ModelViewModelObservableCollectionManagerBase<NotifyLogItemElement, NotifyLogItemViewModel> TopmostNotifyLogCollection { get; }
        public ICollectionView TopmostNotifyLogItems { get; }
        ModelViewModelObservableCollectionManagerBase<NotifyLogItemElement, NotifyLogItemViewModel> StreamNotifyLogCollection { get; }
        public ICollectionView StreamNotifyLogItems { get; }

        public NotifyLogPosition Position => Model.Position;
        public bool CanShowView => Model.CanShowView;


        #region theme

        [ThemeProperty]
        public Thickness ViewBorderThickness => NotifyLogTheme.GetViewBorderThickness();
        [ThemeProperty]
        public Brush ViewBorderBrush => NotifyLogTheme.GetViewBorderBrush();
        [ThemeProperty]
        public CornerRadius ViewBorderCornerRadius => NotifyLogTheme.GetViewBorderCornerRadius();
        [ThemeProperty]
        public Brush ViewBackgroundBrush => NotifyLogTheme.GetViewBackgroundBrush();
        [ThemeProperty]
        public Thickness ViewPaddingThickness => NotifyLogTheme.GetViewPaddingThickness();
        [ThemeProperty]
        public Brush TopmostHeaderForegroundBrush => NotifyLogTheme.GetHeaderForegroundBrush(true);
        [ThemeProperty]
        public Brush TopmostContentForegroundBrush => NotifyLogTheme.GetContentForegroundBrush(true);
        [ThemeProperty]
        public Brush StreamHeaderForegroundBrush => NotifyLogTheme.GetHeaderForegroundBrush(false);
        [ThemeProperty]
        public Brush StreamContentForegroundBrush => NotifyLogTheme.GetContentForegroundBrush(false);

        #endregion

        #endregion

        #region command

        #endregion

        #region function

        private void HideView()
        {
            Model.HideView(false);
        }

        #endregion

        #region ElementViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    TopmostNotifyLogCollection.Dispose();
                    StreamNotifyLogCollection.Dispose();
                }
                DpiScaleOutputor = null;
                PlatformTheme.Changed -= PlatformTheme_Changed;
            }

            base.Dispose(disposing);
        }
        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
            DpiScaleOutputor = (IDpiScaleOutputor)window;
        }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
            HideView();
        }

        public void ReceiveViewClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed(Window window)
        {
            Model.ReceiveViewClosed();
        }

        #endregion

        private void PlatformTheme_Changed(object? sender, EventArgs e)
        {
            DispatcherWrapper.Begin(vm => {
                if(vm.IsDisposed) {
                    return;
                }

                foreach(var themePropertyName in ThemeProperties.GetPropertyNames()) {
                    RaisePropertyChanged(themePropertyName);
                }
            }, this, DispatcherPriority.Render);
        }

    }
}
