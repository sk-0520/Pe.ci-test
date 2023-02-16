using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using Drawing = System.Drawing;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Forms
{
    /// <summary>
    /// <see cref="System.Drawing"/> のラッパー。
    /// </summary>
    public static class DrawingUtility
    {
        #region define

        private const double Dpi = 96;
        private const double Point = 72;

        #endregion

        public static Drawing.Size Convert(Size size)
        {
            return new Drawing.Size((int)size.Width, (int)size.Height);
        }
        public static Size Convert(Drawing.Size size)
        {
            return new Size(size.Width, size.Height);
        }

        public static Drawing.Point Convert(Point point)
        {
            return new Drawing.Point((int)point.X, (int)point.Y);
        }
        public static Point Convert(Drawing.Point point)
        {
            return new Point(point.X, point.Y);
        }

        public static Drawing.Rectangle Convert(Rect rect)
        {
            return new Drawing.Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }
        public static Rect Convert(Drawing.Rectangle rectangle)
        {
            return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static double ConvertFontSizeFromDrawing(double drawingFontPoint)
        {
            return drawingFontPoint * Dpi / Point;
        }
        public static float ConvertFontSizeFromWpf(double wpfFontSize)
        {
            return (float)(wpfFontSize / Dpi * Point);
        }

        public static BitmapSource ImageSourceFromIcon(Drawing.Icon icon)
        {
            var result = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );
            return result;
        }

        public static BitmapSource ImageSourceFromBinaryIcon(byte[] binayIcon, Size iconSize)
        {
            using(var ms = new MemoryReleaseStream(binayIcon)) {
                return ImageSourceFromBinaryStreamIcon(ms, iconSize);
            }
        }

        public static BitmapSource ImageSourceFromBinaryIcon(ArrayPoolObject<byte> binayIcon, Size iconSize)
        {
            using(var ms = new MemoryReleaseStream(binayIcon.Items, 0, binayIcon.Length)) {
                return ImageSourceFromBinaryStreamIcon(ms, iconSize);
            }
        }


        public static BitmapSource ImageSourceFromBinaryStreamIcon(Stream streamIcon, Size iconSize)
        {
            var size = new Drawing.Size((int)iconSize.Width, (int)iconSize.Height);
            using(var icon = new Drawing.Icon(streamIcon, size)) {
                return ImageSourceFromIcon(icon);
            }
        }


        public static Color Convert(Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
