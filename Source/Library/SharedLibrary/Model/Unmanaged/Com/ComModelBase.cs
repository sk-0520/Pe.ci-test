/*
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
using System;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Library.SharedLibrary.Model.Unmanaged.Com
{
    /// <summary>
    /// 生のCOMを管理。
    /// </summary>
    public abstract class ComModelBase: UnmanagedModelBase
    {
        public ComModelBase(object rawCom)
            : base()
        {
            if(rawCom == null) {
                throw new ArgumentNullException(nameof(rawCom));
            }

            RawCom = rawCom;
        }

        /// <summary>
        /// COM生オブジェクト。
        /// </summary>
        public object RawCom { get; private set; }

        #region UnmanagedModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Marshal.ReleaseComObject(RawCom);
                RawCom = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
