using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ICSharpCode.AvalonEdit.CodeCompletion;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Views.NotifyLog
{
    /// <summary>
    /// NotifyLogWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class NotifyLogWindow : Window, IDpiScaleOutputor
    {
        public NotifyLogWindow()
        {
            InitializeComponent();
        }

        #region property

        CommandStore CommandStore { get; } = new CommandStore();
        [Inject]
        ILogger? Logger { get; set; }


        #endregion

        #region function
        #endregion

        #region Window

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            UIUtility.SetToolWindowStyle(this, false, false);
            //UIUtility.ChangeTransparent(this, true);

            // アクティブなるのを抑制
            var hWnd = HandleUtility.GetWindowHandle(this);
            var exStyle = NativeMethods.GetWindowLong(hWnd, (int)GWL.GWL_EXSTYLE);
            NativeMethods.SetWindowLong(hWnd, (int)GWL.GWL_EXSTYLE, exStyle | (int)WS_EX.WS_EX_NOACTIVATE);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            var baseWdinwHandle = HandleUtility.GetWindowHandle(this);
            var hWnd = baseWdinwHandle;
            do {
                hWnd = NativeMethods.GetWindow(hWnd, GW.GW_HWNDNEXT);
                if(baseWdinwHandle == hWnd) {
                    Logger.LogWarning("次のウィンドウ取得できず");
                    return;
                }
            } while(!NativeMethods.IsWindowVisible(hWnd));

            Logger.LogDebug("アクティブウィンドウ移譲: {0} -> {1}", baseWdinwHandle, hWnd);
            NativeMethods.SetActiveWindow(hWnd);
        }

        #endregion

        #region IDpiScaleOutputor

        public Point GetDpiScale() => UIUtility.GetDpiScale(this);
        public IScreen GetOwnerScreen() => Screen.FromHandle(HandleUtility.GetWindowHandle(this));

        #endregion


    }
}
