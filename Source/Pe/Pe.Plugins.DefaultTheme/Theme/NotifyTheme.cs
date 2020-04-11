using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme
{
    /// <inheritdoc cref="INotifyTheme"/>
    internal class NotifyTheme : ThemeBase, INotifyTheme
    {
        public NotifyTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region INotifyTheme

        #endregion
    }
}
