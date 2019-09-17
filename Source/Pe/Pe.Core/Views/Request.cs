using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;

namespace ContentTypeTextNet.Pe.Core.Views
{
    public class RequestEventArgs : EventArgs
    {
        public RequestEventArgs(Action callback)
        {
            Callback = callback;
        }

        #region property

        public Action Callback { get; }

        #endregion
    }

    public class RequestSender
    {
        #region event

        public event EventHandler<RequestEventArgs> Raised;

        #endregion

        #region function

        public void Send(Action response)
        {
            Raised!.Invoke(this, new RequestEventArgs(response));
        }

        #endregion
    }
}
