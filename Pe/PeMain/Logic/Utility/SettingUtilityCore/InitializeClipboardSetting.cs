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
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    internal sealed class InitializeClipboardSetting: InitializeBase<ClipboardSettingModel>
    {
        public InitializeClipboardSetting(ClipboardSettingModel setting, Version previousVersion, INonProcess nonProcess)
            :base(setting, previousVersion, nonProcess)
        { }

        #region InitializeBase

        protected override void Correction_Last()
        {
            Model.WaitTime = Constants.clipboardWaitTime.GetClamp(Model.WaitTime);
            Model.DuplicationCount = Constants.clipboardDuplicationCount.GetClamp(Model.DuplicationCount);
            Model.Font.Size = Constants.clipboardFontSize.GetClamp(Model.Font.Size);
            //setting.CaptureType = EnumUtility.GetNormalization(setting.CaptureType, Constants.clipboardCaptureType);

            if(SettingUtility.IsIllegalPlusNumber(Model.ItemsListWidth)) {
                Model.ItemsListWidth = Constants.clipboardItemsListWidth;
            }

            if(SettingUtility.IsIllegalPlusNumber(Model.WindowWidth)) {
                Model.WindowWidth = Constants.clipboardDefaultWindowSize.Width;
            }
            if(SettingUtility.IsIllegalPlusNumber(Model.WindowHeight)) {
                Model.WindowHeight = Constants.clipboardDefaultWindowSize.Height;
            }

            //setting.LimitSize.LimitType = EnumUtility.GetNormalization(setting.LimitSize.LimitType, Constants.clipboardLimitType);
            Model.LimitSize.Text = Constants.clipboardLimitTextSize.GetClamp(Model.LimitSize.Text);
            Model.LimitSize.Rtf = Constants.clipboardLimitRtfSize.GetClamp(Model.LimitSize.Rtf);
            Model.LimitSize.Html = Constants.clipboardLimitHtmlSize.GetClamp(Model.LimitSize.Html);

            Model.LimitSize.ImageWidth = Constants.clipboardLimitImageWidthSize.GetClamp(Model.LimitSize.ImageWidth);
            Model.LimitSize.ImageHeight = Constants.clipboardLimitImageHeightSize.GetClamp(Model.LimitSize.ImageHeight);
        }

        protected override void Correction_First()
        {
            Model.IsEnabled = true;
            Model.CaptureType = Constants.clipboardCaptureType;
            Model.UsingClipboard = false;
            Model.SaveCount = 0;
            Model.DuplicationCount = Constants.clipboardDuplicationCount.median;
            Model.WaitTime = Constants.clipboardWaitTime.median;
            Model.Font.Size = Constants.clipboardFontSize.median;
            Model.ItemsListWidth = Constants.clipboardItemsListWidth;
            Model.WindowWidth = Constants.clipboardDefaultWindowSize.Width;
            Model.WindowHeight = Constants.clipboardDefaultWindowSize.Height;
            Model.LimitSize.LimitType = Constants.clipboardLimitType;
            Model.LimitSize.Text = Constants.clipboardLimitTextSize.median;
            Model.LimitSize.Rtf = Constants.clipboardLimitRtfSize.median;
            Model.LimitSize.Html = Constants.clipboardLimitHtmlSize.median;
            Model.LimitSize.ImageWidth = Constants.clipboardLimitImageWidthSize.median;
            Model.LimitSize.ImageHeight = Constants.clipboardLimitImageHeightSize.median;
            Model.DuplicationMoveHead = true;
        }

        protected override void Correction_0_65_0()
        {
            Model.DuplicationMoveHead = true;
        }

        protected override void Correction_0_70_0()
        {
            if(Model.DuplicationCount == Constants.Issue_363_oldMediumCount) {
                Model.DuplicationCount = Constants.ClipboardDuplicationMedianCount;
            }
        }

        #endregion
    }
}
