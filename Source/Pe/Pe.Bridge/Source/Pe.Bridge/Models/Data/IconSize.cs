using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    /// <summary>
    /// アイコン固定サイズ。
    /// <para>名前の迷走よ。</para>
    /// </summary>
    public enum IconBox
    {
        /// <summary>
        /// 16px
        /// </summary>
        Small = 16,
        /// <summary>
        /// 32px
        /// </summary>
        Normal = 32,
        /// <summary>
        /// 48px
        /// </summary>
        Big = 48,
        /// <summary>
        /// 256px
        /// </summary>
        Large = 256,
    }

    /// <summary>
    /// アイコンサイズ。
    /// </summary>
    public readonly struct IconSize
    {
        public IconSize([PixelKind(Px.Device)] int width, [PixelKind(Px.Device)] int height)
        {
            if(width <= 0) {
                throw new ArgumentException(nameof(width));
            }
            if(height <= 0) {
                throw new ArgumentException(nameof(height));
            }

            Width = width;
            Height = height;
        }
        public IconSize([PixelKind(Px.Device)] int boxSize)
        {
            if(boxSize <= 0) {
                throw new ArgumentException(nameof(boxSize));
            }

            Width = Height = boxSize;
        }

        public IconSize(IconBox iconBox, Point iconScale)
        {
            // ここだけ X/Y 見るのもどうなんやろね
            Width = (int)((int)iconBox * iconScale.X);
            Height = (int)((int)iconBox * iconScale.Y);
        }


        #region property

        /// <summary>
        /// DPI が取得できないところで使用するしゃあなしDPIスケール。
        /// </summary>
        public static Point DefaultScale => new Point(1, 1);

        /// <summary>
        /// 横幅。
        /// </summary>
        [PixelKind(Px.Device)]
        public int Width { get; }
        /// <summary>
        /// 高さ。
        /// </summary>
        [PixelKind(Px.Device)]
        public int Height { get; }

        /// <summary>
        /// 正方形か。
        /// </summary>
        public readonly bool IsSquare => Width == Height;
        #endregion

        #region function

        /// <summary>
        /// <see cref="Size"/> への変換。
        /// </summary>
        /// <returns></returns>
        public readonly Size ToSize() => new Size(Width, Height);

        #endregion

        #region object

        public readonly override string? ToString() => $"{Width} x {Height}";

        #endregion
    }

    /// <summary>
    /// アイコンの大きさとDPIを持ち歩く。
    /// </summary>
    public readonly struct IconScale
    {
        public IconScale(IconBox box, Point scale)
        {
            if(double.IsNaN(scale.X) || double.IsInfinity(scale.X) || scale.X < 1) {
                throw new ArgumentException(nameof(scale) + "." + nameof(scale.X));
            }
            if(double.IsNaN(scale.Y) || double.IsInfinity(scale.Y) || scale.Y < 1) {
                throw new ArgumentException(nameof(scale) + "." + nameof(scale.Y));
            }

            Box = box;
            Scale = scale;
        }

        #region property

        /// <summary>
        /// アイコン基本サイズ。
        /// </summary>
        public IconBox Box { get; }

        /// <summary>
        /// DPIスケール。
        /// </summary>
        public Point Scale { get; }

        #endregion

        #region function

        /// <summary>
        /// 現在の設定値から<see cref="IconSize"/>を算出。
        /// </summary>
        /// <returns></returns>
        public readonly IconSize ToIconSize() => new IconSize(Box, Scale);

        #endregion

        #region funcrion

        public override string ToString()
        {
            return $"{nameof(IconScale)}: {Box}, {Scale.X}x{Scale.Y} -> {((int)Box) * Scale.X}x{((int)Box) * Scale.Y}";
        }

        #endregion
    }
}
