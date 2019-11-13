using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Views.StandardInputOutput;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.StandardInputOutput
{
    public class StandardInputOutputViewModel : SingleModelViewModelBase<StandardInputOutputElement>, IViewLifecycleReceiver
    {
        #region define

        #endregion

        #region variable

        bool _isTopmost = false;
        bool _autoScroll = true;
        bool _wordWrap = false;
        string _inputValue = string.Empty;

        #endregion

        public StandardInputOutputViewModel(StandardInputOutputElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWapper = dispatcherWapper;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWapper, LoggerFactory);
            PropertyChangedHooker.AddHook(nameof(StandardInputOutputElement.PreparatedReceive), AttachReceiver);
            PropertyChangedHooker.AddHook(nameof(StandardInputOutputElement.ProcessExited), nameof(ProcessExited));
            PropertyChangedHooker.AddHook(nameof(StandardInputOutputElement.ProcessExited), ClearOutputCommand);
            PropertyChangedHooker.AddHook(nameof(StandardInputOutputElement.ProcessExited), SendInputCommand);
        }

        #region property

        IDispatcherWapper DispatcherWapper { get; }
        public RequestSender FileSelectRequest { get; } = new RequestSender();

        public TextDocument TextDocument { get; } = new TextDocument();

        PropertyChangedHooker PropertyChangedHooker { get; }

        TextEditor? Terminal { get; set; }

        public ObservableCollection<StandardInputOutputHistoryViewModel> InputedHistories { get; } = new ObservableCollection<StandardInputOutputHistoryViewModel>();

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

        public string InputValue
        {
            get => this._inputValue;
            set => SetProperty(ref this._inputValue, value);
        }

        public Brush Background => new SolidColorBrush(Colors.Black);
        public Brush StandardOutputForeground => new SolidColorBrush(Colors.White);
        public Brush StandardErrorForegound => new SolidColorBrush(Colors.Red);

        #endregion

        #region command

        public ICommand ClearOutputCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                try {
                    DispatcherWapper.Invoke(() => {
                        Terminal!.Clear();
                    });
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                }
            },
            () => !ProcessExited
        ));

        public ICommand KillOutputCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                try {
                    Model.Kill();
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                }
            },
            () => !ProcessExited
        ));

        public ICommand SaveCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectFile(
                    FileSelectRequest,
                    string.Empty,
                    false,
                    new[] {
                        new DialogFilterItem("log", "log", "*.log"),
                        new DialogFilterItem("all", string.Empty, "*"),
                    },
                    r => {
                        var path = r.ResponseFilePaths[0];
                        try {
                            SaveLog(path);
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                        }
                    }
                );
            },
            () => 0 < TextDocument.TextLength
        ));

        public ICommand SendInputCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var rawValue = InputValue;
                try {
                    var value = rawValue + Environment.NewLine;
                    Model.SendInputValue(value);
                    InputValue = string.Empty;
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                }

                if(string.IsNullOrEmpty(rawValue)) {
                    return;
                }

                Logger.LogDebug("add: " + rawValue);
                // 入力履歴
                var element = new StandardInputOutputHistoryViewModel(rawValue, DateTime.UtcNow, LoggerFactory);
                var item = InputedHistories.FirstOrDefault(i => i.Value == rawValue);
                if(item != null) {
                    InputedHistories.Remove(item);
                }
                InputedHistories.Insert(0, element);
            },
            () => !ProcessExited
        ));

        public ICommand ClearInputCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                InputValue = string.Empty;
            }
        ));

        #endregion

        #region function

        private void AttachReceiver()
        {
            //if(Model.PreparatedReceive && Model.OutputStreamReceiver != null) {
            //    Model.OutputStreamReceiver.StreamReceived -= OutputStreamReceiver_StreamReceived;
            //    Model.OutputStreamReceiver.StreamReceived += OutputStreamReceiver_StreamReceived;
            //}
            //if(Model.PreparatedReceive && Model.ErrorStreamReceiver != null) {
            //    Model.ErrorStreamReceiver.StreamReceived -= ErrorStreamReceiver_StreamReceived;
            //    Model.ErrorStreamReceiver.StreamReceived += ErrorStreamReceiver_StreamReceived;
            //}
            if(Model.PreparatedReceive && Model.ProcessStandardOutputReceiver != null) {
                Model.ProcessStandardOutputReceiver.StandardOutputReceived += ProcessStandardOutputReceiver_StandardOutputReceived;
            }
        }


        private void AppendOutput(string value, bool isError)
        {
            Logger.LogTrace(value);
            if(Terminal == null) {
                Logger.LogTrace("来ちゃいけない制御フロー");
                return;
            }

            DispatcherWapper.Invoke(() => {
                var selectionIndex = Terminal.SelectionStart;
                var selectionLength = Terminal.SelectionLength;

                var index = TextDocument.TextLength;
                var length = value.Length;

                TextDocument.Insert(TextDocument.TextLength, value);

                if(AutoScroll && Terminal != null) {
                    Terminal.ScrollToEnd();
                }
            });
        }

        private void AppendOutput(StandardOutputMode mode, string value)
        {
            Logger.LogTrace(value);
            if(Terminal == null) {
                Logger.LogTrace("来ちゃいけない制御フロー");
                return;
            }

            DispatcherWapper.Invoke(() => {
                var selectionIndex = Terminal.SelectionStart;
                var selectionLength = Terminal.SelectionLength;

                var index = TextDocument.TextLength;
                var length = value.Length;

                TextDocument.Insert(TextDocument.TextLength, value);

                if(AutoScroll && Terminal != null) {
                    Terminal.ScrollToEnd();
                }
            });
        }

        void SaveLog(string path)
        {
            Logger.LogDebug(path);
            using var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            using var writer = new StreamWriter(stream, Model.Process.StandardOutput.CurrentEncoding);
            writer.WriteLine(TextDocument.Text);
        }

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
            var view = (StandardInputOutputWindow)window;

            Terminal = (TextEditor)view.FindName("terminal");
            Terminal.TextChanged += Terminal_TextChanged;
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

        public void ReceiveViewClosed()
        {
            if(Terminal != null) {
                Terminal.TextChanged -= Terminal_TextChanged;
            }

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
                    //if(Model.OutputStreamReceiver != null) {
                    //    Model.OutputStreamReceiver.StreamReceived -= OutputStreamReceiver_StreamReceived;
                    //}
                    //if(Model.ErrorStreamReceiver != null) {
                    //    Model.ErrorStreamReceiver.StreamReceived -= ErrorStreamReceiver_StreamReceived;
                    //}

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

        private void Terminal_TextChanged(object? sender, EventArgs e)
        {
            ((DelegateCommandBase)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OutputStreamReceiver_StreamReceived(object? sender, StreamReceivedEventArgs e)
        {
            AppendOutput(e.Value, false);
        }
        private void ErrorStreamReceiver_StreamReceived(object? sender, StreamReceivedEventArgs e)
        {
            AppendOutput(e.Value, true);
        }

        private void ProcessStandardOutputReceiver_StandardOutputReceived(object? sender, ProcessStandardOutputReceivedEventArgs e)
        {
            AppendOutput(e.Mode, e.Value);
        }

    }
}
