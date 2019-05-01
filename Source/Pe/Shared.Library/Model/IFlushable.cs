using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
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
}
