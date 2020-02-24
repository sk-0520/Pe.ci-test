using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
    public interface ILauncherGroupTheme
    {
        #region function

        DependencyObject GetGroupImage(LauncherGroupImageName imageName, Color imageColor, IconBox iconSize, bool isStrong);

        #endregion
    }
}
