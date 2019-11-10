using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.StandardInputOutput
{
    public class StandardInputOutputViewModel : SingleModelViewModelBase<StandardInputOutputElement>, IViewLifecycleReceiver
    {

        public StandardInputOutputViewModel(StandardInputOutputElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWapper = dispatcherWapper;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWapper, LoggerFactory);
            PropertyChangedHooker.AddHook(nameof(StandardInputOutputElement.PreparatedReceive), AttachReceiver);
        }

        #region property

        IDispatcherWapper DispatcherWapper { get; }

        public TextDocument TextDocument { get; } = new TextDocument();

        PropertyChangedHooker PropertyChangedHooker { get; }

        #endregion

        #region function

        private void AttachReceiver()
        {
            if(Model.PreparatedReceive) {
                Model.InputStreamReceiver!.StreamReceived += InputStreamReceiver_StreamReceived;
            }
        }

        private void AppendOutput(string value, bool isError)
        {
            Logger.LogTrace(value);
            DispatcherWapper.Invoke(() => {
                TextDocument.Insert(TextDocument.TextLength, value);
            });
        }

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(CancelEventArgs e) => Model.ReceiveViewUserClosing();

        public void ReceiveViewClosing(CancelEventArgs e) => Model.ReceiveViewClosing();

        public void ReceiveViewClosed() => Model.ReceiveViewClosed();

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

        private void InputStreamReceiver_StreamReceived(object? sender, Models.Launcher.StreamReceivedEventArgs e)
        {
            AppendOutput(e.Value, false);
        }

    }
}
