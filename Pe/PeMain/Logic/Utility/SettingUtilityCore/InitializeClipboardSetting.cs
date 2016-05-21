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
    internal static class InitializeClipboardSetting
    {
        public static void Correction(ClipboardSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            V_First(setting, previousVersion, nonProcess);
            V_0_65_0(setting, previousVersion, nonProcess);
            V_0_70_0(setting, previousVersion, nonProcess);
            V_Last(setting, previousVersion, nonProcess);
        }

        static void V_Last(ClipboardSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            setting.WaitTime = Constants.clipboardWaitTime.GetClamp(setting.WaitTime);
            setting.DuplicationCount = Constants.clipboardDuplicationCount.GetClamp(setting.DuplicationCount);
            setting.Font.Size = Constants.clipboardFontSize.GetClamp(setting.Font.Size);
            //setting.CaptureType = EnumUtility.GetNormalization(setting.CaptureType, Constants.clipboardCaptureType);

            if(SettingUtility.IsIllegalPlusNumber(setting.ItemsListWidth)) {
                setting.ItemsListWidth = Constants.clipboardItemsListWidth;
            }

            if(SettingUtility.IsIllegalPlusNumber(setting.WindowWidth)) {
                setting.WindowWidth = Constants.clipboardDefaultWindowSize.Width;
            }
            if(SettingUtility.IsIllegalPlusNumber(setting.WindowHeight)) {
                setting.WindowHeight = Constants.clipboardDefaultWindowSize.Height;
            }

            //setting.LimitSize.LimitType = EnumUtility.GetNormalization(setting.LimitSize.LimitType, Constants.clipboardLimitType);
            setting.LimitSize.Text = Constants.clipboardLimitTextSize.GetClamp(setting.LimitSize.Text);
            setting.LimitSize.Rtf = Constants.clipboardLimitRtfSize.GetClamp(setting.LimitSize.Rtf);
            setting.LimitSize.Html = Constants.clipboardLimitHtmlSize.GetClamp(setting.LimitSize.Html);

            setting.LimitSize.ImageWidth = Constants.clipboardLimitImageWidthSize.GetClamp(setting.LimitSize.ImageWidth);
            setting.LimitSize.ImageHeight = Constants.clipboardLimitImageHeightSize.GetClamp(setting.LimitSize.ImageHeight);
        }

        static void V_First(ClipboardSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(previousVersion != null) {
                return;
            }

            nonProcess.Logger.Trace("version setting: first");

            setting.IsEnabled = true;
            setting.CaptureType = Constants.clipboardCaptureType;
            setting.UsingClipboard = false;
            setting.SaveCount = 0;
            setting.DuplicationCount = Constants.clipboardDuplicationCount.median;
            setting.WaitTime = Constants.clipboardWaitTime.median;
            setting.Font.Size = Constants.clipboardFontSize.median;
            setting.ItemsListWidth = Constants.clipboardItemsListWidth;
            setting.WindowWidth = Constants.clipboardDefaultWindowSize.Width;
            setting.WindowHeight = Constants.clipboardDefaultWindowSize.Height;
            setting.LimitSize.LimitType = Constants.clipboardLimitType;
            setting.LimitSize.Text = Constants.clipboardLimitTextSize.median;
            setting.LimitSize.Rtf = Constants.clipboardLimitRtfSize.median;
            setting.LimitSize.Html = Constants.clipboardLimitHtmlSize.median;
            setting.LimitSize.ImageWidth = Constants.clipboardLimitImageWidthSize.median;
            setting.LimitSize.ImageHeight = Constants.clipboardLimitImageHeightSize.median;
            setting.DuplicationMoveHead = true;
        }

        static void V_0_65_0(ClipboardSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(new Version(0, 65, 0, 43015) < previousVersion) {
                return;
            }

            nonProcess.Logger.Trace("version setting: 0.65.0");

            setting.DuplicationMoveHead = true;
        }

        static void V_0_70_0(ClipboardSettingModel setting, Version previousVersion, INonProcess nonProcess)
        {
            if(new Version(0,70,0,40764) < previousVersion) {
                return;
            }

            nonProcess.Logger.Trace("version setting: 0.70.0");

            if(setting.DuplicationCount == Constants.Issue_363_oldMediumCount) {
                setting.DuplicationCount = Constants.ClipboardDuplicationMedianCount;
            }
        }
    }
}
