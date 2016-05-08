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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.IF;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Property;
using ContentTypeTextNet.Pe.PeMain.View.Parts.Control.SettingPage;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
    public class NoteSettingViewModel: SettingPageViewModelBase<NoteSettingControl>, IColorPair
    {
        public NoteSettingViewModel(NoteSettingModel note, NoteSettingControl view, IAppNonProcess appNonProcess, SettingNotifyData settingNotifiyItem)
            : base(view, appNonProcess, settingNotifiyItem)
        {
            Note = note;
        }

        #region property

        NoteSettingModel Note { get; set; }

        public HotKeyModel CreateHotKey
        {
            get { return Note.CreateHotKey; }
            set { SetPropertyValue(Note, value); }
        }

        public HotKeyModel HideHotKey
        {
            get { return Note.HideHotKey; }
            set { SetPropertyValue(Note, value); }
        }

        public HotKeyModel CompactHotKey
        {
            get { return Note.CompactHotKey; }
            set { SetPropertyValue(Note, value); }
        }

        public HotKeyModel ShowFrontHotKey
        {
            get { return Note.ShowFrontHotKey; }
            set { SetPropertyValue(Note, value); }
        }

        //public FontFamily FontFamily
        //{
        //	get { return FontUtility.MakeFontFamily(Note.Font.Family, SystemFonts.MessageFontFamily); }
        //	set
        //	{
        //		if(value != null) {
        //			var fontFamily = FontUtility.GetOriginalFontFamilyName(value);
        //			SetPropertyValue(Note.Font, fontFamily, "Family");
        //		}
        //	}
        //}

        //public bool FontBold
        //{
        //	get { return Note.Font.Bold; }
        //	set { SetPropertyValue(Note.Font, value, "Bold"); }
        //}

        //public bool FontItalic
        //{
        //	get { return Note.Font.Italic; }
        //	set { SetPropertyValue(Note.Font, value, "Italic"); }
        //}

        //public double FontSize
        //{
        //	get { return Note.Font.Size; }
        //	set { SetPropertyValue(Note.Font, value, "Size"); }
        //}
        #region font

        public FontFamily FontFamily
        {
            get { return FontModelProperty.GetFamilyDefault(Note.Font); }
            set { FontModelProperty.SetFamily(Note.Font, value, OnPropertyChanged); }
        }

        public bool FontBold
        {
            get { return FontModelProperty.GetBold(Note.Font); }
            set { FontModelProperty.SetBold(Note.Font, value, OnPropertyChanged); }
        }

        public bool FontItalic
        {
            get { return FontModelProperty.GetItalic(Note.Font); }
            set { FontModelProperty.SetItalic(Note.Font, value, OnPropertyChanged); }
        }

        public double FontSize
        {
            get { return FontModelProperty.GetSize(Note.Font); }
            set { FontModelProperty.SetSize(Note.Font, value, OnPropertyChanged); }
        }

        #endregion

        public NoteTitle NoteTitle
        {
            get { return Note.NoteTitle; }
            set { SetPropertyValue(Note, value); }
        }

        public bool IsTopmost
        {
            get { return TopMostProperty.GetTopMost(Note); }
            set { TopMostProperty.SetTopMost(Note, value, OnPropertyChanged); }
        }

        public bool AutoLineFeed
        {
            get { return Note.AutoLineFeed; }
            set { SetPropertyValue(Note, value); }
        }

        #endregion

        #region IColorPair

        public Color ForeColor
        {
            get { return ColorPairProperty.GetNoneAlphaForeColor(Note); }
            set { ColorPairProperty.SetNoneAlphaForekColor(Note, value, OnPropertyChanged); }
        }

        public Color BackColor
        {
            get { return ColorPairProperty.GetNoneAlphaBackColor(Note); }
            set { ColorPairProperty.SetNoneAlphaBackColor(Note, value, OnPropertyChanged); }
        }

        #endregion

    }
}
