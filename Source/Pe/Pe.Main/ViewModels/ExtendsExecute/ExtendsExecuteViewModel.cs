using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.ExtendsExecute
{
    public class ExtendsExecuteViewModel : SingleModelViewModelBase<ExtendsExecuteElement>, IViewLifecycleReceiver
    {
        #region variable

        TextDocument _mergeTextDocument;
        TextDocument _removeTextDocument;

        #endregion

        public ExtendsExecuteViewModel(ExtendsExecuteElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            // 重複: LauncherItemCustomizeEnvironmentVariableViewModel.InitializeImpl
            var mergeItems = Model.EnvironmentVariables
                .Where(i => !i.IsRemove)
                .Select(i => $"{i.Name}={i.Value}")
            ;

            var removeItems = Model.EnvironmentVariables
                .Where(i => i.IsRemove)
                .Select(i => i.Name)
            ;

            this._mergeTextDocument = new TextDocument(string.Join(Environment.NewLine, mergeItems));
            this._removeTextDocument = new TextDocument(string.Join(Environment.NewLine, removeItems));
        }

        #region property
        public RequestSender CloseRequest { get; } = new RequestSender();

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
