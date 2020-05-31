using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme
{
    internal class DefaultLauncherToolbarTheme : DefaultThemeBase, ILauncherToolbarTheme
    {
        public DefaultLauncherToolbarTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region property
        #endregion

        #region function

        #endregion

        #region ILauncherToolbarDesigner

        [return: PixelKind(Px.Logical)]
        public Thickness GetIconMargin(AppDesktopToolbarPosition toolbarPosition, IconBox iconBox, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            return new Thickness(2);
        }

        [return: PixelKind(Px.Logical)]
        public Thickness GetButtonPadding(AppDesktopToolbarPosition toolbarPosition, IconBox iconBox)
        {
            return new Thickness(2);
        }

        [return: PixelKind(Px.Logical)]
        public Size GetDisplaySize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, IconBox iconBox, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            return new Size(
                GetHorizontal(buttonPadding) + GetHorizontal(iconMargin) + (int)iconBox + (isIconOnly ? 0 : textWidth),
                GetVertical(buttonPadding) + GetVertical(iconMargin) + (int)iconBox
            );
        }

        [return: PixelKind(Px.Logical)]
        public Size GetHiddenSize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, IconBox iconBox, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            return new Size(4, 4);
        }

        public ControlTemplate GetLauncherItemNormalButtonControlTemplate() => GetResourceValue<ControlTemplate>(nameof(DefaultLauncherToolbarTheme), nameof(GetLauncherItemNormalButtonControlTemplate));
        public ControlTemplate GetLauncherItemToggleButtonControlTemplate() => GetResourceValue<ControlTemplate>(nameof(DefaultLauncherToolbarTheme), nameof(GetLauncherItemToggleButtonControlTemplate));

        public Brush GetToolbarBackground(AppDesktopToolbarPosition toolbarPosition, ViewState viewState, IconBox iconBox, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            var color = PlatformTheme.GetTaskbarColor();
            return new SolidColorBrush(color);
        }

        public Brush GetToolbarForeground()
        {
            var color = PlatformTheme.GetTaskbarColor();
            return new SolidColorBrush(MediaUtility.GetAutoColor(color));
        }

        #endregion
    }
}
