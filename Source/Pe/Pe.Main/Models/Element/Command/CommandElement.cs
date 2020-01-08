using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Command
{
    public class CommandElement : ElementBase, IViewShowStarter, IFlushable
    {
        public CommandElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IMainDatabaseLazyWriter mainDatabaseLazyWriter, IOrderManager orderManager, IWindowManager windowManager, INotifyManager notifyManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
            MainDatabaseLazyWriter = mainDatabaseLazyWriter;
            OrderManager = orderManager;
            WindowManager = windowManager;
            NotifyManager = notifyManager;

            NotifyManager.LauncherItemChanged += NotifyManager_LauncherItemChanged;
            NotifyManager.LauncherItemRegistered += NotifyManager_LauncherItemRegistered;

            IconClearTimer = new Timer() {
                Interval = TimeSpan.FromSeconds(10).TotalMilliseconds,
            };
            IconClearTimer.Elapsed += IconClearTimerr_Elapsed;

            ViewCloseTimer = new Timer() {
                Interval = TimeSpan.FromSeconds(10).TotalMilliseconds,
            };
            ViewCloseTimer.Elapsed += ViewCloseTimer_Elapsed;
        }

        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IMainDatabaseLazyWriter MainDatabaseLazyWriter { get; }
        IOrderManager OrderManager { get; }
        IWindowManager WindowManager { get; }
        INotifyManager NotifyManager { get; }
        public bool ViewCreated { get; private set; }
        UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();
        public FontElement? Font { get; private set; }

        IList<LauncherItemElement> LauncherItemElements { get; } = new List<LauncherItemElement>();

        public ObservableCollection<WrapModel<ICommandItem>> CommandItems { get; } = new ObservableCollection<WrapModel<ICommandItem>>();

        public bool FindTag { get; private set; }
        public double Width { get; private set; }
        public TimeSpan HideWaitTime { get; private set; }
        public IconBox IconBox { get; private set; }

        Timer IconClearTimer { get; }
        Timer ViewCloseTimer { get; }

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
            foreach(var element in LauncherItemElements) {
                element.Icon.IconImageLoaderPack.IconItems[IconBox].ClearCache();
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

        private void RefreshSetting()
        {
            SettingAppCommandSettingData setting;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var appCommandSettingEntityDao = new AppCommandSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                setting = appCommandSettingEntityDao.SelectSettingCommandSetting();
            }

            Font = new FontElement(setting.FontId, MainDatabaseBarrier, StatementLoader, LoggerFactory);
            Font.Initialize();

            IconBox = setting.IconBox;
            Width = setting.Width;
            HideWaitTime = setting.HideWaitTime;
            FindTag = setting.FindTag;
        }

        private void RefreshLauncherItems()
        {
            IReadOnlyList<Guid> ids;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                ids = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();
            }

            var launcherItemElements = ids
                .Select(i => OrderManager.GetOrCreateLauncherItemElement(i))
                .Where(i => i.IsEnabledCommandLauncher)
                .ToList();
            ;
            LauncherItemElements.SetRange(launcherItemElements);
        }

        public void Refresh()
        {
            // アイテム一覧とったりなんかしたりあれこれしたり
            RefreshSetting();
            RefreshLauncherItems();
        }

        IEnumerable<ICommandItem> ListupCommandItems(string inputValue, CancellationToken cancellationToken)
        {
            foreach(var item in ListupLauncherItemsElements(inputValue, cancellationToken)) {
                yield return item;
            }
        }


        IEnumerable<LauncherCommandItemElement> ListupLauncherItemsElements(string inputValue, CancellationToken cancellationToken)
        {
            var simpleRegexFactory = new SimpleRegexFactory(LoggerFactory);
            var regex = simpleRegexFactory.CreateFilterRegex(inputValue);

            static IReadOnlyList<Match> Matches(Regex reg, string input) => reg.Matches(input).Cast<Match>().ToList();
            static void SetMatches(IList<Range> buffer, IEnumerable<Match> matches)
            {
                foreach(var match in matches) {
                    buffer.Add(new Range(match.Index, match.Index + match.Length));
                }
            }

            foreach(var element in LauncherItemElements) {
                cancellationToken.ThrowIfCancellationRequested();

                var nameMatches = Matches(regex, element.Name);
                if(nameMatches.Any()) {
                    Logger.LogTrace("ランチャー: 名前一致, {0}, {1}", element.Name, element.LauncherItemId);
                    var result = new LauncherCommandItemElement(element, LoggerFactory) {
                        EditableKind = "item name",
                    };
                    result.Initialize();
                    SetMatches(result.EditableHeaderMatchers, nameMatches);
                    yield return result;
                    continue;
                }

                var codeMatches = regex.Matches(element.Code).Cast<Match>().ToList();
                if(codeMatches.Any()) {
                    Logger.LogTrace("ランチャー: コード一致, {0}, {1}", element.Code, element.LauncherItemId);
                    var result = new LauncherCommandItemElement(element, LoggerFactory) {
                        EditableDescription = element.Code,
                        EditableKind = "item code",
                    };
                    result.Initialize();
                    SetMatches(result.EditableDescriptionMatchers, codeMatches);
                    yield return result;
                    continue;
                }

                if(FindTag) {
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
        }

        public Task UpdateCommandItemsAsync(string inputValue, CancellationToken cancellationToken)
        {
            return Task.Run(() => {
                Logger.LogTrace("検索開始");
                var stopwatch = Stopwatch.StartNew();

                var commandItems = new List<ICommandItem>();
                if(string.IsNullOrWhiteSpace(inputValue)) {
                    var items = LauncherItemElements
                        .Select(i => new LauncherCommandItemElement(i, LoggerFactory))
                    ;
                    commandItems.AddRange(items);
                } else {
                    foreach(var item in ListupCommandItems(inputValue, cancellationToken)) {
                        commandItems.Add(item);
                    }
                }
                cancellationToken.ThrowIfCancellationRequested();

                Logger.LogTrace("検索終了: {0}", stopwatch.Elapsed);

                return commandItems.Select(i => WrapModel.Create(i, LoggerFactory)).ToList();
            }, cancellationToken).ContinueWith(t => {
                if(t.IsCompletedSuccessfully) {
                    var commandItems = t.Result;
                    CommandItems.SetRange(commandItems);
                }
            });
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
                var dao = new AppCommandSettingEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                dao.UpdatCommandSettingWidth(Width, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush();
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

            if(!ViewCreated) {
                var windowItem = OrderManager.CreateCommandWindow(this);
                windowItem.Window.Show();
                MoveCursorPosition(windowItem);
                ViewCreated = true;
            } else {
                StopIconClear();

                var windowItem = WindowManager.GetWindowItems(WindowKind.Command).First();
                MoveCursorPosition(windowItem);
                if(windowItem.Window.IsVisible) {
                    windowItem.Window.Activate();
                } else {
                    windowItem.Window.Show();
                }
            }
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

        public void ReceiveViewClosed()
        {
            ViewCreated = false;
        }


        #endregion

        #region IFlushable

        public void Flush()
        {
            MainDatabaseLazyWriter.SafeFlush();
        }

        #endregion

        private void IconClearTimerr_Elapsed(object sender, ElapsedEventArgs e)
        {
            ClearIcon();
            StartViewClose();
        }

        private void ViewCloseTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CloseView();
        }

        private void NotifyManager_LauncherItemChanged(object? sender, LauncherItemChangedEventArgs e)
        {
            var element = LauncherItemElements.FirstOrDefault(i => i.LauncherItemId == e.LauncherItemId);
            if(element != null) {
                element.Icon.IconImageLoaderPack.IconItems[IconBox].ClearCache();
                if(element.IsEnabledCommandLauncher) {
                    Logger.LogInformation("コマンドランチャーから既存ランチャーアイテムの除外: {0}", element.LauncherItemId);
                    LauncherItemElements.Remove(element);
                }
            }
        }

        private void NotifyManager_LauncherItemRegistered(object? sender, LauncherItemRegisteredEventArgs e)
        {
            var element = OrderManager.GetOrCreateLauncherItemElement(e.LauncherItemId);
            if(element.IsEnabledCommandLauncher) {
                Logger.LogInformation("コマンドランチャーへ新規ランチャーアイテムの追加: {0}", element.LauncherItemId);
                LauncherItemElements.Add(element);
            }
        }

    }
}
