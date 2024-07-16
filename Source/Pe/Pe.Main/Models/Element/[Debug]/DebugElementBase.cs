using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element._Debug_
{
    public abstract class DebugElementBase: ElementBase, IViewCloseReceiver
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

        /// <inheritdoc cref="IViewCloseReceiver.ReceiveViewClosedAsync(bool, CancellationToken)"/>
        public virtual Task ReceiveViewClosedAsync(bool isUserOperation, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


        #endregion
    }
}
