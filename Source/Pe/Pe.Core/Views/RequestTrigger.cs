using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;

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
            return "Raised";
        }

        #endregion
    }
}
