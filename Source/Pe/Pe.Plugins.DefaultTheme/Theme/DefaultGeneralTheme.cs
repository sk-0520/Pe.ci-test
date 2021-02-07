using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme
{
    internal class DefaultGeneralTheme: DefaultThemeBase, IGeneralTheme
    {
        public DefaultGeneralTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region IGeneralTheme

        public Geometry GetPathImage(GeneralPathImageKind kind, in IconScale iconScale)
        {
            var baseKey = "Path-General-" + kind.ToString();
            return GetResourceValue<Geometry>(nameof(DefaultGeneralTheme), baseKey);
        }

        #endregion
    }
}
