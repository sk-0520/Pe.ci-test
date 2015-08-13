namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Shapes;

	public static class ImageUtility
	{
		public static FrameworkElement CreateBox(Color borderColor, Color backColor, Size size)
		{
			var box = new Rectangle() {
				Width = size.Width,
				Height = size.Height,
				Stroke = new SolidColorBrush(borderColor),
				StrokeThickness = 1,
				Fill = new SolidColorBrush(backColor),
			};

			return box;
		}

		public static BitmapSource MakeBitmapBitmapSource(FrameworkElement element, Point dpi)
		{
			var size = new Size(element.Width, element.Height);

			element.Measure(size);
			element.Arrange(new Rect(size));

			var render = new RenderTargetBitmap((int)size.Width, (int)size.Height, dpi.X, dpi.Y, PixelFormats.Pbgra32);
			render.Render(element);

			return render;
		}

		public static BitmapSource MakeBitmapBitmapSourceDefualtDpi(FrameworkElement element)
		{
			return MakeBitmapBitmapSource(element, new Point(96, 96));
		}
	}
}
