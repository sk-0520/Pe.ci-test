using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public interface IDpiScaleOutputor
    {
        #region function

        Point GetDpiScale();

        #endregion
    }

    public sealed class EmptyDpiScaleOutputor : IDpiScaleOutputor
    {
        #region IDpiScaleOutputor

        public Point GetDpiScale() => new Point(1, 1);

        #endregion
    }

    /// <summary>
    /// WPF における UI をサポート。
    /// </summary>
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
                    if(child != null) {
                        foreach(var childOfChild in FindVisualChildren<T>(child)) {
                            yield return childOfChild;
                        }
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
        public static T? GetVisualClosest<T>(DependencyObject depObj)
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
        public static double ToDevicePixel([PixelKind(Px.Logical)] double value, double dpiScale) => value * dpiScale;

        [return: PixelKind(Px.Logical)]
        public static double ToLogicalPixel([PixelKind(Px.Device)] double value, double dpiScale) => value / dpiScale;

        [return: PixelKind(Px.Device)]
        public static Point ToDevicePixel([PixelKind(Px.Logical)] Point point, Visual visual)
        {
            var dpiScale = GetDpiScale(visual);

            return new Point(
                point.X * dpiScale.X,
                point.Y * dpiScale.Y
            );
        }

        [return: PixelKind(Px.Logical)]
        public static Point ToLogicalPixel([PixelKind(Px.Device)] Point point, Point dpiScale)
        {
            return new Point(
                point.X / dpiScale.X,
                point.Y / dpiScale.Y
            );
        }
        [return: PixelKind(Px.Logical)]
        public static Point ToLogicalPixel([PixelKind(Px.Device)] Point point, IDpiScaleOutputor dpiScaleOutputor) => ToLogicalPixel(point, dpiScaleOutputor.GetDpiScale());
        [return: PixelKind(Px.Logical)]
        public static Point ToLogicalPixel([PixelKind(Px.Device)] Point point, Visual visual) => ToLogicalPixel(point, GetDpiScale(visual));

        [return: PixelKind(Px.Device)]
        public static Size ToDevicePixel([PixelKind(Px.Logical)] Size size, Point dpiScale)
        {
            return new Size(
                size.Width * dpiScale.X,
                size.Height * dpiScale.Y
            );
        }
        [return: PixelKind(Px.Device)]
        public static Size ToDevicePixel([PixelKind(Px.Logical)] Size size, IDpiScaleOutputor dpiScaleOutputor) => ToDevicePixel(size, dpiScaleOutputor.GetDpiScale());
        [return: PixelKind(Px.Device)]
        public static Size ToDevicePixel([PixelKind(Px.Logical)] Size size, Visual visual) => ToDevicePixel(size, GetDpiScale(visual));

        [return: PixelKind(Px.Logical)]
        public static Size ToLogicalPixel([PixelKind(Px.Device)] Size size, Point dpiScale)
        {
            return new Size(
                size.Width / dpiScale.X,
                size.Height / dpiScale.Y
            );
        }
        [return: PixelKind(Px.Logical)]
        public static Size ToLogicalPixel([PixelKind(Px.Device)] Size size, IDpiScaleOutputor dpiScaleOutputor) => ToLogicalPixel(size, dpiScaleOutputor.GetDpiScale());
        [return: PixelKind(Px.Logical)]
        public static Size ToLogicalPixel([PixelKind(Px.Device)] Size size, Visual visual) => ToLogicalPixel(size, GetDpiScale(visual));

        [return: PixelKind(Px.Device)]
        public static Rect ToDevicePixel([PixelKind(Px.Logical)] Rect rect, Point dpiScale)
        {
            return new Rect(
                rect.X * dpiScale.X,
                rect.Y * dpiScale.Y,
                rect.Width * dpiScale.X,
                rect.Height * dpiScale.Y
            );
        }
        [return: PixelKind(Px.Device)]
        public static Rect ToDevicePixel([PixelKind(Px.Logical)] Rect rect, IDpiScaleOutputor dpiScaleOutputor) => ToDevicePixel(rect, dpiScaleOutputor.GetDpiScale());
        [return: PixelKind(Px.Device)]
        public static Rect ToDevicePixel([PixelKind(Px.Logical)] Rect rect, Visual visual) => ToDevicePixel(rect, GetDpiScale(visual));

        [return: PixelKind(Px.Logical)]
        public static Rect ToLogicalPixel([PixelKind(Px.Device)] Rect rect, Point dpiScale)
        {
            return new Rect(
                rect.X / dpiScale.X,
                rect.Y / dpiScale.Y,
                rect.Width / dpiScale.X,
                rect.Height / dpiScale.Y
            );
        }
        [return: PixelKind(Px.Logical)]
        public static Rect ToLogicalPixel([PixelKind(Px.Device)] Rect rect, IDpiScaleOutputor dpiScaleOutputor) => ToLogicalPixel(rect, dpiScaleOutputor.GetDpiScale());
        [return: PixelKind(Px.Logical)]
        public static Rect ToLogicalPixel([PixelKind(Px.Device)] Rect rect, Visual visual) => ToLogicalPixel(rect, GetDpiScale(visual));

        /// <summary>
        /// ツールウィンドウにする。
        /// </summary>
        /// <param name="window">対象ウィンドウ。</param>
        /// <param name="enabledMaximizeBox">最大化ボタンを有効にするか。</param>
        /// <param name="enabledMinimizeBox">最小化ボタンを有効にするか。</param>
        public static void SetToolWindowStyle(Window window, bool enabledMaximizeBox, bool enabledMinimizeBox)
        {
            var hWnd = HandleUtility.GetWindowHandle(window);

            int exStyle = (int)WindowsUtility.GetWindowLong(hWnd, (int)GWL.GWL_EXSTYLE);
            exStyle |= (int)WS_EX.WS_EX_TOOLWINDOW;
            WindowsUtility.SetWindowLong(hWnd, (int)GWL.GWL_EXSTYLE, (IntPtr)exStyle);

            if(!enabledMaximizeBox || !enabledMinimizeBox) {
                var style = (int)WindowsUtility.GetWindowLong(hWnd, (int)GWL.GWL_STYLE);
                WS ws = (WS)0;
                if(!enabledMaximizeBox) {
                    ws |= WS.WS_MAXIMIZEBOX;
                }
                if(!enabledMinimizeBox) {
                    ws |= WS.WS_MINIMIZEBOX;
                }
                style &= ~(int)ws;
                WindowsUtility.SetWindowLong(hWnd, (int)GWL.GWL_STYLE, (IntPtr)style);
            }
        }

        public static bool IsEnabledEventArea(DependencyObject dependencyObject, Type[] enableElementTypes, Type[] disableElementTypes)
        {
            if(enableElementTypes == null) {
                throw new ArgumentNullException(nameof(enableElementTypes));
            }
            if(disableElementTypes == null) {
                throw new ArgumentNullException(nameof(disableElementTypes));
            }

            // かなり TextSearchMatchControl に依存してる
            if(dependencyObject is System.Windows.Documents.Run) {
                return true;
            }

            while(dependencyObject != null) {
                var type = dependencyObject.GetType();
                if(disableElementTypes.Any(t => t == type)) {
                    return false;
                }
                if(enableElementTypes.Any(t => t == type)) {
                    return true;
                }
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            return false;
        }
        public static bool IsEnabledEventArea(DependencyObject dependencyObject, Type[] enableElementTypes)
        {
            return IsEnabledEventArea(dependencyObject, enableElementTypes, new Type[0]);
        }
    }
}
