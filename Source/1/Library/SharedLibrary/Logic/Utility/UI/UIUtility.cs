/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Define;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI
{
    public static class UIUtility
    {
        /// <summary>
        /// <para>http://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
            where T : DependencyObject
        {
            if(depObj != null) {
                var childCount = VisualTreeHelper.GetChildrenCount(depObj);
                for(int i = 0; i < childCount; i++) {
                    var child = VisualTreeHelper.GetChild(depObj, i);
                    if(child != null) {
                        var childObj = child as T;
                        if(childObj != null) {
                            yield return childObj;
                        }
                    }

                    foreach(var childOfChild in FindVisualChildren<T>(child)) {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj)
            where T : DependencyObject
        {
            if(depObj != null) {
                foreach(var child in LogicalTreeHelper.GetChildren(depObj).OfType<DependencyObject>()) {
                    if(child != null) {
                        var childObj = child as T;
                        if(childObj != null) {
                            yield return childObj;
                        }
                        foreach(var childOfChild in FindLogicalChildren<T>(child)) {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        public static IEnumerable<T> FindChildren<T>(DependencyObject depObj)
            where T : DependencyObject
        {
            return FindLogicalChildren<T>(depObj).Concat(FindVisualChildren<T>(depObj));
        }

        /// <summary>
        /// 指定要素の表示要素から親要素を取得する。
        /// </summary>
        /// <param name="depObj"></param>
        /// <returns></returns>
        public static DependencyObject GetVisualParent(DependencyObject depObj)
        {
            return VisualTreeHelper.GetParent(depObj);
        }

        /// <summary>
        /// 指定要素の表示要素から指定した祖先要素を取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
        public static DependencyObject GetVisualClosest<T>(DependencyObject depObj)
            where T : DependencyObject
        {
            if(depObj == null) {
                return null;
            }

            var parent = GetVisualParent(depObj);
            var element = parent as T;
            if(element != null) {
                return element;
            } else {
                return GetVisualClosest<T>(parent);
            }
        }

        public static void RecursiveApplyTemplate(IEnumerable<FrameworkElement> elements)
        {
            foreach(var element in elements) {
                element.ApplyTemplate();
                RecursiveApplyTemplate(FindChildren<FrameworkElement>(element));
            }
        }

        /// <summary>
        /// http://grabacr.net/archives/1105
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static Point GetDpiScale(Visual visual)
        {
            var source = PresentationSource.FromVisual(visual);
            if(source != null && source.CompositionTarget != null) {
                return new Point(
                    source.CompositionTarget.TransformToDevice.M11,
                    source.CompositionTarget.TransformToDevice.M22
                );
            }

            return new Point(1.0, 1.0);
        }

        [return: PixelKind(Px.Device)]
        public static double ToDevicePixelFromX(Visual visual, double x)
        {
            var dpiScale = GetDpiScale(visual);

            return x * dpiScale.X;
        }

        [return: PixelKind(Px.Logical)]
        public static double ToLogicalPixelX(Visual visual, double x)
        {
            var dpiScale = GetDpiScale(visual);

            return x / dpiScale.X;
        }

        [return: PixelKind(Px.Device)]
        public static double ToDevicePixelFromY(Visual visual, double y)
        {
            var dpiScale = GetDpiScale(visual);

            return y * dpiScale.Y;
        }

        [return: PixelKind(Px.Logical)]
        public static double ToLogicalPixelY(Visual visual, double y)
        {
            var dpiScale = GetDpiScale(visual);

            return y / dpiScale.Y;
        }

        [return: PixelKind(Px.Device)]
        public static Point ToDevicePixel(Visual visual, Point point)
        {
            var dpiScale = GetDpiScale(visual);

            return new Point(
                point.X * dpiScale.X,
                point.Y * dpiScale.Y
            );
        }

        [return: PixelKind(Px.Logical)]
        public static Point ToLogicalPixel(Visual visual, Point point)
        {
            var dpiScale = GetDpiScale(visual);

            return new Point(
                point.X / dpiScale.X,
                point.Y / dpiScale.Y
            );
        }

        [return: PixelKind(Px.Device)]
        public static Size ToDevicePixel(Visual visual, Size size)
        {
            var dpiScale = GetDpiScale(visual);

            return new Size(
                size.Width * dpiScale.X,
                size.Height * dpiScale.Y
            );
        }

        [return: PixelKind(Px.Logical)]
        public static Size ToLogicalPixel(Visual visual, Size size)
        {
            var dpiScale = GetDpiScale(visual);

            return new Size(
                size.Width / dpiScale.X,
                size.Height / dpiScale.Y
            );
        }

        [return: PixelKind(Px.Device)]
        public static Rect ToDevicePixel(Visual visual, Rect rect)
        {
            var dpiScale = GetDpiScale(visual);

            return new Rect(
                rect.X * dpiScale.X,
                rect.Y * dpiScale.Y,
                rect.Width * dpiScale.X,
                rect.Height * dpiScale.Y
            );
        }

        [return: PixelKind(Px.Logical)]
        public static Rect ToLogicalPixel(Visual visual, Rect rect)
        {
            var dpiScale = GetDpiScale(visual);

            return new Rect(
                rect.X / dpiScale.X,
                rect.Y / dpiScale.Y,
                rect.Width / dpiScale.X,
                rect.Height / dpiScale.Y
            );
        }

        public static void SetStyleToolWindow(Window window, bool enabledMax, bool enabledMin)
        {
            var hWnd = HandleUtility.GetWindowHandle(window);

            int exStyle = (int)WindowsUtility.GetWindowLong(hWnd, (int)GWL.GWL_EXSTYLE);
            exStyle |= (int)WS_EX.WS_EX_TOOLWINDOW;
            WindowsUtility.SetWindowLong(hWnd, (int)GWL.GWL_EXSTYLE, (IntPtr)exStyle);

            if(!enabledMax || !enabledMin) {
                var style = (int)WindowsUtility.GetWindowLong(hWnd, (int)GWL.GWL_STYLE);
                WS ws = (WS)0;
                if(!enabledMax) {
                    ws |= WS.WS_MAXIMIZEBOX;
                }
                if(!enabledMin) {
                    ws |= WS.WS_MINIMIZEBOX;
                }
                style &= ~(int)ws;
                WindowsUtility.SetWindowLong(hWnd, (int)GWL.GWL_STYLE, (IntPtr)style);
            }
        }
    }
}
