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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
    internal static class InitializeLauncherItem
    {
        public static void Correction(LauncherItemModel item, Version previousVersion, INonProcess nonProcess)
        {
            V_First(item, previousVersion, nonProcess);

            V_0_77_0(item, previousVersion, nonProcess);

            V_Last(item, previousVersion, nonProcess);
        }

        static void V_Last(LauncherItemModel item, Version previousVersion, INonProcess nonProcess)
        {
            item.LauncherKind = EnumUtility.GetNormalization(item.LauncherKind, LauncherKind.File);
            // あるだけ
            if(item.LauncherKind == LauncherKind.Directory) {
                item.LauncherKind = LauncherKind.File;
            }

            if(SettingUtility.IsIllegalString(item.Command)) {
                item.Command = string.Empty;
            }
        }

        /// <summary>
        /// 0.77.0.340 以下のバージョン補正。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="previousVersion"></param>
        /// <param name="nonProcess"></param>
        static void V_0_77_0(LauncherItemModel item, Version previousVersion, INonProcess nonProcess)
        {
            if(new Version(0, 77, 0, 340) < previousVersion) {
                return;
            }

            nonProcess.Logger.Trace("version setting: 0.77.0");

            item.IsCommandAutocomplete = true;
        }

        static void V_First(LauncherItemModel item, Version previousVersion, INonProcess nonProcess)
        {
            if(previousVersion != null) {
                return;
            }

            item.LauncherKind = LauncherKind.File;
            item.IsCommandAutocomplete = true;
        }
    }
}
