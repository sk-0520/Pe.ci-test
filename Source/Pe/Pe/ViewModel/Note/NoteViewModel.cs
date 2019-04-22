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

            PropertyChangedHooker = new PropertyChangedHooker(dispatcherWapper, Logger.Factory);
            PropertyChangedHooker.AddHook(nameof(Model.IsVisible), nameof(IsVisible));
            PropertyChangedHooker.AddHook(nameof(Model.IsTopmost), nameof(IsTopmost));
            PropertyChangedHooker.AddHook(nameof(Model.IsCompact), nameof(IsCompact));
            PropertyChangedHooker.AddHook(nameof(Model.IsLocked), nameof(IsLocked));
        }

        #region property

        INoteTheme NoteTheme { get; }

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

        #endregion

        #region command

        public ICommand CloseCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
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

    }
}
