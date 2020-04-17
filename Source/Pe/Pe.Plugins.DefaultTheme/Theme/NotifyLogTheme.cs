using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme
{
    /// <inheritdoc cref="INotifyLogTheme"/>
    internal class NotifyLogTheme : ThemeBase, INotifyLogTheme
    {
        public NotifyLogTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region INotifyTheme

        /// <inheritdoc cref="INotifyLogTheme.GetViewBorderThickness"/>
        public Thickness GetViewBorderThickness()
        {
            return new Thickness(4);
        }
        /// <inheritdoc cref="INotifyLogTheme.GetViewBorderBrush"/>
        public Brush GetViewBorderBrush()
        {
            var colors = PlatformTheme.GetTaskbarColor();
            return FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors));
        }

        /// <inheritdoc cref="INotifyLogTheme.GetViewBorderCornerRadius"/>
        public CornerRadius GetViewBorderCornerRadius()
        {
            return new CornerRadius(2);
        }

        /// <inheritdoc cref="INotifyLogTheme.GetViewBackgroundBrush"/>
        public Brush GetViewBackgroundBrush()
        {
            var colors = PlatformTheme.GetTaskbarColor();
            return FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors));
        }
        /// <inheritdoc cref="INotifyLogTheme.GetViewPaddingThickness"/>
        public Thickness GetViewPaddingThickness()
        {
            return new Thickness(2);
        }

        /// <inheritdoc cref="INotifyLogTheme.GetHeaderForegroundBrush(bool)"/>
        public Brush GetHeaderForegroundBrush(bool isTopmost)
        {
            var color = PlatformTheme.GetTaskbarColor();
            return new SolidColorBrush(MediaUtility.GetAutoColor(color));
        }
        /// <inheritdoc cref="INotifyLogTheme.GetContentForegroundBrush(bool)"/>
        public Brush GetContentForegroundBrush(bool isTopmost)
        {
            var color = PlatformTheme.GetTaskbarColor();
            return new SolidColorBrush(MediaUtility.GetAutoColor(color));
        }

        /// <inheritdoc cref="INotifyLogTheme.GetHyperlinkForegroundBrush(HyperlinkState)"/>
        public Brush GetHyperlinkForegroundBrush(bool isMouseOver)
        {
            var color = isMouseOver
                ? Colors.Lime
                : Colors.Green
            ;
            return new SolidColorBrush(color);
        }


        #endregion
    }
}
