using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme
{
    internal class DefaultGeneralTheme : DefaultThemeBase, IGeneralTheme
    {
        public DefaultGeneralTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region IGeneralTheme

        public Geometry GetPathImage(GeneralPathImageKind kind, IconBox iconBox, Point iconScale)
        {
            var baseKey = "Path-General-" + kind.ToString();
            return GetResourceValue<Geometry>(nameof(DefaultGeneralTheme), baseKey);
        }

        #endregion
    }
}
