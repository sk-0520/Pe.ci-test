using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.ViewModel;
using ContentTypeTextNet.Pe.Main.Model.Element.Accept;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Accept
{
    public class AcceptViewModel : SingleModelViewModelBase<AcceptElement>, IDialogCommand, IDialogService
    {
        public AcceptViewModel(AcceptElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        //public InteractionRequest<Notification> CloseRequest { get; } = new InteractionRequest<Notification>();

        #endregion

        #region IDialogCommand

        public ICommand AffirmativeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.Accepted = true;
                //CloseRequest.Raise(new Notification());
            }
        ));
        public ICommand NegativeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                //CloseRequest.Raise(new Notification());
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
