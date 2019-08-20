using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Model.Unmanaged
{
    /// <summary>
    /// <see cref="Marshal.AllocHGlobal(int)"/>のラッパー。
    /// </summary>
    public class GlobalAllocModel : UnmanagedModelBase<IntPtr>
    {
        /// <summary>
        /// メモリ確保。
        /// </summary>
        /// <param name="cb">確保するサイズ。</param>
        public GlobalAllocModel(int cb)
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

        public static GlobalAllocModel Create<T>()
        {
            return new GlobalAllocModel(Marshal.SizeOf<T>());
        }

        public static GlobalAllocModel Create<T>(T structure)
        {
            var result = new GlobalAllocModel(Marshal.SizeOf(structure));
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
