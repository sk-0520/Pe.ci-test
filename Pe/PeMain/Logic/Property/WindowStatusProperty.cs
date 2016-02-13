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
using System.Windows;
using ContentTypeTextNet.Pe.Library.PeData.IF;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Property
{
    public static class WindowStatusProperty
    {
        public static double GetWindowLeft(IWindowStatus model)
        {
            return WindowAreaProperty.GetWindowLeft(model);
        }

        public static bool SetWindowLeft(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            if(model.WindowState == WindowState.Normal) {
                return WindowAreaProperty.SetWindowLeft(model, value, onPropertyChanged, propertyName);
            }

            return false;
        }

        public static double GetWindowTop(IWindowStatus model)
        {
            return WindowAreaProperty.GetWindowTop(model);
        }

        public static bool SetWindowTop(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            if(model.WindowState == WindowState.Normal) {
                return WindowAreaProperty.SetWindowTop(model, value, onPropertyChanged, propertyName);
            }

            return false;
        }

        public static double GetWindowWidth(IWindowStatus model)
        {
            return WindowAreaProperty.GetWindowWidth(model);
        }

        public static bool SetWindowWidth(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            if(model.WindowState == WindowState.Normal) {
                return WindowAreaProperty.SetWindowWidth(model, value, onPropertyChanged, propertyName);
            }

            return false;
        }

        public static double GetWindowHeight(IWindowStatus model)
        {
            return WindowAreaProperty.GetWindowHeight(model);
        }

        public static bool SetWindowHeight(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            if(model.WindowState == WindowState.Normal) {
                return WindowAreaProperty.SetWindowHeight(model, value, onPropertyChanged, propertyName);
            }

            return false;
        }

        public static WindowState GetWindowState(IWindowState model)
        {
            return WindowStateProperty.GetWindowState(model);
        }

        public static void SetWindowState(IWindowState model, WindowState value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            WindowStateProperty.SetWindowState(model, value, onPropertyChanged, propertyName);
        }

        public static bool GetTopMost(ITopMost model)
        {
            return TopMostProperty.GetTopMost(model);
        }

        public static bool SetTopMost(ITopMost model, bool value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
        {
            return TopMostProperty.SetTopMost(model, value, onPropertyChanged, propertyName);
        }
    }
}
