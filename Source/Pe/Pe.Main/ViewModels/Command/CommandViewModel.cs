using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
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
        public CommandViewModel(CommandElement model, IGeneralTheme generalTheme, ICommandTheme commandTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            GeneralTheme = generalTheme;
            CommandTheme = commandTheme;
            DispatcherWrapper = dispatcherWrapper;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
        }

        #region property
        IGeneralTheme GeneralTheme { get; }
        ICommandTheme CommandTheme { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        PropertyChangedHooker PropertyChangedHooker { get; }

        IDpiScaleOutputor DpiScaleOutputor { get; set; } = new EmptyDpiScaleOutputor();

        #region theme

        public Thickness ViewBorderThickness => CommandTheme.GetViewBorderThickness();

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
                    PropertyChangedHooker.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }
    }
}
