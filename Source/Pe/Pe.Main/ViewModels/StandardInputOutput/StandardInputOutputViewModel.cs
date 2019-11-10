using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Views.StandardInputOutput;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.StandardInputOutput
{
    public class StandardInputOutputViewModel : SingleModelViewModelBase<StandardInputOutputElement>, IViewLifecycleReceiver
    {
        #region variable

        bool _isTopmost = false;
        bool _autoScroll = true;
        bool _wordWrap = false;

        #endregion

        public StandardInputOutputViewModel(StandardInputOutputElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWapper = dispatcherWapper;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWapper, LoggerFactory);
            PropertyChangedHooker.AddHook(nameof(StandardInputOutputElement.PreparatedReceive), AttachReceiver);
            PropertyChangedHooker.AddHook(nameof(StandardInputOutputElement.ProcessExited), nameof(ProcessExited));
            PropertyChangedHooker.AddHook(nameof(StandardInputOutputElement.ProcessExited), ClearOutputCommand);
            PropertyChangedHooker.AddHook(nameof(StandardInputOutputElement.ProcessExited), KillCommand);
        }

        #region property

        IDispatcherWapper DispatcherWapper { get; }

        public TextDocument TextDocument { get; } = new TextDocument();

        PropertyChangedHooker PropertyChangedHooker { get; }

        TextEditor? Terminal { get; set; }

        public bool IsTopmost
        {
            get => this._isTopmost;
            set => SetProperty(ref this._isTopmost, value);
        }
        public bool AutoScroll
        {
            get => this._autoScroll;
            set => SetProperty(ref this._autoScroll, value);
        }
        public bool WordWrap
        {
            get => this._wordWrap;
            set => SetProperty(ref this._wordWrap, value);
        }

        public bool ProcessExited => Model.ProcessExited;


        #endregion

        public ICommand ClearOutputCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                DispatcherWapper.Invoke(() => {
                    Terminal!.Clear();
                });
            },
            () => !ProcessExited
        ));

        public ICommand KillCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.Kill();
            },
            () => !ProcessExited
        ));

        #region function

        private void AttachReceiver()
        {
            if(Model.PreparatedReceive && Model.InputStreamReceiver != null) {
                Model.InputStreamReceiver.StreamReceived -= InputStreamReceiver_StreamReceived;
                Model.InputStreamReceiver.StreamReceived += InputStreamReceiver_StreamReceived;
            }
        }

        private void AppendOutput(string value, bool isError)
        {
            Logger.LogTrace(value);
            DispatcherWapper.Invoke(() => {
                TextDocument.Insert(TextDocument.TextLength, value);
                if(AutoScroll && Terminal != null) {
                    Terminal.ScrollToEnd();
                }
            });
        }

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
            var view = (StandardInputOutputWindow)window;

            Terminal = (TextEditor)view.FindName("terminal");
        }

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
                    if(Model.InputStreamReceiver != null) {
                        Model.InputStreamReceiver.StreamReceived -= InputStreamReceiver_StreamReceived;
                    }

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

        private void InputStreamReceiver_StreamReceived(object? sender, StreamReceivedEventArgs e)
        {
            AppendOutput(e.Value, false);
        }

    }
}
