using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    public enum PlatformThemeKind
    {
        Dark,
        Light,
    }

    public readonly struct PlatformThemeColors
    {
        public PlatformThemeColors(Color background, Color foreground, Color control, Color border)
        {
            Background = background;
            Foreground = foreground;
            Control = control;
            Border = border;
        }

        #region property

        public Color Background { get; }
        public Color Foreground { get; }
        public Color Control { get; }
        public Color Border { get; }

        #endregion
    }

    public readonly struct PlatformAccentColors
    {
        public PlatformAccentColors(Color accent, Color baseColor, Color highlight, Color active, Color disable)
        {
            Accent = accent;
            Base = baseColor;
            Highlight = highlight;
            Active = active;
            Disable = disable;
        }

        #region property

        /// <summary>
        /// 透明度を含む。
        /// </summary>
        public Color Accent { get; }
        /// <summary>
        /// 透明度を含まない。
        /// </summary>
        public Color Base { get; }
        /// <summary>
        /// 透明度を含まない。
        /// </summary>
        public Color Highlight { get; }
        /// <summary>
        /// 透明度を含まない。
        /// </summary>
        public Color Active { get; }
        /// <summary>
        /// 透明度を含まない。
        /// </summary>
        public Color Disable { get; }

        #endregion
    }

    public interface IPlatformTheme
    {
        #region define

        event EventHandler? Changed;

        #endregion

        #region property

        /// <summary>
        /// Windowsモードの色。
        /// <para>タスクバーとかの色っぽい。</para>
        /// </summary>
        PlatformThemeKind WindowsThemeKind { get; }
        /// <summary>
        /// アプリモードの色。
        /// <para>背景色っぽい。</para>
        /// </summary>
        PlatformThemeKind ApplicationThemeKind { get; }
        /// <summary>
        /// アクセントカラー！
        /// </summary>
        Color AccentColor { get; }

        /// <summary>
        /// アクセントカラーをスタートメニューとかに使用するか。
        /// </summary>
        bool ColorPrevalence { get; }
        /// <summary>
        /// 透明効果。
        /// </summary>
        bool EnableTransparency { get; }

        #endregion

        #region function

        PlatformThemeColors GetWindowsThemeColors(PlatformThemeKind themeKind);
        PlatformThemeColors GetApplicationThemeColors(PlatformThemeKind themeKind);
        PlatformAccentColors GetAccentColors(Color accentColor);
        PlatformAccentColors GetTextColor(PlatformAccentColors accentColors);

        #endregion
    }

    public static class IPlatformThemeLoaderExtensions
    {
        #region function

        public static Color GetTaskbarColor(this IPlatformTheme @this)
        {
            // 透明度どうしよう
            Color result;

            if(@this.ColorPrevalence) {
                result = @this.AccentColor;
            } else {
                var color = @this.GetWindowsThemeColors(@this.WindowsThemeKind);
                result = color.Background;
            }

            if(@this.EnableTransparency) {
                result.A = @this.AccentColor.A;
            } else {
                result.A = 0xff;
            }

            return result;
        }

        #endregion
    }

}
