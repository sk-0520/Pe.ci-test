using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
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
                | (color.B << 0)
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
                (byte)(rawColor >> 0)
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
            var colorValue = (new[] { color.R, color.G, color.B }).Distinct();
            var max = colorValue.Max();
            var min = colorValue.Min();
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
        /// 指定ビットマップソースから全ピクセル情報を取得。
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        public static byte[] GetPixels(BitmapSource bitmapSource)
        {
            var usingNewImage = bitmapSource.Format != PixelFormats.Bgra32;
            if(usingNewImage) {
                bitmapSource = new FormatConvertedBitmap(
                    bitmapSource,
                    PixelFormats.Bgra32,
                    null,
                    0
                );
                FreezableUtility.SafeFreeze(bitmapSource);
            }
            int width = (int)bitmapSource.PixelWidth;
            int height = (int)bitmapSource.PixelHeight;
            int stride = width * 4;
            var pixels = new byte[stride * height];
            bitmapSource.CopyPixels(pixels, stride, 0);

            if(usingNewImage) {
                // 解放処理
            }

            return pixels;
        }

        /// <summary>
        /// ピクセル情報から色へ変換。
        /// </summary>
        /// <param name="pixels">[B][G][R][A]... となっていることを期待。</param>
        /// <returns></returns>
        public static IEnumerable<Color> GetColors(byte[] pixels)
        {
            Debug.Assert(pixels.Length % 4 == 0);

            var length = pixels.LongLength;
            for(var i = 0; i < length; i += 4) {
                var b = pixels[i + 0];
                var g = pixels[i + 1];
                var r = pixels[i + 2];
                var a = pixels[i + 3];
                yield return Color.FromArgb(a, r, g, b);
            }
        }

        ///// <summary>
        ///// 指定ビットマップソースから全色情報を取得。
        ///// </summary>
        ///// <param name="bitmapSource"></param>
        ///// <returns></returns>
        //public static IEnumerable<Color> GetColorsFromBitmapSource(BitmapSource bitmapSource)
        //{
        //    var pixels = MediaUtility.GetPixels(bitmapSource);
        //    var colors = MediaUtility.GetColors(pixels);

        //    return colors;
        //}

        /// <summary>
        /// 画像の中から一番多そうな色を取得する。
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <param name="baseAlpha">指定した透明度以上のピクセル情報を算出に使用する。</param>
        /// <returns></returns>
        public static Color GetPredominantColorFromBitmapSource(BitmapSource bitmapSource, byte baseAlpha)
        {
            var pixels = GetPixels(bitmapSource);
            var colors = GetColors(pixels);
            //return GetPredominantColor(colors.Select((c, i) => new { c, i }).Where(ci => (ci.i % 8) == 0).Select(ci => ci.c));
            return GetPredominantColor(colors, baseAlpha);
        }

        /// <summary>
        /// 渡された色の中から一番多そうな色を取得する。
        /// </summary>
        /// <param name="colors"></param>
        /// <param name="baseAlpha">指定した透明度以上のピクセル情報を算出に使用する。</param>
        /// <returns></returns>
        public static Color GetPredominantColor(IEnumerable<Color> colors, byte baseAlpha)
        {
            var map = new Dictionary<Color, int>();
            int tempValue;
            foreach(var color in colors.Where(c => baseAlpha <= c.A)) {
                if(map.TryGetValue(color, out tempValue)) {
                    map[color] += 1;
                } else {
                    map[color] = 1;
                }
            }
            if(map.Any()) {
                return map.OrderByDescending(p => p.Value).First().Key;
            } else {
                return Colors.Transparent;
            }
        }

        /// <summary>
        /// ビットマップに対して着色を行う。
        /// <para>黒が塗られる領域。</para>
        /// </summary>
        /// <param name="targetBitmap"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static BitmapSource ColoringImage(BitmapSource targetBitmap, Color color)
        {
            var pixels = MediaUtility.GetPixels(targetBitmap);
            var pixcelWidth = 4;
            for(var i = 0; i < pixels.Length; i += pixcelWidth) {
                // 0:b 1:g 2:r 3:a
                var b = pixels[i + 0];
                var g = pixels[i + 1];
                var r = pixels[i + 2];
                pixels[i + 0] = (byte)(b + (1 - b / 255.0) * color.B);
                pixels[i + 1] = (byte)(g + (1 - g / 255.0) * color.G);
                pixels[i + 2] = (byte)(r + (1 - r / 255.0) * color.R);
            }

            var result = new WriteableBitmap(targetBitmap);
            try {
                result.Lock();
                result.WritePixels(new Int32Rect(0, 0, targetBitmap.PixelWidth, targetBitmap.PixelHeight), pixels, targetBitmap.PixelWidth * pixcelWidth, 0);
            } finally {
                result.Unlock();
            }

            FreezableUtility.SafeFreeze(result);

            return result;
        }


        /// <summary>
        /// 指定要素をビットマップに落とし込む。
        /// </summary>
        /// <param name="element"></param>
        /// <param name="dpi"></param>
        /// <param name="useActual"></param>
        /// <returns></returns>
        public static BitmapSource MakeBitmapSource(FrameworkElement element, Point dpi, bool useActual)
        {
            var size = useActual
                ? new Size(element.ActualWidth, element.ActualHeight)
                : new Size(element.Width, element.Height)
            ;

            element.Measure(size);
            element.Arrange(new Rect(size));

            var render = new RenderTargetBitmap((int)size.Width, (int)size.Height, dpi.X, dpi.Y, PixelFormats.Pbgra32);
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
        public static BitmapSource MakeBitmapSourceDefualtDpi(FrameworkElement element, bool useActual)
        {
            return MakeBitmapSource(element, new Point(96, 96), useActual);
        }
    }
}
