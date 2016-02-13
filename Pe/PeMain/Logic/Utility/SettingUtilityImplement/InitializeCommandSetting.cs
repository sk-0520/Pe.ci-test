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
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
    internal static class InitializeCommandSetting
    {
        public static void Correction(CommandSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            V_First(setting, previousVersion, nonProcess);
            V_Last(setting, previousVersion, nonProcess);
        }

        static void V_Last(CommandSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            setting.IconScale = EnumUtility.GetNormalization(setting.IconScale, IconScale.Small);
            setting.WindowWidth = Constants.commandWindowWidth.GetClamp(setting.WindowWidth);
            setting.Font.Size = Constants.commandFontSize.GetClamp(setting.Font.Size);
            setting.HideTime = Constants.commandHideTime.GetClamp(setting.HideTime);

        }

        static void V_First(CommandSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(previousVersion != null) {
                return;
            }

            nonProcess.Logger.Trace("version setting: first");

            setting.IconScale = IconScale.Small;
            setting.HideTime = Constants.commandHideTime.median;
            setting.WindowWidth = Constants.commandWindowWidth.median;
            setting.Font.Size = Constants.commandFontSize.median;
            setting.FindTag = true;
            setting.FindFile = false;
        }
    }
}
