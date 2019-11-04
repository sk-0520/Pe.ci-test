using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute;
using ContentTypeTextNet.Pe.Main.Models.Logic;
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

            var envConf = new EnvironmentVariableConfiguration(LoggerFactory);
            this._mergeTextDocument = envConf.CreateMergeDocument(Model.EnvironmentVariables);
            this._removeTextDocument = envConf.CreateRemoveDocument(Model.EnvironmentVariables);
        }

        #region property
        public RequestSender CloseRequest { get; } = new RequestSender();

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
