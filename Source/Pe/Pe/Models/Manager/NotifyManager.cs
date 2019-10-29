using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public class NotifyEventArgs: EventArgs
    { }

    public class LauncherItemChangedEventArgs: NotifyEventArgs
    {
        public LauncherItemChangedEventArgs(IReadOnlyCollection<Guid> launcherItemIds)
        {
            LauncherItemIds = launcherItemIds;
        }

        #region property

        public IReadOnlyCollection<Guid> LauncherItemIds { get; }

        #endregion
    }

    /// <summary>
    /// アプリケーションからの通知を発行する。
    /// </summary>
    public interface INotifyManager
    {
        #region event

        event EventHandler<LauncherItemChangedEventArgs>? LauncherItemChanged;

        #endregion

        #region function

        void SendLauncherItemChanged(IReadOnlyCollection<Guid> launcherItemIds);

        #endregion
    }

    partial class ApplicationManager
    {
        class NotifyManagerImpl : ManagerBase, INotifyManager
        {
            public NotifyManagerImpl(IDiContainer diContainer, ILoggerFactory loggerFactory)
                : base(diContainer, loggerFactory)
            { }

            #region function

            void OnLauncherItemChanged(IReadOnlyCollection<Guid> launcherItemIds)
            {
                var e = new LauncherItemChangedEventArgs(launcherItemIds);
                LauncherItemChanged?.Invoke(this, e);
            }

            #endregion

            #region INotifyManager

            public event EventHandler<LauncherItemChangedEventArgs>? LauncherItemChanged;

            public void SendLauncherItemChanged(IReadOnlyCollection<Guid> launcherItemIds)
            {
                OnLauncherItemChanged(launcherItemIds);
            }

            #endregion
        }
    }
}
