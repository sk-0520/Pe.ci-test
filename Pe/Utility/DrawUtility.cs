namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Linq;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

	/// <summary>
	/// 描画等々の共通処理。
	/// </summary>
	public static class DrawUtility
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

		/// <summary>
		/// 塗りつぶす的な。
		/// </summary>
		/// <param name="srcImage"></param>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <returns></returns>
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
			using(var imageAttribute = new ImageAttributes()) {
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

		/// <summary>
		/// http://stackoverflow.com/questions/4779027/changing-the-opacity-of-a-bitmap-image
		/// </summary>
		/// <param name="srcImage"></param>
		/// <param name="opacity"></param>
		/// <returns></returns>
		public static Image Opacity(Image image, float opacity)
		{
			//create a Bitmap the size of the image provided  
			Bitmap bmp = new Bitmap(image.Width, image.Height);

			//create a graphics object from the image  
			using(Graphics gfx = Graphics.FromImage(bmp)) {

				//create a color matrix object  
				ColorMatrix matrix = new ColorMatrix();

				//set the opacity  
				matrix.Matrix33 = opacity;

				//create image attributes  
				using(ImageAttributes attributes = new ImageAttributes()) {
					//set the color(opacity) of the image  
					attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

					//now draw the image  
					gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
				}
			}

			return bmp;
		}

		/// <summary>
		/// http://stackoverflow.com/questions/2031217/what-is-the-fastest-way-i-can-compare-two-equal-size-bitmaps-to-determine-whethe
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		unsafe static bool IsEqualBitmap(Bitmap a, Bitmap b)
		{
			Debug.Assert(a != null);
			Debug.Assert(b != null);
			Debug.Assert(a.Size == b.Size);
			Debug.Assert(a.PixelFormat == b.PixelFormat);

			var imageArea = new Rectangle(new Point(0, 0), a.Size);

			var bmpDataA = a.LockBits(imageArea, ImageLockMode.ReadOnly, a.PixelFormat);
			try {
				var bmpDataB = b.LockBits(imageArea, ImageLockMode.ReadOnly, b.PixelFormat);
				try {
					var ptrA = bmpDataA.Scan0;
					var ptrB = bmpDataB.Scan0;

					int stride = bmpDataA.Stride;
					int length = stride * bmpDataA.Height;

					var cmpResult = NativeMethods.memcmp(ptrA, ptrB, length);
					Debug.WriteLine("cmpResult: " + cmpResult);
					return cmpResult == 0;
				} finally {
					b.UnlockBits(bmpDataB);
				}
			} finally {
				a.UnlockBits(bmpDataA);
			}
		}

		public static bool IsEqualImage(Image a, Image b)
		{
			if(a == null) {
				throw new ArgumentNullException("a");
			}
			if(b == null) {
				throw new ArgumentNullException("b");
			}

			if(a.Size != b.Size) {
				return false;
			}

			if(a.PixelFormat != b.PixelFormat) {
				return false;
			}

			var typeA = a.GetType();
			var typeB = b.GetType();

			if(typeA == typeof(Bitmap) && typeB == typeof(Bitmap)) {
				var bmpA = (Bitmap)a;
				var bmpB = (Bitmap)b;
				return IsEqualBitmap(bmpA, bmpB);
			}

			return false;
		}
	}
}
