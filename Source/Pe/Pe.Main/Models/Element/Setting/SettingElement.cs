using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class SettingElement : ElementBase, IViewCloseReceiver
    {
        public SettingElement(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        #region property
        #endregion

        #region function
        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            Logger.LogTrace("todo");
        }

        #endregion

        #region IViewCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            return true;
        }

        public bool ReceiveViewClosing()
        {
            return true;
        }

        public void ReceiveViewClosed()
        { }


        #endregion

    }
}
