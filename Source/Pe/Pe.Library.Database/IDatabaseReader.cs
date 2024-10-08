using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// データ読み込みを担当。
    /// </summary>
    /// <remarks>
    /// <para>データが変更されるかはDBMS依存となる。シーケンスやファンクション呼び出し・トリガーなどの実装は<see cref="IDatabaseReader"/>からは判定不可。</para>
    /// </remarks>
    public interface IDatabaseReader
    {
        #region function

        /// <summary>
        /// <inheritdoc cref="IDbCommand.ExecuteReader"/>
        /// </summary>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns></returns>
        IDataReader GetDataReader(string statement, object? parameter = null);

        /// <summary>
        /// 非同期で<inheritdoc cref="IDbCommand.ExecuteReader"/>
        /// </summary>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns></returns>
        Task<IDataReader> GetDataReaderAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// <see cref="DataTable"/> でデータ取得。
        /// </summary>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns><see cref="DataTable"/></returns>
        DataTable GetDataTable(string statement, object? parameter = null);

        /// <inheritdoc cref="GetDataTable(string, object?)"/>
        Task<DataTable> GetDataTableAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IDbCommand.ExecuteScalar"/>
        TResult? GetScalar<TResult>(string statement, object? parameter = null);

        /// <inheritdoc cref="GetScalar{TResult}(string, object?)"/>
        Task<TResult?> GetScalarAsync<TResult>(string statement, object? parameter = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 指定の型で問い合わせ。
        /// </summary>
        /// <typeparam name="T">問い合わせ型。</typeparam>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <param name="buffered"><see cref="Dapper.SqlMapper.Query"/>の<c>buffered</c></param>
        /// <returns>問い合わせ結果。</returns>
        IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true);

        /// <summary>
        /// 非同期で指定の型で問い合わせ。
        /// </summary>
        /// <typeparam name="T">問い合わせ型。</typeparam>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <param name="buffered"><see cref="Dapper.SqlMapper.Query"/>の<c>buffered</c></param>
        /// <param name="cancellationToken"></param>
        /// <returns>問い合わせ結果。</returns>
        Task<IEnumerable<T>> QueryAsync<T>(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// 動的型で問い合わせ。
        /// </summary>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <param name="buffered"><see cref="Dapper.SqlMapper.Query"/>の<c>buffered</c></param>
        /// <returns>問い合わせ結果。</returns>
        IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true);

        /// <summary>
        /// 非同期で動的型で問い合わせ。
        /// </summary>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <param name="buffered"><see cref="Dapper.SqlMapper.Query"/>の<c>buffered</c></param>
        /// <param name="cancellationToken"></param>
        /// <returns>問い合わせ結果。</returns>
        Task<IEnumerable<dynamic>> QueryAsync(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// 最初のデータを取得。
        /// </summary>
        /// <typeparam name="T">問い合わせ型。</typeparam>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="InvalidOperationException">空っぽ。</exception>
        /// <returns>一番最初に見つかったデータ。</returns>
        T QueryFirst<T>(string statement, object? parameter = null);

        /// <summary>
        /// 非同期で最初のデータを取得。
        /// </summary>
        /// <typeparam name="T">問い合わせ型。</typeparam>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="InvalidOperationException">空っぽ。</exception>
        /// <returns>一番最初に見つかったデータ。</returns>
        Task<T> QueryFirstAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 最初のデータを取得。
        /// </summary>
        /// <typeparam name="T">問い合わせ型。</typeparam>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns>一番最初に見つかったデータ。見つかんなかったら <c>default(T)</c></returns>
        [return: MaybeNull]
        T QueryFirstOrDefault<T>(string statement, object? parameter = null);

        /// <summary>
        /// 非同期で最初のデータを取得。
        /// </summary>
        /// <typeparam name="T">問い合わせ型。</typeparam>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns>一番最初に見つかったデータ。見つかんなかったら <c>default(T)</c></returns>
        Task<T?> QueryFirstOrDefaultAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 単一データ取得。
        /// </summary>
        /// <typeparam name="T">問い合わせ型。</typeparam>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="InvalidOperationException">空っぽか複数あり。</exception>
        /// <returns>一意なデータ。</returns>
        T QuerySingle<T>(string statement, object? parameter = null);

        /// <summary>
        /// 非同期で単一データ取得。
        /// </summary>
        /// <typeparam name="T">問い合わせ型。</typeparam>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="InvalidOperationException">空っぽか複数あり。</exception>
        /// <returns>一意なデータ。</returns>
        Task<T> QuerySingleAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 単一データ取得。
        /// </summary>
        /// <typeparam name="T">問い合わせ型。</typeparam>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="InvalidOperationException">複数あり。</exception>
        /// <returns>一意なデータ。</returns>
        [return: MaybeNull]
        T QuerySingleOrDefault<T>(string statement, object? parameter = null);

        /// <summary>
        /// 非同期で単一データ取得。
        /// </summary>
        /// <typeparam name="T">問い合わせ型。</typeparam>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <param name="cancellationToken"></param>
        /// <returns>一意なデータ。一意じゃなかったら <c>default(T)</c></returns>
        Task<T?> QuerySingleOrDefaultAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default);

        #endregion
    }
}
