using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

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
        bool ViewCreated { get; set; }
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

        public void Hide()
        {
            Debug.Assert(ViewCreated);

            WindowManager.GetWindowItems(WindowKind.Command).First().Window.Hide();
            Flush();

            StartIconClear();
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

        IEnumerable<ICommandItem> ListupCommandItems(string inputValue)
        {
            foreach(var item in ListupLauncherItemsElements(inputValue)) {
                yield return item;
            }
        }


        IEnumerable<LauncherCommandItemElement> ListupLauncherItemsElements(string inputValue)
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
                }
            }
        }

        public Task UpdateCommandItemsAsync(string inputValue)
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
                    foreach(var item in ListupCommandItems(inputValue)) {
                        commandItems.Add(item);
                    }
                }

                Logger.LogTrace("検索終了: {0}", stopwatch.Elapsed);

                return commandItems.Select(i => WrapModel.Create(i, LoggerFactory)).ToList();
            }).ContinueWith(t => {
                var commandItems = t.Result;
                CommandItems.SetRange(commandItems);
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
            if(!ViewCreated) {
                var windowItem = OrderManager.CreateCommandWindow(this);
                windowItem.Window.Show();
                ViewCreated = true;
            } else {
                StopIconClear();

                var windoItem = WindowManager.GetWindowItems(WindowKind.Command).First();
                if(windoItem.Window.IsVisible) {
                    windoItem.Window.Activate();
                } else {
                    windoItem.Window.Show();
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
            Logger.LogDebug("アイコンキャッシュ破棄開始");
            foreach(var element in LauncherItemElements) {
                element.Icon.IconImageLoaderPack.IconItems[IconBox].ClearCache();
            }
            StopIconClear();
            Logger.LogDebug("アイコンキャッシュ破棄終了");

            StartViewClose();
        }

        private void ViewCloseTimer_Elapsed(object sender, ElapsedEventArgs e)
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


    }
}
