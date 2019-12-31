using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    public class PlatformThemeLoader : DisposerBase, IPlatformThemeLoader
    {
        public PlatformThemeLoader(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());

            LazyChanger = new LazyAction(GetType().Name, TimeSpan.FromMilliseconds(500), loggerFactory);

            ApplyFromRegistry();
            ApplyAccentColor();
        }

        #region property

        ILogger Logger { get; }

        LazyAction LazyChanger { get; }

        #endregion

        #region function

        void ApplyFromRegistry()
        {
            using var reg = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");

            WindowsColor = Convert.ToBoolean(reg.GetValue("SystemUsesLightTheme"))
                ? PlatformThemeColor.Light
                : PlatformThemeColor.Dark
            ;
            ApplicationColor = Convert.ToBoolean(reg.GetValue("AppsUseLightTheme"))
                ? PlatformThemeColor.Light
                : PlatformThemeColor.Dark
            ;

            ColorPrevalence = Convert.ToBoolean(reg.GetValue("ColorPrevalence"));
            EnableTransparency = Convert.ToBoolean(reg.GetValue("EnableTransparency"));

        }

        void ApplyAccentColor()
        {
            NativeMethods.DwmGetColorizationColor(out var color, out var blend);
            AccentColor = MediaUtility.ConvertColorFromRawColor(color);
        }

        private void OnThemeChanged()
        {
            Logger.LogTrace("changed theme");
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public void WndProc_WM_DWMCOLORIZATIONCOLORCHANGED(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var rawColor = (uint)wParam.ToInt64();
            AccentColor = MediaUtility.ConvertColorFromRawColor(rawColor);
            LazyChanger.DelayAction(OnThemeChanged);
            handled = true;
        }

        public void WndProc_WM_SETTINGCHANGE(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var lParamMessage = Marshal.PtrToStringAuto(lParam);
            if(lParamMessage == "ImmersiveColorSet") {
                ApplyFromRegistry();
                LazyChanger.DelayAction(OnThemeChanged);
            }
            handled = true;
        }

        #endregion

        #region IPlatformThemeLoader

        public event EventHandler? Changed;
        /// <summary>
        /// Windowsモードの色。
        /// <para>タスクバーとかの色っぽい。</para>
        /// </summary>
        public PlatformThemeColor WindowsColor { get; private set; }
        /// <summary>
        /// アプリモードの色。
        /// <para>背景色っぽい。</para>
        /// </summary>
        public PlatformThemeColor ApplicationColor { get; private set; }
        /// <summary>
        /// アクセントカラー！
        /// </summary>
        public Color AccentColor { get; private set; }

        /// <summary>
        /// アクセントカラーをスタートメニューとかに使用するか。
        /// </summary>
        public bool ColorPrevalence { get; private set; }
        /// <summary>
        /// 透明効果。
        /// </summary>
        public bool EnableTransparency { get; private set; }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    LazyChanger.SafeFlush();
                    LazyChanger.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

}
