using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.ViewModels;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;
using System.Windows.Data;
using Microsoft.Web.WebView2.Wpf;
using System.Threading;

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
        /// 情報。
        /// </summary>
        About,
        /// <summary>
        /// リリース情報。
        /// </summary>
        Release,
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
        /// <summary>
        /// 設定。
        /// </summary>
        Setting,
        /// <summary>
        /// フィードバック。
        /// </summary>
        Feedback,
        /// <summary>
        /// 通知ログ。
        /// </summary>
        NotifyLog,
        /// <summary>
        /// プラグインインストーラ(Web)。
        /// </summary>
        PluginWebInstaller,

        /// <summary>
        /// プラグイン ウィジェット。
        /// </summary>
        Widget,
        /// <summary>
        /// プラグイン ランチャーアイテム拡張。
        /// </summary>
        LauncherItemExtension
    }

    public class WindowItem
    {
        /// <summary>
        /// View と Model の間に真っ当な ViewModel が噛んでいる関係を生成。
        /// </summary>
        /// <remarks>
        /// <para>Pe 純正の子たち。</para>
        /// </remarks>
        /// <param name="windowKind"></param>
        /// <param name="model"></param>
        /// <param name="window"></param>
        internal WindowItem(WindowKind windowKind, ElementBase model, Window window)
        {
            WindowKind = windowKind;
            Element = model;
            Window = window;
            ViewModel = (ViewModelBase)Window.DataContext;
        }

        /// <summary>
        /// View と Model のとは関係のない ViewModel が噛んでいる関係を生成。
        /// </summary>
        /// <remarks>
        /// <para>プラグイン系の妾の子たち。</para>
        /// </remarks>
        /// <param name="windowKind"></param>
        /// <param name="model"></param>
        /// <param name="viewModel"></param>
        /// <param name="window"></param>
        internal WindowItem(WindowKind windowKind, ElementBase model, ViewModelBase viewModel, Window window)
        {
            WindowKind = windowKind;
            Element = model;
            Window = window;
            ViewModel = viewModel;
        }

        #region property

        public WindowKind WindowKind { get; }
        public ViewModelBase ViewModel { get; }
        internal ElementBase Element { get; }
        public Window Window { get; }

        /// <summary>
        /// ウィンドウが閉じられた際に <see cref="FrameworkElement.DataContext"/> およびバインド状態をクリアするか。
        /// </summary>
        public bool CloseToClearDataContext { get; set; } = true;
        /// <summary>
        /// ウィンドウが閉じられた際に <see cref="ViewModel"/> の <see cref="IDisposable.Dispose"/> を呼び出すか。
        /// </summary>
        public bool CloseToDispose { get; set; } = true;

        /// <summary>
        /// ウィンドウが開かれたか。
        /// </summary>
        public bool IsOpened { get; internal set; }
        /// <summary>
        /// ウィンドウが閉じられたか。
        /// </summary>
        public bool IsClosed { get; internal set; }

        /// <summary>
        /// ユーザー操作によってウィンドウが閉じられたか。
        /// </summary>
        internal bool IsUserClosed { get; set; }

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

    public class WindowManager: ManagerBase, IWindowManager
    {
        public WindowManager(IDiContainer diContainer, ICultureService cultureService, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            CultureService = cultureService;
            ApplicationConfiguration = DiContainer.Build<ApplicationConfiguration>();
        }

        #region property

        private ICultureService CultureService { get; }
        private ApplicationConfiguration ApplicationConfiguration { get; }

        private ISet<WindowItem> Items { get; } = new HashSet<WindowItem>();
        private ISet<Window> Windows { get; } = new HashSet<Window>();
        private IDictionary<WindowItem, HwndSource> WindowHandleSources { get; } = new Dictionary<WindowItem, HwndSource>();

        #endregion

        #region function

        /// <summary>
        /// <see cref="Window.DataContext"/>に null 入れた際に死ぬやつを事前に調整。
        /// </summary>
        /// <param name="window"></param>
        private void ClearUnsafeElements(Window window)
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

            if(ApplicationConfiguration.Web.DeveloperTools) {
                var hasWebView = UIUtility.FindChildren<WebView2>(item.Window).Any();
                if(hasWebView) {
                    item.Window.PreviewKeyDown += Window_DeveloperTools_KeyDown;
                }
            }

            item.Window.Language = CultureService.GetXmlLanguage();
            item.Window.SourceInitialized += Window_SourceInitialized!;
            item.Window.Loaded += Window_Loaded;
            item.Window.Closing += Window_Closing;
            item.Window.Closed += Window_Closed!;

            return true;
        }

        public IEnumerable<WindowItem> GetWindowItems(WindowKind kind)
        {
            return Items.Where(i => i.WindowKind == kind);
        }

        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch(msg) {
                case (int)WM.WM_SYSCOMMAND: {
                        if(WindowsUtility.ConvertSCFromWParam(wParam) == SC.SC_CLOSE) {
                            var e = new CancelEventArgs(false);

                            var item = Items.First(i => HandleUtility.GetWindowHandle(i.Window) == hWnd);

                            Logger.LogDebug("ウィンドウ破棄前(ユーザー操作): {0}, {1:x16}", item.Window, hWnd.ToInt64());
                            if(item.ViewModel is IViewLifecycleReceiver viewLifecycleReceiver) {
                                viewLifecycleReceiver.ReceiveViewUserClosing(item.Window, e);
                            }

                            item.IsUserClosed = !e.Cancel;

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

            if(item.ViewModel.IsDisposed || item.Element.IsDisposed) {
                Logger.LogWarning(
                    "ウィンドウに紐づく" + nameof(window.DataContext) + ", Model がすでに終了しているためウィンドウ破棄: {0} - {1}." + nameof(item.ViewModel.IsDisposed) + "={2} - {3}." + nameof(item.Element.IsDisposed) + "={4}",
                    window,
                    item.ViewModel,
                    item.ViewModel.IsDisposed,
                    item.Element,
                    item.Element.IsDisposed
                );
                item.Window.Loaded -= Window_Loaded;
                item.Window.Closing -= Window_Closing;
                item.Window.Closed -= Window_Closed!;
                item.Window.Close();

                if(item.CloseToClearDataContext) {
                    ClearUnsafeElements(item.Window);
                    //ClearBindings いる？
                    item.Window.DataContext = null;
                }

                if(!item.ViewModel.IsDisposed && item.CloseToDispose) {
                    item.ViewModel.Dispose();
                }
                // item.Element の生死はノータッチ
                Items.Remove(item);
                return;
            }


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
            item.IsOpened = true;
            if(item.ViewModel is IViewLifecycleReceiver viewLifecycleReceiver) {
                viewLifecycleReceiver.ReceiveViewLoaded(window);
            }
        }

        private void Window_Closing(object? sender, CancelEventArgs e)
        {
            var window = (Window?)sender;
            Logger.LogDebug("ウィンドウ破棄前: {0}", window);

            var item = Items.First(i => i.Window == window);
            if(item.ViewModel is IViewLifecycleReceiver viewLifecycleReceiver) {
                viewLifecycleReceiver.ReceiveViewClosing(item.Window, e);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "<保留中>")]
        private async void Window_Closed(object sender, EventArgs e)
        {
            var window = (Window)sender;
            Logger.LogDebug("ウィンドウ破棄: {0}", window);

            window.Loaded -= Window_Loaded;
            window.SourceInitialized -= Window_SourceInitialized!;
            window.Closing -= Window_Closing;
            window.Closed -= Window_Closed!;
            window.PreviewKeyDown -= Window_DeveloperTools_KeyDown;

            Windows.Remove(window);

            var item = Items.First(i => i.Window == window);
            item.IsClosed = true;
            Items.Remove(item);

            if(WindowHandleSources.TryGetValue(item, out var hWndSource)) {
                WindowHandleSources.Remove(item);
                hWndSource.Dispose();
            }

            if(item.CloseToClearDataContext) {
                ClearUnsafeElements(item.Window);

                static void ClearBindings(DependencyObject dependencyObject)
                {
                    var children = LogicalTreeHelper
                        .GetChildren(dependencyObject)
                        .OfType<DependencyObject>()
                        .ToArray()
                    ;
                    foreach(var child in children) {
                        ClearBindings(child);
                    }

                    BindingOperations.ClearAllBindings(dependencyObject);
                }
                ClearBindings(item.Window);

                item.Window.DataContext = null;
                var dataContextChildren = UIUtility.FindChildren<FrameworkElement>(item.Window)
                    .Where(i => i.DataContext != null)
                    .ToList()
                ;
                foreach(var child in dataContextChildren) {
                    child.DataContext = null;
                }
            }
            if(item.ViewModel is IViewLifecycleReceiver viewLifecycleReceiver) {
                await viewLifecycleReceiver.ReceiveViewClosedAsync(item.Window, item.IsUserClosed, CancellationToken.None);
            }

            if(item.CloseToDispose) {
                item.ViewModel.Dispose();
            }
        }

        private void Window_DeveloperTools_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var dev = e.Key == Key.F12;
            if(!dev && e.Key == Key.I) {
                var mods = ModifierKeys.Control | ModifierKeys.Shift;
                dev = (Keyboard.Modifiers & mods) == mods;
            }

            if(dev) {
                var window = (Window)sender;
                var webView = UIUtility.FindChildren<WebView2>(window).FirstOrDefault();
                if(webView is not null) {
                    webView.CoreWebView2.OpenDevToolsWindow();
                }
            }
        }
    }
}
