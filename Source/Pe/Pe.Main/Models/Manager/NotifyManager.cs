using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Data;
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

    public class LauncherItemRemoveInLauncherGroupEventArgs : NotifyEventArgs
    {
        public LauncherItemRemoveInLauncherGroupEventArgs(Guid launcherGroupId, Guid launcherItemId, int index)
        {
            LauncherGroupId = launcherGroupId;
            LauncherItemId = launcherItemId;
            Index = index;
        }

        #region property

        public Guid LauncherGroupId { get; }
        public Guid LauncherItemId { get; }
        public int Index { get; }

        #endregion
    }

    public class LauncherItemRegisteredEventArgs : NotifyEventArgs
    {
        public LauncherItemRegisteredEventArgs(Guid launcherGroupId, Guid launcherItemId)
        {
            LauncherGroupId = launcherGroupId;
            LauncherItemId = launcherItemId;
        }

        #region property

        public Guid LauncherGroupId { get; }
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
    /// フルスクリーン状態。
    /// </summary>
    public class FullScreenEventArgs : NotifyEventArgs
    {
        public FullScreenEventArgs(IScreen screen, bool isFullScreen, IntPtr hWnd)
        {
            Screen = screen;
            IsFullScreen = isFullScreen;
            FullScreenWindowHandle = hWnd;
        }

        #region property

        /// <summary>
        /// 対象ディスプレイ。
        /// </summary>
        public IScreen Screen { get; }
        /// <summary>
        /// フルスクリーン状態。
        /// </summary>
        public bool IsFullScreen { get; }
        /// <summary>
        /// フルスクリーン化した対象ウィンドウハンドル。
        /// <para><see cref="IsFullScreen"/>が真の場合に有効値が設定される。</para>
        /// </summary>
        public IntPtr FullScreenWindowHandle { get; }


        #endregion
    }

    internal class NotifyLogEventArgs : NotifyEventArgs
    {
        internal NotifyLogEventArgs(NotifyLog log)
        {
            NotifyMessageId = log.Id;
            NotifyMessage = log.Message;
        }

        #region proeprty

        public Guid NotifyMessageId { get; }
        public NotifyMessage NotifyMessage { get; }

        #endregion
    }

    internal class NotifyLog
    {
        public NotifyLog(Guid notifyMessageId, NotifyMessage notifyMessage)
        {
            Id = notifyMessageId;
            Message = notifyMessage;
        }
        #region property

        public Guid Id { get; }
        public NotifyMessage Message { get; }

        #endregion
    }

    /// <summary>
    /// アプリケーションからの通知を発行する。
    /// </summary>
    public interface INotifyManager
    {
        #region event

        event EventHandler<LauncherItemChangedEventArgs>? LauncherItemChanged;
        event EventHandler<LauncherItemRemoveInLauncherGroupEventArgs>? LauncherItemRemovedInLauncherGroup;
        event EventHandler<LauncherItemRegisteredEventArgs>? LauncherItemRegistered;
        event EventHandler<CustomizeLauncherItemExitedEventArgs>? CustomizeLauncherItemExited;

        event EventHandler<FullScreenEventArgs>? FullScreenChanged;

        #endregion

        #region function

        void SendLauncherItemChanged(Guid launcherItemIds);
        void SendLauncherItemRegistered(Guid launcherGroupId, Guid launcherItemId);
        /// <summary>
        /// グループからランチャーアイテムが破棄されたことを通知。
        /// </summary>
        /// <param name="launcherGroupId"></param>
        /// <param name="launcherItemId"></param>
        /// <param name="index">同一の<see cref="launcherItemId"/>に該当するもののうち何番目のアイテムかを示す。</param>
        void SendLauncherItemRemoveInLauncherGroup(Guid launcherGroupId, Guid launcherItemId, int index);
        void SendCustomizeLauncherItemExited(Guid launcherItemId);

        /// <summary>
        /// 通知ログ。
        /// </summary>
        /// <param name="notifyMessage"></param>
        /// <returns></returns>
        Guid SendLog(NotifyMessage notifyMessage);
        /// <summary>
        ///通知ログ置き換え。
        /// </summary>
        /// <param name="notifyMessageId"></param>
        /// <param name="content"></param>
        void ReplaceLog(Guid notifyMessageId, string content);
        /// <summary>
        /// 通知ログクリア。
        /// </summary>
        /// <param name="notifyMessageId"></param>
        /// <param name="content"></param>
        void ClearLog(Guid notifyMessageId);

        #endregion
    }

    internal class NotifyManager : ManagerBase, INotifyManager
    {
        #region event

        internal event EventHandler<NotifyLogEventArgs>? NotifyLog;

        #endregion

        public NotifyManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        { }

        #region property

        public ObservableCollection<NotifyLog> TopmostNotifyLogs { get; } = new ObservableCollection<NotifyLog>();
        public ObservableCollection<NotifyLog> UnTopmostNotifyLogs { get; } = new ObservableCollection<NotifyLog>();

        #endregion

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

        void OnLauncherItemRegistered(Guid launcherGroupId, Guid launcherItemId)
        {
            ThrowIfEmptyLauncherItemId(launcherItemId);

            var e = new LauncherItemRegisteredEventArgs(launcherGroupId, launcherItemId);
            LauncherItemRegistered?.Invoke(this, e);
        }

        void OnLauncherItemRemovedInGroup(Guid launcherGroupId, Guid launcherItemId, int index)
        {
            ThrowIfEmptyLauncherItemId(launcherItemId);

            var e = new LauncherItemRemoveInLauncherGroupEventArgs(launcherGroupId, launcherItemId, index);
            LauncherItemRemovedInLauncherGroup?.Invoke(this, e);
        }

        void OnCustomizeLauncherItemExited(Guid launcherItemId)
        {
            ThrowIfEmptyLauncherItemId(launcherItemId);

            var e = new CustomizeLauncherItemExitedEventArgs(launcherItemId);
            CustomizeLauncherItemExited?.Invoke(this, e);
        }

        void OnFullScreenChanged(IScreen screen, bool isFullScreen, IntPtr hWnd)
        {
            var e = new FullScreenEventArgs(screen, isFullScreen, hWnd);
            FullScreenChanged?.Invoke(this, e);
        }

        void OnNotifyLog(Guid notifyMessageId, NotifyMessage notifyMessage)
        { }

        #endregion

        #region INotifyManager

        public event EventHandler<LauncherItemChangedEventArgs>? LauncherItemChanged;
        public event EventHandler<LauncherItemRemoveInLauncherGroupEventArgs>? LauncherItemRemovedInLauncherGroup;
        public event EventHandler<LauncherItemRegisteredEventArgs>? LauncherItemRegistered;
        public event EventHandler<CustomizeLauncherItemExitedEventArgs>? CustomizeLauncherItemExited;

        public event EventHandler<FullScreenEventArgs>? FullScreenChanged;


        public void SendLauncherItemChanged(Guid launcherItemId)
        {
            OnLauncherItemChanged(launcherItemId);
        }
        public void SendLauncherItemRemoveInLauncherGroup(Guid launcherGroupId, Guid launcherItemId, int number)
        {
            OnLauncherItemRemovedInGroup(launcherGroupId, launcherItemId, number);
        }

        public void SendLauncherItemRegistered(Guid launcherGroupId, Guid launcherItemId)
        {
            OnLauncherItemRegistered(launcherGroupId, launcherItemId);
        }

        public void SendCustomizeLauncherItemExited(Guid launcherItemId)
        {
            OnCustomizeLauncherItemExited(launcherItemId);
        }

        /// <inheritdoc cref="INotifyManager.SendLog(NotifyMessage)" />
        public Guid SendLog(NotifyMessage notifyMessage)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc cref="INotifyManager.ReplaceLog(Guid, string)" />
        public void ReplaceLog(Guid notifyMessageId, string content)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc cref="INotifyManager.ClearLog(Guid)" />
        public void ClearLog(Guid notifyMessageId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
