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
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    internal sealed class InitializeNoteIndexItem: InitializeBase<NoteIndexItemModel>
    {
        public InitializeNoteIndexItem(NoteIndexItemModel model, Version previousVersion, INonProcess nonProcess)
            : base(model, previousVersion, nonProcess)
        { }

        #region InitializeNoteIndexItem

        protected override void Correction_Last()
        {
            Model.NoteKind = EnumUtility.GetNormalization(Model.NoteKind, NoteKind.Text);
            Model.Font.Size = Constants.noteFontSize.GetClamp(Model.Font.Size);

            if(string.IsNullOrWhiteSpace(Model.Font.Family)) {
                Model.Font.Family = FontUtility.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily);
            }

            if(SettingUtility.IsIllegalPlusNumber(Model.WindowWidth)) {
                Model.WindowWidth = Constants.noteDefualtSize.Width;
            }
            if(SettingUtility.IsIllegalPlusNumber(Model.WindowHeight)) {
                Model.WindowHeight = Constants.noteDefualtSize.Height;
            }
        }

        protected override void Correction_First()
        {
            Model.NoteKind = NoteKind.Text;
            //indexItem.Font.Size = Constants.noteFontSize.median;
            //indexItem.Font.Family = FontUtility.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily);
            //indexItem.WindowWidth = Constants.noteDefualtSize.Width;
            //indexItem.WindowHeight = Constants.noteDefualtSize.Height;
            Model.IsLocked = false;
            Model.IsCompacted = false;
            Model.IsTopmost = false;
            Model.AutoLineFeed = true;
        }

        protected override void Correction_0_69_0()
        {
            Model.AutoLineFeed = true;
        }

        #endregion
    }
}
