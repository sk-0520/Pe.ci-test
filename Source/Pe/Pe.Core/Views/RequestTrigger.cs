using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;

namespace ContentTypeTextNet.Pe.Core.Views
{
    public class RequestTrigger : System.Windows.Interactivity.EventTrigger
    {
        public RequestTrigger()
        { }

        public RequestTrigger(string eventName)
            : base(eventName)
        { }

        #region property
        #endregion

        #region TriggerAction

        protected override string GetEventName()
        {
            return nameof(IRequestSender.Raised);
        }

        #endregion
    }

}
