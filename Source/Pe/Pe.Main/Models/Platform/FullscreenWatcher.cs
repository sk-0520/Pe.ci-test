//#define LOGGING

using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    /// <summary>
    /// フルスクリーン状態の検知。
    /// <para>こいつ自身は呼ばれた際にフルスクリーンの確認を行うだけで定周期処理されるわけではない。</para>
    /// </summary>
    public interface IFullscreenWatcher
    {
        #region function

        /// <summary>
        /// ウィンドウハンドルが指定のディスプレイにおいてフルスクリーンか。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="targetScreen"></param>
        /// <returns></returns>
        bool IsFullscreen(IntPtr hWnd, IScreen targetScreen);

        /// <summary>
        /// 指定ディスプレイにおけるフルスクリーンのウィンドウハンドルを取得する。
        /// </summary>
        /// <param name="targetScreen"></param>
        /// <returns>フルスクリーンのウィンドウハンドル。フルスクリーンのウィンドウハンドルが取得できなかった場合は<see cref="IntPtr.Zero"/></returns>
        IntPtr GetFullscreenWindowHandle(IScreen targetScreen);

        #endregion
    }

    public class ClassAndText
    {
        public ClassAndText(string windowClassName, string windowText)
        {
            WindowClassName = windowClassName ?? throw new ArgumentNullException(nameof(windowClassName));
            WindowText = windowText ?? throw new ArgumentNullException(nameof(windowText));
        }

        #region property

        public string WindowClassName { get; }
        public string WindowText { get; }

        #endregion
    }

    /// <inheritdoc cref="IFullscreenWatcher"/>
    internal class FullscreenWatcher: IFullscreenWatcher
    {
        public FullscreenWatcher(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILogger Logger { get; }

        public IList<string> IgnoreFullscreenWindowClassNames { get; } = new List<string>();

        public IList<ClassAndText> ClassAndTexts { get; } = new List<ClassAndText>();

        /// <summary>
        /// フルスクリーン判定を最前面ウィンドウのみにするか。
        /// </summary>
        public bool TopmostOnly { get; set; } = false;
        /// <summary>
        /// フルスクリーン判定から WS_EX_NOACTIVE を除外するか。
        /// </summary>
        public bool ExcludeNoActive { get; set; } = true;
        /// <summary>
        /// フルスクリーン判定から WS_EX_TOOLWINDOW を除外するか。
        /// </summary>
        public bool ExcludeToolWindow { get; set; } = true;

        #endregion

        #region function

        private bool MatchClassAndText(IntPtr hWnd, string windowClassName, in RECT windowRect)
        {
            if(ClassAndTexts.Count == 0) {
                return false;
            }

            var windowText = WindowsUtility.GetWindowText(hWnd);
            foreach(var item in ClassAndTexts) {
                if(item.WindowClassName == windowClassName && item.WindowText == windowText) {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region IFullscreenWatcher

        /// <inheritdoc cref="IFullscreenWatcher.IsFullscreen(IntPtr, IScreen)"/>
        public bool IsFullscreen(IntPtr hWnd, IScreen targetScreen)
        {
            if(!NativeMethods.IsWindowVisible(hWnd)) {
                return false;
            }

            if(hWnd == NativeMethods.GetDesktopWindow() || hWnd == NativeMethods.GetShellWindow()) {
                return false;
            }

            var windowClassName = WindowsUtility.GetWindowClassName(hWnd);
            if(IgnoreFullscreenWindowClassNames.Any(i => i == windowClassName)) {
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
                var exWindowStyle = WindowsUtility.GetWindowLongPtr(hWnd, (int)GWL.GWL_EXSTYLE).ToInt32();

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

            if(MatchClassAndText(hWnd, windowClassName, windowRect)) {
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

        /// <inheritdoc cref="IFullscreenWatcher.GetFullscreenWindowHandle(IScreen)"/>
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
