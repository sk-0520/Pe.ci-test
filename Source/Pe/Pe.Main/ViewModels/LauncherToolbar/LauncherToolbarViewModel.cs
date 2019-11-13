using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Main.Views.Extend;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherGroup;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherIcon;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Core.Views;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using System.Windows.Controls.Primitives;
using System.IO;
using ContentTypeTextNet.Pe.Main.Views.LauncherToolbar;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar
{
    public class LauncherToolbarViewModel : SingleModelViewModelBase<LauncherToolbarElement>, IReadOnlyAppDesktopToolbarExtendData, IViewLifecycleReceiver
    {
        #region variable

        LauncherDetailViewModelBase? _contextMenuOpendItem;

        #endregion

        public LauncherToolbarViewModel(LauncherToolbarElement model, ILauncherToolbarTheme launcherToolbarTheme, IDispatcherWapper dispatcherWapper, ILauncherGroupTheme launcherGroupTheme, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWapper = dispatcherWapper;
            LauncherToolbarTheme = launcherToolbarTheme;
            LauncherGroupTheme = launcherGroupTheme;

            LauncherGroupCollection = new ActionModelViewModelObservableCollectionManager<LauncherGroupElement, LauncherGroupViewModel>(Model.LauncherGroups, LoggerFactory) {
                ToViewModel = (m) => new LauncherGroupViewModel(m, DispatcherWapper, LauncherGroupTheme, LoggerFactory),
            };
            LauncherGroupItems = LauncherGroupCollection.ViewModels;

            LauncherItemCollection = new ActionModelViewModelObservableCollectionManager<LauncherItemElement, LauncherDetailViewModelBase>(Model.LauncherItems, LoggerFactory) {
                ToViewModel = (m) => LauncherItemViewModelFactory.Create(m, DockScreen, DispatcherWapper, LauncherToolbarTheme, LoggerFactory),
            };
            LauncherItems = LauncherItemCollection.GetCollectionView();

            ViewDragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = ViewCanDragStart,
                DragEnterAction = ViewDragOrverOrEnter,
                DragOverAction = ViewDragOrverOrEnter,
                DragLeaveAction = ViewDragLeave,
                DropAction = ViewDrop,
                GetDragParameter = ViewGetDragParameter,
            };
            ItemDragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = ItemCanDragStart,
                DragEnterAction = ItemDragOrverOrEnter,
                DragOverAction = ItemDragOrverOrEnter,
                DragLeaveAction = ItemDragLeave,
                DropAction = ItemDrop,
                GetDragParameter = ItemGetDragParameter,
            };


            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWapper, LoggerFactory);
            PropertyChangedHooker.AddProperties<IReadOnlyAppDesktopToolbarExtendData>();
            PropertyChangedHooker.AddHook(nameof(IAppDesktopToolbarExtendData.ToolbarPosition), nameof(IsVerticalLayout));
            PropertyChangedHooker.AddHook(nameof(IAppDesktopToolbarExtendData.ToolbarPosition), ChangeToolbarPositionCommand);
            PropertyChangedHooker.AddHook(nameof(IAppDesktopToolbarExtendData.IsAutoHide), nameof(IsAutoHide));
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.IsOpendAppMenu), nameof(IsOpendAppMenu));
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.IsOpendItemMenu), nameof(IsOpendItemMenu));
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.IsTopmost), nameof(IsTopmost));
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.SelectedLauncherGroup), nameof(SelectedLauncherGroup));
        }

        #region property

        public RequestSender ExpandShortcutFileRequest { get; } = new RequestSender();

        public AppDesktopToolbarExtend? AppDesktopToolbarExtend { get; set; }
        IDispatcherWapper DispatcherWapper { get; }
        ILauncherToolbarTheme LauncherToolbarTheme { get; }
        ILauncherGroupTheme LauncherGroupTheme { get; }
        PropertyChangedHooker PropertyChangedHooker { get; }

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

        public LauncherToolbarIconDirection IconDirection => Model.IconDirection;

        public bool IsOpendAppMenu
        {
            get => Model.IsOpendAppMenu;
            set => SetModelValue(value);
        }

        public bool IsOpendItemMenu
        {
            get => Model.IsOpendItemMenu;
            set => SetModelValue(value);
        }

        public LauncherDetailViewModelBase? ContextMenuOpendItem
        {
            get => this._contextMenuOpendItem;
            set => SetProperty(ref this._contextMenuOpendItem, value);
        }

        ModelViewModelObservableCollectionManagerBase<LauncherGroupElement, LauncherGroupViewModel> LauncherGroupCollection { get; }
        public ObservableCollection<LauncherGroupViewModel> LauncherGroupItems { get; }

        ModelViewModelObservableCollectionManagerBase<LauncherItemElement, LauncherDetailViewModelBase> LauncherItemCollection { get; }
        public ICollectionView LauncherItems { get; }

        public RequestSender CloseRequest { get; } = new RequestSender();

        public bool IsVerticalLayout => ToolbarPosition == AppDesktopToolbarPosition.Left || ToolbarPosition == AppDesktopToolbarPosition.Right;

        public IDragAndDrop ViewDragAndDrop { get; }
        public IDragAndDrop ItemDragAndDrop { get; }

        public LauncherGroupViewModel? SelectedLauncherGroup
        {
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            get => LauncherGroupCollection.GetViewModel(Model.SelectedLauncherGroup);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
        }

        public DependencyObject ToolbarPositionLeftIcon => CreateToolbarPositionIcon(AppDesktopToolbarPosition.Left);
        public DependencyObject ToolbarPositionTopIcon => CreateToolbarPositionIcon(AppDesktopToolbarPosition.Top);
        public DependencyObject ToolbarPositionRightIcon => CreateToolbarPositionIcon(AppDesktopToolbarPosition.Right);
        public DependencyObject ToolbarPositionBottomIcon => CreateToolbarPositionIcon(AppDesktopToolbarPosition.Bottom);

        public ControlTemplate LauncherItemNormalButtonControlTemplate => DispatcherWapper.Get(() => LauncherToolbarTheme.GetLauncherItemNormalButtonControlTemplate());
        public ControlTemplate LauncherItemToggleButtonControlTemplate => DispatcherWapper.Get(() => LauncherToolbarTheme.GetLauncherItemToggleButtonControlTemplate());

        #endregion

        #region command

        public ICommand ChangeToolbarPositionCommand => GetOrCreateCommand(() => new DelegateCommand<AppDesktopToolbarPosition?>(
            o => {
                if(o.HasValue) {
                    Model.ChangeToolbarPosition(o.Value);
                } else {
                    Logger.LogTrace("こないはず");
                }
            },
            o => o.HasValue && o.Value != ToolbarPosition
        ));

        public ICommand SwitchTopmostCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.ChangeTopmost(!Model.IsTopmost);
            }
        ));

        public ICommand SwitchAutoHideCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.ChangeAutoHide(!Model.IsAutoHide);
            }
        ));

        public ICommand CloseCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 Model.ChangeVisible(false);

                 //var notification = new Notification();
                 //CloseRequest.Raise(notification);
                 CloseRequest.Send();
             }
        ));

        public ICommand ChangeLauncherGroupCommand => GetOrCreateCommand(() => new DelegateCommand<LauncherGroupViewModel>(
           o => {
               var groupModel = LauncherGroupCollection.GetModel(o);
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
               Model.ChangeLauncherGroup(groupModel);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
               LauncherItems.Refresh();
           }
       ));


        #endregion

        #region function

        DependencyObject CreateToolbarPositionIcon(AppDesktopToolbarPosition toolbarPosition)
        {
            return LauncherToolbarTheme.GetToolbarPositionImage(toolbarPosition, IconBox);
        }

        #region ViewDragAndDrop

        private IResultSuccessValue<DragParameter> ViewGetDragParameter(UIElement sender, MouseEventArgs e) => ResultSuccessValue.Failure<DragParameter>();

        private bool ViewCanDragStart(UIElement sender, MouseEventArgs e) => false;

        private void ViewDragOrverOrEnter(UIElement sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                // ファイル登録準備
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if(filePaths.Length == 1) {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
            }
        }

        private void ViewDrop(UIElement sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if(filePaths.Length == 1) {
                    DispatcherWapper.Begin(() => RegisterDropFile(filePaths[0]));
                    e.Handled = true;
                }
            }
        }

        private void ViewDragLeave(UIElement sender, DragEventArgs e)
        { }

        #endregion

        #region ItemDragAndDrop
        private IResultSuccessValue<DragParameter> ItemGetDragParameter(UIElement sender, MouseEventArgs e) => ResultSuccessValue.Failure<DragParameter>();

        private bool ItemCanDragStart(UIElement sender, MouseEventArgs e) => false;

        private void ItemDragOrverOrEnter(UIElement sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effects = DragDropEffects.Move;
            } else if(e.Data.GetDataPresent(DataFormats.UnicodeText)) {
                e.Effects = DragDropEffects.Move;
            }

            e.Handled = true;
        }

        private void ItemDrop(UIElement sender, DragEventArgs e)
        {
            Guid launcherItemId = Guid.Empty;
            var frameworkElement = (FrameworkElement)sender;
            var launcherContentControl = (LauncherContentControl)frameworkElement.DataContext;
            if(launcherContentControl != null) {
                var launcherItem = (ILauncherItemId)launcherContentControl.DataContext;
                launcherItemId = launcherItem.LauncherItemId;
            }

            if(Guid.Empty == launcherItemId) {
                Logger.LogError("ランチャーアイテムID取得できず, {0}, {1}", sender, e);
                return;
            }

            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                var argument = string.Join(' ', filePaths.Select(i => CommandLine.Escape(i)));
                DispatcherWapper.Begin(() => ExecuteExtendDropData(launcherItemId, argument));
            } else if(e.Data.GetDataPresent(DataFormats.UnicodeText)) {
                var argument = (string)e.Data.GetData(DataFormats.UnicodeText);
                DispatcherWapper.Begin(() => ExecuteExtendDropData(launcherItemId, argument));
            }

            e.Handled = true;
        }

        private void ItemDragLeave(UIElement sender, DragEventArgs e)
        { }

        #endregion


        void RegisterDropFile(string path)
        {
            if(PathUtility.IsShortcut(path)) {
                var request = new CommonMessageDialogRequestParameter() {
                    Message = "d&d file is lnk",
                    Caption = "reg type",
                    Button = MessageBoxButton.YesNoCancel,
                    DefaultResult = MessageBoxResult.Yes,
                    Icon = MessageBoxImage.Question,
                };
                ExpandShortcutFileRequest.Send<YesNoResponse>(request, r => {
                    if(r.ResponseIsCancel) {
                        Logger.LogTrace("cancel");
                        return;
                    }
                    Model.RegisterFile(path, r.ResponseIsYes);
                });
            } else {
                Model.RegisterFile(path, false);
            }
        }

        void ExecuteExtendDropData(Guid launcherItemId, string argument)
        {
            Model.OpenExtendsExecuteView(launcherItemId, argument, DockScreen);
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
        public TimeSpan AutoHideTimeout => Model.AutoHideTimeout;

        /// <summary>
        /// 表示中のサイズ。
        /// <para><see cref="AppDesktopToolbarPosition"/>の各辺に対応</para>
        /// </summary>
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
        public Screen DockScreen => Model.DockScreen;

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

        public void ReceiveViewUserClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }


        public void ReceiveViewClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed()
        {
            Model.ReceiveViewClosed();
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
                    LauncherItemCollection.Dispose();
                    LauncherGroupCollection.Dispose();
                }

            }
            base.Dispose(disposing);
        }

        #endregion

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }
    }
}
