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
    public enum NoteCaption
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

        ColorPair<Brush> GetCaptionBrush(ColorPair<Color> baseColor);
        Brush GetBorderBrush(ColorPair<Color> baseColor);
        ColorPair<Brush> GetContentBrush(ColorPair<Color> baseColor);
        Brush GetCaptionButtonBackgroundBrush(NoteCaptionButtonState buttonState, ColorPair<Color> baseColor);

        DependencyObject GetCaptionImage(NoteCaption noteCaption, bool isEnabled, ColorPair<Color> baseColor);
        DependencyObject GetResizeGripImage(ColorPair<Color> baseColor);

        Effect GetBlindEffect(ColorPair<Color> baseColor);
        DependencyObject GetBlindContent(ColorPair<Color> baseColor);

        #endregion
    }
}
