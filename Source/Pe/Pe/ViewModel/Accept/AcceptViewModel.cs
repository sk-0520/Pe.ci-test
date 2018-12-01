using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.ViewElement.Accept;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Accept
{
    public class AcceptViewModel : SingleModelViewModelBase<AcceptViewElement>, IDialogCommand
    {
        public AcceptViewModel(AcceptViewElement model, ILogger logger)
            : base(model, logger)
        { }

        #region property
        #endregion

        #region property

        public InteractionRequest<Notification> CloseRequest { get; } = new InteractionRequest<Notification>();


        #endregion

        #region IDialogCommand

        public ICommand AffirmativeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.Accepted = true;
                CloseRequest.Raise(new Notification());
            }
        ));
        public ICommand NegativeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                CloseRequest.Raise(new Notification());
            }
        ));

        #endregion

        #region function
        #endregion
    }
}
