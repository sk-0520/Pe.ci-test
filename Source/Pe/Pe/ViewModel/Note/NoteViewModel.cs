using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.Note;
using ContentTypeTextNet.Pe.Main.Model.Theme;

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

        public NoteViewModel(NoteElement model, INoteTheme noteTheme, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            NoteTheme = noteTheme;
        }

        #region property

        INoteTheme NoteTheme { get; }

        public bool IsVisible => Model.IsVisible;

        public bool IsTopmost => Model.IsTopmost;
        public bool IsCompact => Model.IsCompact;

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
        #endregion

        #region function
        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        {
            // 各ディスプレイのDPIで事故らないように原点をディスプレイへ移動してウィンドウ位置・サイズをいい感じに頑張る
            var hWnd = HandleUtility.GetWindowHandle(window);
            NativeMethods.SetWindowPos(hWnd, new IntPtr((int)HWND.HWND_TOP), (int)Model.DockScreen.DeviceBounds.X, (int)Model.DockScreen.DeviceBounds.Y, 0, 0, SWP.SWP_NOSIZE);

            var position = Model.Position;
            if(position == Main.Model.Note.NotePosition.Setting) {
                var layout = Model.GetLayout();
                if(layout != null) {
                    return;
                } else {
                    Logger.Information($"レイアウト未取得のため対象ディスプレイ中央表示: {Model.DockScreen.DeviceName}", ObjectDumper.GetDumpString(Model.DockScreen));
                    position = Main.Model.Note.NotePosition.CenterScreen;
                }
            }

            if(position == Main.Model.Note.NotePosition.CenterScreen) {
                var logicalScreenSize = UIUtility.ToLogicalPixel(window, Model.DockScreen.DeviceBounds.Size);
                var rect = new Rect() {
                    Width = 200,
                    Height = 200,
                };
                rect.X = (logicalScreenSize.Width / 2) - (rect.Width / 2);
                rect.Y = (logicalScreenSize.Height / 2) - (rect.Height / 2);

                WindowLeft = rect.X;
                WindowTop = rect.Y;
                WindowWidth = rect.Width;
                WindowHeight = rect.Height;
            }

        }

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

    }
}
