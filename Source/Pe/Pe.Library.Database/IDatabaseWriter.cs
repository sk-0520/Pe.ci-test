using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// データ書き込みを担当。
    /// </summary>
    /// <remarks>
    /// <para>それが実際に書き込んでいるのかはDBMS依存。</para>
    /// </remarks>
    public interface IDatabaseWriter
    {
        #region function

        /// <summary>
        /// insert, update, delete, select(sequence) 的なデータ変動するやつを実行。
        /// </summary>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns>影響行数。自動採番値の取得はDBMS依存となる。</returns>
        int Execute(string statement, object? parameter = null);

        /// <summary>
        /// 非同期で insert, update, delete, select(sequence) 的なデータ変動するやつを実行。
        /// </summary>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <param name="cancellationToken"></param>
        /// <returns>影響行数。自動採番値の取得はDBMS依存となる。</returns>
        Task<int> ExecuteAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default);

        #endregion
    }

}
