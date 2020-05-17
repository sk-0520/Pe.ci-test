using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element._Debug_
{
    public abstract class DebugElementBase : ElementBase, IViewCloseReceiver
    {
        protected DebugElementBase(ILoggerFactory loggerFactory)
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

        /// <inheritdoc cref="IViewCloseReceiver.ReceiveViewClosed(bool)"/>
        public virtual void ReceiveViewClosed(bool isUserOperation)
        {
        }


        #endregion

    }
}
