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

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
    internal static class InitializeNoteIndexItem
    {
        public static void Correction(NoteIndexItemModel indexItem, Version previousVersion, INonProcess nonProcess)
        {
            V_First(indexItem, previousVersion, nonProcess);
            V_0_69_0(indexItem, previousVersion, nonProcess);
            V_Last(indexItem, previousVersion, nonProcess);
        }

        static void V_Last(NoteIndexItemModel indexItem, Version previousVersion, INonProcess nonProcess)
        {
            indexItem.NoteKind = EnumUtility.GetNormalization(indexItem.NoteKind, NoteKind.Text);
            indexItem.Font.Size = Constants.noteFontSize.GetClamp(indexItem.Font.Size);

            if(string.IsNullOrWhiteSpace(indexItem.Font.Family)) {
                indexItem.Font.Family = FontUtility.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily);
            }

            if(SettingUtility.IsIllegalPlusNumber(indexItem.WindowWidth)) {
                indexItem.WindowWidth = Constants.noteDefualtSize.Width;
            }
            if(SettingUtility.IsIllegalPlusNumber(indexItem.WindowHeight)) {
                indexItem.WindowHeight = Constants.noteDefualtSize.Height;
            }
        }

        static void V_First(NoteIndexItemModel indexItem, Version previousVersion, INonProcess nonProcess)
        {
            if(previousVersion != null) {
                return;
            }

            indexItem.NoteKind = NoteKind.Text;
            //indexItem.Font.Size = Constants.noteFontSize.median;
            //indexItem.Font.Family = FontUtility.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily);
            //indexItem.WindowWidth = Constants.noteDefualtSize.Width;
            //indexItem.WindowHeight = Constants.noteDefualtSize.Height;
            indexItem.IsLocked = false;
            indexItem.IsCompacted = false;
            indexItem.IsTopmost = false;
            indexItem.AutoLineFeed = true;
        }

        static void V_0_69_0(NoteIndexItemModel indexItem, Version previousVersion, INonProcess nonProcess)
        {
            if(new Version(0,69,0,38641) < previousVersion) {
                return;
            }

            nonProcess.Logger.Trace("version setting: 0.69.0");

            indexItem.AutoLineFeed = true;
        }

    }
}
