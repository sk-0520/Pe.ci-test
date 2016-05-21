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
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    internal static class InitializeRunningInformationSetting
    {
        public static void Correction(RunningInformationSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            V_First(setting, previousVersion, nonProcess);
            V_0_71_0(setting, previousVersion, nonProcess);
            V_Last(setting, previousVersion, nonProcess);
        }

        static void V_Last(RunningInformationSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(!SettingUtility.CheckUserId(setting)) {
                setting.UserId = SettingUtility.CreateUserIdFromEnvironment();
            }

            if(setting.FirstRunning.Version == null) {
                setting.FirstRunning.Timestamp = DateTime.Now;
                setting.FirstRunning.Version = Constants.ApplicationVersionNumber;
            }
        }

        private static void V_0_71_0(RunningInformationSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(new Version(0,71,0,27279) < previousVersion) {
                return;
            }

            nonProcess.Logger.Trace("version setting: 0.71.0");

            setting.UserId = SettingUtility.CreateUserIdFromEnvironment();
            setting.FirstRunning = new FirstRunningItemModel() {
                Timestamp = DateTime.Now,
                Version = previousVersion
            };
        }

        static void V_First(RunningInformationSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(previousVersion != null) {
                return;
            }

            setting.UserId = SettingUtility.CreateUserIdFromEnvironment();
        }
    }
}
