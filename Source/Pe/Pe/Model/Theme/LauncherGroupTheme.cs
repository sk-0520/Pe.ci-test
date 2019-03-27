using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.View.Extend;

namespace ContentTypeTextNet.Pe.Main.Model.Designer
{
    public interface ILauncherGroupTheme
    {
        #region function

        FrameworkElement CreateGroupImage(LauncherGroupImageName imageName, Color imageColor, IconScale small);

        #endregion
    }

    public class LauncherGroupTheme : ThemeBase, ILauncherGroupTheme
    {
        public LauncherGroupTheme(IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            DispatcherWapper = dispatcherWapper;
        }

        #region property

        IDispatcherWapper DispatcherWapper { get; }

        #endregion

        #region ILauncherGroupTheme

        public FrameworkElement CreateGroupImage(LauncherGroupImageName imageName, Color imageColor, IconScale iconScale)
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
                        var geometry = (PathGeometry)Application.Current.Resources["Image-LauncherGroup-Directory-Normal"];
                        path.Data = geometry;
                        path.Fill = new SolidColorBrush(imageColor);
                        path.Stroke = new SolidColorBrush( MediaUtility.GetAutoColor(imageColor));
                        path.StrokeThickness = 1;
                    }
                    canvas.Children.Add(path);
                }
                viewBox.Child = canvas;
            }

            return viewBox;
        }

        #endregion
    }
}
