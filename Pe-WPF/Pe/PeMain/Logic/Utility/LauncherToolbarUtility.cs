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
	}
}
