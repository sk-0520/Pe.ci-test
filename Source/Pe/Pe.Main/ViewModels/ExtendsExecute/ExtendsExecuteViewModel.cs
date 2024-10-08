using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using ContentTypeTextNet.Pe.Library.Base;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base.Linq;
using System.Threading;

namespace ContentTypeTextNet.Pe.Main.ViewModels.ExtendsExecute
{
    public class ExtendsExecuteViewModel: ElementViewModelBase<ExtendsExecuteElement>, IViewLifecycleReceiver
    {
        #region variable

        private string _option;
        private string _workDirectoryPath;
        private ShowMode _showMode;

        private TextDocument _mergeTextDocument;
        private TextDocument _removeTextDocument;

        private bool _isEnabledCustomEnvironmentVariable;
        private bool _isEnabledStandardInputOutput;
        private Encoding _standardInputOutputEncoding;
        private bool _runAdministrator;

        private RedoMode _redoMode;
        private int _waitTimeSeconds;
        private int _retryCount;
        private string _successExitCodes = string.Empty;

        #endregion

        public ExtendsExecuteViewModel(ExtendsExecuteElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            this._option = Model.LauncherFileData.Option;
            this._workDirectoryPath = Model.LauncherFileData.WorkDirectoryPath;
            this._showMode = Model.LauncherFileData.ShowMode;

            this._isEnabledCustomEnvironmentVariable = Model.LauncherFileData.IsEnabledCustomEnvironmentVariable;
            this._isEnabledStandardInputOutput = Model.LauncherFileData.IsEnabledStandardInputOutput;
            this._standardInputOutputEncoding = Model.LauncherFileData.StandardInputOutputEncoding;
            this._runAdministrator = Model.LauncherFileData.RunAdministrator;

            HistoryOptions = new ObservableCollection<HistoryViewModel>(Model.HistoryOptions.Select(i => new HistoryViewModel(i, CultureInfo.CurrentUICulture, loggerFactory)));
            HistoryOptions.Insert(0, new HistoryViewModel(Model.LauncherFileData.Option, loggerFactory));

            HistoryWorkDirectories = new ObservableCollection<HistoryViewModel>(Model.HistoryWorkDirectories.Select(i => new HistoryViewModel(i, CultureInfo.CurrentUICulture, loggerFactory)));
            HistoryWorkDirectories.Insert(0, new HistoryViewModel(Model.LauncherFileData.WorkDirectoryPath, loggerFactory));

            var envConf = new EnvironmentVariableConfiguration(LoggerFactory);
            this._mergeTextDocument = envConf.CreateMergeDocument(Model.EnvironmentVariables);
            this._removeTextDocument = envConf.CreateRemoveDocument(Model.EnvironmentVariables);

            //TODO: 自家製DIのコンストラクタキャッシュ問題によるダウンキャスト
            if(model is LauncherExtendsExecuteElement element) {
                if(element.CustomOption != null) {
                    this._option = element.CustomOption;
                }
            }

            RedoMode = Model.LauncherRedoData.RedoMode;
            WaitTimeSeconds = (int)Model.LauncherRedoData.WaitTime.TotalSeconds;
            RetryCount = Model.LauncherRedoData.RetryCount;
            if(Model.LauncherRedoData.SuccessExitCodes.Any()) {
                var numericRange = new NumericRange();
                SuccessExitCodes = numericRange.ToString(Model.LauncherRedoData.SuccessExitCodes);
            } else {
                SuccessExitCodes = "0";
            }

            OptionDragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = OptionCanDragStart,
                DragEnterAction = OptionDragOverOrEnter,
                DragOverAction = OptionDragOverOrEnter,
                DragLeaveAction = OptionDragLeave,
                DropActionAsync = OptionDropAsync,
                GetDragParameter = OptionGetDragParameter,
            };
            WorkDirectoryDragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = WorkDirectoryCanDragStart,
                DragEnterAction = WorkDirectoryDragOversOrEnter,
                DragOverAction = WorkDirectoryDragOversOrEnter,
                DragLeaveAction = WorkDirectoryDragLeave,
                DropActionAsync = WorkDirectoryDropAsync,
                GetDragParameter = WorkDirectoryGetDragParameter,
            };
        }

        #region property

        public RequestSender CloseRequest { get; } = new RequestSender();
        public RequestSender FileSelectRequest { get; } = new RequestSender();

        private IDpiScaleOutpour DpiScaleOutpour { get; set; } = new EmptyDpiScaleOutpour();

        public IDragAndDrop OptionDragAndDrop { get; }
        public IDragAndDrop WorkDirectoryDragAndDrop { get; }

        public string Title
        {
            get
            {
                return TextUtility.ReplaceFromDictionary(
                    Properties.Resources.String_ExtendsExecute_Caption_Format,
                    new Dictionary<string, string>() {
                        ["ITEM"] = Model.CaptionName,
                    }
                );
            }
        }

        public string ExecuteValue => Model.LauncherFileData.Path;

        public string Option
        {
            get => this._option;
            set => SetProperty(ref this._option, value);
        }
        public string WorkDirectoryPath
        {
            get => this._workDirectoryPath;
            set => SetProperty(ref this._workDirectoryPath, value);
        }

        public ShowMode ShowMode
        {
            get => this._showMode;
            set => SetProperty(ref this._showMode, value);
        }
        public ObservableCollection<HistoryViewModel> HistoryOptions { get; }
        public ObservableCollection<HistoryViewModel> HistoryWorkDirectories { get; }

        public bool IsEnabledStandardInputOutput
        {
            get => this._isEnabledStandardInputOutput;
            set => SetProperty(ref this._isEnabledStandardInputOutput, value);
        }
        public Encoding StandardInputOutputEncoding
        {
            get => this._standardInputOutputEncoding;
            set => SetProperty(ref this._standardInputOutputEncoding, value);
        }
        public bool RunAdministrator
        {
            get => this._runAdministrator;
            set => SetProperty(ref this._runAdministrator, value);
        }
        public bool IsEnabledCustomEnvironmentVariable
        {
            get => this._isEnabledCustomEnvironmentVariable;
            set => SetProperty(ref this._isEnabledCustomEnvironmentVariable, value);
        }

        public TextDocument MergeTextDocument
        {
            get => this._mergeTextDocument;
            set => SetProperty(ref this._mergeTextDocument, value);
        }
        public TextDocument RemoveTextDocument
        {
            get => this._removeTextDocument;
            set => SetProperty(ref this._removeTextDocument, value);
        }

        public ObservableCollection<string> MergeErrors { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> RemoveErrors { get; } = new ObservableCollection<string>();


        public RedoMode RedoMode
        {
            get => this._redoMode;
            set => SetProperty(ref this._redoMode, value);
        }
        public int WaitTimeSeconds
        {
            get => this._waitTimeSeconds;
            set => SetProperty(ref this._waitTimeSeconds, value);
        }
        public int RetryCount
        {
            get => this._retryCount;
            set => SetProperty(ref this._retryCount, value);
        }
        public string SuccessExitCodes
        {
            get => this._successExitCodes;
            set => SetProperty(ref this._successExitCodes, value);
        }
        public int MinimumWaitTimeSeconds => 1;
        public int MaximumWaitTimeSeconds => 300;
        public int MinimumRetryCount => 1;
        public int MaximumRetryCount => 100;

        #endregion

        #region command

        private ICommand? _ExecuteCommand;
        public ICommand ExecuteCommand => this._ExecuteCommand ??= new DelegateCommand(
            () => {
                if(Validate()) {
                    ExecuteAsync(CancellationToken.None);
                    CloseRequest.Send();
                }
            }
        );

        private ICommand? _OptionFileSelectCommand;
        public ICommand OptionFileSelectCommand => this._OptionFileSelectCommand ??= new DelegateCommand(
            () => {
                var environmentExecuteFile = new EnvironmentExecuteFile(LoggerFactory);
                var exeExtensions = environmentExecuteFile.GetSystemExecuteExtensions(true);

                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectFile(
                    FileSelectRequest,
                    dialogRequester.ExpandPath(Option),
                    true,
                   new[] { dialogRequester.CreateAllFilter() },
                    r => {
                        Option = r.ResponseFilePaths[0];
                    }
                );
            }
         );

        private ICommand? _OptionDirectorySelectCommand;
        public ICommand OptionDirectorySelectCommand => this._OptionDirectorySelectCommand ??= new DelegateCommand(
            () => {
                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectDirectory(
                    FileSelectRequest,
                    dialogRequester.ExpandPath(Option),
                    r => {
                        Option = r.ResponseFilePaths[0];
                    }
                );
            }
        );

        private ICommand? _WorkDirectorySelectCommand;
        public ICommand WorkDirectorySelectCommand => this._WorkDirectorySelectCommand ??= new DelegateCommand(
            () => {
                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectDirectory(
                    FileSelectRequest,
                    dialogRequester.ExpandPath(WorkDirectoryPath),
                    r => {
                        WorkDirectoryPath = r.ResponseFilePaths[0];
                    }
                );
            }
        );

        private ICommand? _RemoveHistoryCommand;
        public ICommand RemoveHistoryCommand => this._RemoveHistoryCommand ??= new DelegateCommand<HistoryViewModel>(
            o => {
                var removed = Model.RemoveHistory(o.Kind, o.LastExecuteTimestamp);
                var items = o.Kind switch {
                    LauncherHistoryKind.Option => HistoryOptions,
                    LauncherHistoryKind.WorkDirectory => HistoryWorkDirectories,
                    _ => throw new NotImplementedException(),
                };

                var removeItems = items.Skip(1).Where(i => i.LastExecuteTimestamp == o.LastExecuteTimestamp);
                foreach(var removeItem in removeItems) {
                    items.Remove(removeItem);
                }
            },
            o => o is not null && o.CanRemove
        );

        #endregion

        #region function

        private Task ExecuteAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var launcherFileData = new LauncherFileData() {
                Caption = Model.CaptionName,
                Path = ExecuteValue,
                Option = Option,
                WorkDirectoryPath = WorkDirectoryPath,
                ShowMode = ShowMode,
                IsEnabledCustomEnvironmentVariable = IsEnabledCustomEnvironmentVariable,
                IsEnabledStandardInputOutput = IsEnabledStandardInputOutput,
                StandardInputOutputEncoding = StandardInputOutputEncoding,
                RunAdministrator = RunAdministrator,
            };
            IReadOnlyList<LauncherEnvironmentVariableData> envItems;
            if(launcherFileData.IsEnabledCustomEnvironmentVariable) {
                var envConf = new EnvironmentVariableConfiguration(LoggerFactory);
                var mergeItems = envConf.GetMergeItems(MergeTextDocument);
                var removeItems = envConf.GetRemoveItems(RemoveTextDocument);
                envItems = envConf.Join(mergeItems, removeItems);
            } else {
                envItems = new List<LauncherEnvironmentVariableData>();
            }

            var redo = new LauncherRedoData() {
                RedoMode = RedoMode,
                RetryCount = RetryCount,
                WaitTime = TimeSpan.FromSeconds(WaitTimeSeconds)
            };
            if(redo.RedoMode != RedoMode.None) {
                var numericRange = new NumericRange();
                if(numericRange.TryParse(SuccessExitCodes, out var values)) {
                    redo.SuccessExitCodes.SetRange(values);
                } else {
                    Logger.LogError("終了コードが分解できず: {0}", SuccessExitCodes);
                    redo.RedoMode = RedoMode.None;
                }
            }

            var screen = DpiScaleOutpour.GetOwnerScreen();

            return Model.ExecuteAsync(launcherFileData, envItems, redo, screen, cancellationToken);
        }

        #endregion

        #region OptionDragAndDrop

        private bool OptionCanDragStart(UIElement sender, MouseEventArgs e)
        {
            var dd = new OptionDragAndDropGuideline(DispatcherWrapper, LoggerFactory);
            return dd.CanDragStart(sender, e);
        }

        private void OptionDragOverOrEnter(UIElement sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effects = DragDropEffects.Copy;
            } else if(e.Data.IsTextPresent()) {
                e.Effects = DragDropEffects.Copy;
            } else {
                e.Effects = DragDropEffects.None;
            }
        }

        private void OptionDragLeave(UIElement sender, DragEventArgs e)
        { }

        private Task OptionDropAsync(UIElement sender, DragEventArgs e, CancellationToken cancellationToken)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                Option = string.Join(" ", filePaths.Select(i => CommandLine.Escape(i)));
                e.Handled = true;
            } else if(e.Data.IsTextPresent()) {
                Option = TextUtility.JoinLines(e.Data.RequireText());
                e.Handled = true;
            }

            return Task.CompletedTask;
        }

        private IResultSuccess<DragParameter> OptionGetDragParameter(UIElement sender, MouseEventArgs e)
        {
            return Result.CreateFailure<DragParameter>();
        }

        #endregion

        #region WorkDirectoryDragAndDrop

        private bool WorkDirectoryCanDragStart(UIElement sender, MouseEventArgs e)
        {
            var dd = new WorkDirectoryDragAndDropGuideline(DispatcherWrapper, LoggerFactory);
            return dd.CanDragStart(sender, e);
        }

        private void WorkDirectoryDragOversOrEnter(UIElement sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effects = DragDropEffects.Copy;
            } else {
                e.Effects = DragDropEffects.None;
            }
        }

        private void WorkDirectoryDragLeave(UIElement sender, DragEventArgs e)
        { }

        private Task WorkDirectoryDropAsync(UIElement sender, DragEventArgs e, CancellationToken cancellationToken)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach(var filePath in filePaths) {
                    try {
                        if(Directory.Exists(filePath)) {
                            WorkDirectoryPath = filePath;
                            e.Handled = true;
                            break;
                        } else {
                            Logger.LogInformation("非ディレクトリ: {0}", filePath);
                        }
                    } catch(Exception ex) {
                        Logger.LogInformation(ex, "非ディレクトリ: {0}", filePath);
                    }
                }

                if(!e.Handled) {
                    Logger.LogInformation("D&Dデータはディレクトリではない");
                }
            }

            return Task.CompletedTask;
        }

        private IResultSuccess<DragParameter> WorkDirectoryGetDragParameter(UIElement sender, MouseEventArgs e)
        {
            return Result.CreateFailure<DragParameter>();
        }

        #endregion

        #region SingleModelViewModelBase

        protected override void ValidateDomain()
        {
            ThrowIfDisposed();

            //if(RedoWait != RedoWait.None) {
            //    var numericRange = new NumericRange();
            //    if(!numericRange.TryParse(SuccessExitCodes, out _)) {
            //        AddError(nameof(NumericRange), nameof(SuccessExitCodes));
            //    }
            //}

            var envConf = new EnvironmentVariableConfiguration(LoggerFactory);

            envConf.SetValidateCommon(MergeTextDocument!, envConf.ValidateMergeDocument, seq => AddErrors(seq, nameof(MergeTextDocument)), MergeErrors);
            envConf.SetValidateCommon(RemoveTextDocument!, envConf.ValidateRemoveDocument, seq => AddErrors(seq, nameof(RemoveTextDocument)), RemoveErrors);
        }

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        {
            DpiScaleOutpour = (IDpiScaleOutpour)window;

            WindowsUtility.ShowActiveForeground(HandleUtility.GetWindowHandle(window));
        }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }


        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public Task ReceiveViewClosedAsync(Window window, bool isUserOperation, CancellationToken cancellationToken)
        {
            return Model.ReceiveViewClosedAsync(isUserOperation, cancellationToken);
        }

        #endregion
    }
}
