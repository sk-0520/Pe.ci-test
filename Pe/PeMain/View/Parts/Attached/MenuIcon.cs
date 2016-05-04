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
using System.Windows.Media.Effects;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Attached
{
    public static class MenuIcon
    {
        static DropShadowEffect _effect;

        static MenuIcon()
        {
            _effect = new DropShadowEffect() {
                Color = ImageUtility.GetMenuIconColor(true, true),
                ShadowDepth = 0,
                BlurRadius = 8,
            };
            if(_effect.CanFreeze) {
                _effect.Freeze();
            }
        }

        #region IsStrongProperty

        public static readonly DependencyProperty IsStrongProperty = DependencyProperty.RegisterAttached(
            DependencyPropertyUtility.GetName(nameof(IsStrongProperty)),
            typeof(bool),
            typeof(MenuIcon),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsStrongChanged)
        );

        static void OnIsStrongChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as UIElement;
            if(element != null) {
                SetIsStrong(element, (bool)e.NewValue);
            }
        }

        public static bool GetIsStrong(UIElement element)
        {
            return (bool)element.GetValue(IsStrongProperty);
        }
        public static void SetIsStrong(UIElement element, bool value)
        {
            element.SetValue(IsStrongProperty, value);
            //var img = imageObject as Image;
            if(element != null) {
                if(value) {
                    //element.Opacity = checkedIsStrong;
                    element.Effect = _effect;
                } else {
                    //element.Opacity = uncheckedIsStrong;
                    element.Effect = null;
                }
            }
        }

        #endregion
    }
}
