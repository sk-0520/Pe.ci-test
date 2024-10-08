using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// DBアクセス処理。
    /// </summary>
    /// <remarks>
    /// <para>使用者側はトランザクション処理を原則使用しない。</para>
    /// </remarks>
    public interface IDatabaseAccessor: IDatabaseContext
    {
        #region property

        /// <summary>
        /// 接続元。
        /// </summary>
        IDbConnection BaseConnection { get; }
        /// <summary>
        /// 対象DBに対する生成処理機。
        /// </summary>
        IDatabaseFactory DatabaseFactory { get; }

        #endregion

        #region function

        /// <summary>
        /// 一時的に切断状態へ遷移。
        /// </summary>
        /// <remarks>
        /// <para><see cref="IDisposable.Dispose()"/>が完了するまでの間接続できない状態になる。</para>
        /// </remarks>
        /// <returns>切断状態終了のトリガー。 GC 任せにせず明示的に <see cref="IDisposable.Dispose()"/> すること。</returns>
        IDisposable PauseConnection();

        /// <inheritdoc cref="IDatabaseReader.GetDataReader(string, object?)"/>
        IDataReader GetDataReader(IDatabaseTransaction? transaction, string statement, object? parameter = null);

        /// <inheritdoc cref="IDatabaseReader.GetDataReaderAsync(string, object?, CancellationToken = default)"/>
        Task<IDataReader> GetDataReaderAsync(IDatabaseTransaction? transaction, string statement, object? parameter = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IDatabaseReader.GetDataTable(string, object?)"/>
        DataTable GetDataTable(IDatabaseTransaction? transaction, string statement, object? parameter);

        TResult? GetScalar<TResult>(IDatabaseTransaction? transaction, string statement, object? parameter = null);
        Task<TResult?> GetScalarAsync<TResult>(IDatabaseTransaction? transaction, string statement, object? parameter = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IDatabaseReader.Query{T}(string, object?, bool)"/>
        IEnumerable<T> Query<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null, bool buffered = true);

        /// <inheritdoc cref="IDatabaseReader.QueryAsync{T}(string, object?, bool, CancellationToken)"/>
        Task<IEnumerable<T>> QueryAsync<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IDatabaseReader.Query(string, object?, bool)"/>
        IEnumerable<dynamic> Query(IDatabaseTransaction? transaction, string statement, object? parameter = null, bool buffered = true);
        /// <inheritdoc cref="IDatabaseReader.Query(string, object?, bool)"/>
        Task<IEnumerable<dynamic>> QueryAsync(IDatabaseTransaction? transaction, string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default);
        /// <inheritdoc cref="IDatabaseReader.QueryFirst{T}(string, object?)"/>
        T QueryFirst<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null);

        /// <inheritdoc cref="IDatabaseReader.QueryFirstAsync{T}(string, object?, CancellationToken)"/>
        Task<T> QueryFirstAsync<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IDatabaseReader.QueryFirstOrDefault{T}(string, object?)"/>
        [return: MaybeNull]
        T QueryFirstOrDefault<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null);

        /// <inheritdoc cref="IDatabaseReader.QueryFirstOrDefaultAsync{T}(string, object?, CancellationToken)"/>
        Task<T?> QueryFirstOrDefaultAsync<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IDatabaseReader.QuerySingle{T}(string, object?)"/>
        T QuerySingle<T>(IDatabaseTransaction? transaction, string statement, object? parameter);

        /// <inheritdoc cref="IDatabaseReader.QuerySingleOrDefault{T}(string, object?)"/>
        [return: MaybeNull]
        T QuerySingleOrDefault<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null);

        /// <inheritdoc cref="IDatabaseReader.QuerySingleOrDefaultAsync{T}(string, object?, CancellationToken)"/>
        Task<T?> QuerySingleOrDefaultAsync<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IDatabaseWriter.Execute(string, object?)"/>
        int Execute(IDatabaseTransaction? transaction, string statement, object? parameter = null);

        /// <inheritdoc cref="IDatabaseWriter.ExecuteAsync(string, object?, CancellationToken)"/>
        Task<int> ExecuteAsync(IDatabaseTransaction? transaction, string statement, object? parameter = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="BeginTransaction(IsolationLevel)"/>
        IDatabaseTransaction BeginTransaction();
        /// <summary>
        /// トランザクションの開始。
        /// </summary>
        /// <returns></returns>
        IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel);

        /// <inheritdoc cref="BeginReadOnlyTransaction(IsolationLevel)"/>
        IDatabaseTransaction BeginReadOnlyTransaction();
        /// <summary>
        /// 読み込み専用でトランザクション開始。
        /// </summary>
        /// <remarks>
        /// <para>意味わからん名前だけどいるの！</para>
        /// </remarks>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        IDatabaseTransaction BeginReadOnlyTransaction(IsolationLevel isolationLevel);

        #endregion
    }
}
