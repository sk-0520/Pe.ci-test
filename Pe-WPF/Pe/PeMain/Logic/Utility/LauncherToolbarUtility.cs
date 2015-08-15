namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public static class LauncherToolbarUtility
	{
		public static FrameworkElement MakeScreenIcon(ScreenModel strongScreen, IconScale iconScale)
		{
			var screens = Screen.AllScreens.ToArray();
			var elements = new List<FrameworkElement>();
			var basePos = new Point(Math.Abs(screens.Min(s => s.DeviceBounds.Left)), Math.Abs(screens.Min(s => s.DeviceBounds.Top)));
			var iconSize = iconScale.ToSize();
			var drawSize = iconSize;
			var maxArea = new Rect() {
				X = screens.Min(s => s.DeviceBounds.Left),
				Y = screens.Min(s => s.DeviceBounds.Top)
			};
			maxArea.Width = Math.Abs(maxArea.X) + screens.Max(s => s.DeviceBounds.Right);
			maxArea.Height = Math.Abs(maxArea.Y) + screens.Max(s => s.DeviceBounds.Bottom);

			var percentage = new Size(
				drawSize.Width / maxArea.Width * 100.0f,
				drawSize.Height / maxArea.Height * 100.0f
			);

			var canvas = new Canvas() {
				Width = iconSize.Width,
				Height = iconSize.Height,
			};

			foreach(var screen in screens) {
				var useScreen = strongScreen.DeviceName == screen.DeviceName;
				var backColor = ImageUtility.GetToolbarPositionColor(true, useScreen);
				var foreColor = ImageUtility.GetToolbarPositionColor(false, useScreen);

				var baseArea = screen.DeviceBounds;
				baseArea.Offset(basePos.X, basePos.Y);

				var drawArea = new Rect(
					baseArea.X / 100.0 * percentage.Width,
					baseArea.Y / 100.0 * percentage.Height,
					baseArea.Width / 100.0 * percentage.Width,
					baseArea.Height / 100.0 * percentage.Height
				);

				var element = ImageUtility.CreateBox(foreColor, backColor, drawArea.Size);
				Canvas.SetLeft(element, drawArea.X);
				Canvas.SetTop(element, drawArea.Y);
				canvas.Children.Add(element);
			}

			//var result = ImageUtility.MakeBitmapBitmapSourceDefualtDpi(canvas);
			//return result;
			return canvas;
		}

		public static FrameworkElement MakeDockIcon(DockType dockType, Size imageSize)
		{
		var drawSize = new Size(0.2f, 0.3f);
			//using(var targetGraphics = CreateGraphics()) {
			//	var image = new Bitmap(imageSize.Width, imageSize.Height, targetGraphics);
				var drawArea = new Rect();
				switch(dockType) {
					case DockType.None:
						drawArea.Width = (int)(imageSize.Width * 0.8);
						drawArea.Height = (int)(imageSize.Height * 0.4);
						drawArea.X = imageSize.Width / 2 - drawArea.Width / 2;
						drawArea.Y = imageSize.Height / 2 - drawArea.Height / 2;
						break;
					case DockType.Left:
						drawArea.Width = (int)(imageSize.Width * drawSize.Width);
						drawArea.Height = imageSize.Height;
						drawArea.X = 0;
						drawArea.Y = imageSize.Height / 2 - drawArea.Height / 2;
						break;
					case DockType.Right:
						drawArea.Width = (int)(imageSize.Width * drawSize.Width);
						drawArea.Height = imageSize.Height;
						drawArea.X = imageSize.Width - drawArea.Width;
						drawArea.Y = imageSize.Height / 2 - drawArea.Height / 2;
						break;
					case DockType.Top:
						drawArea.Width = imageSize.Width;
						drawArea.Height = (int)(imageSize.Height * drawSize.Height);
						drawArea.X = imageSize.Width / 2 - drawArea.Width / 2;
						drawArea.Y = 0;
						break;
					case DockType.Bottom:
						drawArea.Width = imageSize.Width;
						drawArea.Height = (int)(imageSize.Height * drawSize.Height);
						drawArea.X = imageSize.Width / 2 - drawArea.Width / 2;
						drawArea.Y = imageSize.Height - drawArea.Height;
						break;
					default:
						throw new NotImplementedException();
				}


				var screenElement = ImageUtility.CreateBox(ImageUtility.GetToolbarPositionColor(false, false), ImageUtility.GetToolbarPositionColor(true, false), imageSize);
				var toolbarElement = ImageUtility.CreateBox(ImageUtility.GetToolbarPositionColor(false, true), ImageUtility.GetToolbarPositionColor(true, true), drawArea.Size);

			var canvas = new Canvas() {
				Width = imageSize.Width,
				Height = imageSize.Height,
			};
			canvas.Children.Add(screenElement);
			canvas.Children.Add(toolbarElement);
			Canvas.SetLeft(toolbarElement, drawArea.Location.X);
			Canvas.SetTop(toolbarElement, drawArea.Location.Y);

				//using(var g = Graphics.FromImage(image)) {
				//	using(var box = CommonData.Skin.CreateColorBoxImage(AppUtility.GetToolbarPositionColor(true, false), AppUtility.GetToolbarPositionColor(false, false), imageSize)) {
				//		g.DrawImage(box, Point.Empty);
				//	}
				//	using(var box = CommonData.Skin.CreateColorBoxImage(AppUtility.GetToolbarPositionColor(true, true), AppUtility.GetToolbarPositionColor(false, true), drawArea.Size)) {
				//		g.DrawImage(box, drawArea.Location);
				//	}
				//}

				//return image;
			return canvas;
		}
	}
}
