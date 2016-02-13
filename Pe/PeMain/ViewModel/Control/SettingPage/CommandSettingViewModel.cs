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
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Property;
using ContentTypeTextNet.Pe.PeMain.View.Parts.Control.SettingPage;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
    public class CommandSettingViewModel: SettingPageViewModelBase<CommandSettingControl>
    {
        public CommandSettingViewModel(CommandSettingModel command, CommandSettingControl view, IAppNonProcess appNonProcess, SettingNotifyData settingNotifiyItem)
            : base(view, appNonProcess, settingNotifiyItem)
        {
            Command = command;
        }

        #region property

        CommandSettingModel Command { get; set; }

        public IconScale IconScale
        {
            get { return Command.IconScale; }
            set { SetPropertyValue(Command, value); }
        }

        //public double HideTimeMs
        //{
        //	get { return Command.HideTime.TotalMilliseconds; }
        //	set { SetPropertyValue(Command, TimeSpan.FromMilliseconds(value), "HideTime"); }
        //}

        public TimeSpan HideTime
        {
            get { return Command.HideTime; }
            set { SetPropertyValue(Command, value); }
        }


        public HotKeyModel ShowHotkey
        {
            get { return Command.ShowHotkey; }
            set { SetPropertyValue(Command, value); }
        }

        public bool FindTag
        {
            get { return Command.FindTag; }
            set { SetPropertyValue(Command, value); }
        }

        public bool FindFile
        {
            get { return Command.FindFile; }
            set { SetPropertyValue(Command, value); }
        }

        //public FontFamily FontFamily
        //{
        //	get { return FontUtility.MakeFontFamily(Command.Font.Family, SystemFonts.MessageFontFamily); }
        //	set
        //	{
        //		if(value != null) {
        //			var fontFamily = FontUtility.GetOriginalFontFamilyName(value);
        //			SetPropertyValue(Command.Font, fontFamily, "Family");
        //		}
        //	}
        //}

        //public bool FontBold
        //{
        //	get { return Command.Font.Bold; }
        //	set { SetPropertyValue(Command.Font, value, "Bold"); }
        //}

        //public bool FontItalic
        //{
        //	get { return Command.Font.Italic; }
        //	set { SetPropertyValue(Command.Font, value, "Italic"); }
        //}

        //public double FontSize
        //{
        //	get { return Command.Font.Size; }
        //	set { SetPropertyValue(Command.Font, value, "Size"); }
        //}

        #region font

        public FontFamily FontFamily
        {
            get { return FontModelProperty.GetFamilyDefault(Command.Font); }
            set { FontModelProperty.SetFamily(Command.Font, value, OnPropertyChanged); }
        }

        public bool FontBold
        {
            get { return FontModelProperty.GetBold(Command.Font); }
            set { FontModelProperty.SetBold(Command.Font, value, OnPropertyChanged); }
        }

        public bool FontItalic
        {
            get { return FontModelProperty.GetItalic(Command.Font); }
            set { FontModelProperty.SetItalic(Command.Font, value, OnPropertyChanged); }
        }

        public double FontSize
        {
            get { return FontModelProperty.GetSize(Command.Font); }
            set { FontModelProperty.SetSize(Command.Font, value, OnPropertyChanged); }
        }

        #endregion

        #endregion
    }
}
