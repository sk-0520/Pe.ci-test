using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models
{
    internal class MediaConverter: IMediaConverter
    {
        #region IMediaConverter

        /// <inheritdoc cref="IMediaConverter.AddBrightness(Color, double)" />
        public Color AddBrightness(Color baseColor, double brightness) => MediaUtility.AddBrightness(baseColor, brightness);

        /// <inheritdoc cref="IMediaConverter.AddColor(Color, Color)" />
        public Color AddColor(Color baseColor, Color plusColor) => MediaUtility.AddColor(baseColor, plusColor);

        /// <inheritdoc cref="IMediaConverter.GetAutoColor(Color)" />
        public Color GetAutoColor(Color color) => MediaUtility.GetAutoColor(color);

        /// <inheritdoc cref="IMediaConverter.GetBrightness(Color)" />
        public double GetBrightness(Color color) => MediaUtility.GetBrightness(color);

        /// <inheritdoc cref="IMediaConverter.GetComplementaryColor(Color)" />
        public Color GetComplementaryColor(Color color) => MediaUtility.GetComplementaryColor(color);

        /// <inheritdoc cref="IMediaConverter.GetNegativeColor(Color)" />
        public Color GetNegativeColor(Color color) => MediaUtility.GetNegativeColor(color);

        /// <inheritdoc cref="IMediaConverter.GetNonTransparentColor(Color)" />
        public Color GetNonTransparentColor(Color value) => MediaUtility.GetOpaqueColor(value);

        #endregion
    }
}
