using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Views.Extend;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar
{
    public class LauncherToolbarElement : ContextElementBase, IAppDesktopToolbarExtendData, IViewShowStarter, IViewCloseReceiver, IFlushable
    {
        #region variable

        bool _isVisible;
        bool _isTopmost;
        LauncherToolbarIconDirection _iconDirection;
        LauncherGroupElement? _selectedLauncherGroup;
        bool _isOpendAppMenu;

        #endregion

        public LauncherToolbarElement(Screen dockScreen, ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups, IOrderManager orderManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, ILauncherToolbarTheme launcherToolbarTheme, IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            DockScreen = dockScreen;
            LauncherGroups = launcherGroups;

            OrderManager = orderManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            IdFactory = idFactory;
            LauncherToolbarTheme = launcherToolbarTheme;

            MainDatabaseLazyWriter = new DatabaseLazyWriter(MainDatabaseBarrier, Constants.Config.LauncherToolbarMainDatabaseLazyWriterWaitTime, LoggerFactory);
        }

        #region property

        IOrderManager OrderManager { get; }
        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IIdFactory IdFactory { get; }
        ILauncherToolbarTheme LauncherToolbarTheme { get; }

        DatabaseLazyWriter MainDatabaseLazyWriter { get; }
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
        public IconBasicSize IconScale { get; private set; }

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
            set => SetProperty(ref this._isOpendAppMenu, value);
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
            var screenChecker = new ScreenChecker();
            var row = rows.FirstOrDefault(r => screenChecker.FindMaybe(DockScreen, r));
            if(row != null) {
                return row.LauncherToolbarId;
            }

            return Guid.Empty;
        }

        Guid GetLauncherToolbarId()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherToolbarDomainDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var screenToolbars = dao.SelectAllScreenToolbars().ToList();
                var LauncherToolbarId = FindMaybeToolbarId(screenToolbars);
                return LauncherToolbarId;
            }
        }

        Guid CreateLauncherToolbar()
        {
            var toolbarId = IdFactory.CreateLauncherToolbarId();
            Logger.LogDebug("create toolbar: {0}", toolbarId);

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var toolbarsDao = new LauncherToolbarsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                toolbarsDao.InsertNewToolbar(toolbarId, DockScreen.DeviceName, DatabaseCommonStatus.CreateCurrentAccount());
#pragma warning restore CS8604 // Null 参照引数の可能性があります。

                var screenOperator = new ScreenOperator(LoggerFactory);
                screenOperator.RegisterDatabase(DockScreen, commander, StatementLoader, commander.Implementation, DatabaseCommonStatus.CreateCurrentAccount());

                commander.Commit();
            }

            return toolbarId;
        }

        public void ChangeLauncherGroup(LauncherGroupElement launcherGroup)
        {
            if(IsOpendAppMenu) {
                IsOpendAppMenu = false;
            }
            SelectedLauncherGroup = launcherGroup;
            LoadLauncherItems();
        }

        void UpdateDesign()
        {
            ButtonPadding = LauncherToolbarTheme.GetButtonPadding(ToolbarPosition, IconScale);
            IconMargin = LauncherToolbarTheme.GetIconMargin(ToolbarPosition, IconScale, IsIconOnly, TextWidth);
            DisplaySize = LauncherToolbarTheme.GetDisplaySize(ButtonPadding, IconMargin, IconScale, IsIconOnly, TextWidth);
            HiddenSize = LauncherToolbarTheme.GetHiddenSize(ButtonPadding, IconMargin, IconScale, IsIconOnly, TextWidth);
        }

        void LoadLauncherToolbar()
        {
            Logger.LogInformation("toolbar id: {0}", LauncherToolbarId);

            LauncherToolbarsDisplayData displayData;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherToolbarsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                displayData = dao.SelectDisplayData(LauncherToolbarId);
            }

            IconScale = displayData.IconBasicSize;
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
            var items = GetLauncherItems();
            LauncherItems.SetRange(items);
        }

        public void ChangeToolbarPosition(AppDesktopToolbarPosition toolbarPosition)
        {
            ToolbarPosition = toolbarPosition;
            UpdateDesign();
            IsOpendAppMenu = false;

            MainDatabaseLazyWriter.Stock(c => {
                var dao = new LauncherToolbarsEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                dao.UpdateToolbarPosition(LauncherToolbarId, ToolbarPosition, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ChangeTopmost(bool isTopmost)
        {
            IsTopmost = isTopmost;
            IsOpendAppMenu = false;

            MainDatabaseLazyWriter.Stock(c => {
                var dao = new LauncherToolbarsEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                dao.UpdatIsTopmost(LauncherToolbarId, IsTopmost, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ChangeAutoHide(bool isAutoHide)
        {
            IsAutoHide = isAutoHide;
            IsOpendAppMenu = false;

            MainDatabaseLazyWriter.Stock(c => {
                var dao = new LauncherToolbarsEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                dao.UpdatIsAutoHide(LauncherToolbarId, IsAutoHide, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ChangeVisible(bool isVisible)
        {
            IsVisible = isVisible;
            IsOpendAppMenu = false;

            MainDatabaseLazyWriter.Stock(c => {
                var dao = new LauncherToolbarsEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                dao.UpdatIsVisible(LauncherToolbarId, IsVisible, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        #endregion

        #region ContextElementBase

        override protected void InitializeImpl()
        {
            Logger.LogInformation("initialize {0}:{1}, {2}: {3}", DockScreen.DeviceName, DockScreen.DeviceBounds, nameof(DockScreen.Primary), DockScreen.Primary);

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
                Flush();
                if(disposing) {
                    MainDatabaseLazyWriter.Dispose();
                }
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
            ChangeVisible(false);
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
        public Screen DockScreen { get; }

        #endregion
    }
}
