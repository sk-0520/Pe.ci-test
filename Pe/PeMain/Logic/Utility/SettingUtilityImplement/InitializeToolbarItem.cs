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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
    internal static class InitializeToolbarItem
    {
        public static void Correction(ToolbarItemModel setting, Version previousVersion, INonProcess nonProcess)
        {
            V_First(setting, previousVersion, nonProcess);

            V_0_78_0(setting, previousVersion, nonProcess);

            V_Last(setting, previousVersion, nonProcess);
        }

        static void V_Last(ToolbarItemModel setting, Version previousVersion, INonProcess nonProcess)
        {
            setting.HideWaitTime = Constants.toolbarHideWaitTime.GetClamp(setting.HideWaitTime);
            setting.HideAnimateTime = Constants.toolbarHideAnimateTime.GetClamp(setting.HideAnimateTime);
            setting.Font.Size = Constants.toolbarFontSize.GetClamp(setting.Font.Size);
            setting.IconScale = EnumUtility.GetNormalization(setting.IconScale, IconScale.Normal);
            setting.TextWidth = Constants.toolbarTextLength.GetClamp((int)setting.TextWidth);
            setting.ButtonPosition = EnumUtility.GetNormalization(setting.ButtonPosition, ToolbarButtonPosition.Near);

            if(SettingUtility.IsIllegalPlusNumber(setting.FloatToolbar.WidthButtonCount)) {
                setting.FloatToolbar.WidthButtonCount = 1;
            }
            if(SettingUtility.IsIllegalPlusNumber(setting.FloatToolbar.HeightButtonCount)) {
                setting.FloatToolbar.HeightButtonCount = 1;
            }
        }

        /// <summary>
        /// 0.78.0.27501 以下のバージョン補正。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="previousVersion"></param>
        /// <param name="nonProcess"></param>
        static void V_0_78_0(ToolbarItemModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(new Version(0, 78, 0, 27501) < previousVersion) {
                return;
            }

            nonProcess.Logger.Trace("version setting: 0.78.0");

            setting.IsVisibleMenuButton = true;
        }

        static void V_First(ToolbarItemModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(previousVersion != null) {
                return;
            }

            setting.IsVisible = true;
            setting.IsTopmost = true;
            setting.TextWidth = Constants.toolbarTextLength.median;
            setting.IconScale = IconScale.Normal;
            setting.HideWaitTime = Constants.toolbarHideWaitTime.median;
            setting.HideAnimateTime = Constants.toolbarHideAnimateTime.median;
            setting.Font.Size = Constants.toolbarFontSize.median;
            setting.FloatToolbar.WidthButtonCount = 1;
            setting.FloatToolbar.HeightButtonCount = 1;
            setting.DockType = DockType.Right;
            setting.DefaultGroupId = Guid.Empty;
            setting.MenuPositionCorrection = true;
            setting.ButtonPosition = ToolbarButtonPosition.Near;
            setting.IsVisibleMenuButton = true;
        }
    }
}
