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
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    internal sealed class InitializeLoggingSetting: InitializeBase<LoggingSettingModel>
    {
        public InitializeLoggingSetting(LoggingSettingModel setting, Version previousVersion, INonProcess nonProcess)
            :base(setting, previousVersion, nonProcess)
        { }

        #region InitializeLoggingSetting

        protected override void Correction_Last()
        {
            if(SettingUtility.IsIllegalPlusNumber(Model.WindowWidth)) {
                Model.WindowWidth = Constants.loggingDefaultWindowSize.Width;
            }
            if(SettingUtility.IsIllegalPlusNumber(Model.WindowHeight)) {
                Model.WindowHeight = Constants.loggingDefaultWindowSize.Height;
            }
        }

        protected override void Correction_First()
        {
            Model.WindowWidth = Constants.loggingDefaultWindowSize.Width;
            Model.WindowHeight = Constants.loggingDefaultWindowSize.Height;
            Model.AddShow = true;
            Model.IsVisible = false;
            Model.ShowTriggerDebug = false;
            Model.ShowTriggerTrace = false;
            Model.ShowTriggerInformation = false;
            Model.ShowTriggerWarning = true;
            Model.ShowTriggerError = true;
            Model.ShowTriggerFatal = true;
        }

        #endregion
    }
}
