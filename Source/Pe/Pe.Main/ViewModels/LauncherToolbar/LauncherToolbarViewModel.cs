using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.Font;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherGroup;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem;
using ContentTypeTextNet.Pe.Main.Views.Extend;
using ContentTypeTextNet.Pe.Main.Views.LauncherToolbar;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using ContentTypeTextNet.Pe.Library.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base.Linq;
using System.Threading;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Database;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar
{
    public class LauncherToolbarViewModel: ElementViewModelBase<LauncherToolbarElement>, IReadOnlyAppDesktopToolbarExtendData, IViewLifecycleReceiver
    {
        #region variable

        private LauncherDetailViewModelBase? _contextMenuOpenedItem;
        private bool _showWaiting;

        #endregion

        public LauncherToolbarViewModel(LauncherToolbarElement model, IKeyGestureGuide keyGestureGuide, LauncherToolbarConfiguration launcherToolbarConfiguration, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IPlatformTheme platformThemeLoader, ILauncherToolbarTheme launcherToolbarTheme, IGeneralTheme generalTheme, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            KeyGestureGuide = keyGestureGuide;
            LauncherToolbarConfiguration = launcherToolbarConfiguration;
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            PlatformThemeLoader = platformThemeLoader;
            LauncherToolbarTheme = launcherToolbarTheme;
            GeneralTheme = generalTheme;

            LauncherGroupCollection = new ModelViewModelObservableCollectionManager<LauncherGroupElement, LauncherGroupViewModel>(Model.LauncherGroups, new ModelViewModelObservableCollectionOptions<LauncherGroupElement, LauncherGroupViewModel>() {
                ToViewModel = (m) => new LauncherGroupViewModel(m, DispatcherWrapper, LoggerFactory),
            });
            LauncherGroupItems = LauncherGroupCollection.ViewModels;

            LauncherItemCollection = new ModelViewModelObservableCollectionManager<LauncherItemElement, LauncherDetailViewModelBase>(Model.LauncherItems, new ModelViewModelObservableCollectionOptions<LauncherItemElement, LauncherDetailViewModelBase>() {
                ToViewModel = (m) => LauncherItemViewModelFactory.Create(m, DockScreen, KeyGestureGuide, DispatcherWrapper, LauncherToolbarTheme, LoggerFactory),
            });
            LauncherItems = LauncherItemCollection.GetDefaultView();

            ViewDragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = ViewCanDragStart,
                DragEnterAction = ViewDragOverOrEnter,
                DragOverAction = ViewDragOverOrEnter,
                DragLeaveAction = ViewDragLeave,
                DropActionAsync = ViewDropAsync,
                GetDragParameter = ViewGetDragParameter,
            };
            ItemDragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = ItemCanDragStart,
                DragEnterAction = ItemDragOverOrEnter,
                DragOverAction = ItemDragOverOrEnter,
                DragLeaveAction = ItemDragLeave,
                DropActionAsync = ItemDropAsync,
                GetDragParameter = ItemGetDragParameter,
            };

            Font = new FontViewModel(Model.Font!, DispatcherWrapper, LoggerFactory);

            PropertyChangedObserver = new PropertyChangedObserver(DispatcherWrapper, LoggerFactory);
            PropertyChangedObserver.AddProperties<IReadOnlyAppDesktopToolbarExtendData>();
            PropertyChangedObserver.AddObserver(nameof(IAppDesktopToolbarExtendData.ToolbarPosition), nameof(IsVerticalLayout));
            PropertyChangedObserver.AddObserver(nameof(IAppDesktopToolbarExtendData.ToolbarPosition), ChangeToolbarPositionCommand);
            PropertyChangedObserver.AddObserver(nameof(IAppDesktopToolbarExtendData.IsAutoHide), nameof(IsAutoHide));
            PropertyChangedObserver.AddObserver(nameof(LauncherToolbarElement.IsOpenedAppMenu), nameof(IsOpenedAppMenu));
            PropertyChangedObserver.AddObserver(nameof(LauncherToolbarElement.IsOpenedFileItemMenu), nameof(IsOpenedFileItemMenu));
            PropertyChangedObserver.AddObserver(nameof(LauncherToolbarElement.IsOpenedStoreAppItemMenu), nameof(IsOpenedStoreAppItemMenu));
            PropertyChangedObserver.AddObserver(nameof(LauncherToolbarElement.IsOpenedAddonItemMenu), nameof(IsOpenedAddonItemMenu));
            PropertyChangedObserver.AddObserver(nameof(LauncherToolbarElement.IsTopmost), nameof(IsTopmost));
            PropertyChangedObserver.AddObserver(nameof(LauncherToolbarElement.SelectedLauncherGroup), nameof(SelectedLauncherGroup));
            PropertyChangedObserver.AddObserver(nameof(LauncherToolbarElement.ExistsFullScreenWindow), nameof(ExistsFullScreenWindow));

            PlatformThemeLoader.Changed += PlatformThemeLoader_Changed;
            ThemeProperties = new ThemeProperties(this);
        }

        #region property

        public RequestSender ExpandShortcutFileRequest { get; } = new RequestSender();

        public AppDesktopToolbarExtend? AppDesktopToolbarExtend { get; set; }
        private IKeyGestureGuide KeyGestureGuide { get; }
        private LauncherToolbarConfiguration LauncherToolbarConfiguration { get; }
        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private IPlatformTheme PlatformThemeLoader { get; }
        private ILauncherToolbarTheme LauncherToolbarTheme { get; }
        private IGeneralTheme GeneralTheme { get; }
        private PropertyChangedObserver PropertyChangedObserver { get; }
        private ThemeProperties ThemeProperties { get; }

        private DispatcherTimer? AutoHideShowWaitTimer { get; set; }

        public IconBox IconBox => Model.IconBox;
        public Thickness ButtonPadding => Model.ButtonPadding;
        public Thickness IconMargin => Model.IconMargin;
        public bool IsIconOnly => Model.IsIconOnly;
        public double TextWidth => Model.TextWidth;

        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetModelValue(value);
        }

        public bool IsTopmost
        {
            get => Model.IsTopmost;
        }

        [ThemeProperty]
        public object ToolbarMainIcon
        {
            get => GeneralTheme.GetPathImage(GeneralPathImageKind.Menu, new IconScale(IconBox, IconSize.DefaultScale));
        }

        [ThemeProperty]
        public Brush ToolbarBackground
        {
            get => LauncherToolbarTheme.GetToolbarBackground(ToolbarPosition, ViewState.Active, new IconScale(IconBox, IconSize.DefaultScale), IsIconOnly, TextWidth);
        }

        [ThemeProperty]
        public Brush ToolbarForeground
        {
            get => LauncherToolbarTheme.GetToolbarForeground();
        }

        public LauncherToolbarIconDirection IconDirection => Model.IconDirection;

        public bool IsOpenedAppMenu
        {
            get => Model.IsOpenedAppMenu;
            set => SetModelValue(value);
        }

        public bool IsOpenedFileItemMenu
        {
            get => Model.IsOpenedFileItemMenu;
            set => SetModelValue(value);
        }
        public bool IsOpenedStoreAppItemMenu
        {
            get => Model.IsOpenedStoreAppItemMenu;
            set => SetModelValue(value);
        }
        public bool IsOpenedAddonItemMenu
        {
            get => Model.IsOpenedAddonItemMenu;
            set => SetModelValue(value);
        }
        public bool IsOpenedSeparatorItemMenu
        {
            get => Model.IsOpenedSeparatorItemMenu;
            set => SetModelValue(value);
        }

        public LauncherDetailViewModelBase? ContextMenuOpenedItem
        {
            get => this._contextMenuOpenedItem;
            set => SetProperty(ref this._contextMenuOpenedItem, value);
        }

        public bool ShowWaiting
        {
            get => this._showWaiting;
            set => SetProperty(ref this._showWaiting, value);
        }

        private ModelViewModelObservableCollectionManager<LauncherGroupElement, LauncherGroupViewModel> LauncherGroupCollection { get; }
        public ReadOnlyObservableCollection<LauncherGroupViewModel> LauncherGroupItems { get; }

        private ModelViewModelObservableCollectionManager<LauncherItemElement, LauncherDetailViewModelBase> LauncherItemCollection { get; }
        public ICollectionView LauncherItems { get; }

        public RequestSender CloseRequest { get; } = new RequestSender();

        public bool IsVerticalLayout => ToolbarPosition == AppDesktopToolbarPosition.Left || ToolbarPosition == AppDesktopToolbarPosition.Right;

        public FontViewModel Font { get; }
        public IDragAndDrop ViewDragAndDrop { get; }
        public IDragAndDrop ItemDragAndDrop { get; }

        public LauncherGroupViewModel? SelectedLauncherGroup
        {
            get
            {
                if(Model?.SelectedLauncherGroup != null) {
                    if(LauncherGroupCollection.TryGetViewModel(Model.SelectedLauncherGroup, out var result)) {
                        return result;
                    }
                }

                return null;
            }
        }

        public LauncherToolbarContentDropMode ContentDropMode => Model.ContentDropMode;
        public LauncherGroupPosition GroupMenuPosition => Model.GroupMenuPosition;

        private LauncherToolbarIconMaker IconMaker { get; } = new LauncherToolbarIconMaker();

        #region theme

        [ThemeProperty]
        public DependencyObject ToolbarPositionLeftIcon => IconMaker.GetToolbarPositionImage(AppDesktopToolbarPosition.Left, IconBox.Small);
        [ThemeProperty]
        public DependencyObject ToolbarPositionTopIcon => IconMaker.GetToolbarPositionImage(AppDesktopToolbarPosition.Top, IconBox.Small);
        [ThemeProperty]
        public DependencyObject ToolbarPositionRightIcon => IconMaker.GetToolbarPositionImage(AppDesktopToolbarPosition.Right, IconBox.Small);
        [ThemeProperty]
        public DependencyObject ToolbarPositionBottomIcon => IconMaker.GetToolbarPositionImage(AppDesktopToolbarPosition.Bottom, IconBox.Small);

        [ThemeProperty]
        public ControlTemplate LauncherItemNormalButtonControlTemplate => LauncherToolbarTheme.GetLauncherItemNormalButtonControlTemplate();
        [ThemeProperty]
        public ControlTemplate LauncherItemToggleButtonControlTemplate => LauncherToolbarTheme.GetLauncherItemToggleButtonControlTemplate();

        #endregion

        #endregion

        #region command

        private ICommand? _ChangeToolbarPositionCommand;
        public ICommand ChangeToolbarPositionCommand => this._ChangeToolbarPositionCommand ??= new DelegateCommand<AppDesktopToolbarPosition?>(
            o => {
                if(o.HasValue) {
                    Model.ChangeToolbarPositionDelaySave(o.Value);
                    ChangeLauncherGroupCommand.ExecuteIfCanExecute(SelectedLauncherGroup);
                } else {
                    Logger.LogTrace("こないはず");
                }
            },
            o => o.HasValue && o.Value != ToolbarPosition
        );

        private ICommand? _ToggleTopmostCommand;
        public ICommand ToggleTopmostCommand => this._ToggleTopmostCommand ??= new DelegateCommand(
            () => {
                Model.ChangeTopmostDelaySave(!Model.IsTopmost);
            }
        );

        private ICommand? _ToggleAutoHideCommand;
        public ICommand ToggleAutoHideCommand => this._ToggleAutoHideCommand ??= new DelegateCommand(
            () => {
                Model.ChangeAutoHideDelaySave(!Model.IsAutoHide);
            }
        );

        private ICommand? _CloseCommand;
        public ICommand CloseCommand => this._CloseCommand ??= new DelegateCommand(
             () => {
                 Model.ChangeVisibleDelaySave(false);

                 //var notification = new Notification();
                 //CloseRequest.Raise(notification);
                 CloseRequest.Send();
             }
        );

        private ICommand? _ChangeLauncherGroupCommand;
        public ICommand ChangeLauncherGroupCommand => this._ChangeLauncherGroupCommand ??= new DelegateCommand<LauncherGroupViewModel>(
           o => {
               ChangeLauncherGroup(o);
           }
       );

        private ICommand? _RemoveCommand;
        public ICommand RemoveCommand => this._RemoveCommand ??= new DelegateCommand<LauncherDetailViewModelBase>(
            o => {
                Debug.Assert(SelectedLauncherGroup != null);

                var targetIndex = LauncherItemCollection.ViewModels
                    .Where(i => i.LauncherItemId == o.LauncherItemId)
                    .Counting()
                    .First(i => i.Value == o)
                    .Number
                ;

                Model.RemoveLauncherItem(SelectedLauncherGroup.LauncherGroupId, o.LauncherItemId, targetIndex);
            }
        );

        private ICommand? _AutoHideToHideCommand;
        public ICommand AutoHideToHideCommand => this._AutoHideToHideCommand ??= new DelegateCommand(
            () => {
                HideAndShowWaiting();
            }
        );

        private ICommand? _PreviewMouseDownCommand;
        public ICommand PreviewMouseDownCommand => this._PreviewMouseDownCommand ??= new DelegateCommand<MouseButtonEventArgs>(
            o => {
                if(SelectedLauncherGroup == null) {
                    Logger.LogError("こねぇよ");
                    return;
                }
                if(LauncherGroupCollection.Count == 1) {
                    Logger.LogDebug("処理する必要なし");
                    return;
                }

                var prev = o.MouseDevice.XButton1.HasFlag(MouseButtonState.Pressed);
                var next = o.MouseDevice.XButton2.HasFlag(MouseButtonState.Pressed);
                if((prev || next) && o.ButtonState == MouseButtonState.Pressed) {

                    var currentIndex = LauncherGroupCollection.IndexOf(SelectedLauncherGroup);
                    int nextIndex;
                    if(prev) {
                        nextIndex = currentIndex == 0
                            ? LauncherGroupCollection.Count - 1
                            : currentIndex - 1
                        ;
                    } else {
                        Debug.Assert(next);
                        nextIndex = currentIndex == LauncherGroupCollection.Count - 1
                            ? 0
                            : currentIndex + 1
                        ;
                    }

                    var vm = LauncherGroupCollection.ViewModels[nextIndex];
                    ChangeLauncherGroup(vm);

                    o.Handled = true;
                }
            }
        );

        private ICommand? _AddNewGroupCommand;
        public ICommand AddNewGroupCommand => this._AddNewGroupCommand ??= new DelegateCommand(
             () => {
                 var newGroupElement = Model.AddNewGroup(LauncherGroupKind.Normal);

                 if(!LauncherGroupCollection.TryGetViewModel(newGroupElement, out var newGroupViewModel)) {
                     Logger.LogError("生成したグループが存在しない異常: {0}", newGroupElement.LauncherGroupId);
                     return;
                 }

                 ChangeLauncherGroup(newGroupViewModel);
             }
         );

        #endregion

        #region function

        private void ChangeLauncherGroup(LauncherGroupViewModel targetGroup)
        {
            var currentLauncherItems = LauncherItemCollection.ViewModels.ToList();

            if(LauncherGroupCollection.TryGetModel(targetGroup, out var groupModel)) {
                Model.ChangeLauncherGroup(groupModel);
                LauncherItems.Refresh();

                foreach(var vm in currentLauncherItems) {
                    vm.Dispose();
                }
            }
        }


        #region ViewDragAndDrop

        private IResultSuccess<DragParameter> ViewGetDragParameter(UIElement sender, MouseEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            return dd.GetDragParameter(sender, e);
        }

        private bool ViewCanDragStart(UIElement sender, MouseEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            return dd.CanDragStart(sender, e);
        }

        private void ViewDragOverOrEnter(UIElement sender, DragEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            dd.DragOverOrEnter(sender, e);
        }

        private Task ViewDropAsync(UIElement sender, DragEventArgs e, CancellationToken cancellationToken)
        {
            var shortcutDropMode = LauncherToolbarShortcutDropMode.Confirm;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var dao = new AppLauncherToolbarSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var setting = dao.SelectSettingLauncherToolbarSetting();
                shortcutDropMode = setting.ShortcutDropMode;
            }
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory) {
                ShortcutDropMode = shortcutDropMode,
            };
            return dd.DropAsync(sender, e, s => dd.RegisterDropFile(ExpandShortcutFileRequest, s, Model.RegisterFile), cancellationToken);
        }

        private void ViewDragLeave(UIElement sender, DragEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            dd.DragLeave(sender, e);
        }

        #endregion

        #region ItemDragAndDrop

        private IResultSuccess<DragParameter> ItemGetDragParameter(UIElement sender, MouseEventArgs e) => Result.CreateFailure<DragParameter>();

        private bool ItemCanDragStart(UIElement sender, MouseEventArgs e) => false;

        private void ItemDragOverOrEnter(UIElement sender, DragEventArgs e)
        {
            var appButton = UIUtility.GetClosest<ToggleButton>(sender);
            var overAppButton = appButton?.Name == nameof(LauncherToolbarWindow.appButton.Name);

            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                if(overAppButton) {
                    ViewDragOverOrEnter(sender, e);
                    return;
                } else {
                    e.Effects = DragDropEffects.Move;
                }
            } else if(e.Data.IsTextPresent()) {
                if(overAppButton) {
                    e.Effects = DragDropEffects.None;
                } else {
                    e.Effects = DragDropEffects.Move;
                }
            }

            e.Handled = true;
        }

        private async Task ItemDropAsync(UIElement sender, DragEventArgs e, CancellationToken cancellationToken)
        {
            LauncherItemId launcherItemId = LauncherItemId.Empty;
            var frameworkElement = (FrameworkElement)sender;

            if(frameworkElement.DataContext is LauncherContentControl launcherContentControl) {
                var launcherItem = (ILauncherItemId)launcherContentControl.DataContext;
                launcherItemId = launcherItem.LauncherItemId;

                if(LauncherItemId.Empty == launcherItemId) {
                    Logger.LogError("ランチャーアイテムID取得できず, {0}, {1}", sender, e);
                    return;
                }
            } else {
                var appButton = UIUtility.GetClosest<ToggleButton>(frameworkElement);
                if(appButton is null) {
                    return;
                }
                if(appButton.Name != nameof(LauncherToolbarWindow.appButton.Name)) {
                    return;
                }
                await ViewDropAsync(sender, e, cancellationToken);
                return;
            }

            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                var argument = string.Join(' ', filePaths.Select(i => CommandLine.Escape(i)));
                await DispatcherWrapper.BeginAsync(async () => await ExecuteExtendDropDataAsync(launcherItemId, argument, cancellationToken));
            } else if(e.Data.IsTextPresent()) {
                var argument = TextUtility.JoinLines(e.Data.RequireText());
                await DispatcherWrapper.BeginAsync(async () => await ExecuteExtendDropDataAsync(launcherItemId, argument, cancellationToken));
            }

            e.Handled = true;
        }

        private void ItemDragLeave(UIElement sender, DragEventArgs e)
        { }

        #endregion

        private async Task ExecuteExtendDropDataAsync(LauncherItemId launcherItemId, string argument, CancellationToken cancellationToken)
        {
            switch(ContentDropMode) {
                case LauncherToolbarContentDropMode.ExtendsExecute:
                    await Model.OpenExtendsExecuteViewAsync(launcherItemId, argument, DockScreen, cancellationToken);
                    break;

                case LauncherToolbarContentDropMode.DirectExecute:
                    await Model.ExecuteWithArgumentAsync(launcherItemId, argument, cancellationToken);
                    break;
            }
        }

        /// <summary>
        /// 自動的に隠すツールバーを隠して一時的に表示を停止する。
        /// </summary>
        public void HideAndShowWaiting()
        {
            if(!IsVisible) {
                return;
            }
            if(!IsAutoHide) {
                return;
            }
            if(IsHiding) {
                return;
            }

            DispatcherWrapper.VerifyAccess();

            if(TimeSpan.Zero < LauncherToolbarConfiguration.AutoHideShowWaitTime) {
                if(AutoHideShowWaitTimer != null) {
                    AutoHideShowWaitTimer.Stop();
                    AutoHideShowWaitTimer.Tick -= AutoHideShowWaitTimer_Tick;
                    AutoHideShowWaitTimer = null;
                }

                AutoHideShowWaitTimer = new DispatcherTimer(DispatcherPriority.Background) {
                    Interval = LauncherToolbarConfiguration.AutoHideShowWaitTime,
                };
                AutoHideShowWaitTimer.Tick += AutoHideShowWaitTimer_Tick;
            }

            AppDesktopToolbarExtend!.HideView(true);
            if(AutoHideShowWaitTimer != null) {
                Logger.LogTrace("自動的に隠すの強制隠しからの復帰抑制を開始");
                ShowWaiting = true;
                AutoHideShowWaitTimer.Start();
            }
        }

        #endregion

        #region IAppDesktopToolbarExtendData

        /// <summary>
        /// ツールバー位置。
        /// </summary>
        public AppDesktopToolbarPosition ToolbarPosition => Model.ToolbarPosition;

        /// <summary>
        /// ドッキング中か。
        /// </summary>
        public bool IsDocking => Model.IsDocking;

        public TimeSpan DisplayDelayTime => Model.DisplayDelayTime;

        /// <summary>
        /// 自動的に隠すか。
        /// </summary>
        public bool IsAutoHide => Model.IsAutoHide;
        /// <summary>
        /// 自動的に隠す処理を一時的に中断するか。
        /// </summary>
        public bool PausingAutoHide => Model.PausingAutoHide;
        /// <summary>
        /// 隠れているか。
        /// </summary>
        public bool IsHiding => Model.IsHiding;
        /// <summary>
        /// 自動的に隠れるまでの時間。
        /// </summary>
        public TimeSpan AutoHideTime => Model.AutoHideTime;

        /// <summary>
        /// 表示中のサイズ。
        /// </summary>
        /// <remarks>
        /// <para><see cref="AppDesktopToolbarPosition"/>の各辺に対応</para>
        /// </remarks>
        [PixelKind(Px.Logical)]
        public Size DisplaySize => Model.DisplaySize;

        /// <summary>
        /// 表示中の論理バーサイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect DisplayBarArea => Model.DisplayBarArea;
        /// <summary>
        /// 隠れた状態のバー論理サイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size HiddenSize => Model.HiddenSize;
        /// <summary>
        /// 表示中の隠れたバーの論理領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect HiddenBarArea => Model.HiddenBarArea;

        /// <summary>
        /// フルスクリーンウィンドウが存在するか。
        /// </summary>
        public bool ExistsFullScreenWindow => Model.ExistsFullScreenWindow;


        /// <summary>
        /// 対象ディスプレイ。
        /// </summary>
        public IScreen DockScreen => Model.DockScreen;

        #endregion

        //#region ILoggerFactory

        //public ILogger CreateLogger(string header) => LoggerFactory.CreateLogger(header);

        //#endregion

        #region IViewLifecycleReceiver
        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        {
            if(!IsVisible) {
                window.Visibility = Visibility.Collapsed;
            }
        }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }


        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public Task ReceiveViewClosedAsync(Window window, bool isUserOperation, CancellationToken cancellationToken)
        {
            return Model.ReceiveViewClosedAsync(isUserOperation, cancellationToken);
        }


        #endregion

        #region SingleModelViewModelBase

        protected override void AttachModelEventsImpl()
        {
            base.AttachModelEventsImpl();
            Model.PropertyChanged += Model_PropertyChanged;
        }

        protected override void DetachModelEventsImpl()
        {
            base.DetachModelEventsImpl();
            Model.PropertyChanged -= Model_PropertyChanged;
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    PlatformThemeLoader.Changed -= PlatformThemeLoader_Changed;
                    LauncherItemCollection.Dispose();
                    LauncherGroupCollection.Dispose();
                    Font.Dispose();
                }

            }
            base.Dispose(disposing);
        }

        #endregion

        private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChangedObserver.Execute(e, RaisePropertyChanged);
        }

        private void PlatformThemeLoader_Changed(object? sender, EventArgs e)
        {
            DispatcherWrapper.BeginAsync(vm => {
                if(vm.IsDisposed) {
                    return;
                }

                foreach(var propertyName in vm.ThemeProperties.GetPropertyNames()) {
                    vm.RaisePropertyChanged(propertyName);
                }
            }, this, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        private void AutoHideShowWaitTimer_Tick(object? sender, EventArgs e)
        {
            Logger.LogTrace("自動的に隠すの強制隠しからの復帰抑制を解除");
            ShowWaiting = false;

            if(AutoHideShowWaitTimer != null) {
                AutoHideShowWaitTimer.Tick -= AutoHideShowWaitTimer_Tick;
                AutoHideShowWaitTimer.Stop();
                AutoHideShowWaitTimer = null;
            }
        }
    }
}
