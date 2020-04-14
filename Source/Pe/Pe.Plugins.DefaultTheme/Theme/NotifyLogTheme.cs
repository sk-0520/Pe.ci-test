using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;

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
            throw new NotImplementedException();
        }
        /// <inheritdoc cref="INotifyLogTheme.GetViewBorderBrush"/>
        public Brush GetViewBorderBrush()
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc cref="INotifyLogTheme.GetViewBorderCornerRadius"/>
        public CornerRadius GetViewBorderCornerRadius()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="INotifyLogTheme.GetViewBackgroundBrush"/>
        public Brush GetViewBackgroundBrush()
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc cref="INotifyLogTheme.GetViewPaddingThickness"/>
        public Thickness GetViewPaddingThickness()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="INotifyLogTheme.GetHeaderForegroundBrush(bool)"/>
        public Brush GetHeaderForegroundBrush(bool isTopmost)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc cref="INotifyLogTheme.GetContentForegroundBrush(bool)"/>
        public Brush GetContentForegroundBrush(bool isTopmost)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
