using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Embedded.Abstract;

namespace ContentTypeTextNet.Pe.Plugins.ClassicTheme.Theme
{
    internal class ClassicNotifyLogTheme: ThemeDetailBase, INotifyLogTheme
    {
        public ClassicNotifyLogTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region INotifyTheme

        /// <inheritdoc cref="INotifyLogTheme.GetViewBorderThickness"/>
        public Thickness GetViewBorderThickness()
        {
            return new Thickness(1);
        }
        /// <inheritdoc cref="INotifyLogTheme.GetViewBorderBrush"/>
        public Brush GetViewBorderBrush()
        {
            return SystemColors.ControlDarkBrush;
        }

        /// <inheritdoc cref="INotifyLogTheme.GetViewBorderCornerRadius"/>
        public CornerRadius GetViewBorderCornerRadius()
        {
            return new CornerRadius(0);
        }

        /// <inheritdoc cref="INotifyLogTheme.GetViewBackgroundBrush"/>
        public Brush GetViewBackgroundBrush()
        {
            return SystemColors.ControlBrush;
        }
        /// <inheritdoc cref="INotifyLogTheme.GetViewPaddingThickness"/>
        public Thickness GetViewPaddingThickness()
        {
            return new Thickness(1);
        }

        /// <inheritdoc cref="INotifyLogTheme.GetHeaderForegroundBrush(bool)"/>
        public Brush GetHeaderForegroundBrush(bool isTopmost)
        {
            return SystemColors.ControlTextBrush;
        }
        /// <inheritdoc cref="INotifyLogTheme.GetContentForegroundBrush(bool)"/>
        public Brush GetContentForegroundBrush(bool isTopmost)
        {
            return SystemColors.ControlTextBrush;
        }

        /// <inheritdoc cref="INotifyLogTheme.GetHyperlinkForegroundBrush(HyperlinkState)"/>
        public Brush GetHyperlinkForegroundBrush(bool isMouseOver)
        {
            return SystemColors.HotTrackBrush;
        }


        #endregion

    }
}
