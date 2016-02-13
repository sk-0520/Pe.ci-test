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
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
    public static class InitializeLoggingSetting
    {
        public static void Correction(LoggingSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            V_First(setting, previousVersion, nonProcess);
            V_Last(setting, previousVersion, nonProcess);
        }

        static void V_Last(LoggingSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(SettingUtility.IsIllegalPlusNumber(setting.WindowWidth)) {
                setting.WindowWidth = Constants.loggingDefaultWindowSize.Width;
            }
            if(SettingUtility.IsIllegalPlusNumber(setting.WindowHeight)) {
                setting.WindowHeight = Constants.loggingDefaultWindowSize.Height;
            }
        }

        static void V_First(LoggingSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(previousVersion != null) {
                return;
            }

            nonProcess.Logger.Trace("version setting: first");

            setting.WindowWidth = Constants.loggingDefaultWindowSize.Width;
            setting.WindowHeight = Constants.loggingDefaultWindowSize.Height;
            setting.AddShow = true;
            setting.IsVisible = false;
            setting.ShowTriggerDebug = false;
            setting.ShowTriggerTrace = false;
            setting.ShowTriggerInformation = false;
            setting.ShowTriggerWarning = true;
            setting.ShowTriggerError = true;
            setting.ShowTriggerFatal = true;
        }
    }
}
