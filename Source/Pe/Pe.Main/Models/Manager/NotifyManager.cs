using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog;
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

    public class NotifyLogEventArgs : NotifyEventArgs
    {
        public NotifyLogEventArgs(NotifyEventKind kind, IReadOnlyNotifyMessage message)
        {
            Kind = kind;
            Message = message;
        }

        #region property

        public NotifyEventKind Kind { get; }
        public IReadOnlyNotifyMessage Message { get; }

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

        event EventHandler<NotifyLogEventArgs>? NotifyLogChanged;

        #endregion

        #region property

        ReadOnlyObservableCollection<NotifyLogItemElement> TopmostNotifyLogs { get; }
        ReadOnlyObservableCollection<NotifyLogItemElement> StreamNotifyLogs { get; }

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
        /// 通知ログ追加。
        /// </summary>
        /// <param name="notifyMessage"></param>
        /// <returns></returns>
        Guid AppendLog(NotifyMessage notifyMessage);
        /// <summary>
        ///通知ログ置き換え。
        /// </summary>
        /// <param name="notifyLogId"></param>
        /// <param name="content"></param>
        void ReplaceLog(Guid notifyLogId, string content);
        /// <summary>
        /// 通知ログクリア。
        /// </summary>
        /// <param name="notifyLogId"></param>
        /// <param name="content"></param>
        bool ClearLog(Guid notifyLogId);

        #endregion
    }

    internal class NotifyManager : ManagerBase, INotifyManager
    {
        #region event
        #endregion

        public NotifyManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            TopmostNotifyLogsImpl = new ObservableCollection<NotifyLogItemElement>();
            StreamNotifyLogsImpl = new ObservableCollection<NotifyLogItemElement>();
            TopmostNotifyLogs = new ReadOnlyObservableCollection<NotifyLogItemElement>(TopmostNotifyLogsImpl);
            StreamNotifyLogs = new ReadOnlyObservableCollection<NotifyLogItemElement>(StreamNotifyLogsImpl);

            StreamTimer = new Timer() {
                Interval = TimeSpan.FromSeconds(1).TotalMilliseconds,
                AutoReset = true,
            };
            StreamTimer.Elapsed += StreamTimer_Elapsed;
            StreamTimer.Start();
        }

        #region property

        Timer StreamTimer { get; }

        private ObservableCollection<NotifyLogItemElement> TopmostNotifyLogsImpl { get; }
        private ObservableCollection<NotifyLogItemElement> StreamNotifyLogsImpl { get; }
        private KeyedCollection<Guid, NotifyLogItemElement> NotifyLogs { get; } = new SimpleKeyedCollection<Guid, NotifyLogItemElement>(v => v.NotifyLogId);

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

        void OnNotifyEventChanged(NotifyEventKind kind, IReadOnlyNotifyMessage message)
        {
            var e = new NotifyLogEventArgs(kind, message);
            NotifyLogChanged?.Invoke(this, e);
        }

        #endregion

        #region INotifyManager

        public event EventHandler<LauncherItemChangedEventArgs>? LauncherItemChanged;
        public event EventHandler<LauncherItemRemoveInLauncherGroupEventArgs>? LauncherItemRemovedInLauncherGroup;
        public event EventHandler<LauncherItemRegisteredEventArgs>? LauncherItemRegistered;
        public event EventHandler<CustomizeLauncherItemExitedEventArgs>? CustomizeLauncherItemExited;

        public event EventHandler<FullScreenEventArgs>? FullScreenChanged;

        public event EventHandler<NotifyLogEventArgs>? NotifyLogChanged;

        public ReadOnlyObservableCollection<NotifyLogItemElement> TopmostNotifyLogs { get; }
        public ReadOnlyObservableCollection<NotifyLogItemElement> StreamNotifyLogs { get; }

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

        /// <inheritdoc cref="INotifyManager.AppendLog(NotifyMessage)" />
        public Guid AppendLog(NotifyMessage notifyMessage)
        {
            if(notifyMessage == null) {
                throw new ArgumentNullException(nameof(notifyMessage));
            }

            var element = DiContainer.Build<NotifyLogItemElement>(Guid.NewGuid(), notifyMessage);

            NotifyLogs.Add(element);
            if(element.Kind == NotifyLogKind.Topmost) {
                TopmostNotifyLogsImpl.Add(element);
            } else {
                StreamNotifyLogsImpl.Add(element);
            }

            OnNotifyEventChanged(NotifyEventKind.Add, element);

            return element.NotifyLogId;
        }
        /// <inheritdoc cref="INotifyManager.ReplaceLog(Guid, string)" />
        public void ReplaceLog(Guid notifyLogId, string content)
        {
            if(!NotifyLogs.TryGetValue(notifyLogId, out var element)) {
                throw new KeyNotFoundException(notifyLogId.ToString());
            }
            if(element.Kind != NotifyLogKind.Topmost) {
                throw new Exception($"{nameof(element.Kind)}: not {nameof(NotifyLogKind.Topmost)}");
            }

            element.ChangeContent(new NotifyLogContent(content, DateTime.UtcNow));

            OnNotifyEventChanged(NotifyEventKind.Change, element);
        }
        /// <inheritdoc cref="INotifyManager.ClearLog(Guid)" />
        public bool ClearLog(Guid notifyLogId)
        {
            if(!NotifyLogs.TryGetValue(notifyLogId, out var element)) {
                throw new KeyNotFoundException(notifyLogId.ToString());
            }

            if(NotifyLogs.Remove(notifyLogId)) {
                if(element.Kind == NotifyLogKind.Topmost) {
                    TopmostNotifyLogsImpl.Remove(element);
                } else {
                    StreamNotifyLogsImpl.Remove(element);
                }

                OnNotifyEventChanged(NotifyEventKind.Clear, element);
                element.Dispose();

                return true;
            }

            return false;
        }

        #endregion

        private void StreamTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var removeLogs = StreamNotifyLogsImpl
                .Where(i => i.Content.Timestamp + TimeSpan.FromSeconds(3) <= DateTime.UtcNow)
                .Select(i => i.NotifyLogId)
                .ToList()
            ;
            if(removeLogs.Any()) {
                DiContainer.Get<IDispatcherWrapper>().Begin(items => {
                    foreach(var removeLog in items) {
                        ClearLog(removeLog);
                    }
                }, removeLogs, System.Windows.Threading.DispatcherPriority.Normal);
            }
        }

    }
}
