using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar
{
    public class LauncherToolbarIconMaker: ViewElementMakerBase
    {
        #region function

        private DependencyObject GetToolbarImageCore(IScreen currentScreen, IReadOnlyList<IScreen> allScreens, IconBox iconBox, bool isStrong)
        {
            var basePos = new Point(Math.Abs(allScreens.Min(s => s.DeviceBounds.Left)), Math.Abs(allScreens.Min(s => s.DeviceBounds.Top)));
            //var screenOperator = new ScreenOperator(NullLoggerFactory.Instance);
            // WPF上の表示にお任せなのでDPIは96固定でOK
            var drawSize = new IconSize(iconBox, IconSize.DefaultScale);
            var maxArea = new Rect() {
                X = allScreens.Min(s => s.DeviceBounds.Left),
                Y = allScreens.Min(s => s.DeviceBounds.Top)
            };
            maxArea.Width = Math.Abs(maxArea.X) + allScreens.Max(s => s.DeviceBounds.Right);
            maxArea.Height = Math.Abs(maxArea.Y) + allScreens.Max(s => s.DeviceBounds.Bottom);

            var percentage = new Size(
                drawSize.Width / maxArea.Width * 100.0,
                drawSize.Height / maxArea.Height * 100.0
            );

            var canvas = new Canvas();
            using(Initializer.Begin(canvas)) {
                canvas.Width = drawSize.Width;
                canvas.Height = drawSize.Height;

                foreach(var screen in allScreens) {
                    var useScreen = currentScreen.DeviceName == screen.DeviceName;
                    var fillColor = GetMenuImageColor(true, useScreen);
                    var borderColor = GetMenuImageColor(false, useScreen);

                    var baseArea = screen.DeviceBounds;
                    baseArea.Offset(basePos.X, basePos.Y);

                    var drawArea = new Rect(
                        baseArea.X / 100.0 * percentage.Width,
                        baseArea.Y / 100.0 * percentage.Height,
                        baseArea.Width / 100.0 * percentage.Width,
                        baseArea.Height / 100.0 * percentage.Height
                    );

                    var element = CreateBox(borderColor, fillColor, drawArea.Size);
                    Canvas.SetLeft(element, drawArea.X);
                    Canvas.SetTop(element, drawArea.Y);
                    canvas.Children.Add(element);
                }

                if(isStrong) {
                    canvas.Effect = GetStrongEffect();
                }
            }
            //var result = ImageUtility.MakeBitmapBitmapSourceDefualtDpi(canvas);
            //return result;
            return canvas;
        }

        private DependencyObject GetToolbarPositionImageCore(AppDesktopToolbarPosition toolbarPosition, IconBox iconBox)
        {
            var drawSize = new Size((int)iconBox * 2, (int)iconBox);
            var strongSize = new Size(0.2f, 0.3f);
            //using(var targetGraphics = CreateGraphics()) {
            //	var image = new Bitmap(imageSize.Width, imageSize.Height, targetGraphics);
            var drawArea = new Rect();
            switch(toolbarPosition) {

                case AppDesktopToolbarPosition.Left:
                    drawArea.Width = (int)(drawSize.Width * strongSize.Width);
                    drawArea.Height = drawSize.Height;
                    drawArea.X = 0;
                    drawArea.Y = drawSize.Height / 2 - drawArea.Height / 2;
                    break;

                case AppDesktopToolbarPosition.Right:
                    drawArea.Width = (int)(drawSize.Width * strongSize.Width);
                    drawArea.Height = drawSize.Height;
                    drawArea.X = drawSize.Width - drawArea.Width;
                    drawArea.Y = drawSize.Height / 2 - drawArea.Height / 2;
                    break;

                case AppDesktopToolbarPosition.Top:
                    drawArea.Width = drawSize.Width;
                    drawArea.Height = (int)(drawSize.Height * strongSize.Height);
                    drawArea.X = drawSize.Width / 2 - drawArea.Width / 2;
                    drawArea.Y = 0;
                    break;

                case AppDesktopToolbarPosition.Bottom:
                    drawArea.Width = drawSize.Width;
                    drawArea.Height = (int)(drawSize.Height * strongSize.Height);
                    drawArea.X = drawSize.Width / 2 - drawArea.Width / 2;
                    drawArea.Y = drawSize.Height - drawArea.Height;
                    break;

                default:
                    throw new NotImplementedException();
            }


            var screenElement = CreateBox(GetMenuImageColor(false, false), GetMenuImageColor(true, false), drawSize);
            var toolbarElement = CreateBox(GetMenuImageColor(false, true), GetMenuImageColor(true, true), drawArea.Size);

            var canvas = new Canvas() {
                Width = drawSize.Width,
                Height = drawSize.Height,
            };
            canvas.Children.Add(screenElement);
            canvas.Children.Add(toolbarElement);
            Canvas.SetLeft(toolbarElement, drawArea.Location.X);
            Canvas.SetTop(toolbarElement, drawArea.Location.Y);

            return canvas;
        }

        public DependencyObject GetToolbarImage(IScreen currentScreen, IReadOnlyList<IScreen> allScreens, IconBox iconBox, bool isStrong)
        {
            return GetToolbarImageCore(currentScreen, allScreens, iconBox, isStrong);
        }

        public DependencyObject GetToolbarPositionImage(AppDesktopToolbarPosition toolbarPosition, IconBox iconBox)
        {
            return GetToolbarPositionImageCore(toolbarPosition, iconBox);
        }

        #endregion
    }
}
