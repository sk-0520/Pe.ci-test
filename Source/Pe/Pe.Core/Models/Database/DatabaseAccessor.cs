using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Dapper;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// DBアクセス処理。
    /// </summary>
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
        /// <para><see cref="IDisposable.Dispose()"/>が完了するまでの間接続できない状態になる。</para>
        /// </summary>
        /// <returns>切断状態終了のトリガー。 GC 任せにせず明示的に <see cref="IDisposable.Dispose()"/> すること。</returns>
        IDisposable PauseConnection();

        /// <inheritdoc cref="IDatabaseReader.Query{T}(string, object?, bool)"/>
        IEnumerable<T> Query<T>(string statement, object? parameter, IDatabaseTransaction? transaction, bool buffered);

        /// <inheritdoc cref="IDatabaseReader.QueryAsync{T}(string, object?, bool, CancellationToken)"/>
        Task<IEnumerable<T>> QueryAsync<T>(IDatabaseTransaction? transaction, string statement, object? parameter, bool buffered, CancellationToken cancellationToken);

        /// <inheritdoc cref="IDatabaseReader.Query(string, object?, bool)"/>
        IEnumerable<dynamic> Query(string statement, object? parameter, IDatabaseTransaction? transaction, bool buffered);
        /// <inheritdoc cref="IDatabaseReader.QueryFirst{T}(string, object?)"/>
        T QueryFirst<T>(string statement, object? parameter, IDatabaseTransaction? transaction);
        /// <inheritdoc cref="IDatabaseReader.QueryFirstOrDefault{T}(string, object?)"/>
        [return: MaybeNull]
        T QueryFirstOrDefault<T>(string statement, object? parameter, IDatabaseTransaction? transaction);
        /// <inheritdoc cref="IDatabaseReader.QuerySingle{T}(string, object?)"/>
        T QuerySingle<T>(string statement, object? parameter, IDatabaseTransaction? transaction);
        /// <inheritdoc cref="IDatabaseReader.QuerySingleOrDefault{T}(string, object?)"/>
        [return: MaybeNull]
        T QuerySingleOrDefault<T>(string statement, object? parameter, IDatabaseTransaction? transaction);
        /// <inheritdoc cref="IDatabaseReader.GetDataTable(string, object?)"/>
        DataTable GetDataTable(string statement, object? parameter, IDatabaseTransaction? transaction);

        /// <inheritdoc cref="IDatabaseWriter.Execute(string, object?)"/>
        int Execute(string statement, object? parameter, IDatabaseTransaction? transaction);

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
        /// <para>意味わからん名前だけどいるの！</para>
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        IDatabaseTransaction BeginReadOnlyTransaction(IsolationLevel isolationLevel);

        /// <inheritdoc cref="Batch(Func{IDatabaseContext, bool}, IsolationLevel)"/>
        [Obsolete]
        IResultFailureValue<Exception> Batch(Func<IDatabaseContext, bool> executor);
        /// <summary>
        /// バッチ処理の実行。
        /// <para>処理成功時に自動的にコミットされる。</para>
        /// </summary>
        /// <param name="executor">処理内容。</param>
        /// <param name="isolationLevel"></param>
        /// <returns>処理実行結果。</returns>
        /// これもうなくしたいなぁ。
        [Obsolete]
        IResultFailureValue<Exception> Batch(Func<IDatabaseContext, bool> executor, IsolationLevel isolationLevel);

        #endregion
    }

    /// <summary>
    /// DBアクセスに対してラップする。
    /// <para>DBまで行く前にプログラム側で制御する目的。</para>
    /// </summary>
    public class DatabaseAccessor: DisposerBase, IDatabaseAccessor
    {
        public DatabaseAccessor(IDatabaseFactory databaseFactory, ILogger logger)
        {
            Logger = logger;
            DatabaseFactory = databaseFactory;
            LazyConnection = new Lazy<IDbConnection>(OpenConnection);
            LazyImplementation = new Lazy<IDatabaseImplementation>(DatabaseFactory.CreateImplementation);
        }

        public DatabaseAccessor(IDatabaseFactory databaseFactory, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            DatabaseFactory = databaseFactory;
            LazyConnection = new Lazy<IDbConnection>(OpenConnection);
            LazyImplementation = new Lazy<IDatabaseImplementation>(DatabaseFactory.CreateImplementation);
        }

        #region property

        private Lazy<IDbConnection> LazyConnection { get; set; }

        private Lazy<IDatabaseImplementation> LazyImplementation { get; }
        protected IDatabaseImplementation Implementation => LazyImplementation.Value;

        protected ILogger Logger { get; }

        /// <summary>
        /// データベース接続が開いているか。
        /// </summary>
        public bool IsOpend { get; private set; }

        /// <summary>
        /// データベース接続が一時的に閉じているか。
        /// </summary>
        public bool ConnectionPausing { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// DB接続を開く。
        /// </summary>
        /// <returns></returns>
        private IDbConnection OpenConnection()
        {
            if(ConnectionPausing) {
                throw new InvalidOperationException(nameof(ConnectionPausing));
            }
            if(IsOpend) {
                throw new InvalidOperationException(nameof(IsOpend));
            }
            ThrowIfDisposed();

            var con = DatabaseFactory.CreateConnection();
            con.Open();
            IsOpend = true;
            return con;
        }

        [Obsolete]
        protected virtual IResultFailureValue<Exception> BatchImpl(Func<IDatabaseTransaction> transactionCreator, Func<IDatabaseContext, bool> function)
        {
            ThrowIfDisposed();

            var transaction = transactionCreator();
            try {
                var commit = function(transaction);
                if(commit) {
                    transaction.Commit();
                } else {
                    transaction.Rollback();
                }
                return ResultFailureValue.Success<Exception>();
            } catch(Exception ex) {
                transaction.Rollback();
                return ResultFailureValue.Failure(ex);
            }
        }

        /// <summary>
        /// 問い合わせ文をログ出力。
        /// <para>あくまで実行するための文をログに出すだけで実際に実行される文ではない。</para>
        /// </summary>
        /// <param name="statement">問い合わせ文。</param>
        /// <param name="parameter">パラメータ。</param>
        protected virtual void LoggingStatement(string statement, object? parameter)
        {
            if(Logger.IsEnabled(LogLevel.Trace)) {
                Logger.LogTrace("{0}{1}{2}", statement, Environment.NewLine, ObjectDumper.GetDumpString(parameter));
            }
        }

        /// <summary>
        /// 単体結果の問い合わせ結果のログ出力。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        protected virtual void LoggingQueryResult<T>([MaybeNull] T result, [DateTimeKind(DateTimeKind.Utc)] DateTime startTime, [DateTimeKind(DateTimeKind.Utc)] DateTime endTime)
        {
            if(Logger.IsEnabled(LogLevel.Trace)) {
                Logger.LogTrace("{0} -> {1}, {2}", typeof(T), result, endTime - startTime);
            }
        }

        /// <summary>
        /// 複数結果の問い合わせ結果のログ出力。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="bufferd">偽の場合、<paramref name="result"/>に全数は存在しない。</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        protected virtual void LoggingQueryResults<T>(IEnumerable<T> result, bool bufferd, [DateTimeKind(DateTimeKind.Utc)] DateTime startTime, [DateTimeKind(DateTimeKind.Utc)] DateTime endTime)
        {
            if(Logger.IsEnabled(LogLevel.Trace)) {
                if(bufferd) {
                    Logger.LogTrace("{0}<{1}> -> {2}, {3}", nameof(IEnumerable), typeof(T), result.Count(), endTime - startTime);
                } else {
                    Logger.LogTrace("{0}<{1}> -> no buffered, {2}", nameof(IEnumerable), typeof(T), endTime - startTime);
                }
            }
        }

        /// <summary>
        /// 実行結果のログ出力。
        /// <para><see cref="IDatabaseWriter.Execute(string, object?)"/>で使用される。</para>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        [SuppressMessage("Performance", "HAA0101:Array allocation for params parameter")]
        [SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        protected virtual void LoggingExecuteResult(int result, [DateTimeKind(DateTimeKind.Utc)] DateTime startTime, [DateTimeKind(DateTimeKind.Utc)] DateTime endTime)
        {
            if(Logger.IsEnabled(LogLevel.Trace)) {
                Logger.LogTrace("result: {0}, {1}", result, endTime - startTime);
            }
        }

        /// <summary>
        /// 問い合わせ結果のログ出力。
        /// <para><see cref="IDatabaseReader.GetDataTable(string, object?)"/>で使用される。</para>
        /// </summary>
        /// <param name="table"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        [SuppressMessage("Performance", "HAA0101:Array allocation for params parameter")]
        [SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        [SuppressMessage("Performance", "HAA0503:Explicit new anonymous object allocation")]
        protected virtual void LoggingDataTable(DataTable table, [DateTimeKind(DateTimeKind.Utc)] DateTime startTime, [DateTimeKind(DateTimeKind.Utc)] DateTime endTime)
        {
            if(Logger.IsEnabled(LogLevel.Trace)) {
                Logger.LogTrace("table: {0} -> {1} * {2} = {3}, {4}", table.TableName, table.Columns.Count, table.Rows.Count, table.Columns.Count * table.Rows.Count, endTime - startTime);
            }
        }

        #endregion

        #region IDatabaseAccessor

        /// <inheritdoc cref="IDatabaseAccessor.DatabaseFactory"/>
        public IDatabaseFactory DatabaseFactory { get; }

        /// <inheritdoc cref="IDatabaseAccessor.BaseConnection"/>
        public virtual IDbConnection BaseConnection => LazyConnection.Value;

        /// <inheritdoc cref="IDatabaseAccessor.PauseConnection"/>
        public virtual IDisposable PauseConnection()
        {
            ThrowIfDisposed();

            if(!IsOpend) {
                return ActionDisposerHelper.CreateEmpty();
            }

            if(!ConnectionPausing) {
                BaseConnection.Close();
                IsOpend = false;
                ConnectionPausing = true;
                return new ActionDisposer(d => {
                    ConnectionPausing = false;
                    LazyConnection = new Lazy<IDbConnection>(OpenConnection);
                });
            }

            return ActionDisposerHelper.CreateEmpty();
        }

        /// <inheritdoc cref="IDatabaseAccessor.Query{T}(string, object?, IDatabaseTransaction?, bool)"/>
        public virtual IEnumerable<T> Query<T>(string statement, object? parameter, IDatabaseTransaction? transaction, bool buffered)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var result = BaseConnection.Query<T>(formattedStatement, parameter, transaction?.Transaction, buffered);
            LoggingQueryResults(result, buffered, startTime, DateTime.UtcNow);

            return result;
        }

        /// <inheritdoc cref="IDatabaseReader.Query{T}(string, object?, bool)"/>
        public virtual IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return Query<T>(statement, parameter, null, buffered);
        }

        /// <inheritdoc cref="IDatabaseAccessor.QueryAsync{T}(IDatabaseTransaction?, string, object?, CancellationToken)"/>
        public virtual Task<IEnumerable<T>> QueryAsync<T>(IDatabaseTransaction? transaction, string statement, object? parameter, bool buffered, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: transaction?.Transaction,
                flags: buffered ? CommandFlags.Buffered: CommandFlags.NoCache,
                cancellationToken: cancellationToken
            );
            return BaseConnection.QueryAsync<T>(command).ContinueWith(t => {
                LoggingQueryResults(t.Result, buffered, startTime, DateTime.UtcNow);
                return t.Result;
            }, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseReader.QueryAsync{T}(string, object?, bool, CancellationToken)"/>
        public Task<IEnumerable<T>> QueryAsync<T>(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return QueryAsync<T>(null, statement, parameter, buffered, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseAccessor.Query(IDatabaseTransaction?, string, object?, CancellationToken)"/>
        public virtual IEnumerable<dynamic> Query(string statement, object? parameter, IDatabaseTransaction? transaction, bool buffered)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var result = BaseConnection.Query(formattedStatement, parameter, transaction?.Transaction, buffered);
            LoggingQueryResults(result, buffered, startTime, DateTime.UtcNow);

            return result;
        }

        /// <inheritdoc cref="IDatabaseAccessor.QueryAsync(string, object?, bool, CancellationToken)"/>
        public virtual Task<IEnumerable<dynamic>> QueryAsync(IDatabaseTransaction? transaction, string statement, object? parameter, bool buffered, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: transaction?.Transaction,
                flags: buffered ? CommandFlags.Buffered : CommandFlags.NoCache,
                cancellationToken: cancellationToken
            );
            return BaseConnection.QueryAsync(command).ContinueWith(t => {
                LoggingQueryResults(t.Result, buffered, startTime, DateTime.UtcNow);
                return t.Result;
            }, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseReader.QueryAsync(string, object?, bool, CancellationToken)"/>
        public virtual Task<IEnumerable<dynamic>> QueryAsync(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return QueryAsync(null, statement, parameter, buffered, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseReader.Query(string, object?, bool)"/>
        public virtual IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return Query(statement, parameter, null, buffered);
        }

        public virtual T QueryFirst<T>(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var result = BaseConnection.QueryFirst<T>(formattedStatement, parameter, transaction?.Transaction);
            LoggingQueryResult(result, startTime, DateTime.UtcNow);

            return result;
        }

        public virtual T QueryFirst<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return QueryFirst<T>(statement, parameter, null);
        }

        [return: MaybeNull]
        public virtual T QueryFirstOrDefault<T>(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var result = BaseConnection.QueryFirstOrDefault<T>(formattedStatement, parameter, transaction?.Transaction);
            LoggingQueryResult(result, startTime, DateTime.UtcNow);

            return result;
        }

        [return: MaybeNull]
        public T QueryFirstOrDefault<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return QueryFirstOrDefault<T>(statement, parameter, null);
        }

        public virtual T QuerySingle<T>(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var result = BaseConnection.QuerySingle<T>(formattedStatement, parameter, transaction?.Transaction);
            LoggingQueryResult(result, startTime, DateTime.UtcNow);

            return result;
        }

        public virtual T QuerySingle<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return QuerySingle<T>(statement, parameter, null);
        }

        [return: MaybeNull]
        public virtual T QuerySingleOrDefault<T>(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var result = BaseConnection.QuerySingleOrDefault<T>(formattedStatement, parameter, transaction?.Transaction);
            LoggingQueryResult(result, startTime, DateTime.UtcNow);

            return result;
        }
        [return: MaybeNull]
        public virtual T QuerySingleOrDefault<T>(string statement, object? parameter)
        {
            ThrowIfDisposed();

            return QuerySingleOrDefault<T>(statement, parameter, null);
        }

        public virtual int Execute(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var result = BaseConnection.Execute(formattedStatement, parameter, transaction?.Transaction);
            LoggingExecuteResult(result, startTime, DateTime.UtcNow);

            return result;
        }

        public int Execute(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Execute(statement, parameter, null);
        }

        public virtual DataTable GetDataTable(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);

            LoggingStatement(formattedStatement, parameter);

            var dataTable = new DataTable();
            var startTime = DateTime.UtcNow;
            dataTable.Load(BaseConnection.ExecuteReader(statement, parameter, transaction?.Transaction));
            LoggingDataTable(dataTable, startTime, DateTime.UtcNow);

            return dataTable;
        }

        public DataTable GetDataTable(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return GetDataTable(statement, parameter, null);
        }

        /// <summary>
        /// トランザクション開始。
        /// </summary>
        /// <returns></returns>
        public virtual IDatabaseTransaction BeginTransaction()
        {
            ThrowIfDisposed();

            return new DatabaseTransaction(this);
        }

        /// <summary>
        /// トランザクション開始。
        /// </summary>
        /// <param name="isolationLevel">トランザクションの分離レベル。</param>
        /// <returns></returns>
        public virtual IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            return new DatabaseTransaction(this, isolationLevel);
        }

        public virtual IDatabaseTransaction BeginReadOnlyTransaction()
        {
            ThrowIfDisposed();

            return new ReadOnlyDatabaseTransaction(this);
        }
        public virtual IDatabaseTransaction BeginReadOnlyTransaction(IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            return new ReadOnlyDatabaseTransaction(this, isolationLevel);
        }


        [Obsolete]
        public IResultFailureValue<Exception> Batch(Func<IDatabaseContext, bool> executor)
        {
            ThrowIfDisposed();

            return BatchImpl(() => new DatabaseTransaction(this), executor);
        }

        [Obsolete]
        public IResultFailureValue<Exception> Batch(Func<IDatabaseContext, bool> executor, IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            return BatchImpl(() => new DatabaseTransaction(this, isolationLevel), executor);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(LazyConnection.IsValueCreated) {
                        BaseConnection.Dispose();
                    }
                }
                IsOpend = false;
                ConnectionPausing = false;
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// <see cref="DatabaseAccessor"/>の型付き<see cref="DatabaseAccessor.BaseConnection"/>実装。
    /// </summary>
    /// <typeparam name="TDbConnection">具象<see cref="DatabaseAccessor.BaseConnection"/>。</typeparam>
    public class DatabaseAccessor<TDbConnection>: DatabaseAccessor
        where TDbConnection : IDbConnection
    {
        public DatabaseAccessor(IDatabaseFactory connectionFactory, ILogger logger)
            : base(connectionFactory, logger)
        { }

        public DatabaseAccessor(IDatabaseFactory connectionFactory, ILoggerFactory loggerFactory)
            : base(connectionFactory, loggerFactory)
        { }

        #region property

        /// <summary>
        /// 接続元。
        /// </summary>
        public TDbConnection Connection => (TDbConnection)BaseConnection;

        #endregion
    }
}
