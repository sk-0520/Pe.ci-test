using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    /// <summary>
    /// システムテーマ情報を読み込み。
    /// <inheritdoc cref="IPlatformTheme"/>
    /// </summary>
    public class PlatformThemeLoader: DisposerBase, IPlatformTheme
    {
        public PlatformThemeLoader(PlatformConfiguration platformConfiguration, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            PlatformConfiguration = platformConfiguration;

            DelayChanger = new DelayAction(GetType().Name, TimeSpan.FromMilliseconds(400), loggerFactory);
            Refresh();
        }

        #region property

        private ILogger Logger { get; }
        private PlatformConfiguration PlatformConfiguration { get; }

        private DelayAction DelayChanger { get; }

        WeakEvent<EventArgs> ChangedWeakEvent { get; } = new WeakEvent<EventArgs>(nameof(Changed));

        #endregion

        #region function

        private void ApplyFromRegistry()
        {
            using var reg = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            if(reg == null) {
                Logger.LogWarning("テーマ取得できず");
                return;
            }

            WindowsThemeKind = Convert.ToBoolean(reg.GetValue("SystemUsesLightTheme"), CultureInfo.InvariantCulture)
                ? PlatformThemeKind.Light
                : PlatformThemeKind.Dark
            ;
            ApplicationThemeKind = Convert.ToBoolean(reg.GetValue("AppsUseLightTheme"), CultureInfo.InvariantCulture)
                ? PlatformThemeKind.Light
                : PlatformThemeKind.Dark
            ;

            ColorPrevalence = Convert.ToBoolean(reg.GetValue("ColorPrevalence"), CultureInfo.InvariantCulture);
            EnableTransparency = Convert.ToBoolean(reg.GetValue("EnableTransparency"), CultureInfo.InvariantCulture);

        }

        private void ApplyAccentColor()
        {
            NativeMethods.DwmGetColorizationColor(out var color, out var blend);
            SetAccentColor(MediaUtility.ConvertColorFromRawColor(color));
        }

        private void SetAccentColor(Color color)
        {
            if(color.A < PlatformConfiguration.ThemeAccentColorMinimumAlpha) {
                var newAlpha = PlatformConfiguration.ThemeAccentColorDefaultAlpha;
                Logger.LogInformation("アクセントカラー透明度補正: {0} -> {1}", color.A, newAlpha);
                color.A = newAlpha;
            }
            AccentColor = color;
        }

        private void OnThemeChanged()
        {
            Logger.LogTrace("テーマ変更");
            ChangedWeakEvent.Raise(this, EventArgs.Empty);
        }

        public void WndProc_WM_DWMCOLORIZATIONCOLORCHANGED(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            Logger.LogTrace("WM_DWMCOLORIZATIONCOLORCHANGED");
            var rawColor = (uint)wParam.ToInt64();
            SetAccentColor(MediaUtility.ConvertColorFromRawColor(rawColor));
            DelayChanger.Callback(OnThemeChanged);
            handled = true;
        }

        public void WndProc_WM_SETTINGCHANGE(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var lParamMessage = Marshal.PtrToStringAuto(lParam);
            if(lParamMessage == "ImmersiveColorSet") {
                Logger.LogTrace("WM_SETTINGCHANGE");
                ApplyFromRegistry();
                DelayChanger.Callback(OnThemeChanged);
            }
            handled = true;
        }

        public void Refresh()
        {
            ApplyFromRegistry();
            ApplyAccentColor();
        }

        private PlatformThemeColors GetColors(PlatformThemeKind themeKind)
        {
            switch(themeKind) {
                case PlatformThemeKind.Dark:
                    return new PlatformThemeColors(
                        Color.FromRgb(0x00, 0x00, 0x00),
                        Color.FromRgb(0xff, 0xff, 0xff),
                        Color.FromRgb(0x80, 0x80, 0x80),
                        Color.FromRgb(0xcc, 0xcc, 0xcc)
                    );

                case PlatformThemeKind.Light:
                    return new PlatformThemeColors(
                        Color.FromRgb(0xff, 0xff, 0xff),
                        Color.FromRgb(0x00, 0x00, 0x00),
                        Color.FromRgb(0xcc, 0xcc, 0xcc),
                        Color.FromRgb(0x80, 0x80, 0x80)
                    );

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region IPlatformThemeLoader

        public event EventHandler<EventArgs>? Changed
        {
            add => ChangedWeakEvent.Add(value);
            remove => ChangedWeakEvent.Remove(value);
        }
        /// <summary>
        /// Windowsモードの色。
        /// </summary>
        /// <remarks>
        /// <para>タスクバーとかの色っぽい。</para>
        /// </remarks>
        public PlatformThemeKind WindowsThemeKind { get; private set; }
        /// <summary>
        /// アプリモードの色。
        /// </summary>
        /// <remarks>
        /// <para>背景色っぽい。</para>
        /// </remarks>
        public PlatformThemeKind ApplicationThemeKind { get; private set; }
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


        public PlatformThemeColors GetWindowsThemeColors(PlatformThemeKind themeKind) => GetColors(themeKind);
        public PlatformThemeColors GetApplicationThemeColors(PlatformThemeKind themeKind) => GetColors(themeKind);

        public PlatformAccentColors GetAccentColors(Color accentColor)
        {
            var nonTransAccentColor = MediaUtility.GetOpaqueColor(accentColor);
            return new PlatformAccentColors(
                accentColor,
                nonTransAccentColor,
                MediaUtility.AddBrightness(nonTransAccentColor, 0.9),
                MediaUtility.AddBrightness(nonTransAccentColor, 1.2),
                MediaUtility.AddBrightness(nonTransAccentColor, 0.6)
            );
        }

        public PlatformAccentColors GetTextColor(in PlatformAccentColors accentColors)
        {
            return new PlatformAccentColors(
                MediaUtility.GetAutoColor(accentColors.Accent),
                MediaUtility.GetAutoColor(accentColors.Base),
                MediaUtility.GetAutoColor(accentColors.Highlight),
                MediaUtility.GetAutoColor(accentColors.Accent),
                MediaUtility.GetAutoColor(accentColors.Disable)
            );
        }


        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    DelayChanger.SafeFlush();
                    DelayChanger.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
