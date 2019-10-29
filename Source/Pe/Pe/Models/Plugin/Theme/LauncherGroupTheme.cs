using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Theme;
using ContentTypeTextNet.Pe.Main.Views.Extend;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Theme
{
    internal class LauncherGroupTheme : ThemeBase, ILauncherGroupTheme
    {
        public LauncherGroupTheme(IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(dispatcherWapper, loggerFactory)
        { }

        #region property
        #endregion

        #region function

        string GetResourceKey(LauncherGroupImageName imageName)
        {
            switch(imageName) {
                case LauncherGroupImageName.DirectoryNormal:
                    return "Image-LauncherGroup-Directory-Normal";

                case LauncherGroupImageName.DirectoryOpen:
                    return "Image-LauncherGroup-Directory-Open";

                default:
                    throw new NotSupportedException();
            }
        }

        DependencyObject GetGroupImageCore(LauncherGroupImageName imageName, Color imageColor, IconBox iconBox, bool isStrong)
        {
            var viewBox = new Viewbox();
            using(Initializer.Begin(viewBox)) {
                var iconSize = new IconSize(iconBox);
                viewBox.Width = iconSize.Width;
                viewBox.Height = iconSize.Height;

                var canvas = new Canvas();
                using(Initializer.Begin(canvas)) {
                    canvas.Width = 24;
                    canvas.Height = 24;

                    var path = new Path();
                    using(Initializer.Begin(path)) {
                        var resourceKey = GetResourceKey(imageName);
                        var geometry = (Geometry)Application.Current.Resources[resourceKey];
                        FreezableUtility.SafeFreeze(geometry);
                        path.Data = geometry;
                        path.Fill = FreezableUtility.GetSafeFreeze(new SolidColorBrush(imageColor));
                        path.Stroke = FreezableUtility.GetSafeFreeze(new SolidColorBrush(MediaUtility.GetAutoColor(imageColor)));
                        path.StrokeThickness = 1;
                    }
                    canvas.Children.Add(path);
                    if(isStrong) {
                        canvas.Effect = GetStrongEffect();
                    }
                }
                viewBox.Child = canvas;
            }

            return viewBox;
        }

        #endregion

        #region ILauncherGroupTheme

        public DependencyObject GetGroupImage(LauncherGroupImageName imageName, Color imageColor, IconBox iconBox, bool isStrong)
        {
            return GetGroupImageCore(imageName, imageColor, iconBox, isStrong);
        }

        #endregion
    }
}
