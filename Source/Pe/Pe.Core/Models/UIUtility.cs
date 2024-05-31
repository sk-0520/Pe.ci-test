using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// 要素の属する(?)DPIを出力。
    /// </summary>
    /// <remarks>
    /// <para>ウィンドウのいるディスプレイのDPIを出力する感じ。</para>
    /// </remarks>
    public interface IDpiScaleOutpour
    {
        #region function

        /// <summary>
        /// 96 px に対する現在 DPI スケール。
        /// </summary>
        /// <returns></returns>
        Point GetDpiScale();
        /// <summary>
        /// 所属しているディスプレイの取得。
        /// </summary>
        /// <returns></returns>
        IScreen GetOwnerScreen();

        #endregion
    }

    /// <summary>
    /// 空の <see cref="IDpiScaleOutpour"/>。
    /// </summary>
    /// <remarks>
    /// <para>固定値を取得する。</para>
    /// </remarks>
    public sealed class EmptyDpiScaleOutpour: IDpiScaleOutpour
    {
        #region IDpiScaleOutputor

        /// <inheritdoc cref="IDpiScaleOutpour.GetDpiScale"/>
        public Point GetDpiScale() => new Point(1, 1);
        /// <inheritdoc cref="IDpiScaleOutpour.GetOwnerScreen"/>
        public IScreen GetOwnerScreen() => Screen.PrimaryScreen ?? throw new InvalidOperationException("Screen.PrimaryScreen is null");

        #endregion
    }

    /// <summary>
    /// WPF における UI をサポート。
    /// </summary>
    public static class UIUtility
    {
        #region variable

        /// <summary>
        /// デフォルトDPI(X)。
        /// </summary>
        public const int DefaultDpiX = 96;
        /// <summary>
        /// デフォルトDPI(Y)。
        /// </summary>
        public const int DefaultDpiY = 96;

        #endregion

        #region property

        /// <summary>
        /// デフォルトDPI。
        /// </summary>
        /// <remarks><see cref="Size"/>を使うべきかもしれないけど X/Y の名称を使いたいので <see cref="Point"/> を使用している。</remarks>
        public static Point DefaultDpi { get; } = new Point(DefaultDpiX, DefaultDpiY);

        #endregion

        #region function

        /// <summary>
        /// 表示要素の子孫を取得する。
        /// </summary>
        /// <remarks>
        /// <para>http://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type</para>
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject dependencyObject)
            where T : DependencyObject
        {
            if(dependencyObject is null) {
                yield break;
            }

            var childCount = VisualTreeHelper.GetChildrenCount(dependencyObject);
            for(int i = 0; i < childCount; i++) {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);
                if(child is null) {
                    continue;
                }

                if(child is T childObj) {
                    yield return childObj;
                }

                foreach(var childOfChild in FindVisualChildren<T>(child)) {
                    yield return childOfChild;
                }
            }
        }

        /// <summary>
        /// 論理ツリーから子孫を取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject dependencyObject)
            where T : DependencyObject
        {
            if(dependencyObject != null) {
                foreach(var child in LogicalTreeHelper.GetChildren(dependencyObject).OfType<DependencyObject>()) {
                    if(child != null) {
                        if(child is T childObj) {
                            yield return childObj;
                        }
                        foreach(var childOfChild in FindLogicalChildren<T>(child)) {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 表示要素・論理要素から子孫を取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindChildren<T>(DependencyObject dependencyObject)
            where T : DependencyObject
        {
            return FindLogicalChildren<T>(dependencyObject).Concat(FindVisualChildren<T>(dependencyObject));
        }

        /// <summary>
        /// 指定要素の表示要素から親要素を取得する。
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static DependencyObject? GetVisualParent(DependencyObject dependencyObject)
        {
            return VisualTreeHelper.GetParent(dependencyObject);
        }

        /// <summary>
        /// 指定要素の表示要素から親要素を取得する。
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static DependencyObject? GetLogicalParent(DependencyObject dependencyObject)
        {
            return LogicalTreeHelper.GetParent(dependencyObject);
        }

        /// <summary>
        /// 表示要素・論理要素から親要素を取得する。
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static DependencyObject? GetParent(DependencyObject dependencyObject)
        {
            return GetLogicalParent(dependencyObject) ?? GetVisualParent(dependencyObject);
        }

        /// <summary>
        /// 指定要素の表示要素から指定した祖先要素を取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static T? GetVisualClosest<T>(DependencyObject dependencyObject)
            where T : DependencyObject
        {
            if(dependencyObject == null) {
                return null;
            }

            var parent = GetVisualParent(dependencyObject);
            if(parent is T element) {
                return element;
            } else if(parent != null) {
                return GetVisualClosest<T>(parent);
            }
            return null;
        }

        /// <summary>
        /// 指定要素の論理要素から指定した祖先要素を取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static T? GetLogicalClosest<T>(DependencyObject dependencyObject)
            where T : DependencyObject
        {
            if(dependencyObject == null) {
                return null;
            }

            var parent = GetLogicalParent(dependencyObject);
            if(parent is T element) {
                return element;
            } else if(parent != null) {
                return GetLogicalClosest<T>(parent);
            }
            return null;
        }

        /// <summary>
        /// 指定要素の表示要素・論理要素から指定した祖先要素を取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static T? GetClosest<T>(DependencyObject dependencyObject)
            where T : DependencyObject
        {
            if(dependencyObject == null) {
                return null;
            }

            var parent = GetLogicalParent(dependencyObject) ?? GetVisualParent(dependencyObject);
            if(parent is T element) {
                return element;
            } else if(parent != null) {
                return GetLogicalClosest<T>(parent) ?? GetVisualClosest<T>(parent);
            }
            return null;
        }

        /// <summary>
        /// 表示要素から親となる<see cref="Window"/>を取得する。
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Window GetRootView(DependencyObject dependencyObject)
        {
            if(dependencyObject is Window) {
                throw new ArgumentException("window", nameof(dependencyObject));
            };

            var window = GetClosest<Window>(dependencyObject);

            if(window is null) {
                throw new ArgumentException("root is not window", nameof(dependencyObject));
            }

            return window;
        }

        /// <summary>
        /// 対象要素群の<see cref="FrameworkElement.ApplyTemplate"/> を再帰的に呼び出し。
        /// </summary>
        /// <param name="elements"></param>
        public static void RecursiveApplyTemplate(IEnumerable<FrameworkElement> elements)
        {
            foreach(var element in elements) {
                element.ApplyTemplate();
                RecursiveApplyTemplate(FindChildren<FrameworkElement>(element));
            }
        }
        /// <summary>
        /// 対象要素の<see cref="FrameworkElement.ApplyTemplate"/> を再帰的に呼び出し。
        /// </summary>
        /// <param name="element"></param>
        public static void RecursiveApplyTemplate(FrameworkElement element) => RecursiveApplyTemplate(new[] { element });

        /// <summary>
        /// スケールの取得。
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

        /// <summary>
        /// 物理ピクセルに変換。
        /// </summary>
        /// <param name="value">論理ピクセル。</param>
        /// <param name="dpiScale"></param>
        /// <returns></returns>
        [return: PixelKind(Px.Device)]
        public static double ToDevicePixel([PixelKind(Px.Logical)] double value, double dpiScale) => value * dpiScale;
        /// <summary>
        /// 論理ピクセルに変換
        /// </summary>
        /// <param name="value">物理ピクセル。</param>
        /// <param name="dpiScale"></param>
        /// <returns></returns>
        [return: PixelKind(Px.Logical)]
        public static double ToLogicalPixel([PixelKind(Px.Device)] double value, double dpiScale) => value / dpiScale;

        /// <inheritdoc cref="ToDevicePixel(double, double)"/>
        [return: PixelKind(Px.Device)]
        public static Point ToDevicePixel([PixelKind(Px.Logical)] Point point, Visual visual)
        {
            var dpiScale = GetDpiScale(visual);

            return new Point(
                point.X * dpiScale.X,
                point.Y * dpiScale.Y
            );
        }

        /// <inheritdoc cref="ToLogicalPixel(double, double)"/>
        [return: PixelKind(Px.Logical)]
        public static Point ToLogicalPixel([PixelKind(Px.Device)] Point point, Point dpiScale)
        {
            return new Point(
                point.X / dpiScale.X,
                point.Y / dpiScale.Y
            );
        }
        /// <inheritdoc cref="ToLogicalPixel(double, double)"/>
        [return: PixelKind(Px.Logical)]
        public static Point ToLogicalPixel([PixelKind(Px.Device)] Point point, IDpiScaleOutpour dpiScaleOutpour) => ToLogicalPixel(point, dpiScaleOutpour.GetDpiScale());
        /// <inheritdoc cref="ToLogicalPixel(double, double)"/>
        [return: PixelKind(Px.Logical)]
        public static Point ToLogicalPixel([PixelKind(Px.Device)] Point point, Visual visual) => ToLogicalPixel(point, GetDpiScale(visual));

        /// <inheritdoc cref="ToDevicePixel(double, double)"/>
        [return: PixelKind(Px.Device)]
        public static Size ToDevicePixel([PixelKind(Px.Logical)] Size size, Point dpiScale)
        {
            return new Size(
                size.Width * dpiScale.X,
                size.Height * dpiScale.Y
            );
        }
        /// <inheritdoc cref="ToDevicePixel(double, double)"/>
        [return: PixelKind(Px.Device)]
        public static Size ToDevicePixel([PixelKind(Px.Logical)] Size size, IDpiScaleOutpour dpiScaleOutpour) => ToDevicePixel(size, dpiScaleOutpour.GetDpiScale());
        /// <inheritdoc cref="ToDevicePixel(double, double)"/>
        [return: PixelKind(Px.Device)]
        public static Size ToDevicePixel([PixelKind(Px.Logical)] Size size, Visual visual) => ToDevicePixel(size, GetDpiScale(visual));

        /// <inheritdoc cref="ToLogicalPixel(double, double)"/>
        [return: PixelKind(Px.Logical)]
        public static Size ToLogicalPixel([PixelKind(Px.Device)] Size size, Point dpiScale)
        {
            return new Size(
                size.Width / dpiScale.X,
                size.Height / dpiScale.Y
            );
        }
        /// <inheritdoc cref="ToLogicalPixel(double, double)"/>
        [return: PixelKind(Px.Logical)]
        public static Size ToLogicalPixel([PixelKind(Px.Device)] Size size, IDpiScaleOutpour dpiScaleOutpour) => ToLogicalPixel(size, dpiScaleOutpour.GetDpiScale());
        /// <inheritdoc cref="ToLogicalPixel(double, double)"/>
        [return: PixelKind(Px.Logical)]
        public static Size ToLogicalPixel([PixelKind(Px.Device)] Size size, Visual visual) => ToLogicalPixel(size, GetDpiScale(visual));

        /// <inheritdoc cref="ToDevicePixel(double, double)"/>
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
        /// <inheritdoc cref="ToDevicePixel(double, double)"/>
        [return: PixelKind(Px.Device)]
        public static Rect ToDevicePixel([PixelKind(Px.Logical)] Rect rect, IDpiScaleOutpour dpiScaleOutpour) => ToDevicePixel(rect, dpiScaleOutpour.GetDpiScale());
        /// <inheritdoc cref="ToDevicePixel(double, double)"/>
        [return: PixelKind(Px.Device)]
        public static Rect ToDevicePixel([PixelKind(Px.Logical)] Rect rect, Visual visual) => ToDevicePixel(rect, GetDpiScale(visual));

        /// <inheritdoc cref="ToLogicalPixel(double, double)"/>
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
        /// <inheritdoc cref="ToLogicalPixel(double, double)"/>
        [return: PixelKind(Px.Logical)]
        public static Rect ToLogicalPixel([PixelKind(Px.Device)] Rect rect, IDpiScaleOutpour dpiScaleOutpour) => ToLogicalPixel(rect, dpiScaleOutpour.GetDpiScale());
        /// <inheritdoc cref="ToLogicalPixel(double, double)"/>
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

            var exStyle = WindowsUtility.GetWindowLongPtr(hWnd, (int)GWL.GWL_EXSTYLE);
            //exStyle = WindowsUtility.AddIntPtr(exStyle, (nint)WS_EX.WS_EX_TOOLWINDOW);
            exStyle = exStyle | (nint)WS_EX.WS_EX_TOOLWINDOW;
            WindowsUtility.SetWindowLongPtr(hWnd, (int)GWL.GWL_EXSTYLE, exStyle);

            if(!enabledMaximizeBox || !enabledMinimizeBox) {
                var style = WindowsUtility.GetWindowLongPtr(hWnd, (int)GWL.GWL_STYLE);
                WS ws = (WS)0;
                if(!enabledMaximizeBox) {
                    ws |= WS.WS_MAXIMIZEBOX;
                }
                if(!enabledMinimizeBox) {
                    ws |= WS.WS_MINIMIZEBOX;
                }
                style &= ~(nint)ws;
                WindowsUtility.SetWindowLongPtr(hWnd, (int)GWL.GWL_STYLE, style);
            }
        }

        /// <summary>
        /// クリック無効なやつにする。
        /// </summary>
        /// <param name="window"></param>
        /// <param name="isTransparent"></param>
        public static void ChangeTransparent(Window window, bool isTransparent)
        {
            var hWnd = HandleUtility.GetWindowHandle(window);

            int exStyle = (int)WindowsUtility.GetWindowLongPtr(hWnd, (int)GWL.GWL_EXSTYLE);
            if(isTransparent) {
                exStyle |= (int)WS_EX.WS_EX_TRANSPARENT;
            } else {
                exStyle &= ~(int)WS_EX.WS_EX_TRANSPARENT;
            }
            WindowsUtility.SetWindowLongPtr(hWnd, (int)GWL.GWL_EXSTYLE, (IntPtr)exStyle);
        }

        #endregion
    }
}
