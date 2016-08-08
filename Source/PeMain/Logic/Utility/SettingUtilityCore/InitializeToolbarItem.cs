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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    internal sealed class InitializeToolbarItem: InitializeBase<ToolbarItemModel>
    {
        public InitializeToolbarItem(ToolbarItemModel model, Version previousVersion, INonProcess nonProcess)
            : base(model, previousVersion, nonProcess)
        { }

        #region InitializeBase

        protected override void Correction_Last()
        {
            Model.HideWaitTime = Constants.toolbarHideWaitTime.GetClamp(Model.HideWaitTime);
            Model.HideAnimateTime = Constants.toolbarHideAnimateTime.GetClamp(Model.HideAnimateTime);
            Model.Font.Size = Constants.toolbarFontSize.GetClamp(Model.Font.Size);
            Model.IconScale = EnumUtility.GetNormalization(Model.IconScale, IconScale.Normal);
            Model.TextWidth = Constants.toolbarTextLength.GetClamp((int)Model.TextWidth);
            Model.ButtonPosition = EnumUtility.GetNormalization(Model.ButtonPosition, ToolbarButtonPosition.Near);

            if(IsIllegalPlusNumber(Model.FloatToolbar.WidthButtonCount)) {
                Model.FloatToolbar.WidthButtonCount = 1;
            }
            if(IsIllegalPlusNumber(Model.FloatToolbar.HeightButtonCount)) {
                Model.FloatToolbar.HeightButtonCount = 1;
            }
        }

        protected override void Correction_First()
        {
            Model.IsVisible = true;
            Model.IsTopmost = true;
            Model.TextWidth = Constants.toolbarTextLength.median;
            Model.IconScale = IconScale.Normal;
            Model.HideWaitTime = Constants.toolbarHideWaitTime.median;
            Model.HideAnimateTime = Constants.toolbarHideAnimateTime.median;
            Model.Font.Size = Constants.toolbarFontSize.median;
            Model.FloatToolbar.WidthButtonCount = 1;
            Model.FloatToolbar.HeightButtonCount = 1;
            Model.DockType = DockType.Right;
            Model.DefaultGroupId = Guid.Empty;
            Model.MenuPositionCorrection = true;
            Model.ButtonPosition = ToolbarButtonPosition.Near;
            Model.IsVisibleMenuButton = true;
        }

        protected override void Correction_0_78_0()
        {
            Model.IsVisibleMenuButton = true;
        }

        #endregion
    }
}
