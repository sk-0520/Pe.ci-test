using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme
{
    internal class LauncherGroupTheme : ThemeBase, ILauncherGroupTheme
    {
        public LauncherGroupTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region property
        #endregion

        #region function

        string GetResourceKey(LauncherGroupImageName imageName)
        {
            return imageName switch
            {
                LauncherGroupImageName.DirectoryNormal => "Image-LauncherGroup-Directory-Normal",
                LauncherGroupImageName.DirectoryOpen => "Image-LauncherGroup-Directory-Open",
                LauncherGroupImageName.File => "Image-LauncherGroup-File",
                LauncherGroupImageName.Gear => "Image-LauncherGroup-Gear",
                LauncherGroupImageName.Config => "Image-LauncherGroup-Config",
                LauncherGroupImageName.Builder => "Image-LauncherGroup-Builder",
                LauncherGroupImageName.Book => "Image-LauncherGroup-Book",
                LauncherGroupImageName.Bookmark => "Image-LauncherGroup-Bookmark",
                LauncherGroupImageName.Light => "Image-LauncherGroup-Light",
                LauncherGroupImageName.Shortcut => "Image-LauncherGroup-Shortcut",
                LauncherGroupImageName.Storage => "Image-LauncherGroup-Storage",
                LauncherGroupImageName.Cloud => "Image-LauncherGroup-Cloud",
                LauncherGroupImageName.User => "Image-LauncherGroup-User",
                _ => throw new NotImplementedException(),
            };
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
