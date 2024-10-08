using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme
{
    internal class DefaultLauncherToolbarTheme: DefaultThemeBase, ILauncherToolbarTheme
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
        public Thickness GetIconMargin(AppDesktopToolbarPosition toolbarPosition, in IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            return new Thickness(2);
        }

        [return: PixelKind(Px.Logical)]
        public Thickness GetButtonPadding(AppDesktopToolbarPosition toolbarPosition, in IconScale iconScale)
        {
            return new Thickness(2);
        }

        [return: PixelKind(Px.Logical)]
        public Size GetDisplaySize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, in IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            return new Size(
                GetHorizontal(buttonPadding) + GetHorizontal(iconMargin) + (int)iconScale.Box + (isIconOnly ? 0 : textWidth),
                GetVertical(buttonPadding) + GetVertical(iconMargin) + (int)iconScale.Box
            );
        }

        [return: PixelKind(Px.Logical)]
        public Size GetHiddenSize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, in IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            return new Size(4, 4);
        }

        public ControlTemplate GetLauncherItemNormalButtonControlTemplate() => GetResourceValue<ControlTemplate>(nameof(DefaultLauncherToolbarTheme), nameof(GetLauncherItemNormalButtonControlTemplate));
        public ControlTemplate GetLauncherItemToggleButtonControlTemplate() => GetResourceValue<ControlTemplate>(nameof(DefaultLauncherToolbarTheme), nameof(GetLauncherItemToggleButtonControlTemplate));

        public Brush GetToolbarBackground(AppDesktopToolbarPosition toolbarPosition, ViewState viewState, in IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            var color = PlatformTheme.GetTaskbarColor();
            return new SolidColorBrush(color);
        }

        public Brush GetToolbarForeground()
        {
            var color = PlatformTheme.GetTaskbarColor();
            return new SolidColorBrush(MediaUtility.GetAutoColor(color));
        }

        public DependencyObject GetLauncherSeparator(bool isHorizontal, LauncherSeparatorKind kind, int width)
        {
            if(kind == LauncherSeparatorKind.None) {
                return new Rectangle() {
                    Width = width,
                    Height = width,
                };
            }

            var color = PlatformTheme.GetTaskbarColor();
            var autoColor = MediaUtility.GetAutoColor(color);

            switch(kind) {
                case LauncherSeparatorKind.Line: {
                        var rectangle = new Rectangle();
                        var edgeThickness = 2;

                        using(Initializer.Begin(rectangle)) {
                            const int separatorWidth = 1;
                            var separatorBrush = new SolidColorBrush(autoColor);
                            rectangle.Fill = separatorBrush;

                            double directionThickness = width <= separatorWidth ? 0 : ((width - separatorWidth) / 2.0);

                            if(isHorizontal) {
                                rectangle.Height = separatorWidth;
                                rectangle.Margin = new Thickness(
                                    edgeThickness, directionThickness, edgeThickness, directionThickness
                                );
                            } else {
                                rectangle.Width = separatorWidth;
                                rectangle.Margin = new Thickness(
                                    directionThickness, edgeThickness, directionThickness, edgeThickness
                                );
                            }
                        }

                        return rectangle;
                    }

                case LauncherSeparatorKind.Space: {
                        var rectangle = new Rectangle();

                        using(Initializer.Begin(rectangle)) {
                            if(isHorizontal) {
                                rectangle.Height = width;
                            } else {
                                rectangle.Width = width;
                            }
                        }

                        return rectangle;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
