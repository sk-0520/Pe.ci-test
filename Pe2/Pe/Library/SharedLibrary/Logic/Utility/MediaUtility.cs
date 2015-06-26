namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;

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
		/// 指定色から自づ的に見やすそうな色を算出。
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static Color CalculateAutoColor(Color color)
		{
			var brightness = GetBrightness(color);
			if (brightness > 160) {
				return Colors.Black;
			} else {
				return Colors.White;
			}
		}

	}
}
