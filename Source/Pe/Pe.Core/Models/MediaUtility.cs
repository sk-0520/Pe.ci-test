using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public static class MediaUtility
    {
        /// <summary>
        /// 色を生値に変換。
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static uint ConvertRawColorFromColor(Color color)
        {
            return (uint)(
                  (color.A << 24)
                | (color.R << 16)
                | (color.G << 8)
                | (color.B)
            );
        }
        /// <summary>
        /// 生値を色に変換。
        /// </summary>
        /// <param name="rawColor"></param>
        /// <returns></returns>
        public static Color ConvertColorFromRawColor(uint rawColor)
        {
            return Color.FromArgb(
                (byte)(rawColor >> 24),
                (byte)(rawColor >> 16),
                (byte)(rawColor >> 8),
                (byte)(rawColor)
            );
        }
        /// <summary>
        /// 色反転。
        /// <para>透明度は保ったまま。</para>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color GetNegativeColor(Color color)
        {
            return Color.FromArgb(
                color.A,
                (byte)(0xff - color.R),
                (byte)(0xff - color.G),
                (byte)(0xff - color.B)
            );
        }

        /// <summary>
        /// 補色。
        /// <para>透明度は保ったまま。</para>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color GetComplementaryColor(Color color)
        {
            var max = Math.Max(color.R, Math.Max(color.G, color.B));
            var min = Math.Min(color.R, Math.Min(color.G, color.B));
            var v = max + min;

            return Color.FromArgb(
                color.A,
                (byte)(v - color.R),
                (byte)(v - color.G),
                (byte)(v - color.B)
            );
        }

        /// <summary>
        /// 明るさを算出。
        /// </summary>
        /// http://www.kanzaki.com/docs/html/color-check
        /// <param name="color"></param>
        /// <returns></returns>
        public static double GetBrightness(Color color)
        {
            return (((color.R * 299) + (color.G * 587) + (color.B * 114)) / 1000.0);
        }

        /// <summary>
        /// 指定色から自動的に見やすそうな色を算出。
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color GetAutoColor(Color color)
        {
            var brightness = GetBrightness(color);
            if(brightness > 160) {
                return Colors.Black;
            } else {
                return Colors.White;
            }
        }

        /// <summary>
        /// 指定色を非透明にする。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Color GetNonTransparentColor(Color value)
        {
            value.A = 0xff;
            return value;
        }

        /// <summary>
        /// 色を加算する。
        /// </summary>
        /// <param name="baseColor">基本色。</param>
        /// <param name="plusColor">加算する色。</param>
        /// <returns></returns>
        public static Color AddColor(Color baseColor, Color plusColor)
        {
            return Color.FromArgb(
                baseColor.A,
                (byte)(baseColor.R + (plusColor.R - baseColor.R) * (plusColor.A / 255.0)),
                (byte)(baseColor.G + (plusColor.G - baseColor.G) * (plusColor.A / 255.0)),
                (byte)(baseColor.B + (plusColor.B - baseColor.B) * (plusColor.A / 255.0))
            );
        }

        /// <summary>
        /// 明るさを加算する。
        /// </summary>
        /// <param name="baseColor"></param>
        /// <param name="brightness"></param>
        /// <returns></returns>
        public static Color AddBrightness(Color baseColor, double brightness)
        {
            return Color.FromArgb(
                baseColor.A,
                (byte)(baseColor.R * brightness),
                (byte)(baseColor.G * brightness),
                (byte)(baseColor.B * brightness)
            );
        }

        /// <summary>
        /// 指定要素をビットマップに落とし込む。
        /// </summary>
        /// <param name="element"></param>
        /// <param name="dpiScale"></param>
        /// <param name="useActual"></param>
        /// <returns></returns>
        public static BitmapSource MakeBitmapSource(FrameworkElement element, Point dpiScale, bool useActual)
        {
            var size = useActual
                ? new Size(element.ActualWidth, element.ActualHeight)
                : new Size(element.Width, element.Height)
            ;

            element.Measure(size);
            element.Arrange(new Rect(size));

            var render = new RenderTargetBitmap((int)size.Width, (int)size.Height, dpiScale.X, dpiScale.Y, PixelFormats.Pbgra32);
            render.Render(element);

            return render;
        }

        /// <summary>
        /// 指定要素をビットマップに落とし込む。
        /// <para>DPIは96を使用する。</para>
        /// </summary>
        /// <param name="element"></param>
        /// <param name="useActual"></param>
        /// <returns></returns>
        public static BitmapSource MakeBitmapSourceDefaultDpi(FrameworkElement element, bool useActual)
        {
            return MakeBitmapSource(element, new Point(96, 96), useActual);
        }
    }
}
