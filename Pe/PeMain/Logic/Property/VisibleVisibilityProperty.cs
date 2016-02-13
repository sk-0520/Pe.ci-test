/**
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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.PeData.IF;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Property
{
    public static class VisibleVisibilityProperty
    {
        public static bool GetVisible(IVisible model)
        {
            return model.IsVisible;
        }

        public static bool SetVisible(IVisible model, bool value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            if(model.IsVisible != value) {
                model.IsVisible = value;
                onPropertyChanged(propertyName);
                onPropertyChanged("Visibility");

                return true;
            }

            return false;
        }

        public static Visibility GetVisibility(IVisible model)
        {
            return GetVisible(model) ? Visibility.Visible : Visibility.Hidden;
        }

        public static bool SetVisibility(IVisible model, Visibility value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            return SetVisible(model, value == Visibility.Visible, onPropertyChanged, propertyName);
        }


    }
}
