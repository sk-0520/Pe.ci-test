using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.StandardInputOutput;

namespace ContentTypeTextNet.Pe.Main.ViewModel.StandardInputOutput
{
    public class StandardInputOutputViewModel : SingleModelViewModelBase<StandardInputOutputElement>, IViewLifecycleReceiver
    {
        public StandardInputOutputViewModel(StandardInputOutputElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region IViewLifecycleReceiver
        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(CancelEventArgs e) => Model.ReceiveViewUserClosing();

        public void ReceiveViewClosing(CancelEventArgs e) => Model.ReceiveViewClosing();

        public void ReceiveViewClosed() => Model.ReceiveViewClosed();

        #endregion

        #region SingleModelViewModelBase
        #endregion
    }
}
