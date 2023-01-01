using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// データ読み込みを担当。
    /// <para>データが変更されるかはDBMS依存となる。シーケンスやファンクション呼び出し・トリガーなどの実装は<see cref="IDatabaseReader"/>からは判定不可。</para>
    /// </summary>
    public interface IDatabaseReader
    {
        #region function

        /// <summary>
        /// 指定の型で問い合わせ。
        /// </summary>
        /// <typeparam name="T">問い合わせ型。</typeparam>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <param name="buffered"><see cref="Dapper.SqlMapper.Query"/>のbufferd</param>
        /// <returns>問い合わせ結果。</returns>
        IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true);

        /// <summary>
        /// 非同期で指定の型で問い合わせ。
        /// </summary>
        /// <typeparam name="T">問い合わせ型。</typeparam>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <param name="buffered"><see cref="Dapper.SqlMapper.Query"/>のbufferd</param>
        /// <param name="cancellationToken"></param>
        /// <returns>問い合わせ結果。</returns>
        Task<IEnumerable<T>> QueryAsync<T>(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// 動的型で問い合わせ。
        /// </summary>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <param name="buffered"><see cref="Dapper.SqlMapper.Query"/>のbufferd</param>
        /// <returns>問い合わせ結果。</returns>
        IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true);

        /// <summary>
        /// 非同期で動的型で問い合わせ。
        /// </summary>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <param name="buffered"><see cref="Dapper.SqlMapper.Query"/>のbufferd</param>
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

        /// <summary>
        /// <see cref="DataTable"/> でデータ取得。
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        DataTable GetDataTable(string statement, object? parameter = null);

        #endregion
    }

    /// <summary>
    /// データ書き込みを担当。
    /// <para>それが実際に書き込んでいるのかはDBMS依存。</para>
    /// </summary>
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

        #endregion
    }

    /// <summary>
    /// データベースとの会話用インターフェイス。
    /// <para><see cref="IDatabaseReader"/>, <see cref="IDatabaseWriter"/>による明確な分離状態で処理するのは現実的でないため本IFで統合して扱う。</para>
    /// </summary>
    public interface IDatabaseContext: IDatabaseReader, IDatabaseWriter
    { }
}
