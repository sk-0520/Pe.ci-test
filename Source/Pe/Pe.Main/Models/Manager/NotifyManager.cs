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

    public class CustomizeLauncherItemExitedEventArgs: NotifyEventArgs
    {
        public CustomizeLauncherItemExitedEventArgs(Guid launcherItemId)
        {
            LauncherItemId = launcherItemId;
        }

        #region property

        public Guid LauncherItemId { get; }
        #endregion
    }

    /// <summary>
    /// アプリケーションからの通知を発行する。
    /// </summary>
    public interface INotifyManager
    {
        #region event

        event EventHandler<LauncherItemChangedEventArgs>? LauncherItemChanged;
        event EventHandler<CustomizeLauncherItemExitedEventArgs>? CustomizeLauncherItemExited;

        #endregion

        #region function

        void SendLauncherItemChanged(IReadOnlyCollection<Guid> launcherItemIds);
        void SendCustomizeLauncherItemExited(Guid launcherItemId);

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

            void OnCustomizeLauncherItemExited(Guid launcherItemId)
            {
                var e = new CustomizeLauncherItemExitedEventArgs(launcherItemId);
                CustomizeLauncherItemExited?.Invoke(this, e);
            }

            #endregion

            #region INotifyManager

            public event EventHandler<LauncherItemChangedEventArgs>? LauncherItemChanged;
            public event EventHandler<CustomizeLauncherItemExitedEventArgs>? CustomizeLauncherItemExited;

            public void SendLauncherItemChanged(IReadOnlyCollection<Guid> launcherItemIds)
            {
                OnLauncherItemChanged(launcherItemIds);
            }

            public void SendCustomizeLauncherItemExited(Guid launcherItemId)
            {
                OnCustomizeLauncherItemExited(launcherItemId);
            }


            #endregion
        }
    }
}
