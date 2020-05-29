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
    internal class DefaultNotifyLogTheme : DefaultThemeBase, INotifyLogTheme
    {
        public DefaultNotifyLogTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region property

        byte BaseAlplha { get; } = 180;

        #endregion

        #region function

        Color ToBaseColor(Color color)
        {
            color.A = BaseAlplha;
            return color;
        }

        #endregion

        #region INotifyTheme

        /// <inheritdoc cref="INotifyLogTheme.GetViewBorderThickness"/>
        public Thickness GetViewBorderThickness()
        {
            return new Thickness(2);
        }
        /// <inheritdoc cref="INotifyLogTheme.GetViewBorderBrush"/>
        public Brush GetViewBorderBrush()
        {
            var color = PlatformTheme.GetTaskbarColor();
            color.A = 255;
            return FreezableUtility.GetSafeFreeze(new SolidColorBrush(color));
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
            return new SolidColorBrush(ToBaseColor(MediaUtility.GetAutoColor(color)));
        }
        /// <inheritdoc cref="INotifyLogTheme.GetContentForegroundBrush(bool)"/>
        public Brush GetContentForegroundBrush(bool isTopmost)
        {
            var color = PlatformTheme.GetTaskbarColor();
            return new SolidColorBrush(ToBaseColor(MediaUtility.GetAutoColor(color)));
        }

        /// <inheritdoc cref="INotifyLogTheme.GetHyperlinkForegroundBrush(HyperlinkState)"/>
        public Brush GetHyperlinkForegroundBrush(bool isMouseOver)
        {
            var color = MediaUtility.GetAutoColor(PlatformTheme.GetTaskbarColor());
            color.A = isMouseOver
                ? (byte)255
                : BaseAlplha
            ;
            return new SolidColorBrush(color);
        }


        #endregion
    }
}
