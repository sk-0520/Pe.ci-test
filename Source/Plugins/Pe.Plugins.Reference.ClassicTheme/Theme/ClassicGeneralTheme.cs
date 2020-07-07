using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Embedded.Abstract;

namespace ContentTypeTextNet.Pe.Plugins.Reference.ClassicTheme.Theme
{
    internal class ClassicGeneralTheme: ThemeDetailBase, IGeneralTheme
    {
        public ClassicGeneralTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region IGeneralTheme

        public Geometry GetPathImage(GeneralPathImageKind kind, in IconScale iconScale)
        {
            var baseKey = "Path-General-" + kind.ToString();
            return GetResourceValue<Geometry>(nameof(ClassicGeneralTheme), baseKey);
        }

        #endregion
    }
}
