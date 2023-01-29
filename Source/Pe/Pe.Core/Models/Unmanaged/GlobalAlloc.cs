using System;
using System.Runtime.InteropServices;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Models.Unmanaged
{
    /// <summary>
    /// <see cref="Marshal.AllocHGlobal(int)"/>のラッパー。
    /// </summary>
    public class GlobalAlloc: SafeHandle
    {
        /// <summary>
        /// メモリ確保。
        /// </summary>
        /// <param name="cb">確保するサイズ。</param>
        public GlobalAlloc(int cb)
            : base(Marshal.AllocHGlobal(cb), true)
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

        public static GlobalAlloc Create<T>()
        {
            return new GlobalAlloc(Marshal.SizeOf<T>());
        }

        public static GlobalAlloc Create<T>(T structure)
            where T : struct
        {
            var result = new GlobalAlloc(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, result.handle, false);

            return result;
        }

        #endregion

        #region SafeHandle

        public override bool IsInvalid => this.handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            Marshal.FreeHGlobal(this.handle);
            return true;
        }

        #endregion
    }
}
