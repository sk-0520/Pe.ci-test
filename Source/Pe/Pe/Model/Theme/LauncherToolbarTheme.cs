using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.View.Extend;

namespace ContentTypeTextNet.Pe.Main.Model.Designer
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

        DependencyObject CreateToolbarImage(Screen currentScreen, IReadOnlyList<Screen> allScreens, IconScale iconScale);

        #endregion
    }

    public class LauncherToolbarTheme : ThemeBase, ILauncherToolbarTheme
    {
        public LauncherToolbarTheme(IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            DispatcherWapper = dispatcherWapper;
        }

        #region property

        IDispatcherWapper DispatcherWapper { get; }

        #endregion

        #region function

        DependencyObject CreateToolbarImageCore(Screen currentScreen, IReadOnlyList<Screen> allScreens, IconScale iconScale)
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
            }
            //var result = ImageUtility.MakeBitmapBitmapSourceDefualtDpi(canvas);
            //return result;
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

        public DependencyObject CreateToolbarImage(Screen currentScreen, IReadOnlyList<Screen> allScreens, IconScale iconScale)
        {
            return DispatcherWapper.Get(() => CreateToolbarImageCore(currentScreen, allScreens, iconScale));
        }

        #endregion
    }
}
