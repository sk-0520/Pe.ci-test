using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        WindowKind WindowKind { get; }
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

            item.Window.Closing += Window_Closing;
            item.Window.Closed += Window_Closed;

            return true;
        }

        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var window = (Window)sender;
            Logger.Debug($"ウィンドウ破棄前: {window}");

            var item = Items.First(i => i.Window == window);
            if(item is IWindowNotify windowNotify) {
                windowNotify.OnClosingView(e);
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

            if(item.CloseToDataContextNull) {
                item.Window.DataContext = null;
            }
            if(item is IWindowNotify windowNotify) {
                windowNotify.OnClosedView();
            }
        }

    }
}
