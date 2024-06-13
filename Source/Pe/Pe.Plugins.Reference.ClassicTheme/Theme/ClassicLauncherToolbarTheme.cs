using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Embedded.Abstract;

namespace ContentTypeTextNet.Pe.Plugins.Reference.ClassicTheme.Theme
{
    internal class ClassicLauncherToolbarTheme: ThemeDetailBase, ILauncherToolbarTheme
    {
        public ClassicLauncherToolbarTheme(IThemeParameter parameter)
            : base(parameter)
        { }


        #region ILauncherToolbarTheme

        [return: PixelKind(Px.Logical)]
        public Thickness GetButtonPadding(AppDesktopToolbarPosition toolbarPosition, in IconScale iconScale)
        {
            return new Thickness(10);
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
            return new Size(8, 8);
        }

        [return: PixelKind(Px.Logical)]
        public Thickness GetIconMargin(AppDesktopToolbarPosition toolbarPosition, in IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            return new Thickness(0);
        }

        public ControlTemplate GetLauncherItemNormalButtonControlTemplate()
        {
            return GetResourceValue<ControlTemplate>(nameof(ClassicLauncherToolbarTheme), nameof(GetLauncherItemNormalButtonControlTemplate));
        }

        public ControlTemplate GetLauncherItemToggleButtonControlTemplate()
        {
            return GetResourceValue<ControlTemplate>(nameof(ClassicLauncherToolbarTheme), nameof(GetLauncherItemToggleButtonControlTemplate));
        }

        public Brush GetToolbarBackground(AppDesktopToolbarPosition toolbarPosition, ViewState viewState, in IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            return SystemColors.ControlBrush;
        }

        public Brush GetToolbarForeground()
        {
            return SystemColors.ControlTextBrush;
        }

        public DependencyObject GetLauncherSeparator(bool isHorizontal, LauncherSeparatorKind kind, int width)
        {
            switch(kind) {
                case LauncherSeparatorKind.None: {
                        return new Rectangle() {
                            Width = width,
                            Height = width,
                        };
                    }

                case LauncherSeparatorKind.Line: {
                        var rectangle = new Rectangle();
                        var edgeThickness = 2;

                        rectangle.BeginInit();
                        const int separatorWidth = 1;
                        var separatorBrush = SystemColors.ControlTextBrush;
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
                        rectangle.EndInit();

                        return rectangle;
                    }

                case LauncherSeparatorKind.Space: {
                        var rectangle = new Rectangle();

                        rectangle.BeginInit();
                        if(isHorizontal) {
                            rectangle.Height = width;
                        } else {
                            rectangle.Width = width;
                        }
                        rectangle.EndInit();

                        return rectangle;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
