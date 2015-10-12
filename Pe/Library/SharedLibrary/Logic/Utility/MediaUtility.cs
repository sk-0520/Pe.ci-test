namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
using System.Windows.Media.Imaging;

	public static class MediaUtility
	{
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
		/// <seealso cref="http://www.kanzaki.com/docs/html/color-check" />
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
			if (brightness > 160) {
				return Colors.Black;
			} else {
				return Colors.White;
			}
		}

		public static Color GetNoneAlphaColor(Color value)
		{
			value.A = 0xff;
			return value;
		}

		/// <summary>
		/// 指定ビットマップソースから全ピクセル情報を取得。
		/// </summary>
		/// <param name="bitmapSource"></param>
		/// <returns></returns>
		public static byte[] GetPixels(BitmapSource bitmapSource)
		{
			var usingNewImage = bitmapSource.Format != PixelFormats.Bgra32;
			if (usingNewImage) {
				bitmapSource = new FormatConvertedBitmap(
					bitmapSource,
					PixelFormats.Bgra32,
					null,
					0
				);
				bitmapSource.Freeze();
			}
			int width = (int)bitmapSource.Width;
			int height = (int)bitmapSource.Height;
			int stride = width * 4;
			var pixels = new byte[stride * height];
			bitmapSource.CopyPixels(pixels, stride, 0);

			if (usingNewImage) {
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
			CheckUtility.Enforce<ArgumentException>(pixels.Length % 4 == 0);
			for (var i = 0; i < pixels.Length; i += 4) {
				var b = pixels[i + 0];
				var g = pixels[i + 1];
				var r = pixels[i + 2];
				var a = pixels[i + 3];
				yield return Color.FromArgb(a, r, g, b);
			}
		}

		/// <summary>
		/// 画像の中から一番多そうな色を取得する。
		/// </summary>
		/// <param name="bitmapSource"></param>
		/// <returns></returns>
		public static Color GetPredominantColorFromBitmapSource(BitmapSource bitmapSource)
		{
			var pixels = GetPixels(bitmapSource);
			var colors = GetColors(pixels);
			//return GetPredominantColor(colors.Select((c, i) => new { c, i }).Where(ci => (ci.i % 8) == 0).Select(ci => ci.c));
			return GetPredominantColor(colors);
		}

		/// <summary>
		/// 渡された色の中から一番多そうな色を取得する。
		/// </summary>
		/// <param name="colors"></param>
		/// <returns></returns>
		public static Color GetPredominantColor(IEnumerable<Color> colors)
		{
			var map = new Dictionary<Color, int>();
			int tempValue;
			foreach (var color in colors.Where(c => c.A > 120)) {
				if (map.TryGetValue(color, out tempValue)) {
					map[color] += 1;
				} else {
					map[color] = 1;
				}
			}
			return map.OrderByDescending(p => p.Value).First().Key;
		}

	}
}
