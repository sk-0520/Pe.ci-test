using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    public enum PlatformThemeColor
    {
        Dark,
        Light,
    }

    public interface IPlatformThemeLoader
    {
        #region define

        event EventHandler? Changed;

        #endregion

        #region property

        /// <summary>
        /// Windowsモードの色。
        /// <para>タスクバーとかの色っぽい。</para>
        /// </summary>
        PlatformThemeColor WindowsColor { get; }
        /// <summary>
        /// アプリモードの色。
        /// <para>背景色っぽい。</para>
        /// </summary>
        PlatformThemeColor ApplicationColor { get; }
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
    }

    public static class IPlatformThemeLoaderExtensions
    {
        #region function

        public static Color GetWindowColor(this IPlatformThemeLoader @this)
        {
            if(@this.ColorPrevalence) {
                // 透明度どうしよう
                return @this.AccentColor;
            }

            switch(@this.WindowsColor) {
                case PlatformThemeColor.Dark:
                    return Color.FromArgb(0xff, 0x11, 0x11, 0x11);

                case PlatformThemeColor.Light:
                    return Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }

}
