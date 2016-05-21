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
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    internal sealed class InitializeNoteSetting: InitializeBase<NoteSettingModel>
    {
        public InitializeNoteSetting(NoteSettingModel model, Version previousVersion, INonProcess nonProcess)
            :base(model, previousVersion, nonProcess)
        { }

        #region InitializeBase

        protected override void V_LastCore()
        {
            if(PreviousVersion == null) {
                Model.Font.Size = Constants.noteFontSize.median;
                Model.Font.Family = FontUtility.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily);
            }
            Model.Font.Size = Constants.noteFontSize.GetClamp(Model.Font.Size);
            Model.NoteTitle = EnumUtility.GetNormalization(Model.NoteTitle, NoteTitle.Timestamp);

            if(Model.ForeColor == default(Color)) {
                Model.ForeColor = Constants.noteColor.ForeColor;
            }
            if(Model.BackColor == default(Color)) {
                Model.BackColor = Constants.noteColor.BackColor;
            }
        }

        protected override void V_FirstCore()
        {
            Model.Font.Size = Constants.noteFontSize.median;
            Model.ForeColor = Constants.noteColor.ForeColor;
            Model.BackColor = Constants.noteColor.BackColor;
            Model.NoteTitle = NoteTitle.Timestamp;
            Model.IsTopmost = false;
            Model.AutoLineFeed = true;
        }

        protected override void V_0_77_0Core()
        {
            Model.IsTopmost = false;
            Model.AutoLineFeed = true;
        }

        #endregion

        //public static void Correction(NoteSettingModel setting, Version previousVersion, INonProcess nonProcess)
        //{
        //    V_First(setting, previousVersion, nonProcess);

        //    V_0_77_0(setting, previousVersion, nonProcess);

        //    V_Last(setting, previousVersion, nonProcess);
        //}

        //static void V_Last(NoteSettingModel setting, Version previousVersion, INonProcess nonProcess)
        //{
        //    if(previousVersion == null) {
        //        setting.Font.Size = Constants.noteFontSize.median;
        //        setting.Font.Family = FontUtility.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily);
        //    }
        //    setting.Font.Size = Constants.noteFontSize.GetClamp(setting.Font.Size);
        //    setting.NoteTitle = EnumUtility.GetNormalization(setting.NoteTitle, NoteTitle.Timestamp);

        //    if(setting.ForeColor == default(Color)) {
        //        setting.ForeColor = Constants.noteColor.ForeColor;
        //    }
        //    if(setting.BackColor == default(Color)) {
        //        setting.BackColor = Constants.noteColor.BackColor;
        //    }
        //}

        ///// <summary>
        ///// 0.77.0.340 以下のバージョン補正。
        ///// </summary>
        ///// <param name="item"></param>
        ///// <param name="previousVersion"></param>
        ///// <param name="nonProcess"></param>
        //static void V_0_77_0(NoteSettingModel setting, Version previousVersion, INonProcess nonProcess)
        //{
        //    if(new Version(0, 77, 0, 340) < previousVersion) {
        //        return;
        //    }

        //    nonProcess.Logger.Trace("version setting: 0.77.0");

        //    setting.IsTopmost = false;
        //    setting.AutoLineFeed = true;
        //}

        //static void V_First(NoteSettingModel setting, Version previousVersion, INonProcess nonProcess)
        //{
        //    if(previousVersion != null) {
        //        return;
        //    }

        //    setting.Font.Size = Constants.noteFontSize.median;
        //    setting.ForeColor = Constants.noteColor.ForeColor;
        //    setting.BackColor = Constants.noteColor.BackColor;
        //    setting.NoteTitle = NoteTitle.Timestamp;
        //    setting.IsTopmost = false;
        //    setting.AutoLineFeed = true;
        //}
    }
}
