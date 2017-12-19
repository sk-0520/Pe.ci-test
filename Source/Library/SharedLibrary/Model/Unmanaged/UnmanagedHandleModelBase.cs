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
using ContentTypeTextNet.Library.PInvoke.Windows;

namespace ContentTypeTextNet.Library.SharedLibrary.Model.Unmanaged
{
    /// <summary>
    /// アンマネージドなOS提供ハンドルを管理。
    /// </summary>
    public abstract class UnmanagedHandleModelBase: UnmanagedModelBase
    {
        public UnmanagedHandleModelBase(IntPtr hHandle)
            : base()
        {
            if(hHandle == IntPtr.Zero) {
                throw new ArgumentNullException(nameof(hHandle));
            }

            Handle = hHandle;
        }

        /// <summary>
        /// ハンドル。
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// 解放処理。
        /// <para>ハンドルにより処理色々なんでオーバーライドしてごちゃごちゃする。</para>
        /// </summary>
        protected virtual void ReleaseHandle()
        {
            throw new NotImplementedException();
        }

        #region UnmanagedBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                ReleaseHandle();
                Handle = IntPtr.Zero;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
