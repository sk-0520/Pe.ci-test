using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element._Debug_;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels._Debug_
{
    public abstract class DebugViewModelBase<TModel>: SingleModelViewModelBase<TModel>, IViewLifecycleReceiver
        where TModel : DebugElementBase
    {
        protected DebugViewModelBase(TModel model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region IViewLifecycleReceiver

        public virtual void ReceiveViewInitialized(Window window)
        {
        }

        public virtual void ReceiveViewLoaded(Window window)
        {
        }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }

        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        /// <inheritdoc cref="IViewLifecycleReceiver.ReceiveViewClosedAsync(Window, bool, CancellationToken)"/>
        public Task ReceiveViewClosedAsync(Window window, bool isUserOperation, CancellationToken cancellationToken)
        {
            return Model.ReceiveViewClosedAsync(isUserOperation, cancellationToken);
        }

        #endregion
    }
}
