using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.Model.Logic.Designer;
using ContentTypeTextNet.Pe.Main.Model.Logic.Theme;
using ContentTypeTextNet.Pe.Main.Model.Manager;
using ContentTypeTextNet.Pe.Main.View.Extend;

namespace ContentTypeTextNet.Pe.Main.Model.Element.LauncherToolbar
{
    public class LauncherToolbarElement : ContextElementBase, IAppDesktopToolbarExtendData, IViewShowStarter, IViewCloseReceiver
    {
        #region variable

        bool _isTopmost;
        LauncherGroupElement _selectedLauncherGroup;
        bool _isOpendAppMenu;

        #endregion

        public LauncherToolbarElement(Screen dockScreen, ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups, IOrderManager orderManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, ILauncherToolbarDesigner launcherToolbarDesigner, IImagePainter imagePainter, IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            DockScreen = dockScreen;
            LauncherGroups = launcherGroups;

            OrderManager = orderManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            IdFactory = idFactory;
            LauncherToolbarDesigner = launcherToolbarDesigner;
            ImagePainter = imagePainter;
        }

        #region property

        IOrderManager OrderManager { get; }
        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IIdFactory IdFactory { get; }
        ILauncherToolbarDesigner LauncherToolbarDesigner { get; }
        public IImagePainter ImagePainter { get; }

        public ReadOnlyObservableCollection<LauncherGroupElement> LauncherGroups { get; }

        public Guid LauncherToolbarId { get; private set; }

        bool ViewCreated { get; set; }

        public ObservableCollection<LauncherItemElement> LauncherItems { get; } = new ObservableCollection<LauncherItemElement>();

        /// <summary>
        /// 表示されているか。
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// 表示アイコンサイズ。
        /// </summary>
        public IconScale IconScale { get; private set; }

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

        public LauncherGroupElement SelectedLauncherGroup
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
                var dao = new LauncherToolbarsDao(commander, StatementLoader, this);
                var screenToolbars = dao.SelectAllToolbars().ToList();
                var LauncherToolbarId = FindMaybeToolbarId(screenToolbars);
                return LauncherToolbarId;
            }
        }

        Guid CreateLauncherToolbar()
        {
            var toolbarId = IdFactory.CreateLauncherToolbarId();
            Logger.Debug($"create toolbar: {toolbarId}");

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var dao = new LauncherToolbarsDao(commander, StatementLoader, this);
                dao.InsertNewToolbar(toolbarId, DockScreen);

                commander.Commit();
            }

            return toolbarId;
        }

        void UpdateDesign()
        {
            ButtonPadding = LauncherToolbarDesigner.GetButtonPadding(ToolbarPosition, IconScale);
            IconMargin = LauncherToolbarDesigner.GetIconMargin(ToolbarPosition, IconScale, IsIconOnly, TextWidth);
            DisplaySize = LauncherToolbarDesigner.GetDisplaySize(ButtonPadding, IconMargin, IconScale, IsIconOnly, TextWidth);
            HiddenSize = LauncherToolbarDesigner.GetHiddenSize(ButtonPadding, IconMargin, IconScale, IsIconOnly, TextWidth);
        }

        void LoadLauncherToolbar()
        {
            Logger.Information($"toolbar id: {LauncherToolbarId}");

            LauncherToolbarsDisplayData displayData;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherToolbarsDao(commander, StatementLoader, this);
                displayData = dao.SelectDisplayData(LauncherToolbarId);
            }

            IconScale = displayData.IconScale;
            TextWidth = displayData.TextWidth;
            IsIconOnly = displayData.IsIconOnly;
            IsTopmost = displayData.IsTopmost;
            IsAutoHide = displayData.IsAutoHide;
            AutoHideTimeout = displayData.AutoHideTimeout;
            ToolbarPosition = displayData.ToolbarPosition;
            IsVisible = displayData.IsVisible;

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
        }

        #endregion

        #region ContextElementBase

        override protected void InitializeImpl()
        {
            Logger.Information($"initialize {DockScreen.DeviceName}:{DockScreen.DeviceBounds}, {nameof(DockScreen.Primary)}: {DockScreen.Primary}");

            var launcherToolbarId = GetLauncherToolbarId();
            if(launcherToolbarId == Guid.Empty) {
                launcherToolbarId = CreateLauncherToolbar();
            }
            LauncherToolbarId = launcherToolbarId;
            LoadLauncherToolbar();
            LoadLauncherItems();
            UpdateDesign();
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

        #region IWindowCloseReceiver

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
