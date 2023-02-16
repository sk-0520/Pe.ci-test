using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.Font;
using ContentTypeTextNet.Pe.Standard.Base;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Command
{
    public class CommandViewModel: ElementViewModelBase<CommandElement>, IViewLifecycleReceiver
    {
        #region variable

        private bool _isOpend;
        private CommandItemViewModel? _currentSelectedItem;
        private CommandItemViewModel? _selectedItem;
        private string _inputValue = string.Empty;
        private InputState _inputState;
        //bool _isActive;
        private List<CommandItemViewModel> _commandItems = new List<CommandItemViewModel>();

        #endregion

        public CommandViewModel(CommandElement model, IGeneralTheme generalTheme, ICommandTheme commandTheme, IPlatformTheme platformTheme, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            GeneralTheme = generalTheme;
            CommandTheme = commandTheme;
            PlatformTheme = platformTheme;

            ThemeProperties = new ThemeProperties(this);

            //CommandItemCollection = new ActionModelViewModelObservableCollectionManager<WrapModel<ICommandItem>, CommandItemViewModel>(Model.CommandItems) {
            //    ToViewModel = m => new CommandItemViewModel(m.Data, IconBox, DispatcherWrapper, LoggerFactory),
            //};
            //CommandItems = CommandItemCollection.GetDefaultView();

            Font = new FontViewModel(Model.Font!, DispatcherWrapper, LoggerFactory);

            HideWaitTimer = new DispatcherTimer(DispatcherPriority.Normal) {
                Interval = Model.HideWaitTime,
            };
            HideWaitTimer.Tick += HideWaitTimer_Tick;

            PlatformTheme.Changed += PlatformTheme_Changed;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
            //PropertyChangedHooker.AddHook(nameof(Model.CommandItems), BuildCommandItems);
        }

        #region property
        public RequestSender ScrollSelectedItemRequest { get; } = new RequestSender();
        public RequestSender FocusEndRequest { get; } = new RequestSender();

        private IGeneralTheme GeneralTheme { get; }
        private ICommandTheme CommandTheme { get; }
        private IPlatformTheme PlatformTheme { get; }

        private DispatcherTimer HideWaitTimer { get; }

        //ModelViewModelObservableCollectionManagerBase<WrapModel<ICommandItem>, CommandItemViewModel> CommandItemCollection { get; }
        public IReadOnlyList<CommandItemViewModel> CommandItems
        {
            get => this._commandItems;
            private set => SetProperty(ref this._commandItems, (List<CommandItemViewModel>)value);
        }

        public CommandItemViewModel? CurrentSelectedItem
        {
            get => this._currentSelectedItem;
            set => SetProperty(ref this._currentSelectedItem, value);
        }

        public CommandItemViewModel? SelectedItem
        {
            get => this._selectedItem;
            set
            {
                SetProperty(ref this._selectedItem, value);
                if(SelectedItem != null) {
                    CurrentSelectedItem = SelectedItem;
                }
            }
        }

        private ThemeProperties ThemeProperties { get; }
        private PropertyChangedHooker PropertyChangedHooker { get; }

        private IDpiScaleOutputor DpiScaleOutputor { get; set; } = new EmptyDpiScaleOutputor();

        public double WindowWidth
        {
            get => Model.Width;
            set => Model.ChangeViewWidthDelaySave(value);
        }

        public bool IsOpend
        {
            get => this._isOpend;
            set => SetProperty(ref this._isOpend, value);
        }

        public IconBox IconBox => Model.IconBox;

        private CancellationTokenSource? InputCancellationTokenSource { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields")]
        public string InputValue
        {
            get => this._inputValue;
            set
            {
                ChangeInutValueAsync(value).ConfigureAwait(false);
            }
        }

        private async Task ChangeInutValueAsync(string value)
        {
#if DEBUG
            DispatcherWrapper.VerifyAccess();
#endif
            var prevSelectedItem = CurrentSelectedItem = SelectedItem;

            //SetProperty(ref this._inputValue, value);
            this._inputValue = value;

            var prevInputCancellationTokenSource = InputCancellationTokenSource;
            if(prevInputCancellationTokenSource != null) {
                Logger.LogDebug("入力中の何かしらをキャンセル");
                prevInputCancellationTokenSource?.Cancel();
            }

            InputCancellationTokenSource = new CancellationTokenSource();

            var isEmpty = string.IsNullOrWhiteSpace(this._inputValue);
            if(isEmpty) {
                InputState = InputState.Empty;
            } else {
                InputState = InputState.Finding;
            }

            try {
#if DEBUG
                DispatcherWrapper.VerifyAccess();
#endif

                var commandItems = await Model.EnumerateCommandItemsAsync(this._inputValue, InputCancellationTokenSource.Token);
                InputCancellationTokenSource?.Dispose();
                InputCancellationTokenSource = null;
#if DEBUG
                DispatcherWrapper.VerifyAccess();
#endif

                SetCommandItems(commandItems);
                //SelectedItem = CommandItems.FirstOrDefault();
                var selectedItem = prevSelectedItem == null
                    ? CommandItems.FirstOrDefault()
                    : CommandItems.FirstOrDefault(i => prevSelectedItem.IsEquals(i))
                ;
                if(selectedItem == null || 0 < CommandItems.Count) {
                    SelectedItem = CommandItems.First();
                } else {
                    SelectedItem = selectedItem;
                }

                if(SelectedItem == null) {
                    CurrentSelectedItem = null;
                    InputState = InputState.NotFound;
                } else if(!string.IsNullOrWhiteSpace(InputValue)) {
                    InputState = InputState.Complete;
                }
                ScrollSelectedItemRequest.Send();

            } catch(OperationCanceledException ex) {
                Logger.LogDebug(ex, "入力処理はキャンセルされた");
            }
        }

        public FontViewModel Font { get; private set; }

        public InputState InputState
        {
            get => this._inputState;
            set => SetProperty(ref this._inputState, value);
        }

        #region theme

        [ThemeProperty]
        public Thickness ViewBorderThickness => CommandTheme.GetViewBorderThickness();
        [ThemeProperty]
        public Thickness ViewResizeThickness
        {
            get
            {
                var thickness = CommandTheme.GetViewBorderThickness();
                thickness.Top = 0;
                thickness.Bottom = 0;
                return thickness;
            }
            [Unused(UnusedKinds.TwoWayBinding)]
            set { }
        }
        [ThemeProperty]
        public Brush ViewActiveBorderBrush => CommandTheme.GetViewBorderBrush(true);
        [ThemeProperty]
        public Brush ViewInactiveBorderBrush => CommandTheme.GetViewBorderBrush(false);

        [ThemeProperty]
        public Brush ViewActiveBackgroundBrush => CommandTheme.GetViewBackgroundBrush(true);
        [ThemeProperty]
        public Brush ViewInactiveBackgroundBrush => CommandTheme.GetViewBackgroundBrush(false);

        [ThemeProperty]
        public double GripWidth => CommandTheme.GetGripWidth();
        [ThemeProperty]
        public Brush GripActiveBrush => CommandTheme.GetGripBrush(true);
        [ThemeProperty]
        public Brush GripInactiveBrush => CommandTheme.GetGripBrush(false);

        [ThemeProperty]
        public Thickness SelectedIconMargin => CommandTheme.GetSelectedIconMargin(new IconScale(IconBox, IconSize.DefaultScale));

        [ThemeProperty]
        public Thickness InputBorderThickness => CommandTheme.GetInputBorderThickness();

        [ThemeProperty]
        public Brush InputEmptyBorderBrush => CommandTheme.GetInputBorderBrush(InputState.Empty);
        [ThemeProperty]
        public Brush InputFindingBorderBrush => CommandTheme.GetInputBorderBrush(InputState.Finding);
        [ThemeProperty]
        public Brush InputCompleteBorderBrush => CommandTheme.GetInputBorderBrush(InputState.Complete);
        [ThemeProperty]
        public Brush InputNotFoundBorderBrush => CommandTheme.GetInputBorderBrush(InputState.NotFound);

        [ThemeProperty]
        public Brush InputEmptyBackground => CommandTheme.GetInputBackground(InputState.Empty);
        [ThemeProperty]
        public Brush InputFindingBackground => CommandTheme.GetInputBackground(InputState.Finding);
        [ThemeProperty]
        public Brush InputCompleteBackground => CommandTheme.GetInputBackground(InputState.Complete);
        [ThemeProperty]
        public Brush InputNotFoundBackground => CommandTheme.GetInputBackground(InputState.NotFound);

        [ThemeProperty]
        public Brush InputEmptyForeground => CommandTheme.GetInputForeground(InputState.Empty);
        [ThemeProperty]
        public Brush InputFindingForeground => CommandTheme.GetInputForeground(InputState.Finding);
        [ThemeProperty]
        public Brush InputCompleteForeground => CommandTheme.GetInputForeground(InputState.Complete);
        [ThemeProperty]
        public Brush InputNotFoundForeground => CommandTheme.GetInputForeground(InputState.NotFound);

        [ThemeProperty]
        public ControlTemplate ExecuteButtonControlTemplate => CommandTheme.GetExecuteButtonControlTemplate(new IconScale(IconBox, IconSize.DefaultScale));

        #endregion

        #endregion

        #region command

        public ICommand ExecuteCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Logger.LogInformation("コマンドアイテムの起動: {0}", SelectedItem!.Header);
                SelectedItem.Execute(DpiScaleOutputor.GetOwnerScreen());

                // 役目は終わったのでコマンドランチャーを閉じる
                HideView();
            },
            () => SelectedItem != null
        ).ObservesProperty(() => SelectedItem));

        public ICommand HideCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                HideView();
            }
        ));

        public ICommand UpSelectItemCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                UpDownSelectItem(true);
            }
        ));

        public ICommand DownSelectItemCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                UpDownSelectItem(false);
            }
        ));

        public ICommand EnterSelectedItemCommand => GetOrCreateCommand(() => new DelegateCommand(
            async () => {
                if(SelectedItem == null) {
                    return;
                }

                Logger.LogTrace("補完！");
                var now = SelectedItem;

                await ChangeInutValueAsync(now.FullMatchValue);

                RaisePropertyChanged(nameof(InputValue));

                // 識別できんから無理だわ
                //SelectedItem = now;

                FocusEndRequest.Send();
                ScrollSelectedItemRequest.Send();
            }
        ));

        public ICommand ViewActivatedCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                HideWaitTimer.Stop();
            }
        ));
        public ICommand ViewDeactivatedCommand => GetOrCreateCommand(() => new DelegateCommand<Window>(
            o => {
                if(o.IsVisible) {
                    HideWaitTimer.Stop();
                    HideWaitTimer.Start();
                }
            }
        ));

        public ICommand ViewIsVisibleChangedCommand => GetOrCreateCommand(() => new DelegateCommand<Window>(
             o => {
                 if(o.IsVisible) {
                     InputValue = string.Empty;
                     RaisePropertyChanged(nameof(InputValue));
                 }
             }
         ));


        #endregion

        #region function

        private void UpDownSelectItem(bool isUp)
        {
            if(CommandItems.Count == 0) {
                Logger.LogTrace("列挙アイテムなし");
                return;
            }

            if(SelectedItem == null) {
                // 多分ここには来ないはずだけど一応
                SelectedItem = isUp
                    ? CommandItems.First()
                    : CommandItems.Last()
                ;
            } else {
                var index = this._commandItems.IndexOf(SelectedItem);
                if(isUp) {
                    SelectedItem = index == 0
                        ? CommandItems.Last()
                        : CommandItems[index - 1]
                    ;
                } else {
                    SelectedItem = index == CommandItems.Count - 1
                        ? CommandItems[0]
                        : CommandItems[index + 1]
                    ;
                }
            }

            ScrollSelectedItemRequest.Send();
        }

        private void HideView()
        {
            if(!IsDisposed) {
                Model.HideView(false);
                SetCommandItems(new List<ICommandItem>());
            }
        }

        private void SetCommandItems(IReadOnlyList<ICommandItem> commandItems)
        {
            var prevItems = CommandItems;
            CommandItems = commandItems
                .Select(i => new CommandItemViewModel(i, new IconScale(IconBox, DpiScaleOutputor.GetDpiScale()), DispatcherWrapper, LoggerFactory))
                .ToList()
            ;
            foreach(var item in prevItems) {
                item.Dispose();
            }
        }

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
            DpiScaleOutputor = (IDpiScaleOutputor)window;
        }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
            HideView();
        }

        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed(Window window, bool isUserOperation)
        {
            Model.ReceiveViewClosed(isUserOperation);
            HideWaitTimer.Stop();
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
                PlatformTheme.Changed -= PlatformTheme_Changed;

                if(disposing) {
                    PropertyChangedHooker.Dispose();
                    //CommandItems.Dispose();
                    Font.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void PlatformTheme_Changed(object? sender, EventArgs e)
        {
            DispatcherWrapper.BeginAsync(vm => {
                if(vm.IsDisposed) {
                    return;
                }

                foreach(var themePropertyName in vm.ThemeProperties.GetPropertyNames()) {
                    vm.RaisePropertyChanged(themePropertyName);
                }
            }, this, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }

        private void HideWaitTimer_Tick(object? sender, EventArgs e)
        {
            HideWaitTimer.Stop();
            HideView();
        }
    }
}
