using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.CompatibleWindows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.Note;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Theme;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using ContentTypeTextNet.Pe.Main.ViewModel.Font;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Note
{
    public class NoteViewModel : SingleModelViewModelBase<NoteElement>, IViewLifecycleReceiver
    {
        #region variable

        double _windowLeft;
        double _windowTop;
        double _windowWidth;
        double _windowHeight;

        bool _titleEditMode;
        string _editingTile;

        bool _showContentKindChangeConfim;
        NoteContentKind _changingContentKind;

        #endregion

        public NoteViewModel(NoteElement model, INoteTheme noteTheme, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            NoteTheme = noteTheme;
            DispatcherWapper = dispatcherWapper;

            WindowAreaChangedTimer = new DispatcherTimer() {
                Interval = TimeSpan.FromSeconds(0.5),
            };
            WindowAreaChangedTimer.Tick += WindowAreaChangedTimer_Tick;

            Font = new NoteFontViewModel(Model.FontElement, DispatcherWapper, Logger.Factory);

            PropertyChangedHooker = new PropertyChangedHooker(dispatcherWapper, Logger.Factory);
            PropertyChangedHooker.AddHook(nameof(Model.IsVisible), nameof(IsVisible));
            PropertyChangedHooker.AddHook(nameof(Model.IsTopmost), nameof(IsTopmost));
            PropertyChangedHooker.AddHook(nameof(Model.IsCompact), nameof(IsCompact));
            PropertyChangedHooker.AddHook(nameof(Model.IsLocked), nameof(IsLocked));
            PropertyChangedHooker.AddHook(nameof(Model.TextWrap), nameof(TextWrap));
            PropertyChangedHooker.AddHook(nameof(Model.Title), nameof(Title));
            PropertyChangedHooker.AddHook(nameof(Model.ForegroundColor), () => ApplyTheme());
            PropertyChangedHooker.AddHook(nameof(Model.BackgroundColor), () => ApplyTheme());
        }


        #region property

        public InteractionRequest<Notification> TitleEditStartRequest { get; } = new InteractionRequest<Notification>();
        public InteractionRequest<FileSaveDialogNotification> SelectLinkFileRequest { get; } = new InteractionRequest<FileSaveDialogNotification>();

        bool CanLayoutNotify { get; set; }

        INoteTheme NoteTheme { get; }
        IDispatcherWapper DispatcherWapper { get; }
        PropertyChangedHooker PropertyChangedHooker { get; }

        IDpiScaleOutputor DpiScaleOutputor { get; set; }
        IDisposable WindowHandleSource { get; set; }

        DispatcherTimer WindowAreaChangedTimer { get; }

        public FontViewModel Font { get; }

        public bool IsVisible => Model.IsVisible;

        public bool IsTopmost => Model.IsTopmost;
        public bool IsCompact => Model.IsCompact;
        public bool IsLocked => Model.IsLocked;
        public bool TextWrap => Model.TextWrap;

        public double WindowLeft
        {
            get => this._windowLeft;
            set
            {
                if(SetProperty(ref this._windowLeft, value)) {
                    DelayNotifyWindowAreaChange();
                }
            }
        }
        public double WindowTop
        {
            get => this._windowTop;
            set
            {
                if(SetProperty(ref this._windowTop, value)) {
                    DelayNotifyWindowAreaChange();
                }
            }
        }

        public double WindowWidth
        {
            get => this._windowWidth;
            set
            {
                if(SetProperty(ref this._windowWidth, value)) {
                    DelayNotifyWindowAreaChange();
                }
            }
        }
        double NormalWindowHeight { get; set; }
        public double WindowHeight
        {
            get => this._windowHeight;
            set
            {
                if(SetProperty(ref this._windowHeight, value)) {
                    DelayNotifyWindowAreaChange();
                }
            }
        }

        public bool TitleEditMode
        {
            get => this._titleEditMode;
            set
            {
                if(SetProperty(ref this._titleEditMode, value)) {
                    if(TitleEditMode) {
                        TitleEditStartRequest.Raise(new Notification());
                    }
                }
            }
        }
        public string EditingTitle
        {
            get => this._editingTile;
            set => SetProperty(ref this._editingTile, value);
        }

        public Color ForegroundColor
        {
            get => Model.ForegroundColor;
            set => Model.ChangeForegroundColor(value);
        }
        public Color BackgroundColor
        {
            get => Model.BackgroundColor;
            set => Model.ChangeBackgroundColor(value);
        }

        public string Title => Model.Title;

        public NoteContentKind ContentKind
        {
            get => Model.ContentKind;
            set
            {
                if(!Model.CanChangeContentKind(ContentKind)) {
                    // 単純変換が出来ない場合はあれやこれや頑張る
                    ChangingContentKind = value;
                    CanLoadContentKind = Model.HasContent(ChangingContentKind);
                    ShowContentKindChangeConfim = true;
                } else {
                    Model.ChangeContentKind(value);
                }
            }
        }

        public NoteLayoutKind LayoutKind => Model.LayoutKind;

        #region theme
        public double CaptionHeight => NoteTheme.GetCaptionHeight();
        public Brush BorderBrush => NoteTheme.GetBorderBrush(GetColorPair());
        public Thickness BorderThickness => NoteTheme.GetBorderThickness();
        public Brush CaptionBackgroundNoneBrush => NoteTheme.GetCaptionBackgroundBrush(NoteCaptionButtonState.None, GetColorPair());
        public Brush CaptionBackgroundOverBrush => NoteTheme.GetCaptionBackgroundBrush(NoteCaptionButtonState.Over, GetColorPair());
        public Brush CaptionBackgroundPressedBrush => NoteTheme.GetCaptionBackgroundBrush(NoteCaptionButtonState.Pressed, GetColorPair());
        public Brush CaptionForeground { get; private set; }
        public Brush CaptionBackground { get; private set; }
        public Brush ContentBackground => NoteTheme.GetContentBrush(GetColorPair());
        public DependencyObject ResizeGripImage => NoteTheme.GetResizeGripImage(GetColorPair());

        public DependencyObject CaptionCompactEnabledImage => NoteTheme.GetCaptionImage(NoteCaption.Compact, true, GetColorPair());
        public DependencyObject CaptionCompactDisabledImage => NoteTheme.GetCaptionImage(NoteCaption.Compact, false, GetColorPair());
        public DependencyObject CaptionTopmostEnabledImage => NoteTheme.GetCaptionImage(NoteCaption.Topmost, true, GetColorPair());
        public DependencyObject CaptionTopmostDisabledImage => NoteTheme.GetCaptionImage(NoteCaption.Topmost, false, GetColorPair());

        public DependencyObject CaptionCloseImage => NoteTheme.GetCaptionImage(NoteCaption.Close, false, GetColorPair());
        public double MinHeight => CaptionHeight + BorderThickness.Top + BorderThickness.Bottom;

        #endregion

        #region content kind changing
        public NoteContentKind ChangingContentKind
        {
            get => this._changingContentKind;
            set => SetProperty(ref this._changingContentKind, value);
        }

        public bool ShowContentKindChangeConfim
        {
            get => this._showContentKindChangeConfim;
            set => SetProperty(ref this._showContentKindChangeConfim, value);
        }

        bool CanLoadContentKind { get; set; }

        #endregion


        #endregion

        #region command

        public ICommand SwitchCompactCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                if(!IsCompact) {
                    NormalWindowHeight = WindowHeight;
                }
                Model.SwitchCompact();
                // レイアウト変更(高さ)通知を抑制
                if(!IsCompact) {
                    this._windowHeight = NormalWindowHeight;
                } else {
                    this._windowHeight = 0;
                }
                RaisePropertyChanged(nameof(WindowHeight));
            }
        ));
        public ICommand SwitchTopmostCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.SwitchTopmost();
            }
        ));

        public ICommand SwitchLockCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.SwitchLock();
            }
        ));

        public ICommand SwitchTextWrapCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.SwitchTextWrap();
            }
        ));

        public ICommand CancelTitleEditCommand => GetOrCreateCommand(() => new DelegateCommand<TextBox>(
            o => {
                TitleEditMode = false;
                EditingTitle = string.Empty;
                o.Select(0, 0);
            }
        ));
        public ICommand SaveTitleEditCommand => GetOrCreateCommand(() => new DelegateCommand<TextBox>(
            o => {
                TitleEditMode = false;
                Model.ChangeTitle(EditingTitle);
                o.Select(0, 0);
            },
            o => TitleEditMode
        ));

        public ICommand ContentKindChangeConvertCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ShowContentKindChangeConfim = false;
            }
        ));
        public ICommand ContentKindChangeLoadCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ShowContentKindChangeConfim = false;
            },
            () => CanLoadContentKind
        ));
        public ICommand ContentKindChangeCreateCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ShowContentKindChangeConfim = false;
            }
        ));
        public ICommand ContentKindChangeCancelCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ShowContentKindChangeConfim = false;
            }
        ));
        #endregion

        #region function

        (bool isCreated, NoteLayoutData layout) GetOrCreateLayout(NotePosition position)
        {
            if(position == NotePosition.Setting) {
                var settingLayout = Model.GetLayout();
                if(settingLayout != null) {
                    return (false, settingLayout);
                } else {
                    Logger.Information($"レイアウト未取得のため対象ディスプレイ中央表示: {Model.DockScreen.DeviceName}", ObjectDumper.GetDumpString(Model.DockScreen));
                    position = NotePosition.CenterScreen;
                }
            }

            //TODO: 未検証ゾーン
            var logicalScreenSize = UIUtility.ToLogicalPixel(Model.DockScreen.DeviceBounds.Size, DpiScaleOutputor);
            var layout = new NoteLayoutData() {
                NoteId = Model.NoteId,
                LayoutKind = Model.LayoutKind,
            };

            if(position == NotePosition.CenterScreen) {
                if(layout.LayoutKind == NoteLayoutKind.Absolute) {
                    layout.Width = 200;
                    layout.Height = 200;
                    layout.X = (logicalScreenSize.Width / 2) - (layout.Width / 2);
                    layout.Y = (logicalScreenSize.Height / 2) - (layout.Height / 2);
                } else {
                    Debug.Assert(layout.LayoutKind == NoteLayoutKind.Relative);
                    layout.Width = 20;
                    layout.Height = 20;
                    layout.X = 0;
                    layout.Y = 0;
                }
            } else {
                Debug.Assert(position == NotePosition.CursorPosition);

                var deviceScreenBounds = Model.DockScreen.DeviceBounds;

                NativeMethods.GetCursorPos(out var podPoint);
                var deviceCursorLocation = PodStructUtility.Convert(podPoint);

                var deviceScreenCursorLocation = new Point(
                    deviceCursorLocation.X - deviceScreenBounds.X,
                    deviceCursorLocation.Y - deviceScreenBounds.Y
                );
                var logicalScreenCursorLocation = UIUtility.ToLogicalPixel(deviceScreenCursorLocation, DpiScaleOutputor);

                if(layout.LayoutKind == NoteLayoutKind.Absolute) {
                    layout.Width = 200;
                    layout.Height = 200;
                    layout.X = logicalScreenCursorLocation.X;
                    layout.Y = logicalScreenCursorLocation.Y;
                } else {
                    Debug.Assert(layout.LayoutKind == NoteLayoutKind.Relative);

                    layout.Width = 20;
                    layout.Height = 20;
                    layout.X = deviceScreenCursorLocation.X * (deviceScreenBounds.Width / 100);
                    layout.Y = deviceScreenCursorLocation.Y * (deviceScreenBounds.Height / 100);
                }
            }

            return (true, layout);
        }

        void SetLayout(NoteLayoutData layout)
        {
            WindowLeft = layout.X;
            WindowTop = layout.Y;
            WindowWidth = layout.Width;
            NormalWindowHeight = layout.Height;
            if(IsCompact) {
                WindowHeight = 0;
            } else {
                WindowHeight = NormalWindowHeight;
            }
        }

        void ApplyCaption()
        {
            var pair = NoteTheme.GetCaptionBrush(GetColorPair());
            CaptionForeground = pair.Foreground;
            CaptionBackground = pair.Background;

            var propertyNames = new[] {
                nameof(CaptionForeground),
                nameof(CaptionBackground),
                nameof(CaptionCompactEnabledImage),
                nameof(CaptionCompactDisabledImage),
                nameof(CaptionTopmostEnabledImage),
                nameof(CaptionTopmostDisabledImage),
                nameof(CaptionCloseImage),
            };
            foreach(var propertyName in propertyNames) {
                RaisePropertyChanged(propertyName);
            }
        }

        void ApplyBorder()
        {
            var propertyNames = new[] {
                nameof(BorderBrush),
                nameof(BorderThickness),
                nameof(ResizeGripImage),
            };
            foreach(var propertyName in propertyNames) {
                RaisePropertyChanged(propertyName);
            }
        }

        void ApplyContent()
        {
            var propertyNames = new[] {
                nameof(ContentBackground),
            };
            foreach(var propertyName in propertyNames) {
                RaisePropertyChanged(propertyName);
            }
        }

        void ApplyTheme()
        {
            DispatcherWapper.Begin(() => {
                ApplyCaption();
                ApplyBorder();
                ApplyContent();
            }, DispatcherPriority.Render);
        }

        void DelayNotifyWindowAreaChange()
        {
            if(!CanLayoutNotify) {
                Logger.Trace($"モデルへの位置・サイズ通知抑制 無効: {Model.NoteId}, {WindowAreaChangedTimer.Interval}");
                return;
            }

            if(WindowAreaChangedTimer.IsEnabled) {
                Logger.Trace($"モデルへの位置・サイズ通知抑制: {Model.NoteId}, {WindowAreaChangedTimer.Interval}");
                WindowAreaChangedTimer.Stop();
            }
            WindowAreaChangedTimer.Start();
        }

        void DelayNotifyWindowAreaChanged()
        {
            Logger.Debug($"モデルへの位置・サイズ通知抑制: {Model.NoteId}, {WindowAreaChangedTimer.Interval}");

            var viewAreaChangeTargets = ViewAreaChangeTarget.None;
            var location = new Point();
            var size = new Size();

            if(Model.LayoutKind == NoteLayoutKind.Absolute) {
                viewAreaChangeTargets |= ViewAreaChangeTarget.Location;
                var logicalScreenLocation = UIUtility.ToLogicalPixel(Model.DockScreen.DeviceBounds.Location, DpiScaleOutputor);
                location.X = WindowLeft - logicalScreenLocation.X;
                location.Y = WindowTop - logicalScreenLocation.Y;
            } else {
                Debug.Assert(Model.LayoutKind == NoteLayoutKind.Relative);
            }

            // 最小化中はウィンドウサイズに対して何もしない
            if(!IsCompact) {
                if(Model.LayoutKind == NoteLayoutKind.Absolute) {
                    viewAreaChangeTargets |= ViewAreaChangeTarget.Suze;
                    size.Width = WindowWidth;
                    size.Height = WindowHeight;
                } else {
                    Debug.Assert(Model.LayoutKind == NoteLayoutKind.Relative);
                }
            }

            Model.ChangeViewArea(viewAreaChangeTargets, location, size);
        }

        IReadOnlyColorPair<Color> GetColorPair() => ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor);

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch(msg) {
                case (int)WM.WM_NCLBUTTONDBLCLK:
                    if(WindowsUtility.ConvertHTFromWParam(wParam) == HT.HTCAPTION) {
                        if(!IsLocked) {
                            EditingTitle = Title;
                            TitleEditMode = true;
                        }
                    }
                    break;

                default:
                    break;
            }
            return IntPtr.Zero;
        }

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
            // 各ディスプレイのDPIで事故らないように原点をディスプレイへ移動してウィンドウ位置・サイズをいい感じに頑張る
            var hWnd = HandleUtility.GetWindowHandle(window);
            NativeMethods.SetWindowPos(hWnd, new IntPtr((int)HWND.HWND_TOP), (int)Model.DockScreen.DeviceBounds.X, (int)Model.DockScreen.DeviceBounds.Y, 0, 0, SWP.SWP_NOSIZE);

            // タイトルバーのダブルクリックを拾う必要がある
            var hWndSource = HwndSource.FromHwnd(hWnd);
            hWndSource.AddHook(WndProc);
            WindowHandleSource = hWndSource;

            DpiScaleOutputor = window as IDpiScaleOutputor ?? new EmptyDpiScaleOutputor();

            var layoutValue = GetOrCreateLayout(Model.Position);
            if(layoutValue.isCreated) {
                Model.SaveLayout(layoutValue.layout);
            }

            SetLayout(layoutValue.layout);

            ApplyTheme();

            CanLayoutNotify = true;
        }

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
                    WindowHandleSource?.Dispose();
                    PropertyChangedHooker.Dispose();
                }
                if(WindowAreaChangedTimer.IsEnabled) {
                    WindowAreaChangedTimer.Stop();
                    DelayNotifyWindowAreaChanged();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }

        private void WindowAreaChangedTimer_Tick(object sender, EventArgs e)
        {
            WindowAreaChangedTimer.Stop();
            DelayNotifyWindowAreaChanged();
        }

    }
}
