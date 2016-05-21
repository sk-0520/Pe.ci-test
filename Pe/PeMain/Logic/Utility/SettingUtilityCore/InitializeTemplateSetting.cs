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

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    internal static class InitializeTemplateSetting
    {
        public static void Correction(TemplateSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            V_First(setting, previousVersion, nonProcess);
            V_Last(setting, previousVersion, nonProcess);
        }

        static void V_Last(TemplateSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            setting.Font.Size = Constants.templateFontSize.GetClamp(setting.Font.Size);

            if(SettingUtility.IsIllegalPlusNumber(setting.ItemsListWidth)) {
                setting.ItemsListWidth = Constants.templateItemsListWidth;
            }
            if(SettingUtility.IsIllegalPlusNumber(setting.ReplaceListWidth)) {
                setting.ReplaceListWidth = Constants.templateReplaceListWidth;
            }

            if(SettingUtility.IsIllegalPlusNumber(setting.WindowWidth)) {
                setting.WindowWidth = Constants.templateDefaultWindowSize.Width;
            }
            if(SettingUtility.IsIllegalPlusNumber(setting.WindowHeight)) {
                setting.WindowHeight = Constants.templateDefaultWindowSize.Height;
            }
        }

        static void V_First(TemplateSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(previousVersion != null) {
                return;
            }

            setting.Font.Size = Constants.templateFontSize.median;
            setting.ItemsListWidth = Constants.templateItemsListWidth;
            setting.ReplaceListWidth = Constants.templateReplaceListWidth;
            setting.WindowWidth = Constants.templateDefaultWindowSize.Width;
            setting.WindowHeight = Constants.templateDefaultWindowSize.Height;
        }
    }
}
