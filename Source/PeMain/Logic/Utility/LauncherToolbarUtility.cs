/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
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

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
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
                var backColor = ImageUtility.GetMenuIconColor(true, useScreen);
                var foreColor = ImageUtility.GetMenuIconColor(false, useScreen);

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


            var screenElement = ImageUtility.CreateBox(ImageUtility.GetMenuIconColor(false, false), ImageUtility.GetMenuIconColor(true, false), imageSize);
            var toolbarElement = ImageUtility.CreateBox(ImageUtility.GetMenuIconColor(false, true), ImageUtility.GetMenuIconColor(true, true), drawArea.Size);

            var canvas = new Canvas() {
                Width = imageSize.Width,
                Height = imageSize.Height,
            };
            canvas.Children.Add(screenElement);
            canvas.Children.Add(toolbarElement);
            Canvas.SetLeft(toolbarElement, drawArea.Location.X);
            Canvas.SetTop(toolbarElement, drawArea.Location.Y);

            return canvas;
        }
    }
}
