using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public class NotifyEventArgs: EventArgs
    { }

    public class LauncherItemChangedEventArgs: NotifyEventArgs
    {
        public LauncherItemChangedEventArgs(Guid launcherItemId)
        {
            LauncherItemId = launcherItemId;
        }

        #region property

        public Guid LauncherItemId { get; }

        #endregion
    }

    public class LauncherItemRemoveInLauncherGroupEventArgs: NotifyEventArgs
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

    public class LauncherItemRegisteredEventArgs: NotifyEventArgs
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
    /// フルスクリーン状態。
    /// </summary>
    public class FullscreenEventArgs: NotifyEventArgs
    {
        public FullscreenEventArgs(IScreen screen, bool isFullScreen, IntPtr hWnd)
        {
            Screen = screen;
            IsFullscreen = isFullScreen;
            FullscreenWindowHandle = hWnd;
        }

        #region property

        /// <summary>
        /// 対象ディスプレイ。
        /// </summary>
        public IScreen Screen { get; }
        /// <summary>
        /// フルスクリーン状態。
        /// </summary>
        public bool IsFullscreen { get; }
        /// <summary>
        /// フルスクリーン化した対象ウィンドウハンドル。
        /// <para><see cref="IsFullscreen"/>が真の場合に有効値が設定される。</para>
        /// </summary>
        public IntPtr FullscreenWindowHandle { get; }


        #endregion
    }

    public class NotifyLogEventArgs: NotifyEventArgs
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
        /// <summary>
        /// アプリケーションの設定が変更された際に通知される。
        /// </summary>
        event EventHandler<NotifyEventArgs>? SettingChanged;


        event EventHandler<FullscreenEventArgs>? FullscreenChanged;

        event EventHandler<NotifyLogEventArgs>? NotifyLogChanged;

        #endregion

        #region property

        ReadOnlyObservableCollection<NotifyLogItemElement> TopmostNotifyLogs { get; }
        ReadOnlyObservableCollection<NotifyLogItemElement> StreamNotifyLogs { get; }

        #endregion

        #region function

        /// <summary>
        /// ランチャーアイテム変更通知。
        /// <para>ランチャーアイテム使用機能側に変更を通知するだけなので受け取り手によるが可能な限り<see cref="IOrderManager.RefreshLauncherItemElement(Guid)"/>を先に呼んでおくこと。</para>
        /// </summary>
        /// <param name="launcherItemId">変更されたランチャーアイテムID。</param>
        void SendLauncherItemChanged(Guid launcherItemId);
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
        /// アプリケーション設定の変更を通知。
        /// </summary>
        void SendSettingChanged();

        void SendFullscreenChanged(IScreen screen, bool isFullScreen, IntPtr hWnd);

        /// <summary>
        /// 通知ログは存在するか。
        /// </summary>
        /// <param name="notifyLogId"></param>
        /// <returns></returns>
        bool ExistsLog(Guid notifyLogId);

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
        /// <param name="contentMessage"></param>
        void ReplaceLog(Guid notifyLogId, string contentMessage);
        /// <summary>
        /// 通知ログクリア。
        /// </summary>
        /// <param name="notifyLogId"></param>
        bool ClearLog(Guid notifyLogId);
        /// <summary>
        /// 通知ログを時間差で破棄。
        /// </summary>
        /// <param name="notifyLogId"></param>
        void FadeoutLog(Guid notifyLogId);

        #endregion
    }

    internal class NotifyManager: ManagerBase, INotifyManager
    {
        #region event
        #endregion

        public NotifyManager(IDiContainer diContainer, NotifyLogConfiguration notifyLogConfiguration, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;
            TopmostNotifyLogsImpl = new ObservableCollection<NotifyLogItemElement>();
            StreamNotifyLogsImpl = new ObservableCollection<NotifyLogItemElement>();
            TopmostNotifyLogs = new ReadOnlyObservableCollection<NotifyLogItemElement>(TopmostNotifyLogsImpl);
            StreamNotifyLogs = new ReadOnlyObservableCollection<NotifyLogItemElement>(StreamNotifyLogsImpl);

            NotifyLogLifeTimes = new Dictionary<NotifyLogKind, TimeSpan>() {
                [NotifyLogKind.Normal] = notifyLogConfiguration.NormalLogDisplayTime,
                [NotifyLogKind.Command] = notifyLogConfiguration.CommandLogDisplayTime,
                [NotifyLogKind.Undo] = notifyLogConfiguration.UndoLogDisplayTime,
                [NotifyLogKind.Topmost] = notifyLogConfiguration.FadeoutTime,
                //[NotifyLogKind.Platform] = TimeSpan.Zero,
            };

            StreamTimer = new Timer() {
                Interval = TimeSpan.FromSeconds(1).TotalMilliseconds,
                AutoReset = true,
            };
            StreamTimer.Elapsed += StreamTimer_Elapsed;
            StreamTimer.Start();
        }

        #region property

        Timer StreamTimer { get; }
        IReadOnlyDictionary<NotifyLogKind, TimeSpan> NotifyLogLifeTimes { get; }

        IDispatcherWrapper DispatcherWrapper { get; }
        private ObservableCollection<NotifyLogItemElement> TopmostNotifyLogsImpl { get; }
        private ObservableCollection<NotifyLogItemElement> StreamNotifyLogsImpl { get; }
        private KeyedCollection<Guid, NotifyLogItemElement> NotifyLogs { get; } = new SimpleKeyedCollection<Guid, NotifyLogItemElement>(v => v.NotifyLogId);

        private IDictionary<IScreen, bool> FullscreenStatus { get; } = new Dictionary<IScreen, bool>();

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

        void OnSettingChanged()
        {
            var e = new NotifyEventArgs();
            SettingChanged?.Invoke(this, e);
        }


        void OnFullscreenChanged(IScreen screen, bool isFullScreen, IntPtr hWnd)
        {
            var e = new FullscreenEventArgs(screen, isFullScreen, hWnd);
            FullscreenChanged?.Invoke(this, e);
        }

        void OnNotifyEventChanged(NotifyEventKind kind, IReadOnlyNotifyMessage message)
        {
            var e = new NotifyLogEventArgs(kind, message);
            NotifyLogChanged?.Invoke(this, e);
        }

        public void ClearAllLogs()
        {
            TopmostNotifyLogsImpl.Clear();
            StreamNotifyLogsImpl.Clear();
            NotifyLogs.Clear();
        }

        #endregion

        #region INotifyManager

        public event EventHandler<LauncherItemChangedEventArgs>? LauncherItemChanged;
        public event EventHandler<LauncherItemRemoveInLauncherGroupEventArgs>? LauncherItemRemovedInLauncherGroup;
        public event EventHandler<LauncherItemRegisteredEventArgs>? LauncherItemRegistered;
        public event EventHandler<CustomizeLauncherItemExitedEventArgs>? CustomizeLauncherItemExited;
        public event EventHandler<NotifyEventArgs>? SettingChanged;

        public event EventHandler<FullscreenEventArgs>? FullscreenChanged;

        public event EventHandler<NotifyLogEventArgs>? NotifyLogChanged;

        public ReadOnlyObservableCollection<NotifyLogItemElement> TopmostNotifyLogs { get; }
        public ReadOnlyObservableCollection<NotifyLogItemElement> StreamNotifyLogs { get; }

        public void SendLauncherItemChanged(Guid launcherItemId)
        {
            OnLauncherItemChanged(launcherItemId);
        }
        public void SendLauncherItemRemoveInLauncherGroup(Guid launcherGroupId, Guid launcherItemId, int index)
        {
            OnLauncherItemRemovedInGroup(launcherGroupId, launcherItemId, index);
        }

        public void SendLauncherItemRegistered(Guid launcherGroupId, Guid launcherItemId)
        {
            OnLauncherItemRegistered(launcherGroupId, launcherItemId);
        }

        public void SendCustomizeLauncherItemExited(Guid launcherItemId)
        {
            OnCustomizeLauncherItemExited(launcherItemId);
        }

        public void SendSettingChanged()
        {
            OnSettingChanged();
        }


        public void SendFullscreenChanged(IScreen screen, bool isFullScreen, IntPtr hWnd)
        {
            var fire = false;

            if(FullscreenStatus.TryGetValue(screen, out var currentValue)) {
                if(isFullScreen != currentValue) {
                    Logger.LogTrace("通常: {0}", isFullScreen);
                    FullscreenStatus[screen] = isFullScreen;
                    fire = true;
                }
            } else {
                var existsScreen = FullscreenStatus.Keys.FirstOrDefault(i => i.DeviceName == screen.DeviceName);
                if(existsScreen != null) {
                    if(FullscreenStatus[existsScreen] != isFullScreen) {
                        Logger.LogTrace("デバイス名: {0}", isFullScreen);
                        FullscreenStatus[existsScreen] = isFullScreen;
                        fire = true;
                    }
                } else {
                    // 未登録ディスプレイでフルスクリーンじゃなければ別になんもしない
                    if(isFullScreen) {
                        Logger.LogTrace("未登録: {0}", isFullScreen);
                        FullscreenStatus.Add(screen, isFullScreen);
                        fire = true;
                    }
                }
            }
            if(fire) {
                Logger.LogDebug("フルスクリーン状態発火: {0}, {1}", screen, isFullScreen);
                OnFullscreenChanged(screen, isFullScreen, hWnd);
            }
        }



        /// <inheritdoc cref="INotifyManager.ExistsLog(Guid)" />
        public bool ExistsLog(Guid notifyLogId)
        {
            return NotifyLogs.Contains(notifyLogId);
        }

        /// <inheritdoc cref="INotifyManager.AppendLog(NotifyMessage)" />
        public Guid AppendLog(NotifyMessage notifyMessage)
        {
            if(notifyMessage == null) {
                throw new ArgumentNullException(nameof(notifyMessage));
            }

            var element = DiContainer.Build<NotifyLogItemElement>(Guid.NewGuid(), notifyMessage);
            element.Initialize();

            Logger.LogDebug("[{0}] {1}: {2}, {3}", notifyMessage.Header, notifyMessage.Kind, notifyMessage.Content.Message, element.NotifyLogId);

            DispatcherWrapper.Begin(() => {
                NotifyLogs.Add(element);
                if(element.Kind == NotifyLogKind.Topmost) {
                    TopmostNotifyLogsImpl.Add(element);
                } else {
                    StreamNotifyLogsImpl.Add(element);
                }

                OnNotifyEventChanged(NotifyEventKind.Add, element);
            });

            return element.NotifyLogId;
        }
        /// <inheritdoc cref="INotifyManager.ReplaceLog(Guid, string)" />
        public void ReplaceLog(Guid notifyLogId, string contentMessage)
        {
            if(!NotifyLogs.TryGetValue(notifyLogId, out var element)) {
                throw new KeyNotFoundException(notifyLogId.ToString());
            }

            Logger.LogDebug("[{0}] 変更: {1}, {2}", element.Header, contentMessage, element.NotifyLogId);

            DispatcherWrapper.Begin(() => {
                element.ChangeContent(new NotifyLogContent(contentMessage, DateTime.UtcNow));
                OnNotifyEventChanged(NotifyEventKind.Change, element);
            });
        }
        /// <inheritdoc cref="INotifyManager.ClearLog(Guid)" />
        public bool ClearLog(Guid notifyLogId)
        {
            if(!NotifyLogs.TryGetValue(notifyLogId, out var element)) {
                return false;
            }

            if(0 < NotifyLogs.Count && NotifyLogs.Remove(notifyLogId)) {
                DispatcherWrapper.Begin(() => {
                    if(element.Kind == NotifyLogKind.Topmost) {
                        TopmostNotifyLogsImpl.Remove(element);
                    } else {
                        StreamNotifyLogsImpl.Remove(element);
                    }

                    OnNotifyEventChanged(NotifyEventKind.Clear, element);
                    element.Dispose();
                });

                return true;
            }

            return false;
        }

        /// <inheritdoc cref="INotifyManager.FadeoutLog(Guid)" />
        public void FadeoutLog(Guid notifyLogId)
        {
            if(!NotifyLogs.TryGetValue(notifyLogId, out var element)) {
                return;
            }

            Task.Delay(NotifyLogLifeTimes[element.Kind]).ContinueWith(t => {
                ClearLog(notifyLogId);
            });
        }

        #endregion

        private void StreamTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var time = DateTime.UtcNow;
            var removeLogs = StreamNotifyLogsImpl
                .Where(i => i.Content.Timestamp + NotifyLogLifeTimes[i.Kind] < time)
                .Select(i => i.NotifyLogId)
                .ToList()
            ;
            if(0 < removeLogs.Count) {
                foreach(var removeLog in removeLogs) {
                    ClearLog(removeLog);
                }
            }
        }
    }
}
