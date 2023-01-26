using System;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// OS テーマ種別。
    /// </summary>
    public enum PlatformThemeKind
    {
        /// <summary>
        /// 黒。
        /// </summary>
        Dark,
        /// <summary>
        /// 白。
        /// </summary>
        Light,
    }

    /// <summary>
    /// OS テーマ色の集合。
    /// </summary>
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

        /// <summary>
        /// 背景色。
        /// </summary>
        public Color Background { get; }
        /// <summary>
        /// 前景色。
        /// </summary>
        public Color Foreground { get; }
        /// <summary>
        /// コントロール基本色。
        /// </summary>
        public Color Control { get; }
        /// <summary>
        /// 境界線。
        /// </summary>
        public Color Border { get; }

        #endregion
    }

    /// <summary>
    /// アクセントカラー。
    /// </summary>
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

    /// <summary>
    /// OSテーマ情報。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPlatformTheme
    {
        #region event

        /// <summary>
        /// OSのテーマ情報が変更された。
        /// </summary>
        event EventHandler<EventArgs>? Changed;

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

        /// <summary>
        /// 既定の Windows モードの色一覧を取得。
        /// </summary>
        /// <param name="themeKind"></param>
        /// <returns></returns>
        PlatformThemeColors GetWindowsThemeColors(PlatformThemeKind themeKind);
        /// <summary>
        /// 既定のアプリモードの色一覧を取得。
        /// </summary>
        /// <param name="themeKind"></param>
        /// <returns></returns>
        PlatformThemeColors GetApplicationThemeColors(PlatformThemeKind themeKind);
        /// <summary>
        ///アクセントカラー一覧を取得。
        /// </summary>
        /// <param name="accentColor"></param>
        /// <returns></returns>
        PlatformAccentColors GetAccentColors(Color accentColor);
        /// <summary>
        /// アクセントカラー一覧からテキスト色一覧を取得。
        /// </summary>
        /// <param name="accentColors"></param>
        /// <returns></returns>
        PlatformAccentColors GetTextColor(in PlatformAccentColors accentColors);

        #endregion
    }

    public static class IPlatformThemeLoaderExtensions
    {
        #region function

        /// <summary>
        /// タスクバーの色を取得。
        /// </summary>
        /// <param name="platformTheme"></param>
        /// <returns></returns>
        public static Color GetTaskbarColor(this IPlatformTheme platformTheme)
        {
            // 透明度どうしよう
            Color result;

            if(platformTheme.ColorPrevalence) {
                result = platformTheme.AccentColor;
            } else {
                var color = platformTheme.GetWindowsThemeColors(platformTheme.WindowsThemeKind);
                result = color.Background;
            }

            if(platformTheme.EnableTransparency) {
                result.A = platformTheme.AccentColor.A;
            } else {
                result.A = 0xff;
            }

            return result;
        }

        #endregion
    }

}
