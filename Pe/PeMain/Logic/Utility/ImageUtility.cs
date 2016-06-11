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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    public static class ImageUtility
    {
        /// <summary>
        /// 箱形の要素を作成する。
        /// <para>同じようなものを共通の見栄えで作成するため細かい部分は内部実装に隠ぺいする。</para>
        /// </summary>
        /// <param name="borderColor">境界線の色。</param>
        /// <param name="backColor">背景色</param>
        /// <param name="size">サイズ。</param>
        /// <returns>生成された要素。</returns>
        public static FrameworkElement CreateBox(Color borderColor, Color backColor, Size size)
        {
            var box = new Rectangle();

            box.BeginInit();
            try {
                box.Width = size.Width;
                box.Height = size.Height;
                box.Stroke = new SolidColorBrush(borderColor);
                box.StrokeThickness = 1;
                box.Fill = new SolidColorBrush(backColor);
            } finally {
                box.EndInit();
            }

            return box;
        }

        /// <summary>
        /// 指定要素をビットマップに落とし込む。
        /// </summary>
        /// <param name="element"></param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        public static BitmapSource MakeBitmapSource(FrameworkElement element, Point dpi)
        {
            var size = new Size(element.Width, element.Height);

            element.Measure(size);
            element.Arrange(new Rect(size));

            var render = new RenderTargetBitmap((int)size.Width, (int)size.Height, dpi.X, dpi.Y, PixelFormats.Pbgra32);
            render.Render(element);

            return render;
        }

        /// <summary>
        /// 指定要素をビットマップに落とし込む。
        /// <para>DPIは96を使用する。</para>
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static BitmapSource MakeBitmapSourceDefualtDpi(FrameworkElement element)
        {
            return MakeBitmapSource(element, new Point(96, 96));
        }

        public static Color GetMenuIconColor(bool getBoxColor, bool isActiveColor)
        {
            byte alpha = 80;

            if(getBoxColor) {
                if(isActiveColor) {
                    return SystemColors.ActiveCaptionColor;
                } else {
                    var color = SystemColors.InactiveCaptionColor;
                    return Color.FromArgb(alpha, color.R, color.G, color.B);
                }
            } else {
                if(isActiveColor) {
                    return SystemColors.ActiveCaptionTextColor;
                } else {
                    var color = SystemColors.InactiveCaptionTextColor;
                    return Color.FromArgb(alpha, color.R, color.G, color.B);
                }
            }
        }

        public static FrameworkElement MakeOverlayImage(ImageSource parent, ImageSource child)
        {
            var canvas = new Canvas();
            canvas.BeginInit();
            try {
                canvas.Width = parent.Width;
                canvas.Height = parent.Height;

                var parentImage = new Image() {
                    Source = parent,
                };
                var childImage = new Image() {
                    Source = child,
                };

                canvas.Children.Add(parentImage);
                canvas.Children.Add(childImage);

                Canvas.SetLeft(childImage, parent.Width - child.Width);
                Canvas.SetTop(childImage, parent.Height - child.Height);

                return canvas;
            } finally {
                canvas.EndInit();
            }
        }
    }
}
