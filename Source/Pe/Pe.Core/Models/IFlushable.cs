using System;
using ContentTypeTextNet.Pe.Standard.Models;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// 保留中の処理を実施。
    /// </summary>
    public interface IFlushable
    {
        #region function

        /// <summary>
        /// 現在保留中の処理を実施する。
        /// <para>実施後、<see cref="IDisposable.Dispose"/>と異なりオブジェクト自体は生きている。</para>
        /// <para><see cref="IDisposable.Dispose(false)"/>でも実施できるように実装するのが努力目標。</para>
        /// </summary>
        void Flush();

        #endregion
    }

    public static class IFlushableExtensions
    {
        #region function

        public static bool SafeFlush<T>(this T? flushable)
            where T : class, IDisposedChackable, IFlushable
        {
            if(flushable == null) {
                return false;
            }

            if(flushable.IsDisposed) {
                return false;
            }

            flushable.Flush();
            return true;
        }

        #endregion
    }
}
