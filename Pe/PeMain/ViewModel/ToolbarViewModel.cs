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
namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
    using ContentTypeTextNet.Library.SharedLibrary.Define;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
    using ContentTypeTextNet.Pe.Library.PeData.IF;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.PeMain.Logic.Property;
    using ContentTypeTextNet.Pe.PeMain.IF;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
    using ContentTypeTextNet.Pe.PeMain.Data;
    using System.Windows.Media;
    using System.Windows;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using ContentTypeTextNet.Pe.Library.PeData.Define;

    public class ToolbarViewModel: SingleModelWrapperViewModelBase<ToolbarItemModel>, IHasAppNonProcess
    {
        public ToolbarViewModel(ToolbarItemModel toolbarItemModel, LauncherGroupItemCollectionModel group, IAppNonProcess appNonProcess)
            : base(toolbarItemModel)
        {
            Group = group;
            AppNonProcess = appNonProcess;
        }

        #region property

        LauncherGroupItemCollectionModel Group { get; set; }

        public Guid DefaultGroupId
        {
            get { return Model.DefaultGroupId; }
            set { SetModelValue(value); }
        }

        public DockType DockType
        {
            get { return Model.DockType; }
            set { SetModelValue(value); }
        }

        public IconScale IconScale
        {
            get { return Model.IconScale; }
            set { SetModelValue(value); }
        }

        public bool TextVisible
        {
            get { return Model.TextVisible; }
            set { SetModelValue(value); }
        }

        public int TextWidth
        {
            get { return (int)Model.TextWidth; }
            set { SetModelValue((double)value); }
        }

        public bool AutoHide
        {
            get { return Model.AutoHide; }
            set { SetModelValue(value); }
        }

        public TimeSpan HideWaitTime
        {
            get { return Model.HideWaitTime; }
            set { SetModelValue(value); }
        }

        public bool MenuPositionCorrection
        {
            get { return Model.MenuPositionCorrection; }
            set { SetModelValue(value); }
        }

        #region font

        //public FontFamily FontFamily
        //{
        //	get { return FontUtility.MakeFontFamily(Model.Font.Family, SystemFonts.MessageFontFamily); }
        //	set
        //	{
        //		if (value != null) {
        //			var fontFamily = FontUtility.GetOriginalFontFamilyName(value);
        //			SetPropertyValue(Model.Font, fontFamily, "Family");
        //		}
        //	}
        //}

        //public bool FontBold
        //{
        //	get { return Model.Font.Bold; }
        //	set { SetPropertyValue(Model.Font, value, "Bold"); }
        //}

        //public bool FontItalic
        //{
        //	get { return Model.Font.Italic; }
        //	set { SetPropertyValue(Model.Font, value, "Italic"); }
        //}

        //public double FontSize
        //{
        //	get { return Model.Font.Size; }
        //	set { SetPropertyValue(Model.Font, value, "Size"); }
        //}

        public FontFamily FontFamily
        {
            get { return FontModelProperty.GetFamilyDefault(Model.Font); }
            set { FontModelProperty.SetFamily(Model.Font, value, OnPropertyChanged); }
        }

        public bool FontBold
        {
            get { return FontModelProperty.GetBold(Model.Font); }
            set { FontModelProperty.SetBold(Model.Font, value, OnPropertyChanged); }
        }

        public bool FontItalic
        {
            get { return FontModelProperty.GetItalic(Model.Font); }
            set { FontModelProperty.SetItalic(Model.Font, value, OnPropertyChanged); }
        }

        public double FontSize
        {
            get { return FontModelProperty.GetSize(Model.Font); }
            set { FontModelProperty.SetSize(Model.Font, value, OnPropertyChanged); }
        }

        #endregion

        #region ITopMost

        public bool IsTopmost
        {
            get { return TopMostProperty.GetTopMost(Model); }
            set { TopMostProperty.SetTopMost(Model, value, OnPropertyChanged); }
        }

        #endregion

        public bool IsVisible
        {
            get { return VisibleVisibilityProperty.GetVisible(Model); }
            set { VisibleVisibilityProperty.SetVisible(Model, value, OnPropertyChanged); }
        }

        public ToolbarButtonPosition ButtonPosition
        {
            get { return Model.ButtonPosition; }
            set { SetModelValue(value); }
        }

        #endregion

        #region IHasAppNonProcess

        public IAppNonProcess AppNonProcess { get; set; }

        #endregion

        #region SingleModelWrapperViewModelBase

        public override string DisplayText
        {
            get
            {
                return ScreenUtility.GetScreenName(Model.Id, AppNonProcess.Logger);
            }
        }

        #endregion
    }
}
