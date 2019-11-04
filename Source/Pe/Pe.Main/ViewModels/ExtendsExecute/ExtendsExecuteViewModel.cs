using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.ExtendsExecute
{
    public class ExtendsExecuteViewModel : SingleModelViewModelBase<ExtendsExecuteElement>, IViewLifecycleReceiver
    {
        public ExtendsExecuteViewModel(ExtendsExecuteElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property
        public RequestSender CloseRequest { get; } = new RequestSender();

        #endregion

        #region command

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
