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
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    internal class InitializeClipboardIndexItem: InitializeBase<ClipboardIndexItemModel>
    {
        public InitializeClipboardIndexItem(ClipboardIndexItemModel model, Version previousVersion, INonProcess nonProcess)
            : base(model, previousVersion, nonProcess)
        { }

        #region InitializeBase

        protected override void V_LastCore()
        { }

        protected override void V_FirstCore()
        { }

        protected override void V_0_70_0Core()
        {
            Model.Sort = Model.History.CreateTimestamp;
        }

        #endregion

        //public static void Correction(ClipboardIndexItemModel model, Version previousVersion, INonProcess nonProcess)
        //{
        //    V_First(model, previousVersion, nonProcess);
        //    V_0_70_0(model, previousVersion, nonProcess);
        //    V_Last(model, previousVersion, nonProcess);
        //}

        //static void V_Last(ClipboardIndexItemModel model, Version previousVersion, INonProcess nonProcess)
        //{ }

        //static void V_First(ClipboardIndexItemModel model, Version previousVersion, INonProcess nonProcess)
        //{
        //    if(previousVersion != null) {
        //        return;
        //    }
        //}

        //private static void V_0_70_0(ClipboardIndexItemModel model, Version previousVersion, INonProcess nonProcess)
        //{
        //    if(new Version(0,70, 0, 40764) < previousVersion) {
        //        return;
        //    }

        //    model.Sort = model.History.CreateTimestamp;
        //}
    }
}
