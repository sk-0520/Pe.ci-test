using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.CompatibleWindows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.Note;
using ContentTypeTextNet.Pe.Main.Model.Note;
using ContentTypeTextNet.Pe.Main.Model.Theme;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Note
{
    public class NoteViewModel : SingleModelViewModelBase<NoteElement>, IViewLifecycleReceiver
    {
        #region variable

        double _windowLeft;
        double _windowTop;
        double _windowWidth;
        double _windowHeight;

        #endregion

        public NoteViewModel(NoteElement model, INoteTheme noteTheme, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            NoteTheme = noteTheme;
            DispatcherWapper = dispatcherWapper;

            PropertyChangedHooker = new PropertyChangedHooker(dispatcherWapper, Logger.Factory);
            PropertyChangedHooker.AddHook(nameof(Model.IsVisible), nameof(IsVisible));
            PropertyChangedHooker.AddHook(nameof(Model.IsTopmost), nameof(IsTopmost));
            PropertyChangedHooker.AddHook(nameof(Model.IsCompact), nameof(IsCompact));
            PropertyChangedHooker.AddHook(nameof(Model.IsLocked), nameof(IsLocked));
        }

        #region property

        INoteTheme NoteTheme { get; }
        IDispatcherWapper DispatcherWapper { get; }
        PropertyChangedHooker PropertyChangedHooker { get; }

        public bool IsVisible => Model.IsVisible;

        public bool IsTopmost => Model.IsTopmost;
        public bool IsCompact => Model.IsCompact;
        public bool IsLocked => Model.IsLocked;

        public double WindowLeft
        {
            get => this._windowLeft;
            set => SetProperty(ref this._windowLeft, value);
        }
        public double WindowTop
        {
            get => this._windowTop;
            set => SetProperty(ref this._windowTop, value);
        }
        public double WindowWidth
        {
            get => this._windowWidth;
            set => SetProperty(ref this._windowWidth, value);
        }
        public double WindowHeight
        {
            get => this._windowHeight;
            set => SetProperty(ref this._windowHeight, value);
        }

        public double CaptionHeight => NoteTheme.GetCaptionHeight();
        public Brush BorderBrush => NoteTheme.GetBorderBrush(ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor));
        public Thickness BorderThickness => NoteTheme.GetBorderThickness();
        public Brush CaptionBackgroundNoneBrush => NoteTheme.GetCaptionBackgroundBrush(NoteCaptionButtonState.None, ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor));
        public Brush CaptionBackgroundOverBrush => NoteTheme.GetCaptionBackgroundBrush(NoteCaptionButtonState.Over, ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor));
        public Brush CaptionBackgroundPressedBrush => NoteTheme.GetCaptionBackgroundBrush(NoteCaptionButtonState.Pressed, ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor));
        public Brush CaptionForeground { get; private set; }
        public Brush CaptionBackground { get; private set; }
        public Brush ContentBackground => NoteTheme.GetContentBrush(ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor));
        public DependencyObject ResizeGripImage => NoteTheme.GetResizeGripImage(ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor));

        public DependencyObject CaptionCompactEnabledImage => NoteTheme.GetCaptionImage(NoteCaption.Compact, true, ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor));
        public DependencyObject CaptionCompactDisabledImage => NoteTheme.GetCaptionImage(NoteCaption.Compact, false, ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor));
        public DependencyObject CaptionTopmostEnabledImage => NoteTheme.GetCaptionImage(NoteCaption.Topmost, true, ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor));
        public DependencyObject CaptionTopmostDisabledImage => NoteTheme.GetCaptionImage(NoteCaption.Topmost, false, ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor));

        public DependencyObject CaptionCloseImage => NoteTheme.GetCaptionImage(NoteCaption.Close, false, ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor));
        #endregion

        #region command

        public ICommand SwitchCompactCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.SwitchCompact();
            }
        ));
        public ICommand SwitchTopmostCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.SwitchTopmost();
            }
        ));

        #endregion

        #region function

        (bool isCreated, NoteLayoutData layout) GetOrCreateLayout(NotePosition position, Visual dpiVisual)
        {
            if(position == Main.Model.Note.NotePosition.Setting) {
                var settingLayout = Model.GetLayout();
                if(settingLayout != null) {
                    return (false, settingLayout);
                } else {
                    Logger.Information($"レイアウト未取得のため対象ディスプレイ中央表示: {Model.DockScreen.DeviceName}", ObjectDumper.GetDumpString(Model.DockScreen));
                    position = Main.Model.Note.NotePosition.CenterScreen;
                }
            }

            //TODO: 未検証ゾーン
            var logicalScreenSize = UIUtility.ToLogicalPixel(dpiVisual, Model.DockScreen.DeviceBounds.Size);
            var layout = new NoteLayoutData() {
                NoteId = Model.NoteId,
                LayoutKind = Model.LayoutKind,
            };

            if(position == Main.Model.Note.NotePosition.CenterScreen) {
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
                Debug.Assert(position == Main.Model.Note.NotePosition.CursorPosition);

                var deviceScreenBounds = Model.DockScreen.DeviceBounds;

                NativeMethods.GetCursorPos(out var podPoint);
                var deviceCursorLocation = PodStructUtility.Convert(podPoint);

                var deviceScreenCursorLocation = new Point(
                    deviceCursorLocation.X - deviceScreenBounds.X,
                    deviceCursorLocation.Y - deviceScreenBounds.Y
                );
                var logicalScreenCursorLocation = UIUtility.ToLogicalPixel(dpiVisual, deviceScreenCursorLocation);

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

        void SetLayout(NoteLayoutData layout, Visual dpiVisual)
        {
            WindowLeft = layout.X;
            WindowTop = layout.Y;
            WindowWidth = layout.Width;
            WindowHeight = layout.Height;
        }

        void ApplyCaptionBrush()
        {
            DispatcherWapper.Invoke(() => {
                var pair = NoteTheme.GetCaptionBrush(ColorPair.Create(Model.ForegroundColor, Model.BackgroundColor));
                CaptionForeground = pair.Foreground;
                CaptionBackground = pair.Background;

                RaisePropertyChanged(nameof(CaptionForeground));
                RaisePropertyChanged(nameof(CaptionBackground));
            });
        }

        void ApplyTheme()
        {
            ApplyCaptionBrush();
        }

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
            // 各ディスプレイのDPIで事故らないように原点をディスプレイへ移動してウィンドウ位置・サイズをいい感じに頑張る
            var hWnd = HandleUtility.GetWindowHandle(window);
            NativeMethods.SetWindowPos(hWnd, new IntPtr((int)HWND.HWND_TOP), (int)Model.DockScreen.DeviceBounds.X, (int)Model.DockScreen.DeviceBounds.Y, 0, 0, SWP.SWP_NOSIZE);

            var layoutValue = GetOrCreateLayout(Model.Position, window);
            if(layoutValue.isCreated) {
                Model.SaveLayout(layoutValue.layout);
            }
            SetLayout(layoutValue.layout, window);

            ApplyTheme();
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

        #endregion
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }

    }
}
