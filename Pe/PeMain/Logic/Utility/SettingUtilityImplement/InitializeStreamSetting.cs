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
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
    internal static class InitializeStreamSetting
    {
        internal static void Correction(StreamSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            V_First(setting, previousVersion, nonProcess);
            V_Last(setting, previousVersion, nonProcess);
        }

        static void V_Last(StreamSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            setting.Font.Size = Constants.streamFontSize.GetClamp(setting.Font.Size);

            if(setting.OutputColor.ForeColor == default(Color)) {
                setting.OutputColor.ForeColor = Constants.streamOutputColor.ForeColor;
            }
            if(setting.OutputColor.BackColor == default(Color)) {
                setting.OutputColor.BackColor = Constants.streamOutputColor.BackColor;
            }

            if(setting.ErrorColor.ForeColor == default(Color)) {
                setting.ErrorColor.ForeColor = Constants.streamErrorColor.ForeColor;
            }
            if(setting.ErrorColor.BackColor == default(Color)) {
                setting.ErrorColor.BackColor = Constants.streamErrorColor.BackColor;
            }
        }

        private static void V_First(StreamSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(previousVersion != null) {
                return;
            }

            setting.Font.Size = Constants.streamFontSize.median;
            //Constants.streamOutputColor.DeepCloneTo(setting.OutputColor);
            //Constants.streamErrorColor.DeepCloneTo(setting.ErrorColor);
            setting.OutputColor = (ColorPairItemModel)Constants.streamOutputColor.DeepClone();
            setting.ErrorColor = (ColorPairItemModel)Constants.streamErrorColor.DeepClone();
        }
    }
}
