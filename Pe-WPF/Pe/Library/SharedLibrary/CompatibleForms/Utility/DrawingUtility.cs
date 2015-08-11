namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using Drawing = System.Drawing;

	public static class DrawingUtility
	{
		public static Drawing.Size Convert(Size size)
		{
			return new Drawing.Size((int)size.Width, (int)size.Height);
		}
		public static Size Convert(Drawing.Size size)
		{
			return new Size(size.Width, size.Height);
		}

		public static Drawing.Point Convert(Point point)
		{
			return new Drawing.Point((int)point.X, (int)point.Y);
		}
		public static Point Convert(Drawing.Point point)
		{
			return new Point(point.X, point.Y);
		}

		public static Drawing.Rectangle Convert(Rect rect)
		{
			return new Drawing.Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
		}
		public static Rect Convert(Drawing.Rectangle rectangle)
		{
			return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}

		public static double ConvertFontSizeFromDrawing(double drawingFontPoint)
		{
			return drawingFontPoint / 72.0 * 96.0;
		}
		public static float ConvertFontSizeFromWpf(double dpwFontSize)
		{
			return (float)(dpwFontSize * 72.0 / 96.0);
		}

	}
}
