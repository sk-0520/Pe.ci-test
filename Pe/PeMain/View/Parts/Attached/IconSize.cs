/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Attached
{
    public class IconSize
    {
        public static readonly DependencyProperty IconScaleProperty = DependencyProperty.RegisterAttached(
            "IconScale",
            typeof(IconScale),
            typeof(Browser),
            new FrameworkPropertyMetadata(OnIconScaleChanged)
        );

        public static IconScale GetIconScale(DependencyObject dependencyObject)
        {
            return (IconScale)dependencyObject.GetValue(IconScaleProperty);
        }
        public static void SetIconScale(DependencyObject dependencyObject, IconScale value)
        {
            dependencyObject.SetValue(IconScaleProperty, value);
        }

        private static void OnIconScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var iconScale = GetIconScale(d);
            var element = d as FrameworkElement;
            if(element != null) {
                var size = iconScale.ToSize();
                element.Width = size.Width;
                element.Height = size.Height;
            }
        }


    }
}
