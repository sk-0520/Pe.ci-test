using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
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

        public IconSize(IconBox iconBox)
        {
            Width = Height = (int)iconBox;
        }

        #region property

        [PixelKind(Px.Device)]
        public int Width { get; }
        [PixelKind(Px.Device)]
        public int Height { get; }

        readonly bool IsSquare => Width == Height;
        #endregion

        #region function

        public readonly Size ToSize() => new Size(Width, Height);
        public readonly IconBox ToIconSize()
        {
            var size = IsSquare ? Width: Math.Max(Width, Height);
            var kinds = new[] { IconBox.Small, IconBox.Normal, IconBox.Big, IconBox.Large };
            foreach(var kind in kinds) {
                if(size <= (int)kind) {
                    return kind;
                }
            }

            return IconBox.Large;
        }

        #endregion

        #region object

        public readonly override string? ToString() => $"{Width} x {Height}";

        #endregion
    }
}
