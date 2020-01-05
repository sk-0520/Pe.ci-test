using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Core.Views;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Accept;
using ContentTypeTextNet.Pe.Main.Views.Accept;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Accept
{
    public class AcceptViewModel : SingleModelViewModelBase<AcceptElement>, IDialogCommand, IDialogService, IViewLifecycleReceiver
    {
        public AcceptViewModel(AcceptElement model, Configuration configuration, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            Configuration = configuration;
        }

        #region property

        //public InteractionRequest<Notification> CloseRequest { get; } = new InteractionRequest<Notification>();
        public RequestSender CloseRequest { get; } = new RequestSender();

        Configuration Configuration { get; }

        public bool SendUsageStatistics
        {
            get => Model.SendUsageStatistics;
            set => SetModelValue(value);
        }

        public bool CheckUpdate
        {
            get => Model.CheckUpdate;
            set => SetModelValue(value);
        }

        #endregion

        #region command

        public ICommand OpenProjectUriCommand => GetOrCreateCommand(() => new DelegateCommand(
           () => OpenUri(Configuration.General.ProjectRepositoryUri)
        ));
        public ICommand OpenForumUriCommand => GetOrCreateCommand(() => new DelegateCommand(
           () => OpenUri(Configuration.General.ProjectForumUri)
        ));
        public ICommand OpenWebSiteUriCommand => GetOrCreateCommand(() => new DelegateCommand(
           () => OpenUri(Configuration.General.ProjectWebSiteUri)
        ));

        #endregion

        #region function

        void OpenUri(Uri uri)
        {

        }

        #endregion

        #region IDialogCommand

        public ICommand AffirmativeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ThrowIfDisposed();

                Model.Accepted = true;
                CloseRequest.Send();
            }
        ));
        public ICommand NegativeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ThrowIfDisposed();

                CloseRequest.Send();
            }
        ));

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
            var view = (AcceptWindow)window;
            //view.documentAccept.do
            using(var stream = Model.GetAcceptDocumentXamlStream()) {
                view.documentAccept.Document = (FlowDocument)XamlReader.Load(stream);
            }
        }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(CancelEventArgs e)
        { }


        public void ReceiveViewClosing(CancelEventArgs e)
        { }

        public void ReceiveViewClosed()
        { }

        #endregion

        #region IDialogService

        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            throw new NotImplementedException();
        }

        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
