using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Designer;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.Model.Manager;
using ContentTypeTextNet.Pe.Main.View.Extend;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherGroup;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherIcon;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherItem;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherToolbar
{
    public class LauncherToolbarViewModel : SingleModelViewModelBase<LauncherToolbarElement>, IReadOnlyAppDesktopToolbarExtendData, ILoggerFactory, IViewLifecycleReceiver
    {
        #region variable

        LauncherItemViewModelBase _contextMenuOpendItem;

        #endregion

        public LauncherToolbarViewModel(LauncherToolbarElement model, ILauncherToolbarTheme launcherToolbarTheme, IDispatcherWapper dispatcherWapper, ILauncherGroupTheme launcherGroupTheme, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWapper = dispatcherWapper;
            LauncherToolbarTheme = launcherToolbarTheme;
            LauncherGroupTheme = launcherGroupTheme;

            LauncherGroupCollection = new ActionModelViewModelObservableCollectionManager<LauncherGroupElement, LauncherGroupViewModel>(Model.LauncherGroups, Logger.Factory) {
                ToViewModel = (m) => new LauncherGroupViewModel(m, LauncherGroupTheme, Logger.Factory),
            };
            LauncherGroupItems = LauncherGroupCollection.ViewModels;

            LauncherItemCollection = new ActionModelViewModelObservableCollectionManager<LauncherItemElement, LauncherItemViewModelBase>(Model.LauncherItems, Logger.Factory) {
                ToViewModel = (m) => LauncherItemViewModelFactory.Create(m, DispatcherWapper, Logger.Factory),
            };
            LauncherItems = LauncherItemCollection.GetCollectionView();


            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWapper, Logger.Factory);
            PropertyChangedHooker.AddProperties<IReadOnlyAppDesktopToolbarExtendData>();
            PropertyChangedHooker.AddHook(nameof(IAppDesktopToolbarExtendData.ToolbarPosition), nameof(IsVerticalLayout));
            PropertyChangedHooker.AddHook(nameof(IAppDesktopToolbarExtendData.ToolbarPosition), ChangeToolbarPositionCommand);
            PropertyChangedHooker.AddHook(nameof(IAppDesktopToolbarExtendData.IsAutoHide), nameof(IsAutoHide));
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.IsOpendAppMenu), nameof(IsOpendAppMenu));
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.IsTopmost), nameof(IsTopmost));
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.SelectedLauncherGroup), nameof(SelectedLauncherGroup));
        }

        #region property

        public AppDesktopToolbarExtend AppDesktopToolbarExtend { get; set; }
        IDispatcherWapper DispatcherWapper { get; }
        ILauncherToolbarTheme LauncherToolbarTheme { get; }
        ILauncherGroupTheme LauncherGroupTheme { get; }
        PropertyChangedHooker PropertyChangedHooker { get; }

        public IconScale IconScale => Model.IconScale;
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

        public LauncherItemViewModelBase ContextMenuOpendItem
        {
            get => this._contextMenuOpendItem;
            set => SetProperty(ref this._contextMenuOpendItem, value);
        }

        ModelViewModelObservableCollectionManagerBase<LauncherGroupElement, LauncherGroupViewModel> LauncherGroupCollection { get; }
        public ObservableCollection<LauncherGroupViewModel> LauncherGroupItems { get; }

        ModelViewModelObservableCollectionManagerBase<LauncherItemElement, LauncherItemViewModelBase> LauncherItemCollection { get; }
        public ICollectionView LauncherItems { get; }

        public InteractionRequest<Notification> CloseRequest { get; } = new InteractionRequest<Notification>();

        public bool IsVerticalLayout => ToolbarPosition == AppDesktopToolbarPosition.Left || ToolbarPosition == AppDesktopToolbarPosition.Right;

        public LauncherGroupViewModel SelectedLauncherGroup
        {
            get => LauncherGroupCollection.GetViewModel(Model.SelectedLauncherGroup);
        }

        public DependencyObject ToolbarPositionLeftIcon => CreateToolbarPositionIcon(AppDesktopToolbarPosition.Left);
        public DependencyObject ToolbarPositionTopIcon => CreateToolbarPositionIcon(AppDesktopToolbarPosition.Top);
        public DependencyObject ToolbarPositionRightIcon => CreateToolbarPositionIcon(AppDesktopToolbarPosition.Right);
        public DependencyObject ToolbarPositionBottomIcon => CreateToolbarPositionIcon(AppDesktopToolbarPosition.Bottom);

        #endregion

        #region command

        public ICommand ChangeToolbarPositionCommand => GetOrCreateCommand(() => new DelegateCommand<AppDesktopToolbarPosition?>(
            o => {
                Model.ChangeToolbarPosition(o.Value);
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

                 var notification = new Notification();
                 CloseRequest.Raise(notification);
             }
         ));

        public ICommand  ChangeLauncherGroupCommand => GetOrCreateCommand(() => new DelegateCommand<LauncherGroupViewModel>(
            o => {
                var groupModel = LauncherGroupCollection.GetModel(o);
                Model.ChangeLauncherGroup(groupModel);
            }
        ));


        #endregion

        #region function

        DependencyObject CreateToolbarPositionIcon(AppDesktopToolbarPosition toolbarPosition)
        {
            return LauncherToolbarTheme.CreateToolbarPositionImage(toolbarPosition, IconScale.Small);
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

        #region ILoggerFactory

        public ILogger CreateLogger(string header) => Logger.Factory.CreateLogger(header);

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewLoaded(Window window)
        {
            if(!IsVisible) {
                window.Visibility = Visibility.Collapsed;
            }
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

        #endregion

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }

    }
}
