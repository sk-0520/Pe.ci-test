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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.PeData.IF;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Property
{
    public static class WindowAreaProperty
    {
        public static double GetWindowLeft(IWindowArea model)
        {
            return model.WindowLeft;
        }

        public static bool SetWindowLeft(IWindowArea model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            if(model.WindowLeft != value) {
                model.WindowLeft = value;
                onPropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        public static double GetWindowTop(IWindowArea model)
        {
            return model.WindowTop;
        }

        public static bool SetWindowTop(IWindowArea model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            if(model.WindowTop != value) {
                model.WindowTop = value;
                onPropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        public static double GetWindowWidth(IWindowArea model)
        {
            return model.WindowWidth;
        }

        public static bool SetWindowWidth(IWindowArea model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            if(model.WindowWidth != value) {
                model.WindowWidth = value;
                onPropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        public static double GetWindowHeight(IWindowArea model)
        {
            return model.WindowHeight;
        }

        public static bool SetWindowHeight(IWindowArea model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            if(model.WindowHeight != value) {
                model.WindowHeight = value;
                onPropertyChanged(propertyName);

                return true;
            }

            return false;
        }
    }
}
