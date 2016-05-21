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
    internal sealed class InitializeLauncherGroupItem: InitializeBase<LauncherGroupItemModel>
    {
        public InitializeLauncherGroupItem(LauncherGroupItemModel model, Version previousVersion, INonProcess nonProcess)
            : base(model, previousVersion, nonProcess)
        { }

        #region InitializeBase

        protected override void V_LastCore()
        {
            Model.GroupKind = EnumUtility.GetNormalization(Model.GroupKind, GroupKind.LauncherItems);
            Model.GroupIconType = EnumUtility.GetNormalization(Model.GroupIconType, LauncherGroupIconType.File);
        }

        protected override void V_FirstCore()
        {
            Model.GroupKind = GroupKind.LauncherItems;
            Model.GroupIconType = Constants.launcherGroupIconType;
            Model.GroupIconColor = Constants.launcherGroupIconColor;
        }

        protected override void V_0_77_0Core()
        {
            Model.GroupIconType = Constants.launcherGroupIconType;
            Model.GroupIconColor = Constants.launcherGroupIconColor;
        }

        #endregion

        //public static void Correction(LauncherGroupItemModel item, Version previousVersion, INonProcess nonProcess)
        //{
        //    V_First(item, previousVersion, nonProcess);

        //    V_0_77_0(item, previousVersion, nonProcess);

        //    V_Last(item, previousVersion, nonProcess);
        //}

        //static void V_Last(LauncherGroupItemModel item, Version previousVersion, INonProcess nonProcess)
        //{
        //    item.GroupKind = EnumUtility.GetNormalization(item.GroupKind, GroupKind.LauncherItems);
        //    item.GroupIconType = EnumUtility.GetNormalization(item.GroupIconType, LauncherGroupIconType.File);
        //}

        ///// <summary>
        ///// 0.77.0.340 以下のバージョン補正。
        ///// </summary>
        ///// <param name="item"></param>
        ///// <param name="previousVersion"></param>
        ///// <param name="nonProcess"></param>
        //static void V_0_77_0(LauncherGroupItemModel item, Version previousVersion, INonProcess nonProcess)
        //{
        //    if(new Version(0, 77, 0, 340) < previousVersion) {
        //        return;
        //    }

        //    nonProcess.Logger.Trace("version setting: 0.77.0");

        //    item.GroupIconType = Constants.launcherGroupIconType;
        //    item.GroupIconColor = Constants.launcherGroupIconColor;
        //}

        //static void V_First(LauncherGroupItemModel item, Version previousVersion, INonProcess nonProcess)
        //{
        //    if(previousVersion != null) {
        //        return;
        //    }

        //    item.GroupKind = GroupKind.LauncherItems;
        //    item.GroupIconType = Constants.launcherGroupIconType;
        //    item.GroupIconColor = Constants.launcherGroupIconColor;
        //}
    }
}
