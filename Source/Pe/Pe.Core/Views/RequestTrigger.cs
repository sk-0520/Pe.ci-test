using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;

namespace ContentTypeTextNet.Pe.Core.Views
{
    public class RequestTrigger : Microsoft.Xaml.Behaviors.EventTrigger
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
