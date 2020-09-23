using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
    public enum NoteCaptionButtonKind
    {
        Compact,
        Topmost,
        Close,
    }

    public enum NoteCaptionButtonState
    {
        None,
        Over,
        Pressed,
    }

    public interface INoteTheme
    {
        #region function

        [return: PixelKind(Px.Logical)]
        double GetCaptionHeight();
        [return: PixelKind(Px.Logical)]
        double GetCaptionFontSize(double baseFontSize);
        FontFamily GetCaptionFontFamily(FontFamily baseFontFamily);

        [return: PixelKind(Px.Logical)]
        Thickness GetBorderThickness();
        [return: PixelKind(Px.Logical)]
        Size GetResizeGripSize();

        ColorPair<Brush> GetCaptionBrush(NoteCaptionPosition captionPosition, ColorPair<Color> baseColor);
        Brush GetBorderBrush(NoteCaptionPosition captionPosition, ColorPair<Color> baseColor);
        ColorPair<Brush> GetContentBrush(NoteCaptionPosition captionPosition, ColorPair<Color> baseColor);
        Brush GetCaptionButtonBackgroundBrush(NoteCaptionButtonState buttonState, NoteCaptionPosition captionPosition, ColorPair<Color> baseColor);

        DependencyObject GetCaptionImage(NoteCaptionButtonKind buttonKind, NoteCaptionPosition captionPosition, bool isEnabled, ColorPair<Color> baseColor);
        DependencyObject GetResizeGripImage(NoteCaptionPosition captionPosition, ColorPair<Color> baseColor);

        Effect GetBlindEffect(ColorPair<Color> baseColor);
        DependencyObject GetBlindContent(ColorPair<Color> baseColor);

        #endregion
    }
}
