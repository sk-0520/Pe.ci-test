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
namespace ContentTypeTextNet.Pe.PeMain.Logic.Property
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using ContentTypeTextNet.Pe.Library.PeData.IF;

    public static class ColorPairProperty
    {
        public static Color GetForeColor(IColorPair model)
        {
            return model.ForeColor;
        }

        public static bool SetForeColor(IColorPair model, Color value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            if(model.ForeColor != value) {
                model.ForeColor = value;
                onPropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        public static Color GetBackColor(IColorPair model)
        {
            return model.BackColor;
        }

        public static bool SetBackColor(IColorPair model, Color value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            if(model.BackColor != value) {
                model.BackColor = value;
                onPropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        public static Color GetNoneAlphaForeColor(IColorPair model)
        {
            var result = MediaUtility.GetNonTransparentColor(GetForeColor(model));
            return result;
        }

        public static bool SetNoneAlphaForekColor(IColorPair model, Color value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            return SetForeColor(model, MediaUtility.GetNonTransparentColor(value), onPropertyChanged, propertyName);
        }

        public static Color GetNoneAlphaBackColor(IColorPair model)
        {
            var result = MediaUtility.GetNonTransparentColor(GetBackColor(model));
            return result;
        }

        public static bool SetNoneAlphaBackColor(IColorPair model, Color value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            return SetBackColor(model, MediaUtility.GetNonTransparentColor(value), onPropertyChanged, propertyName);
        }


    }
}
