using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.ExtendsExecute
{
    public class ExtendsExecuteViewModel : SingleModelViewModelBase<ExtendsExecuteElement>, IViewLifecycleReceiver
    {
        #region variable

        string _option;
        string _workDirectoryPath;

        TextDocument _mergeTextDocument;
        TextDocument _removeTextDocument;

        bool _isEnabledStandardInputOutput;
        bool _runAdministrator;
        bool _isEnabledCustomEnvironmentVariable;
        #endregion

        public ExtendsExecuteViewModel(ExtendsExecuteElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            this._option = Model.LauncherFileData.Option;
            this._workDirectoryPath = Model.LauncherFileData.WorkDirectoryPath;

            this._isEnabledStandardInputOutput = Model.LauncherFileData.IsEnabledStandardInputOutput;
            this._runAdministrator = Model.LauncherFileData.RunAdministrator;
            this._isEnabledCustomEnvironmentVariable = Model.LauncherFileData.IsEnabledCustomEnvironmentVariable;

            HistoryOptions = new ObservableCollection<HistoryViewModel>(Model.HistoryOptions.Select(i => new HistoryViewModel(i, CultureInfo.CurrentUICulture, loggerFactory)));
            HistoryOptions.Insert(0, new HistoryViewModel(Model.LauncherFileData.Option, loggerFactory));

            HistoryWorkDirectories = new ObservableCollection<HistoryViewModel>(Model.HistoryWorkDirectories.Select(i => new HistoryViewModel(i, CultureInfo.CurrentUICulture, loggerFactory)));
            HistoryWorkDirectories.Insert(0, new HistoryViewModel(Model.LauncherFileData.WorkDirectoryPath, loggerFactory));

            var envConf = new EnvironmentVariableConfiguration(LoggerFactory);
            this._mergeTextDocument = envConf.CreateMergeDocument(Model.EnvironmentVariables);
            this._removeTextDocument = envConf.CreateRemoveDocument(Model.EnvironmentVariables);
        }

        #region property
        public RequestSender CloseRequest { get; } = new RequestSender();
        public RequestSender FileSelectRequest { get; } = new RequestSender();


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

        public ObservableCollection<HistoryViewModel> HistoryOptions { get; }
        public ObservableCollection<HistoryViewModel> HistoryWorkDirectories { get; }

        public bool IsEnabledStandardInputOutput
        {
            get => this._isEnabledStandardInputOutput;
            set => SetProperty(ref this._isEnabledStandardInputOutput, value);
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

        #endregion

        #region command

        public ICommand ExecuteCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                CloseRequest.Send();
            }
        ));

        public ICommand OptionFileSelectCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var environmentExecuteFile = new EnvironmentExecuteFile(LoggerFactory);
                var exeExts = environmentExecuteFile.GetSystemExecuteExtensions(true);

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
         ));
        public ICommand OptionDirectorySelectCommand => GetOrCreateCommand(() => new DelegateCommand(
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
        ));

        public ICommand WorkDirectorySelectCommand => GetOrCreateCommand(() => new DelegateCommand(
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
        ));

        #endregion

        #region function

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

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
            Model.ReceiveViewClosed();
        }

        #endregion

    }
}
