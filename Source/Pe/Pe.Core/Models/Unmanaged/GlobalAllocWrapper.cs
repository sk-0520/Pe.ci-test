using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.Unmanaged
{
    /// <summary>
    /// <see cref="Marshal.AllocHGlobal(int)"/>のラッパー。
    /// </summary>
    public class GlobalAllocWrapper : UnmanagedWrapperBase<IntPtr>
    {
        /// <summary>
        /// メモリ確保。
        /// </summary>
        /// <param name="cb">確保するサイズ。</param>
        public GlobalAllocWrapper(int cb)
            : base(Marshal.AllocHGlobal(cb))
        {
            Size = cb;
        }

        #region property

        /// <summary>
        /// 確保サイズ。
        /// </summary>
        public int Size { get; private set; }

        #endregion

        #region function

        public static GlobalAllocWrapper Create<T>()
        {
            return new GlobalAllocWrapper(Marshal.SizeOf<T>());
        }

        public static GlobalAllocWrapper Create<T>(T structure)
            where T: struct
        {
            var result = new GlobalAllocWrapper(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, result.Raw, false);

            return result;
        }

        #endregion

        #region UnmanagedModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Marshal.FreeHGlobal(Raw);
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
