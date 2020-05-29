using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup
{
    public class LauncherGroupIconFactory: ViewElementFactoryBase
    {
        #region function

        string GetResourceKey(LauncherGroupImageName imageName)
        {
            return imageName switch
            {
                LauncherGroupImageName.DirectoryNormal => "Path-LauncherGroup-Directory-Normal",
                LauncherGroupImageName.DirectoryOpen => "Path-LauncherGroup-Directory-Open",
                LauncherGroupImageName.File => "Path-LauncherGroup-File",
                LauncherGroupImageName.Gear => "Path-LauncherGroup-Gear",
                LauncherGroupImageName.Config => "Path-LauncherGroup-Config",
                LauncherGroupImageName.Builder => "Path-LauncherGroup-Builder",
                LauncherGroupImageName.Book => "Path-LauncherGroup-Book",
                LauncherGroupImageName.Bookmark => "Path-LauncherGroup-Bookmark",
                LauncherGroupImageName.Light => "Path-LauncherGroup-Light",
                LauncherGroupImageName.Shortcut => "Path-LauncherGroup-Shortcut",
                LauncherGroupImageName.Storage => "Path-LauncherGroup-Storage",
                LauncherGroupImageName.Cloud => "Path-LauncherGroup-Cloud",
                LauncherGroupImageName.User => "Path-LauncherGroup-User",
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

        public DependencyObject GetGroupImage(LauncherGroupImageName imageName, Color imageColor, IconBox iconBox, bool isStrong)
        {
            return GetGroupImageCore(imageName, imageColor, iconBox, isStrong);
        }

        #endregion
    }
}
