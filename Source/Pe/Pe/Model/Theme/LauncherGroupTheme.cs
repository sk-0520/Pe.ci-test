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
using ContentTypeTextNet.Pe.Bridge.Model;
using ContentTypeTextNet.Pe.Core.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Theme;
using ContentTypeTextNet.Pe.Main.View.Extend;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Model.Theme
{
    public interface ILauncherGroupTheme
    {
        #region function

        DependencyObject GetGroupImage(LauncherGroupImageName imageName, Color imageColor, IconScale iconScale, bool isStrong);

        #endregion
    }

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

        DependencyObject GetGroupImageCore(LauncherGroupImageName imageName, Color imageColor, IconScale iconScale, bool isStrong)
        {
            var viewBox = new Viewbox();
            using(Initializer.BeginInitialize(viewBox)) {
                viewBox.Width = iconScale.ToWidth();
                viewBox.Height = iconScale.ToHeight();

                var canvas = new Canvas();
                using(Initializer.BeginInitialize(canvas)) {
                    canvas.Width = 24;
                    canvas.Height = 24;

                    var path = new Path();
                    using(Initializer.BeginInitialize(path)) {
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

        public DependencyObject GetGroupImage(LauncherGroupImageName imageName, Color imageColor, IconScale iconScale, bool isStrong)
        {
            return GetGroupImageCore(imageName, imageColor, iconScale, isStrong);
        }

        #endregion
    }
}
