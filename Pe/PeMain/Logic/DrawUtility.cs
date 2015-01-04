using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	/// <summary>
	/// 描画等々の共通処理。
	/// </summary>
	public class DrawUtility
	{
		/// <summary>
		/// デバッグ時にデバッグ用と分かるように印付け。
		/// </summary>
		/// <param name="g">描画対象</param>
		/// <param name="drawArea">描画領域</param>
		[Conditional("DEBUG")]
		public static void MarkingDebug(Graphics g, Rectangle drawArea)
		{
			using(var brush = new SolidBrush(Color.FromArgb(90, Color.Red))) {
				g.FillRectangle(brush, drawArea);
			}
		}
		
		
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
		
		public static Image Coloring(Image srcImage, float r, float g, float b)
		{
			var matrixData = new float[5][] {
				new float[] { 1, 0, 0, 0, 0 },
				new float[] { 0, 1, 0, 0, 0 },
				new float[] { 0, 0, 1, 0, 0 },
				new float[] { 0, 0, 0, 1, 0 },
				new float[] { r, g, b, 0, 1 },
			};
			
			var colorMatrix = new ColorMatrix(matrixData);
			var imageAttribute = new ImageAttributes();
			imageAttribute.SetColorMatrix(colorMatrix);
			var imageSize = new Size(srcImage.Width, srcImage.Height);
			var alphaImage = new Bitmap(imageSize.Width, imageSize.Height);
			using(var graphics = Graphics.FromImage(alphaImage)) {
				graphics.DrawImage(
					srcImage,
					new Rectangle(Point.Empty, imageSize),
					0, 0, imageSize.Width, imageSize.Height,
					GraphicsUnit.Pixel,
					imageAttribute
				);
			}
			
			return alphaImage;
		}
	}
}
