using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
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

        IReadOnlyColorPair<Brush> GetCaptionBrush(IReadOnlyColorPair<Color> baseColor);
        Brush GetBorderBrush(IReadOnlyColorPair<Color> baseColor);
        IReadOnlyColorPair<Brush> GetContentBrush(IReadOnlyColorPair<Color> baseColor);
        Brush GetCaptionButtonBackgroundBrush(NoteCaptionButtonState buttonState, IReadOnlyColorPair<Color> baseColor);

        DependencyObject GetCaptionImage(NoteCaption noteCaption, bool isEnabled, IReadOnlyColorPair<Color> baseColor);
        DependencyObject GetResizeGripImage(IReadOnlyColorPair<Color> baseColor);

        DependencyObject GetIconImage(IconBox iconBox, bool isCompact, bool isLocked, IReadOnlyColorPair<Color> baseColor);

        #endregion
    }
}
