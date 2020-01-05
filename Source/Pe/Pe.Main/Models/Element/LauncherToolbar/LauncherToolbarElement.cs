using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Views.Extend;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar
{
    public class LauncherToolbarElement : ElementBase, IAppDesktopToolbarExtendData, IViewShowStarter, IViewCloseReceiver, IFlushable
    {
        #region variable

        bool _isVisible;
        bool _isTopmost;
        LauncherToolbarIconDirection _iconDirection;
        LauncherGroupElement? _selectedLauncherGroup;
        bool _isOpendAppMenu;
        bool _isOpendItemMenu;

        #endregion

        public LauncherToolbarElement(IScreen dockScreen, ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups, IOrderManager orderManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IMainDatabaseLazyWriter mainDatabaseLazyWriter, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, ILauncherToolbarTheme launcherToolbarTheme, IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            DockScreen = dockScreen;
            LauncherGroups = launcherGroups;

            OrderManager = orderManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            IdFactory = idFactory;
            LauncherToolbarTheme = launcherToolbarTheme;

            MainDatabaseLazyWriter = mainDatabaseLazyWriter;
        }

        #region property

        IOrderManager OrderManager { get; }
        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IIdFactory IdFactory { get; }
        ILauncherToolbarTheme LauncherToolbarTheme { get; }

        IMainDatabaseLazyWriter MainDatabaseLazyWriter { get; }
        UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        public ReadOnlyObservableCollection<LauncherGroupElement> LauncherGroups { get; }

        public Guid LauncherToolbarId { get; private set; }

        bool ViewCreated { get; set; }

        public ObservableCollection<LauncherItemElement> LauncherItems { get; } = new ObservableCollection<LauncherItemElement>();

        /// <summary>
        /// 表示されているか。
        /// </summary>
        public bool IsVisible
        {
            get => this._isVisible;
            private set => SetProperty(ref this._isVisible, value);
        }

        /// <summary>
        /// 表示アイコンサイズ。
        /// </summary>
        public IconBox IconBox { get; private set; }

        /// <summary>
        /// アイコンの余白。
        /// <para>CSS の margin と同じ考え方。</para>
        /// </summary>
        [PixelKind(Px.Logical)]
        public Thickness IconMargin { get; private set; }

        /// <summary>
        /// ボタンの詰め領域。
        /// <para>CSS の padding と同じ考え方。</para>
        /// </summary>
        [PixelKind(Px.Logical)]
        public Thickness ButtonPadding { get; private set; }

        /// <summary>
        /// アイコンのみを表示するか。
        /// </summary>
        public bool IsIconOnly { get; private set; }

        /// <summary>
        /// テキスト表示の際の表示幅。
        /// </summary>
        [PixelKind(Px.Logical)]
        public double TextWidth { get; private set; }

        public bool IsTopmost
        {
            get => this._isTopmost;
            set => SetProperty(ref this._isTopmost, value);
        }

        public LauncherToolbarIconDirection IconDirection
        {
            get => this._iconDirection;
            set => SetProperty(ref this._iconDirection, value);
        }

        public LauncherGroupElement? SelectedLauncherGroup
        {
            get => this._selectedLauncherGroup;
            set => SetProperty(ref this._selectedLauncherGroup, value);
        }

        public bool IsOpendAppMenu
        {
            get => this._isOpendAppMenu;
            set
            {
                SetProperty(ref this._isOpendAppMenu, value);
                PausingAutoHide = IsOpendAppMenu;
            }
        }
        public bool IsOpendItemMenu
        {
            get => this._isOpendItemMenu;
            set
            {
                SetProperty(ref this._isOpendItemMenu, value);
                PausingAutoHide = IsOpendItemMenu;
            }
        }

        #endregion

        #region function

        /// <summary>
        /// <see cref="DockScreen"/> から近しい ツールバー設定を読み込む。
        /// </summary>
        /// <param name="rows"></param>
        /// <returns>見つかったツールバー。見つからない場合は<see cref="Guid.Empty"/>を返す。</returns>
        Guid FindMaybeToolbarId(IEnumerable<LauncherToolbarsScreenData> rows)
        {
            ThrowIfDisposed();

            var screenChecker = new ScreenChecker();
            var row = rows.FirstOrDefault(r => screenChecker.FindMaybe(DockScreen, r));
            if(row != null) {
                return row.LauncherToolbarId;
            }

            return Guid.Empty;
        }

        Guid GetLauncherToolbarId()
        {
            ThrowIfDisposed();

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherToolbarDomainDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var screenToolbars = dao.SelectAllScreenToolbars().ToList();
                var LauncherToolbarId = FindMaybeToolbarId(screenToolbars);
                return LauncherToolbarId;
            }
        }

        Guid CreateLauncherToolbar()
        {
            ThrowIfDisposed();

            var toolbarId = IdFactory.CreateLauncherToolbarId();
            Logger.LogDebug("create toolbar: {0}", toolbarId);

            var newFontId = IdFactory.CreateFontId();

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var appLauncherToolbarSettingEntityDao = new AppLauncherToolbarSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var srcFontId = appLauncherToolbarSettingEntityDao.SelectAppLauncherToolbarSettingFontId();

                var fontsEntityDao = new FontsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                fontsEntityDao.InsertCopyFont(srcFontId, newFontId, DatabaseCommonStatus.CreateCurrentAccount());

                var toolbarsDao = new LauncherToolbarsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                toolbarsDao.InsertNewToolbar(toolbarId, newFontId, DockScreen.DeviceName, DatabaseCommonStatus.CreateCurrentAccount());

                var screenOperator = new ScreenOperator(LoggerFactory);
                screenOperator.RegisterDatabase(DockScreen, commander, StatementLoader, commander.Implementation, DatabaseCommonStatus.CreateCurrentAccount());

                commander.Commit();
            }

            return toolbarId;
        }

        public void ChangeLauncherGroup(LauncherGroupElement launcherGroup)
        {
            ThrowIfDisposed();

            if(IsOpendAppMenu) {
                IsOpendAppMenu = false;
            }
            SelectedLauncherGroup = launcherGroup;
            LoadLauncherItems();
        }

        public void RemoveLauncherItem(Guid launcherGroupId, Guid launcherItemId, int index)
        {
            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var launcherGroupItemsEntityDao = new LauncherGroupItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                launcherGroupItemsEntityDao.DeleteGroupItemsLauncherItem(launcherGroupId, launcherItemId, index);

                commander.Commit();
            }

            // モデル側で削除するために通知
            NotifyManager.SendLauncherItemRemoveInLauncherGroup(launcherGroupId, launcherItemId, index);
        }


        void UpdateDesign()
        {
            ThrowIfDisposed();

            ButtonPadding = LauncherToolbarTheme.GetButtonPadding(ToolbarPosition, IconBox);
            IconMargin = LauncherToolbarTheme.GetIconMargin(ToolbarPosition, IconBox, IsIconOnly, TextWidth);
            DisplaySize = LauncherToolbarTheme.GetDisplaySize(ButtonPadding, IconMargin, IconBox, IsIconOnly, TextWidth);
            HiddenSize = LauncherToolbarTheme.GetHiddenSize(ButtonPadding, IconMargin, IconBox, IsIconOnly, TextWidth);
        }

        void LoadLauncherToolbar()
        {
            Logger.LogInformation("toolbar id: {0}", LauncherToolbarId);
            ThrowIfDisposed();

            LauncherToolbarsDisplayData displayData;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherToolbarsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                displayData = dao.SelectDisplayData(LauncherToolbarId);
            }

            IconBox = displayData.IconBox;
            TextWidth = displayData.TextWidth;
            IsIconOnly = displayData.IsIconOnly;
            IsTopmost = displayData.IsTopmost;
            IsAutoHide = displayData.IsAutoHide;
            AutoHideTimeout = displayData.AutoHideTimeout;
            ToolbarPosition = displayData.ToolbarPosition;
            IsVisible = displayData.IsVisible;
            IconDirection = displayData.IconDirection;

            SelectedLauncherGroup = LauncherGroups
                .FirstOrDefault(i => i.LauncherGroupId == displayData.LauncherGroupId)
                ?? LauncherGroups.First()
            ;
        }

        IEnumerable<LauncherItemElement> GetLauncherItems()
        {
            Debug.Assert(SelectedLauncherGroup != null);

            var launcherItemIds = SelectedLauncherGroup.GetLauncherItemIds();
            var result = new List<LauncherItemElement>(launcherItemIds.Count);

            foreach(var launcherItemId in launcherItemIds) {
                var element = OrderManager.GetOrCreateLauncherItemElement(launcherItemId);
                result.Add(element);
            }

            return result;
        }

        void LoadLauncherItems()
        {
            ThrowIfDisposed();

            var items = GetLauncherItems();
            LauncherItems.SetRange(items);
        }

        public void ChangeToolbarPositionDelaySave(AppDesktopToolbarPosition toolbarPosition)
        {
            ThrowIfDisposed();

            ToolbarPosition = toolbarPosition;
            UpdateDesign();
            IsOpendAppMenu = false;

            MainDatabaseLazyWriter.Stock(c => {
                var dao = new LauncherToolbarsEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                dao.UpdateToolbarPosition(LauncherToolbarId, ToolbarPosition, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ChangeTopmostDelaySave(bool isTopmost)
        {
            ThrowIfDisposed();

            IsTopmost = isTopmost;
            IsOpendAppMenu = false;

            MainDatabaseLazyWriter.Stock(c => {
                var dao = new LauncherToolbarsEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                dao.UpdatIsTopmost(LauncherToolbarId, IsTopmost, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ChangeAutoHideDelaySave(bool isAutoHide)
        {
            ThrowIfDisposed();

            IsAutoHide = isAutoHide;
            IsOpendAppMenu = false;

            MainDatabaseLazyWriter.Stock(c => {
                var dao = new LauncherToolbarsEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                dao.UpdatIsAutoHide(LauncherToolbarId, IsAutoHide, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ChangeVisibleDelaySave(bool isVisible)
        {
            ThrowIfDisposed();

            IsVisible = isVisible;
            IsOpendAppMenu = false;

            MainDatabaseLazyWriter.Stock(c => {
                var dao = new LauncherToolbarsEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                dao.UpdatIsVisible(LauncherToolbarId, IsVisible, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        /// <summary>
        /// ファイルを現在のグループに登録する。
        /// </summary>
        /// <param name="filePath">対象ファイルパス。</param>
        /// <param name="expandShortcut"><paramref name="filePath"/>がショートカットの場合にショートカットの内容を登録するか</param>
        public void RegisterFile(string filePath, bool expandShortcut)
        {
            Debug.Assert(SelectedLauncherGroup != null);
            ThrowIfDisposed();

            var file = new FileInfo(filePath);
            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);
            var data = launcherFactory.FromFile(file, expandShortcut);
            var tags = launcherFactory.GetTags(file).ToList();

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var launcherItemsDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherTagsDao = new LauncherTagsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherFilesDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherGroupItemsDao = new LauncherGroupItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);

                var codes = launcherItemsDao.SelectFuzzyCodes(data.Item.Code).ToList();
                data.Item.Code = launcherFactory.GetUniqueCode(data.Item.Code, codes);

                launcherItemsDao.InsertLauncherItem(data.Item, DatabaseCommonStatus.CreateCurrentAccount());
                launcherFilesDao.InsertFile(data.Item.LauncherItemId, data.File, DatabaseCommonStatus.CreateCurrentAccount());
                launcherTagsDao.InsertTags(data.Item.LauncherItemId, tags, DatabaseCommonStatus.CreateCurrentAccount());

                var currentMaxSequence = launcherGroupItemsDao.SelectMaxSequence(SelectedLauncherGroup.LauncherGroupId);
                launcherGroupItemsDao.InsertNewItems(SelectedLauncherGroup.LauncherGroupId, new[] { data.Item.LauncherItemId }, currentMaxSequence + launcherFactory.GroupItemsStep, launcherFactory.GroupItemsStep, DatabaseCommonStatus.CreateCurrentAccount());

                commander.Commit();
            }

            NotifyManager.SendLauncherItemRegistered(SelectedLauncherGroup.LauncherGroupId, data.Item.LauncherItemId);
        }

        public void OpenExtendsExecuteView(Guid launcherItemId, string argument, IScreen screen)
        {
            ThrowIfDisposed();

            var launcherItem = LauncherItems.FirstOrDefault(i => i.LauncherItemId == launcherItemId);
            if(launcherItem == null) {
                Logger.LogError("指定のランチャーアイテムは存在しない: {0}", launcherItemId);
                return;
            }

            launcherItem.OpenExtendsExecuteViewWidthArgument(argument, screen);
        }

        #endregion

        #region ContextElementBase

        override protected void InitializeImpl()
        {
            Logger.LogInformation("initialize {0}:{1}, {2}: {3}", DockScreen.DeviceName, DockScreen.DeviceBounds, nameof(DockScreen.Primary), DockScreen.Primary);

            NotifyManager.LauncherItemChanged += NotifyManager_LauncherItemChanged;
            NotifyManager.LauncherItemRegistered += NotifyManager_LauncherItemRegistered;
            NotifyManager.LauncherItemRemovedInLauncherGroup += NotifyManager_LauncherItemRemovedInLauncherGroup;

            var launcherToolbarId = GetLauncherToolbarId();
            if(launcherToolbarId == Guid.Empty) {
                launcherToolbarId = CreateLauncherToolbar();
            }
            LauncherToolbarId = launcherToolbarId;
            LoadLauncherToolbar();
            LoadLauncherItems();
            UpdateDesign();
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                NotifyManager.LauncherItemChanged -= NotifyManager_LauncherItemChanged;
                NotifyManager.LauncherItemRegistered -= NotifyManager_LauncherItemRegistered;
                NotifyManager.LauncherItemRemovedInLauncherGroup -= NotifyManager_LauncherItemRemovedInLauncherGroup;
                Flush();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IFlushable

        public void Flush()
        {
            MainDatabaseLazyWriter.SafeFlush();
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

                return IsVisible;
            }
        }

        public void StartView()
        {
            var windowItem = OrderManager.CreateLauncherToolbarWindow(this);
            ViewCreated = true;
        }

        #endregion

        #region IViewCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            ChangeVisibleDelaySave(false);
            return true;
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

        #region IAppDesktopToolbarExtendData

        AppDesktopToolbarPosition _ToolbarPosition;
        /// <summary>
        /// ツールバー位置。
        /// </summary>
        public AppDesktopToolbarPosition ToolbarPosition
        {
            get => this._ToolbarPosition;
            set => SetProperty(ref this._ToolbarPosition, value);
        }
        /// <summary>
        /// ドッキング中か。
        /// </summary>
        public bool IsDocking { get; set; }

        bool _IsAutoHide;
        /// <summary>
        /// 自動的に隠すか。
        /// </summary>
        public bool IsAutoHide
        {
            get => this._IsAutoHide;
            set => SetProperty(ref this._IsAutoHide, value);
        }

        bool _PausingAutoHide;
        /// <summary>
        /// 自動的に隠す処理を一時的に中断するか。
        /// </summary>
        public bool PausingAutoHide
        {
            get => this._PausingAutoHide;
            set => SetProperty(ref this._PausingAutoHide, value);
        }

        /// <summary>
        /// 隠れているか。
        /// </summary>
        public bool IsHiding { get; set; }
        /// <summary>
        /// 自動的に隠れるまでの時間。
        /// </summary>
        public TimeSpan AutoHideTimeout { get; private set; }

        /// <summary>
        /// 表示中のサイズ。
        /// <para><see cref="AppDesktopToolbarPosition"/>の各辺に対応</para>
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size DisplaySize { get; set; }

        /// <summary>
        /// 表示中の論理バーサイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect DisplayBarArea { get; set; }
        /// <summary>
        /// 隠れた状態のバー論理サイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size HiddenSize { get; private set; }
        /// <summary>
        /// 表示中の隠れたバーの論理領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect HiddenBarArea { get; set; }

        bool _ExistsFullScreenWindow;
        /// <summary>
        /// フルスクリーンウィンドウが存在するか。
        /// </summary>
        public bool ExistsFullScreenWindow
        {
            get => this._ExistsFullScreenWindow;
            set => SetProperty(ref this._ExistsFullScreenWindow, value);
        }


        /// <summary>
        /// 対象ディスプレイ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public IScreen DockScreen { get; }

        #endregion

        private void NotifyManager_LauncherItemChanged(object? sender, LauncherItemChangedEventArgs e)
        {
            var launcherItem = LauncherItems.FirstOrDefault(i => i.LauncherItemId == e.LauncherItemId);
            if(launcherItem != null) {
                var index = LauncherItems.IndexOf(launcherItem);
                LauncherItems.RemoveAt(index);
                LauncherItems.Insert(index, launcherItem);
                //LoadLauncherItems();
            }
        }
        private void NotifyManager_LauncherItemRegistered(object? sender, LauncherItemRegisteredEventArgs e)
        {
            if(e.LauncherGroupId == SelectedLauncherGroup?.LauncherGroupId) {
                // 現在表示中グループの表示を更新
                var element = OrderManager.GetOrCreateLauncherItemElement(e.LauncherItemId);
                LauncherItems.Add(element);
            }
        }

        private void NotifyManager_LauncherItemRemovedInLauncherGroup(object? sender, LauncherItemRemoveInLauncherGroupEventArgs e)
        {
            if(e.LauncherGroupId == SelectedLauncherGroup?.LauncherGroupId) {
                var removedItemIndex = LauncherItems
                    .Counting()
                    .Where(i => i.Value.LauncherItemId == e.LauncherItemId)
                    .Counting()
                    .First(i => i.Number == e.Index)
                    .Value.Number
                ;
                LauncherItems.RemoveAt(removedItemIndex);
            }
        }


    }
}
