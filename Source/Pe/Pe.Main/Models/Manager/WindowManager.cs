using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.ViewModels;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public enum WindowKind
    {
        Debug,
        /// <summary>
        /// 使用許諾。
        /// </summary>
        Accept,
        /// <summary>
        /// スタートアップ。
        /// </summary>
        Startup,
        /// <summary>
        /// プログラム取り込み。
        /// </summary>
        ImportPrograms,
        /// <summary>
        /// ランチャーツールバー。
        /// </summary>
        LauncherToolbar,
        /// <summary>
        /// 指定して実行。
        /// </summary>
        ExtendsExecute,
        /// <summary>
        /// ランチャーアイテム編集。
        /// </summary>
        LauncherCustomize,
        /// <summary>
        /// 標準入出力。
        /// </summary>
        StandardInputOutput,
        /// <summary>
        /// コマンド。
        /// </summary>
        Command,
        /// <summary>
        /// ノート。
        /// </summary>
        Note,
        /// <summary>
        /// なんだこれ。。。
        /// </summary>
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
        /// <summary>
        /// ウィンドウが閉じられた際に <see cref="ViewModel"/> の <see cref="IDisposable.Dispose"/> を呼び出すか。
        /// </summary>
        public bool CloseToDispose { get; set; } = true;

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

        void Flash(WindowItem windowItem);

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

        /// <summary>
        /// <see cref="Window.DataContext"/>に null 入れた際に死ぬやつを事前に調整。
        /// </summary>
        /// <param name="window"></param>
        void ClearUnsafeElements(Window window)
        {
            var editors = UIUtility.FindChildren<ICSharpCode.AvalonEdit.TextEditor>(window);
            foreach(var editor in editors) {
                editor.Options = new ICSharpCode.AvalonEdit.TextEditorOptions();
            }
        }

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
                item.Window.SourceInitialized += Window_SourceInitialized!;
                item.Window.Loaded += Window_Loaded;
                item.Window.Closing += Window_Closing;
            }
            item.Window.Closed += Window_Closed!;

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

                            Logger.LogDebug("ウィンドウ破棄前(ユーザー操作): {0}, {1:x16}", item.Window, hwnd.ToInt64());
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

        public void Flash(WindowItem windowItem)
        {
            var hWnd = HandleUtility.GetWindowHandle(windowItem.Window);
            var flashInfo = new FLASHWINFO() {
                cbSize = (uint)Marshal.SizeOf(typeof(FLASHWINFO)),
                hwnd = hWnd,
                dwFlags = FLASHW.FLASHW_ALL,
                uCount = 3,
                dwTimeout = 0, // (uint)TimeSpan.FromSeconds(5).TotalMilliseconds,
            };
            NativeMethods.FlashWindowEx(ref flashInfo);
            NativeMethods.SetForegroundWindow(hWnd);
        }


        #endregion

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            var window = (Window)sender;
            Logger.LogDebug("ウィンドウハンドル生成: {0}", window);

            window.SourceInitialized -= Window_SourceInitialized!;

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
            Logger.LogDebug("ウィンドウ生成完了: {0}", window);

            window.Loaded -= Window_Loaded;

            var item = Items.First(i => i.Window == window);
            if(item.ViewModel is IViewLifecycleReceiver viewLifecycleReceiver) {
                viewLifecycleReceiver.ReceiveViewLoaded(window);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var window = (Window)sender;
            Logger.LogDebug("ウィンドウ破棄前: {0}", window);

            var item = Items.First(i => i.Window == window);
            if(item.ViewModel is IViewLifecycleReceiver viewLifecycleReceiver) {
                viewLifecycleReceiver.ReceiveViewClosing(e);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var window = (Window)sender;
            Logger.LogDebug("ウィンドウ破棄: {0}", window);

            window.Closing -= Window_Closing;
            window.Closed -= Window_Closed!;

            Windows.Remove(window);

            var item = Items.First(i => i.Window == window);
            Items.Remove(item);

            if(WindowHandleSources.TryGetValue(item, out var hWndSource)) {
                WindowHandleSources.Remove(item);
                hWndSource.Dispose();
            }

            if(item.CloseToDataContextNull) {
                ClearUnsafeElements(item.Window);
                item.Window.DataContext = null;
            }
            if(item.ViewModel is IViewLifecycleReceiver viewLifecycleReceiver) {
                viewLifecycleReceiver.ReceiveViewClosed();
            }

            if(item.CloseToDispose) {
                item.ViewModel.Dispose();
            }
        }

    }
}
