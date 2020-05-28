using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Embedded.Abstract;

namespace ContentTypeTextNet.Pe.Plugins.ClassicTheme.Theme
{
    internal class ClassicGeneralTheme: ThemeDetailBase, IGeneralTheme
    {
        public ClassicGeneralTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region IGeneralTheme

        public Geometry GetGeometryImage(GeneralGeometryImageKind kind, IconBox iconBox)
        {
            var key = nameof(ClassicGeneralTheme) + ".Image-General-" + kind.ToString();
            return (Geometry)Application.Current.Resources[key];
        }

        #endregion
    }
}
