using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Theme;
using ContentTypeTextNet.Pe.Main.View.Extend;

namespace ContentTypeTextNet.Pe.Main.Model.Theme
{
    public interface ILauncherToolbarTheme
    {
        #region function

        [return: PixelKind(Px.Logical)]
        Thickness GetButtonPadding(AppDesktopToolbarPosition toolbarPosition, IconScale iconScale);
        [return: PixelKind(Px.Logical)]
        Thickness GetIconMargin(AppDesktopToolbarPosition toolbarPosition, IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);

        [return: PixelKind(Px.Logical)]
        Size GetDisplaySize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);
        [return: PixelKind(Px.Logical)]
        Size GetHiddenSize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);

        DependencyObject GetToolbarImage(Screen currentScreen, IReadOnlyList<Screen> allScreens, IconScale iconScale, bool isStrong);
        DependencyObject GetToolbarPositionImage(AppDesktopToolbarPosition toolbarPosition, IconScale iconScale);

        #endregion
    }

    internal class LauncherToolbarTheme : ThemeBase, ILauncherToolbarTheme
    {
        public LauncherToolbarTheme(IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(dispatcherWapper, loggerFactory)
        { }

        #region property
        #endregion

        #region function

        DependencyObject GetToolbarImageCore(Screen currentScreen, IReadOnlyList<Screen> allScreens, IconScale iconScale, bool isStrong)
        {
            var basePos = new Point(Math.Abs(allScreens.Min(s => s.DeviceBounds.Left)), Math.Abs(allScreens.Min(s => s.DeviceBounds.Top)));
            var drawSize = iconScale.ToSize();
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
            using(Initializer.BeginInitialize(canvas)) {
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

        DependencyObject GetToolbarPositionImageCore(AppDesktopToolbarPosition toolbarPosition, IconScale iconScale)
        {
            var drawSize = iconScale.ToSize();
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

        #endregion

        #region ILauncherToolbarDesigner

        [return: PixelKind(Px.Logical)]
        public Thickness GetIconMargin(AppDesktopToolbarPosition toolbarPosition, IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            return new Thickness(2);
        }

        [return: PixelKind(Px.Logical)]
        public Thickness GetButtonPadding(AppDesktopToolbarPosition toolbarPosition, IconScale iconScale)
        {
            return new Thickness(2);
        }

        [return: PixelKind(Px.Logical)]
        public Size GetDisplaySize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            return new Size(
                GetHorizontal(buttonPadding) + GetHorizontal(iconMargin) + (int)iconScale + (isIconOnly ? 0 : textWidth),
                GetVertical(buttonPadding) + GetVertical(iconMargin) + (int)iconScale
            );
        }

        [return: PixelKind(Px.Logical)]
        public Size GetHiddenSize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            return new Size(4, 4);
        }

        public DependencyObject GetToolbarImage(Screen currentScreen, IReadOnlyList<Screen> allScreens, IconScale iconScale, bool isStrong)
        {
            return GetToolbarImageCore(currentScreen, allScreens, iconScale, isStrong);
        }

        public DependencyObject GetToolbarPositionImage(AppDesktopToolbarPosition toolbarPosition, IconScale iconScale)
        {
            return GetToolbarPositionImageCore(toolbarPosition, iconScale);
        }


        #endregion
    }
}
