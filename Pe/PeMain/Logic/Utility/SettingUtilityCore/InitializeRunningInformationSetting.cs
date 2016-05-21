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
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    internal sealed class InitializeRunningInformationSetting: InitializeBase<RunningInformationSettingModel>
    {
        public InitializeRunningInformationSetting(RunningInformationSettingModel model, Version previousVersion, INonProcess nonProcess)
            : base(model, previousVersion, nonProcess)
        { }

        #region InitializeBase

        protected override void Correction_Last()
        {
            if(!SettingUtility.CheckUserId(Model)) {
                Model.UserId = SettingUtility.CreateUserIdFromEnvironment();
            }

            if(Model.FirstRunning.Version == null) {
                Model.FirstRunning.Timestamp = DateTime.Now;
                Model.FirstRunning.Version = Constants.ApplicationVersionNumber;
            }
        }

        protected override void Correction_First()
        {
            Model.UserId = SettingUtility.CreateUserIdFromEnvironment();
        }

        protected override void Correction_0_71_0()
        {
            Model.UserId = SettingUtility.CreateUserIdFromEnvironment();
            Model.FirstRunning = new FirstRunningItemModel() {
                Timestamp = DateTime.Now,
                Version = PreviousVersion,
            };
        }

        #endregion
    }
}
