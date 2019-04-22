using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element;
using ContentTypeTextNet.Pe.Main.ViewModel;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
{
    public enum WindowKind
    {
        Accept,
        Startup,
        ImportPrograms,
        LauncherToolbar,
        LauncherExecute,
        LauncherCustomize,
        Command,
        Note,
        Screen,
    }

    public class WindowItem
    {
        public WindowItem(WindowKind windowKind, Window window)
        {
            WindowKind = windowKind;
            Window = window;
            ViewModel = (ViewModelBase)Window.DataContext;
        }

        #region property

        public WindowKind WindowKind { get; }
        public ViewModelBase ViewModel { get; }
        public Window Window { get; }

        /// <summary>
        /// ウィンドウが閉じられた際に <see cref="System.Windows.Window.DataContext"/> に null を設定するか。
        /// </summary>
        public bool CloseToDataContextNull { get; set; } = true;

        #endregion
    }

    public interface IWindowManager
    {
        #region function

        /// <summary>
        /// ウィンドウアイテムを登録。
        /// </summary>
        /// <param name="item"></param>
        /// <returns>登録の成功・失敗。</returns>
        bool Register(WindowItem item);

        /// <summary>
        /// ウィンドウアイテムを取得。
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        IEnumerable<WindowItem> GetWindowItems(WindowKind kind);

        #endregion
    }

    public class WindowManager : ManagerBase, IWindowManager
    {
        public WindowManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        { }

        #region property

        ISet<WindowItem> Items { get; } = new HashSet<WindowItem>();
        ISet<Window> Windows { get; } = new HashSet<Window>();
        IDictionary<WindowItem, HwndSource> WindowHandleSources { get; } = new Dictionary<WindowItem, HwndSource>();

        #endregion

        #region function
        #endregion

        #region IWindowManager

        public bool Register(WindowItem item)
        {
            if(!Items.Add(item)) {
                return false;
            }

            if(!Windows.Add(item.Window)) {
                return false;
            }

            if(item.ViewModel is IViewLifecycleReceiver viewLifecycleReceiver) {
                item.Window.SourceInitialized += Window_SourceInitialized;
                item.Window.Loaded += Window_Loaded;
                item.Window.Closing += Window_Closing;
            }
            item.Window.Closed += Window_Closed;

            return true;
        }

        public IEnumerable<WindowItem> GetWindowItems(WindowKind kind)
        {
            return Items.Where(i => i.WindowKind == kind);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch(msg) {
                case (int)WM.WM_SYSCOMMAND: {
                        if(WindowsUtility.ConvertSCFromWParam(wParam) == SC.SC_CLOSE) {
                            var e = new CancelEventArgs(false);

                            var item = Items.First(i => HandleUtility.GetWindowHandle(i.Window) == hwnd);

                            Logger.Debug($"ウィンドウ破棄前(ユーザー操作): {item.Window}, {hwnd.ToInt64():x16}");
                            if(item.ViewModel is IViewLifecycleReceiver viewLifecycleReceiver) {
                                viewLifecycleReceiver.ReceiveViewUserClosing(e);
                            }

                            if(e.Cancel) {
                                handled = true;
                            }
                        }
                    }
                    break;

                default:
                    break;
            }

            return IntPtr.Zero;
        }

        #endregion

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            var window = (Window)sender;
            Logger.Debug($"ウィンドウハンドル生成: {window}");

            window.SourceInitialized -= Window_SourceInitialized;

            var item = Items.First(i => i.Window == window);
            if(item.ViewModel is IViewLifecycleReceiver viewLifecycleReceiver) {
                var hWnd = HandleUtility.GetWindowHandle(window);
                var hWndSource = HwndSource.FromHwnd(hWnd);
                hWndSource.AddHook(WndProc);
                WindowHandleSources.Add(item, hWndSource);

                viewLifecycleReceiver.ReceiveViewInitialized(window);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var window = (Window)sender;
            Logger.Debug($"ウィンドウ生成完了: {window}");

            window.Loaded -= Window_Loaded;

            var item = Items.First(i => i.Window == window);
            if(item.ViewModel is IViewLifecycleReceiver viewLifecycleReceiver) {
                viewLifecycleReceiver.ReceiveViewLoaded(window);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var window = (Window)sender;
            Logger.Debug($"ウィンドウ破棄前: {window}");

            var item = Items.First(i => i.Window == window);
            if(item.ViewModel is IViewLifecycleReceiver viewLifecycleReceiver) {
                viewLifecycleReceiver.ReceiveViewClosing(e);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var window = (Window)sender;
            Logger.Debug($"ウィンドウ破棄: {window}");

            window.Closing -= Window_Closing;
            window.Closed -= Window_Closed;

            Windows.Remove(window);

            var item = Items.First(i => i.Window == window);
            Items.Remove(item);

            if(WindowHandleSources.TryGetValue(item, out var hWndSource)) {
                WindowHandleSources.Remove(item);
                hWndSource.Dispose();
            }

            if(item.CloseToDataContextNull) {
                item.Window.DataContext = null;
            }
            if(item.ViewModel is IViewLifecycleReceiver viewLifecycleReceiver) {
                viewLifecycleReceiver.ReceiveViewClosed();
            }
        }

    }
}
