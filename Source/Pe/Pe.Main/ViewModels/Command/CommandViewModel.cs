using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Command
{
    public class CommandViewModel : SingleModelViewModelBase<CommandElement>, IViewLifecycleReceiver
    {
        public CommandViewModel(CommandElement model, IGeneralTheme generalTheme, ICommandTheme commandTheme, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            GeneralTheme = generalTheme;
            CommandTheme = commandTheme;
            PlatformTheme = platformTheme;
            DispatcherWrapper = dispatcherWrapper;

            PlatformTheme.Changed += PlatformTheme_Changed;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
        }


        #region property
        IGeneralTheme GeneralTheme { get; }
        ICommandTheme CommandTheme { get; }
        IPlatformTheme PlatformTheme { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        PropertyChangedHooker PropertyChangedHooker { get; }

        IDpiScaleOutputor DpiScaleOutputor { get; set; } = new EmptyDpiScaleOutputor();

        #region theme

        public Thickness ViewBorderThickness => CommandTheme.GetViewBorderThickness();
        public Thickness ViewResizeThickness
        {
            get
            {
                var thickness = CommandTheme.GetViewBorderThickness();
                thickness.Top = 0;
                thickness.Bottom = 0;
                return thickness;
            }
        }
        public Brush ViewActiveBorderBrush => CommandTheme.GetViewBorderBrush(true);
        public Brush ViewInactiveBorderBrush => CommandTheme.GetViewBorderBrush(false);

        public Brush ViewActiveBackgroundBrush => CommandTheme.GetViewBackgroundBrush(true);
        public Brush ViewInactiveBackgroundBrush => CommandTheme.GetViewBackgroundBrush(false);
        public double GripWidth => CommandTheme.GetGripWidth();
        public Brush GripActiveBrush => CommandTheme.GetGripBrush(true);
        public Brush GripInactiveBrush => CommandTheme.GetGripBrush(true);

        public Thickness InputBorderThickness => CommandTheme.GetInputBorderThickness();

        public Brush InputEmptyBorderBrush => CommandTheme.GetInputBorderBrush(InputState.Empty);
        public Brush InputFindingBorderBrush => CommandTheme.GetInputBorderBrush(InputState.Finding);
        public Brush InputNotFoundBorderBrush => CommandTheme.GetInputBorderBrush(InputState.NotFound);

        public Brush InputEmptyBackground => CommandTheme.GetInputBackground(InputState.Empty);
        public Brush InputFindingBackground => CommandTheme.GetInputBackground(InputState.Finding);
        public Brush InputNotFoundBackground => CommandTheme.GetInputBackground(InputState.NotFound);

        public Brush InputEmptyForeground => CommandTheme.GetInputForeground(InputState.Empty);
        public Brush InputFindingForeground => CommandTheme.GetInputForeground(InputState.Finding);
        public Brush InputNotFoundForeground => CommandTheme.GetInputForeground(InputState.NotFound);

        #endregion

        #endregion

        #region command
        #endregion

        #region function

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
            Model.Hide();
        }

        public void ReceiveViewClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed()
        {
            Model.ReceiveViewClosed();
        }

        #endregion

        #region SingleModelViewModelBase

        protected override void AttachModelEventsImpl()
        {
            base.AttachModelEventsImpl();

            Model.PropertyChanged += Model_PropertyChanged;
        }

        protected override void DetachModelEventsImpl()
        {
            base.DetachModelEventsImpl();

            Model.PropertyChanged -= Model_PropertyChanged;
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    PlatformTheme.Changed -= PlatformTheme_Changed;
                    PropertyChangedHooker.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void PlatformTheme_Changed(object? sender, EventArgs e)
        {
            DispatcherWrapper.Begin(() => {
                var themePropertyNames = new[] {
                    nameof(ViewBorderThickness),
                    nameof(ViewResizeThickness),

                    nameof(ViewActiveBorderBrush),
                    nameof(ViewInactiveBorderBrush),

                    nameof(ViewActiveBackgroundBrush),
                    nameof(ViewInactiveBackgroundBrush),

                    nameof(GripWidth),

                    nameof(GripActiveBrush),
                    nameof(GripInactiveBrush),

                    nameof(InputBorderThickness),

                    nameof(InputEmptyBorderBrush ),
                    nameof(InputFindingBorderBrush ),
                    nameof(InputNotFoundBorderBrush ),

                    nameof(InputEmptyBackground ),
                    nameof(InputFindingBackground ),
                    nameof(InputNotFoundBackground ),

                    nameof(InputEmptyForeground),
                    nameof(InputFindingForeground),
                    nameof(InputNotFoundForeground),
                };
                foreach(var themePropertyName in themePropertyNames) {
                    RaisePropertyChanged(themePropertyName);
                }
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }
    }
}
