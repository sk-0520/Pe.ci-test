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
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
    internal static class InitializeNoteSetting
    {
        public static void Correction(NoteSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            V_First(setting, previousVersion, nonProcess);
            V_Last(setting, previousVersion, nonProcess);
        }

        static void V_Last(NoteSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            setting.Font.Size = Constants.noteFontSize.GetClamp(setting.Font.Size);
            setting.NoteTitle = EnumUtility.GetNormalization(setting.NoteTitle, NoteTitle.Timestamp);

            if(setting.ForeColor == default(Color)) {
                setting.ForeColor = Constants.noteColor.ForeColor;
            }
            if(setting.BackColor == default(Color)) {
                setting.BackColor = Constants.noteColor.BackColor;
            }
        }

        static void V_First(NoteSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(previousVersion != null) {
                return;
            }

            setting.Font.Size = Constants.noteFontSize.median;
            setting.ForeColor = Constants.noteColor.ForeColor;
            setting.BackColor = Constants.noteColor.BackColor;
            setting.NoteTitle = NoteTitle.Timestamp;
        }
    }
}
