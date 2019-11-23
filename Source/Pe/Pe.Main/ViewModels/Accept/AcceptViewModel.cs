using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Core.Views;
using ContentTypeTextNet.Pe.Main.Models.Element.Accept;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Accept
{
    public class AcceptViewModel : SingleModelViewModelBase<AcceptElement>, IDialogCommand, IDialogService
    {
        public AcceptViewModel(AcceptElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        //public InteractionRequest<Notification> CloseRequest { get; } = new InteractionRequest<Notification>();
        public RequestSender CloseRequest { get; } = new RequestSender();

        #endregion

        #region IDialogCommand

        public ICommand AffirmativeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.Accepted = true;
                CloseRequest.Send();
            }
        ));
        public ICommand NegativeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                CloseRequest.Send();
            }
        ));

        #endregion

        #region function
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
