using System;
using System.Windows;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    /// <summary>
    /// アイコン固定サイズ。
    /// </summary>
    /// <remarks>
    /// <para>名前の迷走よ。</para>
    /// </remarks>
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
                throw new ArgumentException(null, nameof(width));
            }
            if(height <= 0) {
                throw new ArgumentException(null, nameof(height));
            }

            Width = width;
            Height = height;
        }

        public IconSize([PixelKind(Px.Device)] int boxSize)
        {
            if(boxSize <= 0) {
                throw new ArgumentException(null, nameof(boxSize));
            }

            Width = Height = boxSize;
        }

        public IconSize(IconBox iconBox, Point dpiScale)
        {
            // ここだけ X/Y 見るのもどうなんやろね
            Width = (int)((int)iconBox * dpiScale.X);
            Height = (int)((int)iconBox * dpiScale.Y);
        }

        #region property

        /// <summary>
        /// DPI が取得できないところで使用するしゃあなしDPIスケール。
        /// </summary>
        public readonly static Point DefaultScale = new(1, 1);

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
        public readonly Size ToSize() => new(Width, Height);

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
        public IconScale(IconBox box, Point dpiScale)
        {
            if(double.IsNaN(dpiScale.X) || double.IsInfinity(dpiScale.X) || dpiScale.X < 1) {
                throw new ArgumentException(nameof(dpiScale) + "." + nameof(dpiScale.X));
            }
            if(double.IsNaN(dpiScale.Y) || double.IsInfinity(dpiScale.Y) || dpiScale.Y < 1) {
                throw new ArgumentException(nameof(dpiScale) + "." + nameof(dpiScale.Y));
            }

            Box = box;
            Dpi = dpiScale;
        }

        #region property

        /// <summary>
        /// アイコン基本サイズ。
        /// </summary>
        public IconBox Box { get; }

        /// <summary>
        /// DPIスケール。
        /// </summary>
        public Point Dpi { get; }

        #endregion

        #region function

        /// <summary>
        /// 現在の設定値から<see cref="IconSize"/>を算出。
        /// </summary>
        /// <returns></returns>
        public readonly IconSize ToIconSize() => new(Box, Dpi);

        #endregion

        #region function

        public override string ToString()
        {
            return $"{nameof(IconScale)}: {Box}, {Dpi.X}x{Dpi.Y} -> {((int)Box) * Dpi.X}x{((int)Box) * Dpi.Y}";
        }

        #endregion
    }
}
