/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/10/17
 * 時刻: 13:12
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Linq;

namespace PeMain.Logic
{
	/// <summary>
	/// 描画等々の共通処理。
	/// </summary>
	public class DrawUtility
	{
		/// <summary>
		/// RGB反転。
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static Color ToNegativeColor(Color color)
		{
			return Color.FromArgb(
				color.A,
				0xff - color.R,
				0xff - color.G,
				0xff - color.B
			);
		}
		
		/// <summary>
		/// 補色。
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static Color ToComplementaryColor(Color color)
		{
			var colorValue = (new [] { color.R, color.G, color.B }).Distinct();
			var max = colorValue.Max();
			var min = colorValue.Min();
			var v = max + min;
			
			return Color.FromArgb(
				color.A,
				v - color.R,
				v - color.G,
				v - color.B
			);
		}
		
		/// <summary>
		/// 明るさを算出。
		/// </summary>
		/// <seealso cref="http://www.kanzaki.com/docs/html/color-check" />
		/// <param name="color"></param>
		/// <returns></returns>
		public static float GetBrightness(Color color)
		{
			return (float)(((color.R * 299) + (color.G * 587) + (color.B * 114)) / 1000.0);
		}
		
		/// <summary>
		/// 指定色から自づ的に見やすそうな色を算出。
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static Color CalcAutoColor(Color color)
		{
			var brightness = GetBrightness(color);
			if(brightness > 160) {
				return Color.Black;
			} else {
				return Color.White;
			}
		}
	}
}
