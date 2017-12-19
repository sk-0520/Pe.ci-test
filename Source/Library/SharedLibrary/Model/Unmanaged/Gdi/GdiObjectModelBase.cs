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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Library.SharedLibrary.Model.Unmanaged.Gdi
{
    /// <summary>
    /// アンマネージドなGDIオブジェクトを管理。
    /// </summary>
    public abstract class GdiObjectModelBase: UnmanagedHandleModelBase, IMakeBitmapSource
    {
        public GdiObjectModelBase(IntPtr hHandle)
            : base(hHandle)
        { }

        #region property

        public virtual bool CanMakeImageSource { get { return false; } }

        #endregion

        #region UnmanagedHandleModelBase

        protected override void ReleaseHandle()
        {
            NativeMethods.DeleteObject(Handle);
        }

        #endregion

        #region IMakeBitmapSource

        /// <summary>
        /// GDIオブジェクトから<see cref="BitmapSource"/>作成。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">CanMakeImageSource</exception>
        public BitmapSource MakeBitmapSource()
        {
            CheckUtility.Enforce<NotImplementedException>(CanMakeImageSource);
            return MakeBitmapSourceImpl();
        }

        #endregion

        #region function

        protected virtual BitmapSource MakeBitmapSourceImpl()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
