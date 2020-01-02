using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
    public interface ILauncherToolbarTheme
    {
        #region function

        [return: PixelKind(Px.Logical)]
        Thickness GetButtonPadding(AppDesktopToolbarPosition toolbarPosition, IconBox iconBox);
        [return: PixelKind(Px.Logical)]
        Thickness GetIconMargin(AppDesktopToolbarPosition toolbarPosition, IconBox iconBox, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);

        [return: PixelKind(Px.Logical)]
        Size GetDisplaySize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, IconBox iconBox, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);
        [return: PixelKind(Px.Logical)]
        Size GetHiddenSize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, IconBox iconBox, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);

        DependencyObject GetToolbarMainIcon(IconBox iconBox);
        DependencyObject GetToolbarImage(IReadOnlyScreenData currentScreen, IReadOnlyList<IReadOnlyScreenData> allScreens, IconBox iconBox, bool isStrong);
        DependencyObject GetToolbarPositionImage(AppDesktopToolbarPosition toolbarPosition, IconBox iconBox);

        ControlTemplate GetLauncherItemNormalButtonControlTemplate();
        ControlTemplate GetLauncherItemToggleButtonControlTemplate();

        Brush GetToolbarBackground(AppDesktopToolbarPosition toolbarPosition, ViewState viewState, IconBox iconBox, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);
        Brush GetToolbarForeground();
        #endregion
    }
}
