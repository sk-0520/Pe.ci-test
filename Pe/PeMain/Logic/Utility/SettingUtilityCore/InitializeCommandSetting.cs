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
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    internal sealed class InitializeCommandSetting: InitializeBase<CommandSettingModel>
    {
        public InitializeCommandSetting(CommandSettingModel setting, Version previousVersion, INonProcess nonProcess)
            :base(setting, previousVersion, nonProcess)
        { }

        #region InitializeBase

        protected override void Correction_Last()
        {
            Model.IconScale = EnumUtility.GetNormalization(Model.IconScale, IconScale.Small);
            Model.WindowWidth = Constants.commandWindowWidth.GetClamp(Model.WindowWidth);
            Model.Font.Size = Constants.commandFontSize.GetClamp(Model.Font.Size);
            Model.HideTime = Constants.commandHideTime.GetClamp(Model.HideTime);
        }

        protected override void Correction_First()
        {
            Model.IconScale = IconScale.Small;
            Model.HideTime = Constants.commandHideTime.median;
            Model.WindowWidth = Constants.commandWindowWidth.median;
            Model.Font.Size = Constants.commandFontSize.median;
            Model.FindTag = true;
            Model.FindFile = false;
        }

        #endregion
    }
}
