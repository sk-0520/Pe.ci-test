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
        Thickness GetButtonPadding(AppDesktopToolbarPosition toolbarPosition, in IconScale iconScale);
        [return: PixelKind(Px.Logical)]
        Thickness GetIconMargin(AppDesktopToolbarPosition toolbarPosition, in IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);

        [return: PixelKind(Px.Logical)]
        Size GetDisplaySize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, in IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);
        [return: PixelKind(Px.Logical)]
        Size GetHiddenSize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, in IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);

        ControlTemplate GetLauncherItemNormalButtonControlTemplate();
        ControlTemplate GetLauncherItemToggleButtonControlTemplate();

        Brush GetToolbarBackground(AppDesktopToolbarPosition toolbarPosition, ViewState viewState, in IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);
        Brush GetToolbarForeground();
        #endregion
    }
}
