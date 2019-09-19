using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.StandardInputOutput
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
