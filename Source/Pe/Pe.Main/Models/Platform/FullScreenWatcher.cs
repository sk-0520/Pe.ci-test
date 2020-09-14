using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    /// <summary>
    /// フルスクリーン状態の検知。
    /// <para>こいつ自身は呼ばれた際にフルスクリーンの確認を行うだけで定周期処理されるわけではない。</para>
    /// </summary>
    public class FullScreenWatcher
    {
        public FullScreenWatcher(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        public int WindowClassNameLength { get; set; } = 128;

        public IReadOnlyCollection<string> IgnoreFullScreenWindowClassNames { get; } = new[] {
            "Shell_TrayWnd",
            "Progman",
        };

        #endregion

        #region function

        /// <summary>
        /// ウィンドウハンドルが指定のディスプレイにおいてフルスクリーンか。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="targetScreen"></param>
        /// <returns></returns>
        public bool IsFullScreen(IntPtr hWnd, IScreen targetScreen)
        {
            if(!NativeMethods.IsWindowVisible(hWnd)) {
                return true;
            }

            if(hWnd == NativeMethods.GetDesktopWindow() || hWnd == NativeMethods.GetShellWindow()) {
                return true;
            }

            var buffer = new StringBuilder(WindowClassNameLength);
            NativeMethods.GetClassName(hWnd, buffer, buffer.Capacity);
            var className = buffer.ToString();

            if(IgnoreFullScreenWindowClassNames.Any(i => i == className)) {
                // [#679] 環境によるかもだけどこいつが最前面判定されているときがある
                Logger.LogTrace("[#679] フルスクリーン検知除外 {0}, {1:x}", className, hWnd.ToInt64());
                return false;
            }
            NativeMethods.GetWindowRect(hWnd, out var windowRect);

            var screenRect = PodStructUtility.Convert(targetScreen.DeviceBounds);
            if(
                windowRect.Left != screenRect.Left
                ||
                windowRect.Top != screenRect.Top
                ||
                windowRect.Right != screenRect.Right
                ||
                windowRect.Bottom != screenRect.Bottom
            ) {
                Logger.LogTrace("[#679] フルスクリーン検知除外 {0}, {1:x}, (hWnd){2} != {3}(screen)", className, hWnd.ToInt64(), windowRect, screenRect);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 指定ディスプレイにおけるフルスクリーンのウィンドウハンドルを取得する。
        /// </summary>
        /// <param name="targetScreen"></param>
        /// <returns>フルスクリーンのウィンドウハンドル。フルスクリーンのウィンドウハンドルが取得できなかった場合は<see cref="IntPtr.Zero"/></returns>
        public IntPtr GetFullScreenWindowHandle(IScreen targetScreen)
        {
            var hResultWnd = IntPtr.Zero;

            NativeMethods.EnumWindows((hWnd, lParam) => {
                if(!IsFullScreen(hWnd, targetScreen)) {
                    return true;
                }

                hResultWnd = hWnd;
                return false;
            }, IntPtr.Zero);

            return hResultWnd;
        }

        #endregion
    }
}
