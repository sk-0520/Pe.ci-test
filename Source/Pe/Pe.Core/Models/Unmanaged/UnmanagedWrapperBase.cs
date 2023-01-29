using System;

namespace ContentTypeTextNet.Pe.Core.Models.Unmanaged
{
    /// <summary>
    /// 非マネージドオブジェクトをマネージドオブジェクトとして扱う。
    /// <para>CLIでこういうクラスあったよなぁ → <see cref="System.Runtime.InteropServices.SafeHandle"/>だったのでメモ。</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UnmanagedWrapperBase<T>: RawModel<T>
    {
        protected UnmanagedWrapperBase(T rawObject)
            : base(rawObject)
        { }
    }

    /// <summary>
    /// アンマネージドなOS提供ハンドルを管理。
    /// </summary>
    public abstract class UnmanagedHandleWrapper: UnmanagedWrapperBase<IntPtr>
    {
        protected UnmanagedHandleWrapper(IntPtr hHandle)
            : base(hHandle)
        {
            if(hHandle == IntPtr.Zero) {
                throw new ArgumentNullException(nameof(hHandle));
            }
        }


        #region function

        /// <summary>
        /// 解放処理。
        /// <para>ハンドルにより処理色々なんでオーバーライドしてごちゃごちゃする。</para>
        /// </summary>
        protected virtual void ReleaseHandle()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region UnmanagedBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                ReleaseHandle();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
