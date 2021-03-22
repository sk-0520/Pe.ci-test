using System;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager.Setting
{
    public class SettingNotifyEventArgs: EventArgs
    { }

    public class LauncherItemRemovedEventArgs: SettingNotifyEventArgs
    {
        public LauncherItemRemovedEventArgs(Guid launcherItemId)
        {
            LauncherItemId = launcherItemId;
        }

        #region property

        public Guid LauncherItemId { get; }

        #endregion
    }

    public interface ISettingNotifyManager
    {
        #region event

        event EventHandler<LauncherItemRemovedEventArgs>? LauncherItemRemoved;


        #endregion

        #region function

        void SendLauncherItemRemove(Guid launcherItemId);

        #endregion
    }

    internal class SettingNotifyManager: ISettingNotifyManager
    {
        public SettingNotifyManager(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region function

        private void OnLauncherItemRemoved(Guid launcherItemId)
        {
            LauncherItemRemoved?.Invoke(this, new LauncherItemRemovedEventArgs(launcherItemId));
        }

        #endregion

        #region ISettingNotifyManager

        /// <inheritdoc cref="ISettingNotifyManager.LauncherItemRemoved"/>
        public event EventHandler<LauncherItemRemovedEventArgs>? LauncherItemRemoved;

        /// <inheritdoc cref="ISettingNotifyManager.SendLauncherItemRemove(Guid)"/>
        public void SendLauncherItemRemove(Guid launcherItemId)
        {
            OnLauncherItemRemoved(launcherItemId);
        }


        #endregion
    }
}
