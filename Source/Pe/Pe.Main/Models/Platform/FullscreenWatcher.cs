//#define LOGGING

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
    /// TODO: 外部から設定を渡せるようにしないとまずい気がする。
    /// <para>こいつ自身は呼ばれた際にフルスクリーンの確認を行うだけで定周期処理されるわけではない。</para>
    /// </summary>
    public class FullscreenWatcher
    {
        public FullscreenWatcher(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        private int WindowClassNameLength { get; } = WindowsUtility.classNameLength;

        private IReadOnlyCollection<string> IgnoreFullScreenWindowClassNames { get; } = new[] {
            "Shell_TrayWnd",
            "Progman",
            "WorkerW",
        };

        /// <summary>
        /// フルスクリーン判定を最前面ウィンドウのみにするか。
        /// </summary>
        private bool TopmostOnly { get; } = false;
        /// <summary>
        /// フルスクリーン判定から WS_EX_NOACTIVE を除外するか。
        /// </summary>
        private bool ExcludeNoActive { get; } = true;
        /// <summary>
        /// フルスクリーン判定から WS_EX_TOOLWINDOW を除外するか。
        /// </summary>
        private bool ExcludeToolWindow { get; } = true;

        #endregion

        #region function

        private bool IsNVidiaGeForceOverlay(IntPtr hWnd, string windowClassName, string windowText, in RECT windowRect)
        {
            if(windowClassName != "CEF-OSC-WIDGET") {
                return false;
            }

            if(windowText != "NVIDIA GeForce Overlay") {
                return false;
            }

            return true;
        }

        private bool IsMicrosoftTextInputApplication(IntPtr hWnd, string windowClassName, string windowText, in RECT windowRect)
        {
            if(windowClassName != "Windows.UI.Core.CoreWindow") {
                return false;
            }

            if(windowText != "Microsoft Text Input Application") {
                return false;
            }

            return true;
        }

        private bool IsSpecialIgnore(IntPtr hWnd, string windowClassName, in RECT windowRect)
        {
            var windowText = WindowsUtility.GetWindowText(hWnd);
            return
                IsNVidiaGeForceOverlay(hWnd, windowClassName, windowText, windowRect)
                ||
                IsMicrosoftTextInputApplication(hWnd, windowClassName, windowText, windowRect)
            ;
        }

        /// <summary>
        /// ウィンドウハンドルが指定のディスプレイにおいてフルスクリーンか。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="targetScreen"></param>
        /// <returns></returns>
        public bool IsFullscreen(IntPtr hWnd, IScreen targetScreen)
        {
            if(!NativeMethods.IsWindowVisible(hWnd)) {
                return false;
            }

            if(hWnd == NativeMethods.GetDesktopWindow() || hWnd == NativeMethods.GetShellWindow()) {
                return false;
            }

            var windowClassName = WindowsUtility.GetWindowClassName(hWnd);
            if(IgnoreFullScreenWindowClassNames.Any(i => i == windowClassName)) {
                // [#679] 環境によるかもだけどこいつが最前面判定されているときがある
#if LOGGING
                Logger.LogTrace("[#679] フルスクリーン検知除外 {0}, {1:x}", windowClassName, hWnd.ToInt64());
#endif
                return false;
            }
            NativeMethods.GetWindowRect(hWnd, out var windowRect);

            var screenRect = PodStructUtility.Convert(targetScreen.DeviceBounds);
            if(
                windowRect.Left > screenRect.Left
                ||
                windowRect.Top > screenRect.Top
                ||
                windowRect.Right < screenRect.Right
                ||
                windowRect.Bottom < screenRect.Bottom
            ) {
#if LOGGING
                Logger.LogTrace("[#679] フルスクリーン検知除外 {0}, {1:x}, (hWnd){2} != {3}(screen)", windowClassName, hWnd.ToInt64(), windowRect, screenRect);
#endif
                return false;
            }

            if(TopmostOnly || ExcludeNoActive || ExcludeToolWindow) {
                var exWindowStyle = WindowsUtility.GetWindowLong(hWnd, (int)GWL.GWL_EXSTYLE).ToInt32();

                if(TopmostOnly) {
                    if((exWindowStyle & (int)WS_EX.WS_EX_TOPMOST) == (int)WS_EX.WS_EX_TOPMOST) {
#if LOGGING
                        Logger.LogTrace("[#679] フルスクリーン検知除外({0}) {1}, {2:x}, {3}, !WS_EX_TOPMOST", nameof(TopmostOnly), windowClassName, windowRect, hWnd.ToInt64());
#endif
                        return false;
                    }
                }

                if(ExcludeNoActive) {
                    if((exWindowStyle & (int)WS_EX.WS_EX_NOACTIVATE) == (int)WS_EX.WS_EX_NOACTIVATE) {
#if LOGGING
                        Logger.LogTrace("[#679] フルスクリーン検知除外({0}) {1}, {2:x}, {3}, !WS_EX_NOACTIVATE", nameof(ExcludeNoActive), windowClassName, windowRect, hWnd.ToInt64());
#endif
                        return false;
                    }
                }

                if(ExcludeToolWindow) {
                    if((exWindowStyle & (int)WS_EX.WS_EX_TOOLWINDOW) == (int)WS_EX.WS_EX_TOOLWINDOW) {
#if LOGGING
                        Logger.LogTrace("[#679] フルスクリーン検知除外({0}) {1}, {2:x}, {3}, !WS_EX_TOOLWINDOW", nameof(ExcludeToolWindow), windowClassName, windowRect, hWnd.ToInt64());
#endif
                        return false;
                    }
                }
            }

            if(IsSpecialIgnore(hWnd, windowClassName, windowRect)) {
#if LOGGING
                Logger.LogTrace("[#679] フルスクリーン検知特殊除外 {0}, {1:x}", windowClassName, hWnd.ToInt64());
#endif
                return false;
            }

#if LOGGING
            Logger.LogTrace("フルスクリーン対象 {0}, {1:x}", windowClassName, hWnd.ToInt64());
#endif

            return true;
        }

        /// <summary>
        /// 指定ディスプレイにおけるフルスクリーンのウィンドウハンドルを取得する。
        /// </summary>
        /// <param name="targetScreen"></param>
        /// <returns>フルスクリーンのウィンドウハンドル。フルスクリーンのウィンドウハンドルが取得できなかった場合は<see cref="IntPtr.Zero"/></returns>
        public IntPtr GetFullscreenWindowHandle(IScreen targetScreen)
        {
            IntPtr hResultWnd = IntPtr.Zero;

            NativeMethods.EnumWindows((hWnd, lParam) => {
                if(!IsFullscreen(hWnd, targetScreen)) {
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
