using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public class NotifyEventArgs : EventArgs
    { }

    public class LauncherItemChangedEventArgs : NotifyEventArgs
    {
        public LauncherItemChangedEventArgs(Guid launcherItemId)
        {
            LauncherItemId = launcherItemId;
        }

        #region property

        public Guid LauncherItemId { get; }

        #endregion
    }

    public class LauncherItemRegisteredEventArgs : NotifyEventArgs
    {
        public LauncherItemRegisteredEventArgs(Guid groupId, Guid launcherItemId)
        {
            GroupId = groupId;
            LauncherItemId = launcherItemId;
        }

        #region property

        public Guid GroupId { get; }
        public Guid LauncherItemId { get; }

        #endregion
    }


    public class CustomizeLauncherItemExitedEventArgs : NotifyEventArgs
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
        event EventHandler<LauncherItemRegisteredEventArgs>? LauncherItemRegistered;
        event EventHandler<CustomizeLauncherItemExitedEventArgs>? CustomizeLauncherItemExited;

        #endregion

        #region function

        void SendLauncherItemChanged(Guid launcherItemIds);
        void SendLauncherItemRegistered(Guid groupId, Guid launcherItemId);
        void SendCustomizeLauncherItemExited(Guid launcherItemId);

        #endregion
    }

    public class NotifyManager : ManagerBase, INotifyManager
    {
        public NotifyManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        { }

        #region function

        [Conditional("DEBUG")]
        private void ThrowIfEmptyLauncherItemId(Guid launcherItemId)
        {
            if(launcherItemId == Guid.Empty) {
                throw new InvalidOperationException();
            }
        }


        void OnLauncherItemChanged(Guid launcherItemId)
        {
            ThrowIfEmptyLauncherItemId(launcherItemId);

            var e = new LauncherItemChangedEventArgs(launcherItemId);
            LauncherItemChanged?.Invoke(this, e);
        }

        void OnLauncherItemRegistered(Guid groupId, Guid launcherItemId)
        {
            ThrowIfEmptyLauncherItemId(launcherItemId);

            var e = new LauncherItemRegisteredEventArgs(groupId, launcherItemId);
            LauncherItemRegistered?.Invoke(this, e);
        }

        void OnCustomizeLauncherItemExited(Guid launcherItemId)
        {
            ThrowIfEmptyLauncherItemId(launcherItemId);

            var e = new CustomizeLauncherItemExitedEventArgs(launcherItemId);
            CustomizeLauncherItemExited?.Invoke(this, e);
        }

        #endregion

        #region INotifyManager

        public event EventHandler<LauncherItemChangedEventArgs>? LauncherItemChanged;
        public event EventHandler<LauncherItemRegisteredEventArgs>? LauncherItemRegistered;
        public event EventHandler<CustomizeLauncherItemExitedEventArgs>? CustomizeLauncherItemExited;

        public void SendLauncherItemChanged(Guid launcherItemId)
        {
            OnLauncherItemChanged(launcherItemId);
        }
        public void SendLauncherItemRegistered(Guid groupId, Guid launcherItemId)
        {
            OnLauncherItemRegistered(groupId, launcherItemId);
        }

        public void SendCustomizeLauncherItemExited(Guid launcherItemId)
        {
            OnCustomizeLauncherItemExited(launcherItemId);
        }


        #endregion
    }
}
