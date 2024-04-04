using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Standard.Base;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Command
{
    public class CommandElement: ElementBase, IViewShowStarter, IViewCloseReceiver, IFlushable
    {
        public CommandElement(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IMainDatabaseLazyWriter mainDatabaseLazyWriter, ApplicationConfiguration applicationConfiguration, IOrderManager orderManager, IWindowManager windowManager, INotifyManager notifyManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            LargeDatabaseBarrier = largeDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            MainDatabaseLazyWriter = mainDatabaseLazyWriter;
            OrderManager = orderManager;
            WindowManager = windowManager;
            NotifyManager = notifyManager;
            DispatcherWrapper = dispatcherWrapper;

            IconClearTimer = new Timer() {
                Interval = applicationConfiguration.Command.IconClearWaitTime.TotalMilliseconds,
            };
            IconClearTimer.Elapsed += IconClearTimer_Elapsed;

            ViewCloseTimer = new Timer() {
                Interval = applicationConfiguration.Command.ViewCloseWaitTime.TotalMilliseconds,
            };
            ViewCloseTimer.Elapsed += ViewCloseTimer_Elapsed;
        }

        #region property

        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private IMainDatabaseLazyWriter MainDatabaseLazyWriter { get; }
        private IOrderManager OrderManager { get; }
        private IWindowManager WindowManager { get; }
        private INotifyManager NotifyManager { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }
        public bool ViewCreated { get; private set; }
        private UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();
        public FontElement? Font { get; private set; }

        public bool FindTag { get; private set; }
        public double Width { get; private set; }
        public TimeSpan HideWaitTime { get; private set; }
        public IconBox IconBox { get; private set; }

        private Timer ViewCloseTimer { get; }
        private Timer IconClearTimer { get; }

        private LauncherItemCommandFinder? LauncherItemCommandFinder { get; set; }

        private List<ICommandFinder> CommandFindersImpl { get; } = new List<ICommandFinder>(2);
        private IReadOnlyCollection<ICommandFinder> CommandFinders => CommandFindersImpl;

        #endregion

        #region function

        public void HideView(bool force)
        {
            Debug.Assert(ViewCreated);

            Flush();
            if(force) {
                ClearIcon();
                CloseView();
            } else {
                WindowManager.GetWindowItems(WindowKind.Command).First().Window.Hide();
                StartIconClear();
            }
        }

        private void StartIconClear()
        {
            IconClearTimer.Stop();
            IconClearTimer.Start();
        }

        private void StopIconClear()
        {
            StopViewClose();
            if(IconClearTimer.Enabled) {
                Logger.LogTrace("アイコンキャッシュ破棄待機 停止");
            }
            IconClearTimer.Stop();
        }

        private void ClearIcon()
        {
            Logger.LogDebug("アイコンキャッシュ破棄開始");

            if(LauncherItemCommandFinder != null) {
                LauncherItemCommandFinder.ClearIcon();
            }

            StopIconClear();
            Logger.LogDebug("アイコンキャッシュ破棄終了");
        }

        private void StartViewClose()
        {
            ViewCloseTimer.Stop();
            ViewCloseTimer.Start();
        }
        private void StopViewClose()
        {
            if(ViewCloseTimer.Enabled) {
                Logger.LogTrace("ビュー破棄待機 停止");
            }
            ViewCloseTimer.Stop();
        }
        private void CloseView()
        {
            Logger.LogDebug("ビュー破棄開始");

            var view = WindowManager.GetWindowItems(WindowKind.Command).First().Window;
            view.Dispatcher.Invoke(() => {
                view.Close();
                ViewCreated = false;
                Logger.LogDebug("ビュー破棄終了");
            });
            StopViewClose();
        }

        private async Task RefreshSettingAsync()
        {
            SettingAppCommandSettingData setting;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var appCommandSettingEntityDao = new AppCommandSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                setting = appCommandSettingEntityDao.SelectSettingCommandSetting();
            }

            Font = new FontElement(setting.FontId, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            await Font.InitializeAsync();

            IconBox = setting.IconBox;
            Width = setting.Width;
            HideWaitTime = setting.HideWaitTime;
            FindTag = setting.FindTag;
        }

        public async Task RefreshAsync()
        {
            // アイテム一覧とったりなんかしたりあれこれしたり
            await RefreshSettingAsync();

            if(LauncherItemCommandFinder != null) {
                // 諦め
                LauncherItemCommandFinder.FindTag = FindTag;
                LauncherItemCommandFinder.IconBox = IconBox;
            }

            var pluginContext = PluginContextFactory.CreateNullContext(LoggerFactory);
            foreach(var commandFinder in CommandFinders) {
                commandFinder.Refresh(pluginContext);
            }
        }

        private async IAsyncEnumerable<ICommandItem> EnumerateCommandItemsAsync(string inputValue, IHitValuesCreator hitValuesCreator, [EnumeratorCancellation]  CancellationToken cancellationToken)
        {
            var simpleRegexFactory = new SimpleRegexFactory(LoggerFactory);
            var regex = simpleRegexFactory.CreateFilterRegex(inputValue);

            foreach(var commandFinder in CommandFinders) {
                var items = commandFinder.EnumerateCommandItemsAsync(inputValue, regex, hitValuesCreator, cancellationToken);
                await foreach(var item in items) {
                    yield return item;
                }
            }
        }



        public Task<IReadOnlyList<ICommandItem>> EnumerateCommandItemsAsync(string inputValue, CancellationToken cancellationToken)
        {
            return Task.Run(async () => {
                Logger.LogTrace("検索開始");
                var stopwatch = Stopwatch.StartNew();

                var hitValuesCreator = new HitValuesCreator(LoggerFactory);

                var commandItems = new List<ICommandItem>();
                await foreach(var item in EnumerateCommandItemsAsync(inputValue, hitValuesCreator, cancellationToken)) {
                    commandItems.Add(item);
                    //Logger.LogDebug(string.Join(" - ", item.HeaderValues.Select(i => i.Value)));
                }
                cancellationToken.ThrowIfCancellationRequested();

                Logger.LogTrace("検索終了: {0}", stopwatch.Elapsed);

                return (IReadOnlyList<ICommandItem>)commandItems
                    .OrderByDescending(i => i.Score)
                    .ToList()
                ;
            }, cancellationToken);
        }

        public void ChangeViewWidthDelaySave(double width)
        {
            var diff = Math.Abs(Width - width);
            if(diff < double.Epsilon) {
                Logger.LogTrace("{Width} - {width}: {Abs} < {Epsilon}", Width, width, diff, double.Epsilon);
                return;
            }
            Width = width;

            MainDatabaseLazyWriter.Stock(c => {
                var dao = new AppCommandSettingEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                dao.UpdateCommandSettingWidth(Width, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void AddCommandFinder(ICommandFinder commandFinder)
        {
            if(!commandFinder.IsInitialized) {
                commandFinder.Initialize();
            }

            CommandFindersImpl.Add(commandFinder);

            if(commandFinder is LauncherItemCommandFinder launcherItemCommandFinder) {
                LauncherItemCommandFinder = launcherItemCommandFinder;
            }
        }

        #endregion

        #region ElementBase

        protected override async Task InitializeCoreAsync()
        {
            foreach(var commandFinder in CommandFinders) {
                if(!commandFinder.IsInitialized) {
                    commandFinder.Initialize();
                }
            }

            await RefreshAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush();
                IconClearTimer.Elapsed -= IconClearTimer_Elapsed;
                ViewCloseTimer.Elapsed -= ViewCloseTimer_Elapsed;

                IconClearTimer.Dispose();
                ViewCloseTimer.Dispose();

                foreach(var commandFinder in CommandFinders) {
                    commandFinder.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IViewShowStarter

        public bool CanStartShowView
        {
            get
            {
                if(ViewCreated) {
                    return false;
                }

                return true;
            }
        }

        public void StartView()
        {
            static void MoveCursorPosition(WindowItem item)
            {
                var hWnd = HandleUtility.GetWindowHandle(item.Window);
                if(hWnd != IntPtr.Zero) {
                    var deviceCursorPosition = MouseUtility.GetDevicePosition();
                    NativeMethods.SetWindowPos(hWnd, IntPtr.Zero, (int)deviceCursorPosition.X, (int)deviceCursorPosition.Y, 0, 0, SWP.SWP_NOSIZE);
                }
            }

            WindowItem windowItem;
            if(!ViewCreated) {
                windowItem = OrderManager.CreateCommandWindow(this);
                windowItem.Window.Show();
                MoveCursorPosition(windowItem);
                ViewCreated = true;
            } else {
                StopIconClear();
                windowItem = WindowManager.GetWindowItems(WindowKind.Command).First();
                if(windowItem.Window.IsVisible) {
                    MoveCursorPosition(windowItem);
                    windowItem.Window.Activate();
                } else {
                    MoveCursorPosition(windowItem);
                    windowItem.Window.Show();
                }
            }

            windowItem.Window.Activate();
        }

        #endregion

        #region IViewCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            return false;
        }
        public bool ReceiveViewClosing()
        {
            return true;
        }

        /// <inheritdoc cref="IViewCloseReceiver.ReceiveViewClosedAsync(bool)"/>
        public Task ReceiveViewClosedAsync(bool isUserOperation)
        {
            ViewCreated = false;
            return Task.CompletedTask;
        }


        #endregion

        #region IFlushable

        public void Flush()
        {
            MainDatabaseLazyWriter.SafeFlush();
        }

        #endregion

        private void IconClearTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            ClearIcon();
            StartViewClose();
        }

        private void ViewCloseTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            CloseView();
        }
    }
}
