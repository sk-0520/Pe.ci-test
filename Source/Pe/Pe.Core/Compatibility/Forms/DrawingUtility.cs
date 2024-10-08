using System;
using System.IO;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Forms
{
    /// <summary>
    /// <see cref="System.Drawing"/> のラッパー。
    /// </summary>
    public static class DrawingUtility
    {
        /// <summary>
        /// <see cref="System.Windows.Size"/> から <see cref="System.Drawing.Size"/> に変換。
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static System.Drawing.Size Convert(System.Windows.Size size)
        {
            return new System.Drawing.Size((int)size.Width, (int)size.Height);
        }
        /// <summary>
        /// <see cref="System.Drawing.Size"/> から <see cref="System.Windows.Size"/> に変換。
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static System.Windows.Size Convert(System.Drawing.Size size)
        {
            return new System.Windows.Size(size.Width, size.Height);
        }

        /// <summary>
        /// <see cref="System.Windows.Point"/> から <see cref="System.Drawing.Point"/> に変換。
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static System.Drawing.Point Convert(System.Windows.Point point)
        {
            return new System.Drawing.Point((int)point.X, (int)point.Y);
        }
        /// <summary>
        /// <see cref="System.Drawing.Point"/> から <see cref="System.Windows.Point"/> に変換。
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static System.Windows.Point Convert(System.Drawing.Point point)
        {
            return new System.Windows.Point(point.X, point.Y);
        }

        /// <summary>
        /// <see cref="System.Windows.Rect"/> から <see cref="System.Drawing.Rectangle"/> に変換。
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static System.Drawing.Rectangle Convert(System.Windows.Rect rect)
        {
            return new System.Drawing.Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }
        /// <summary>
        /// <see cref="System.Drawing.Rectangle"/> から <see cref="System.Windows.Rect"/> に変換。
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static System.Windows.Rect Convert(System.Drawing.Rectangle rectangle)
        {
            return new System.Windows.Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// <see cref="System.Drawing.Icon"/> を <see cref="BitmapSource"/> に変換。
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static BitmapSource ImageSourceFromIcon(System.Drawing.Icon icon)
        {
            var result = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );
            return result;
        }

        public static BitmapSource ImageSourceFromBinaryIcon(byte[] binaryIcon, System.Windows.Size iconSize)
        {
            using(var ms = new MemoryReleaseStream(binaryIcon)) {
                return ImageSourceFromBinaryStreamIcon(ms, iconSize);
            }
        }

        public static BitmapSource ImageSourceFromBinaryIcon(ArrayPoolObject<byte> binaryIcon, System.Windows.Size iconSize)
        {
            using(var ms = new MemoryReleaseStream(binaryIcon.Items, 0, binaryIcon.Length)) {
                return ImageSourceFromBinaryStreamIcon(ms, iconSize);
            }
        }


        public static BitmapSource ImageSourceFromBinaryStreamIcon(Stream streamIcon, System.Windows.Size iconSize)
        {
            var size = new System.Drawing.Size((int)iconSize.Width, (int)iconSize.Height);
            using(var icon = new System.Drawing.Icon(streamIcon, size)) {
                return ImageSourceFromIcon(icon);
            }
        }


        public static System.Windows.Media.Color Convert(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
