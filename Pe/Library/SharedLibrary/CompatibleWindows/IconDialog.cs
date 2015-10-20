﻿/**
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.PInvoke.Windows;
    using ContentTypeTextNet.Library.SharedLibrary.Model;
    using Microsoft.Win32;

    public class IconDialog: CommonDialog
    {
        public IconDialog()
            : base()
        {
            Reset();
        }

        #region property

        public IconPathModel Icon { get; private set; }

        #endregion

        #region CommonDialog

        public override void Reset()
        {
            Icon = new IconPathModel();
        }

        protected override bool RunDialog(IntPtr hwndOwner)
        {
            var iconIndex = Icon.Index;
            var sb = new StringBuilder(Icon.Path, (int)MAX.MAX_PATH);
            var result = NativeMethods.SHChangeIconDialog(hwndOwner, sb, sb.Capacity, ref iconIndex);
            if(result) {
                Icon.Index = iconIndex;
                Icon.Path = sb.ToString();
            }

            return result;
        }

        #endregion
    }
}
