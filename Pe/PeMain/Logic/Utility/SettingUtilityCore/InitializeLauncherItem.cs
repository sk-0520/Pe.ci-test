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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    internal sealed class InitializeLauncherItem: InitializeBase<LauncherItemModel>
    {
        public InitializeLauncherItem(LauncherItemModel model, Version previousVersion, INonProcess nonProcess)
            :base(model, previousVersion, nonProcess)
        { }

        #region InitializeBase

        protected override void Correction_Last()
        {
            Model.LauncherKind = EnumUtility.GetNormalization(Model.LauncherKind, LauncherKind.File);
            // あるだけ
            if(Model.LauncherKind == LauncherKind.Directory) {
                Model.LauncherKind = LauncherKind.File;
            }

            if(SettingUtility.IsIllegalString(Model.Command)) {
                Model.Command = string.Empty;
            }
        }

        protected override void Correction_First()
        {
            Model.LauncherKind = LauncherKind.File;
            Model.IsCommandAutocomplete = true;
        }

        protected override void Correction_0_77_0()
        {
            Model.IsCommandAutocomplete = true;
        }

        #endregion
    }
}
