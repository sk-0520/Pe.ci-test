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
using ContentTypeTextNet.Library.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
    public class ChangedDevice
    {
        #region varable

        IntPtr _hWnd;
        int _msg;
        IntPtr _wParam;
        IntPtr _lParam;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        public ChangedDevice(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            this._hWnd = hWnd;
            this._msg = msg;
            this._wParam = wParam;
            this._lParam = lParam;

            DBT = (DBT)this._wParam.ToInt32();
        }

        #region property

        /// <summary>
        /// DBT! DBT!
        /// </summary>
        public DBT DBT { get; private set; }

        #endregion
    }
}
