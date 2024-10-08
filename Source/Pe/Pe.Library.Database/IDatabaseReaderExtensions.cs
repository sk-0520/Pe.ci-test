using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

// DBMS の依存は極力考えない処理。

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// 読み込み処理の安全のしおり。
    /// </summary>
    /// <remarks>
    /// <para>問い合わせ文として非ユーザー入力でデバッグ中に検証可能なものを想定している。</para>
    /// <para>そのため問い合わせ文の確認自体もデバッグ時のみ有効となる。</para>
    /// <para>なお確認自体はただの文字列比較であるため該当ワードをコメントアウトしても通過する点に注意。</para>
    /// </remarks>
    public static class IDatabaseReaderExtensions
    {
        #region function

        [Conditional("DEBUG")]
        private static void ThrowIfNotOrderBy(string statement)
        {
            if(!Regex.IsMatch(statement, @"\border\s+by\b", RegexOptions.IgnoreCase | RegexOptions.Multiline, Timeout.InfiniteTimeSpan)) {
                throw new DatabaseStatementException("order by");
            }
        }

        /// <summary>
        /// 検索処理時に順序指定を強制する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns></returns>
        public static IEnumerable<T> SelectOrdered<T>(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfNotOrderBy(statement);

            return reader.Query<T>(statement, parameter, buffered);
        }

        /// <inheritdoc cref="SelectOrdered{T}(IDatabaseReader, string, object?, bool)"/>
        public static IEnumerable<dynamic> SelectOrdered(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfNotOrderBy(statement);

            return reader.Query(statement, parameter, buffered);
        }

        /// <inheritdoc cref="SelectOrdered{T}(IDatabaseReader, string, object?, bool)"/>
        public static Task<IEnumerable<T>> SelectOrderedAsync<T>(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            ThrowIfNotOrderBy(statement);

            return reader.QueryAsync<T>(statement, parameter, buffered, cancellationToken);
        }

        /// <inheritdoc cref="SelectOrdered{T}(IDatabaseReader, string, object?, bool)"/>
        public static Task<IEnumerable<dynamic>> SelectOrderedAsync(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            ThrowIfNotOrderBy(statement);

            return reader.QueryAsync(statement, parameter, buffered, cancellationToken);
        }


        [Conditional("DEBUG")]
        private static void ThrowIfNotSingleCount(string statement)
        {
            if(!Regex.IsMatch(statement, @"\bselect\s+count\s*\(", RegexOptions.IgnoreCase | RegexOptions.Multiline, Timeout.InfiniteTimeSpan)) {
                throw new DatabaseStatementException("select count()");
            }
        }

        /// <summary>
        /// 単一の件数取得を強制。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns></returns>
        public static long SelectSingleCount(this IDatabaseReader reader, string statement, object? parameter = null)
        {
            ThrowIfNotSingleCount(statement);

            return reader.QueryFirst<long>(statement, parameter);
        }

        /// <inheritdoc cref="SelectSingleCount(IDatabaseReader, string, object?)"/>
        public static Task<long> SelectSingleCountAsync(this IDatabaseReader reader, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotSingleCount(statement);

            return reader.QueryFirstAsync<long>(statement, parameter, cancellationToken);
        }

        #endregion
    }
}
