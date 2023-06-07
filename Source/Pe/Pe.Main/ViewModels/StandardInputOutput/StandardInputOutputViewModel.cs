using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.Font;
using ContentTypeTextNet.Pe.Main.Views.StandardInputOutput;
using ContentTypeTextNet.Pe.Standard.Base;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.StandardInputOutput
{
    /// <summary>
    /// TODO: 標準出力中にまざる標準エラー処理がぐっだぐだ
    /// </summary>
    public class StandardInputOutputViewModel: ElementViewModelBase<StandardInputOutputElement>, IViewLifecycleReceiver
    {
        #region define

        #endregion

        #region variable

        private bool _isTopmost = false;
        private bool _autoScroll = true;
        private bool _wordWrap = false;
        private string _inputValue = string.Empty;

        #endregion

        public StandardInputOutputViewModel(StandardInputOutputElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            Font = new FontViewModel(model.Font!, DispatcherWrapper, LoggerFactory);

            this._isTopmost = model.IsTopmost;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
            PropertyChangedHooker.AddHook(nameof(StandardInputOutputElement.PreparedReceive), AttachReceiver);
            PropertyChangedHooker.AddHook(nameof(StandardInputOutputElement.ProcessExited), nameof(ProcessExited));
            PropertyChangedHooker.AddHook(nameof(StandardInputOutputElement.ProcessExited), ClearOutputCommand);
            PropertyChangedHooker.AddHook(nameof(StandardInputOutputElement.ProcessExited), SendInputCommand);
            PropertyChangedHooker.AddHook(nameof(StandardInputOutputElement.ProcessExited), KillOutputCommand);
        }

        #region property

        public RequestSender FileSelectRequest { get; } = new RequestSender();

        public TextDocument TextDocument { get; } = new TextDocument();

        private PropertyChangedHooker PropertyChangedHooker { get; }

        private TextEditor? Terminal { get; set; }

        public FontViewModel Font { get; }

        public ObservableCollection<StandardInputOutputHistoryViewModel> InputtedHistories { get; } = new ObservableCollection<StandardInputOutputHistoryViewModel>();

        public string Title
        {
            get
            {
                return TextUtility.ReplaceFromDictionary(
                    Properties.Resources.String_StandardInputOutput_Caption,
                    new Dictionary<string, string>() {
                        ["ITEM"] = Model.CaptionName,
                    }
                );
            }
        }

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
        public int ExitCode => ProcessExited ? Model.Process.ExitCode : int.MinValue;

        public string InputValue
        {
            get => this._inputValue;
            set => SetProperty(ref this._inputValue, value);
        }

        public Brush StandardOutputBackground => new SolidColorBrush(Model.OutputBackgroundColor);
        public Brush StandardOutputForeground => new SolidColorBrush(Model.OutputForegroundColor);
        public Brush StandardErrorForeground => new SolidColorBrush(Model.ErrorForegroundColor);
        private StandardOutputColorizingTransformer? StandardOutputColorizingTransformer { get; set; }

        #endregion

        #region command

        public ICommand ClearOutputCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                try {
                    DispatcherWrapper.BeginAsync(() => {
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
                        new DialogFilterItem(Properties.Resources.String_FileDialog_Filter_Common_Log, "log", "*.log"),
                        new DialogFilterItem(Properties.Resources.String_FileDialog_Filter_Common_All, string.Empty, "*"),
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
                var item = InputtedHistories.FirstOrDefault(i => i.Value == rawValue);
                if(item != null) {
                    InputtedHistories.Remove(item);
                }
                InputtedHistories.Insert(0, element);
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
            if(Model.PreparedReceive && Model.OutputStreamReceiver != null) {
                Model.OutputStreamReceiver.StreamReceived -= OutputStreamReceiver_StreamReceived;
                Model.OutputStreamReceiver.StreamReceived += OutputStreamReceiver_StreamReceived;
            }
            if(Model.PreparedReceive && Model.ErrorStreamReceiver != null) {
                Model.ErrorStreamReceiver.StreamReceived -= ErrorStreamReceiver_StreamReceived;
                Model.ErrorStreamReceiver.StreamReceived += ErrorStreamReceiver_StreamReceived;
            }
            //if(Model.PreparatedReceive && Model.ProcessStandardOutputReceiver != null) {
            //    Model.ProcessStandardOutputReceiver.StandardOutputReceived += ProcessStandardOutputReceiver_StandardOutputReceived;
            //}
        }

        private void DetachReceiver()
        {
            if(Model.ProcessExited) {
                if(Model.OutputStreamReceiver != null) {
                    Model.OutputStreamReceiver.StreamReceived -= OutputStreamReceiver_StreamReceived;
                }
                if(Model.ErrorStreamReceiver != null) {
                    Model.ErrorStreamReceiver.StreamReceived -= ErrorStreamReceiver_StreamReceived;
                }
            }
        }


        private void AppendOutput(string value, bool isError)
        {
            Logger.LogTrace("{0}", value);
            if(Terminal == null) {
                Logger.LogTrace("来ちゃいけない制御フロー");
                return;
            }

            DispatcherWrapper.BeginAsync(() => {
                var prevLine = TextDocument.Lines.Last<DocumentLine>();
                var prevEndOffset = prevLine.EndOffset;

                var index = TextDocument.TextLength;
                var length = value.Length;

                TextDocument.Insert(TextDocument.TextLength, value);
                if(isError) {
                    var lastLine = TextDocument.Lines.Last<DocumentLine>();
                    var lastEndOffset = lastLine.EndOffset;
                    if(StandardOutputColorizingTransformer == null) {
                        StandardOutputColorizingTransformer = new StandardOutputColorizingTransformer(StandardErrorForeground);
                        Terminal.TextArea.TextView.LineTransformers.Add(StandardOutputColorizingTransformer);
                    }
                    StandardOutputColorizingTransformer.Add(prevEndOffset, lastEndOffset);
                }

                if(AutoScroll && Terminal != null) {
                    Terminal.ScrollToEnd();
                }
            });
        }

        void SaveLog(string path)
        {
            Logger.LogDebug("{0}", path);
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
            Terminal.TextArea.Caret.CaretBrush = Brushes.Transparent;
            Terminal.TextChanged += Terminal_TextChanged;
        }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }

        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed(Window window, bool isUserOperation)
        {
            if(Terminal != null) {
                Terminal.TextChanged -= Terminal_TextChanged;
            }

            Model.ReceiveViewClosed(isUserOperation);
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
                    if(Model.OutputStreamReceiver != null) {
                        Model.OutputStreamReceiver.StreamReceived -= OutputStreamReceiver_StreamReceived;
                    }
                    if(Model.ErrorStreamReceiver != null) {
                        Model.ErrorStreamReceiver.StreamReceived -= ErrorStreamReceiver_StreamReceived;
                    }

                    PropertyChangedHooker.Dispose();
                }
            }

            base.Dispose(disposing);
        }


        #endregion

        private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
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
            if(e.Exited) {
                Model.OutputStreamReceiver!.StreamReceived -= OutputStreamReceiver_StreamReceived;
            }
        }
        private void ErrorStreamReceiver_StreamReceived(object? sender, StreamReceivedEventArgs e)
        {
            AppendOutput(e.Value, true);
            if(e.Exited) {
                Model.ErrorStreamReceiver!.StreamReceived -= ErrorStreamReceiver_StreamReceived;
            }
        }
    }
}
