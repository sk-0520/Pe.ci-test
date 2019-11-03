using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element._Debug_
{
    public abstract class DebugElementBase : ElementBase, IViewCloseReceiver
    {
        public DebugElementBase(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region property

        #endregion

        #region IViewCloseReceiver

        public virtual bool ReceiveViewUserClosing()
        {
            return true;
        }
        public virtual bool ReceiveViewClosing()
        {
            return true;
        }

        public virtual void ReceiveViewClosed()
        {
        }


        #endregion

    }
}
