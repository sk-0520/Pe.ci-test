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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Library.SharedLibrary.Model.Unmanaged
{
    /// <summary>
    /// <see cref="Marshal.AllocHGlobal(int)"/>のラッパー。
    /// </summary>
    public class GlobalAllocModel: UnmanagedModelBase
    {
        #region static

        public static GlobalAllocModel Create<T>()
        {
            return new GlobalAllocModel(Marshal.SizeOf<T>());
        }

        public static GlobalAllocModel Create<T>(T structure)
        {
            var result = new GlobalAllocModel(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, result.Buffer, false);

            return result;
        }

        #endregion

        /// <summary>
        /// メモリ確保。
        /// </summary>
        /// <param name="cb">確保するサイズ。</param>
        public GlobalAllocModel(int cb)
        {
            Buffer = Marshal.AllocHGlobal(cb);
            Size = cb;
        }

        #region property

        /// <summary>
        /// 確保領域のポインタ。
        /// </summary>
        public IntPtr Buffer { get; private set; }
        /// <summary>
        /// 確保サイズ。
        /// </summary>
        public int Size { get; private set; }

        #endregion

        #region UnmanagedModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Marshal.FreeHGlobal(Buffer);
                Buffer = IntPtr.Zero;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
