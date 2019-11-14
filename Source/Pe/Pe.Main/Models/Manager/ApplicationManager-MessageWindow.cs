using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    partial class ApplicationManager
    {
        #region function

        private void MakeMessageWindow()
        {
            /*
            var thread = new Thread(() => {
                MessageWindowDispatcherWapper = new CurrentDispatcherWapper();
                MessageWindowHandleSource = new HwndSource(new HwndSourceParameters(nameof(MessageWindowHandleSource)) {
                    Width = 0,
                    Height = 0,
                    WindowStyle = (int)WindowStyle.None,
                    //ParentWindow = WindowsUtility.ToIntPtr(HWND.HWND_MESSAGE),
                    HwndSourceHook = MessageWindowProc,
                });
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
            */
            MessageWindowHandleSource = new HwndSource(new HwndSourceParameters(nameof(MessageWindowHandleSource)) {
                Width = 0,
                Height = 0,
                WindowStyle = (int)WindowStyle.None,
                //ParentWindow = WindowsUtility.ToIntPtr(HWND.HWND_MESSAGE),
                HwndSourceHook = MessageWindowProc,
            });
        }

        private IntPtr MessageWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            Logger.LogTrace("[MSG WND] hwnd = {0}, msg = {1}({2}), wParam = {3}, lParam = {4}", hwnd, msg, (WM)msg, wParam, lParam);
            return IntPtr.Zero;
        }

        #endregion
    }
}
