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
    internal class GeneralTheme : ThemeBase, IGeneralTheme
    {
        public GeneralTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region IGeneralTheme

        public Geometry GetGeometryImage(GeneralGeometryImageKind kind, IconBox iconBox)
        {
            var key = "Image-General-" + kind.ToString();
            return (Geometry)Application.Current.Resources[key];
        }

        #endregion
    }
}
