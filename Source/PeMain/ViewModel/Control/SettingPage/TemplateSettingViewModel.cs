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
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Property;
using ContentTypeTextNet.Pe.PeMain.View.Parts.Control.SettingPage;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
    public class TemplateSettingViewModel: SettingPageViewModelBase<TemplateSettingControl>
    {
        public TemplateSettingViewModel(TemplateSettingModel template, TemplateSettingControl view, IAppNonProcess appNonProcess, SettingNotifyData settingNotifiyItem)
            : base(view, appNonProcess, settingNotifiyItem)
        {
            Template = template;
        }

        #region property

        TemplateSettingModel Template { get; set; }

        public bool IsTopmost
        {
            get { return Template.IsTopmost; }
            set { SetPropertyValue(Template, value); }
        }

        public bool IsVisible
        {
            get { return Template.IsVisible; }
            set { SetPropertyValue(Template, value); }
        }

        public HotKeyModel ToggleHotKey
        {
            get { return Template.ToggleHotKey; }
            set { SetPropertyValue(Template, value); }
        }

        //public FontFamily FontFamily
        //{
        //	get { return FontUtility.MakeFontFamily(Template.Font.Family, SystemFonts.MessageFontFamily); }
        //	set
        //	{
        //		if(value != null) {
        //			var fontFamily = FontUtility.GetOriginalFontFamilyName(value);
        //			SetPropertyValue(Template.Font, fontFamily, "Family");
        //		}
        //	}
        //}

        //public bool FontBold
        //{
        //	get { return Template.Font.Bold; }
        //	set { SetPropertyValue(Template.Font, value, "Bold"); }
        //}

        //public bool FontItalic
        //{
        //	get { return Template.Font.Italic; }
        //	set { SetPropertyValue(Template.Font, value, "Italic"); }
        //}

        //public double FontSize
        //{
        //	get { return Template.Font.Size; }
        //	set { SetPropertyValue(Template.Font, value, "Size"); }
        //}

        #region font

        public FontFamily FontFamily
        {
            get { return FontModelProperty.GetFamilyDefault(Template.Font); }
            set { FontModelProperty.SetFamily(Template.Font, value, OnPropertyChanged); }
        }

        public bool FontBold
        {
            get { return FontModelProperty.GetBold(Template.Font); }
            set { FontModelProperty.SetBold(Template.Font, value, OnPropertyChanged); }
        }

        public bool FontItalic
        {
            get { return FontModelProperty.GetItalic(Template.Font); }
            set { FontModelProperty.SetItalic(Template.Font, value, OnPropertyChanged); }
        }

        public double FontSize
        {
            get { return FontModelProperty.GetSize(Template.Font); }
            set { FontModelProperty.SetSize(Template.Font, value, OnPropertyChanged); }
        }

        #endregion

        public IndexItemsDoubleClickBehavior DoubleClickBehavior
        {
            get { return Template.DoubleClickBehavior; }
            set { SetPropertyValue(Template, value); }
        }

        #endregion
    }
}
