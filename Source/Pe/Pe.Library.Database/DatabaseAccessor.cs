using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base;
using Dapper;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// DBアクセスに対してラップする。
    /// </summary>
    /// <remarks>
    /// <para>DBまで行く前にプログラム側で制御する目的。</para>
    /// </remarks>
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
        public bool IsOpened { get; private set; }

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
            if(IsOpened) {
                throw new InvalidOperationException(nameof(IsOpened));
            }
            ThrowIfDisposed();

            var con = DatabaseFactory.CreateConnection();
            con.Open();
            IsOpened = true;
            return con;
        }


        /// <summary>
        /// 問い合わせ文をログ出力。
        /// </summary>
        /// <remarks>
        /// <para>あくまで実行するための文をログに出すだけで実際に実行される文ではない。</para>
        /// </remarks>
        /// <param name="statement">問い合わせ文。</param>
        /// <param name="parameter">パラメータ。</param>
        protected virtual void LoggingStatement(string statement, object? parameter)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result"></param>
        /// <param name="startUtcTime"></param>
        /// <param name="endUtcTime"></param>
        protected virtual void LoggingExecuteScalarResult<TResult>(TResult result, DateTime startUtcTime, DateTime endUtcTime)
        { }

        /// <summary>
        /// 単体結果の問い合わせ結果のログ出力。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="startUtcTime"></param>
        /// <param name="endUtcTime"></param>
        protected virtual void LoggingQueryResult<T>([MaybeNull] T result, DateTime startUtcTime, DateTime endUtcTime)
        { }

        /// <summary>
        /// 複数結果の問い合わせ結果のログ出力。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="buffered">偽の場合、<paramref name="result"/>に全数は存在しない。</param>
        /// <param name="startUtcTime"></param>
        /// <param name="endUtcTime"></param>
        protected virtual void LoggingQueryResults<T>(IEnumerable<T> result, bool buffered, DateTime startUtcTime, DateTime endUtcTime)
        { }

        /// <summary>
        /// 実行結果のログ出力。
        /// </summary>
        /// <remarks>
        /// <para><see cref="IDatabaseWriter.Execute(string, object?)"/>で使用される。</para>
        /// </remarks>
        /// <param name="result"></param>
        /// <param name="startUtcTime"></param>
        /// <param name="endUtcTime"></param>
        protected virtual void LoggingExecuteResult(int result, DateTime startUtcTime, DateTime endUtcTime)
        { }

        /// <summary>
        /// 問い合わせ結果のログ出力。
        /// </summary>
        /// <remarks>
        /// <para><see cref="IDatabaseReader.GetDataTable(string, object?)"/>で使用される。</para>
        /// </remarks>
        /// <param name="table"></param>
        /// <param name="startUtcTime"></param>
        /// <param name="endUtcTime"></param>
        protected virtual void LoggingDataTable(DataTable table, DateTime startUtcTime, DateTime endUtcTime)
        { }

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

            if(!IsOpened || ConnectionPausing) {
                return ActionDisposerHelper.CreateEmpty();
            }

            BaseConnection.Close();
            IsOpened = false;
            ConnectionPausing = true;
            return new ActionDisposer(d => {
                ConnectionPausing = false;
                LazyConnection = new Lazy<IDbConnection>(OpenConnection);
            });
        }

        public IDataReader GetDataReader(IDatabaseTransaction? transaction, string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var result = BaseConnection.ExecuteReader(formattedStatement, parameter, transaction?.Transaction);
            return result;
        }

        public IDataReader GetDataReader(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return GetDataReader(null, statement, parameter);
        }

        public Task<IDataReader> GetDataReaderAsync(IDatabaseTransaction? transaction, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: transaction?.Transaction,
                cancellationToken: cancellationToken
            );

            var result = BaseConnection.ExecuteReaderAsync(command);
            return result;
        }

        public Task<IDataReader> GetDataReaderAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            return GetDataReaderAsync(null, statement, parameter, cancellationToken);
        }

        public virtual DataTable GetDataTable(IDatabaseTransaction? transaction, string statement, object? parameter)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);

            LoggingStatement(formattedStatement, parameter);

            var dataTable = new DataTable();
            var startTime = DateTime.UtcNow;
            using(var reader = GetDataReader(transaction, statement, parameter)) {
                dataTable.Load(reader);
            }
            LoggingDataTable(dataTable, startTime, DateTime.UtcNow);

            return dataTable;
        }

        public virtual DataTable GetDataTable(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return GetDataTable(null, statement, parameter);
        }

        public async virtual Task<DataTable> GetDataTableAsync(IDatabaseTransaction? transaction, string statement, object? parameter, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);

            LoggingStatement(formattedStatement, parameter);

            var dataTable = new DataTable();
            var startTime = DateTime.UtcNow;
            using(var reader = await GetDataReaderAsync(transaction, statement, parameter, cancellationToken)) {
                dataTable.Load(reader);
            }
            LoggingDataTable(dataTable, startTime, DateTime.UtcNow);

            return dataTable;
        }

        public virtual Task<DataTable> GetDataTableAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return GetDataTableAsync(null, statement, parameter, cancellationToken);
        }

        public virtual TResult? GetScalar<TResult>(IDatabaseTransaction? transaction, string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var result = BaseConnection.ExecuteScalar<TResult>(formattedStatement, parameter, transaction?.Transaction);
            LoggingExecuteScalarResult(result, startTime, DateTime.UtcNow);

            return result;
        }

        public virtual TResult? GetScalar<TResult>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return GetScalar<TResult>(null, statement, parameter);
        }

        public virtual async Task<TResult?> GetScalarAsync<TResult>(IDatabaseTransaction? transaction, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: transaction?.Transaction,
                cancellationToken: cancellationToken
            );
            var result = await BaseConnection.ExecuteScalarAsync<TResult>(command);
            LoggingExecuteScalarResult(result, startTime, DateTime.UtcNow);

            return result;
        }

        public virtual Task<TResult?> GetScalarAsync<TResult>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return GetScalarAsync<TResult?>(null, statement, parameter, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseAccessor.Query{T}(IDatabaseTransaction?, string, object?, bool)"/>
        public virtual IEnumerable<T> Query<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null, bool buffered = true)
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

            return Query<T>(null, statement, parameter, buffered);
        }

        /// <inheritdoc cref="IDatabaseAccessor.QueryAsync{T}(IDatabaseTransaction?, string, object?, bool, CancellationToken)"/>
        public virtual async Task<IEnumerable<T>> QueryAsync<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
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

            var result = await BaseConnection.QueryAsync<T>(command);
            LoggingQueryResults(result, buffered, startTime, DateTime.UtcNow);
            return result;
        }

        /// <inheritdoc cref="IDatabaseReader.QueryAsync{T}(string, object?, bool, CancellationToken)"/>
        public Task<IEnumerable<T>> QueryAsync<T>(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return QueryAsync<T>(null, statement, parameter, buffered, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseReader.Query(string, object?, bool)"/>
        public virtual IEnumerable<dynamic> Query(IDatabaseTransaction? transaction, string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var result = BaseConnection.Query(formattedStatement, parameter, transaction?.Transaction, buffered);
            LoggingQueryResults(result, buffered, startTime, DateTime.UtcNow);

            return result;
        }

        /// <inheritdoc cref="IDatabaseReader.Query(string, object?, bool)"/>
        public virtual IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return Query(null, statement, parameter, buffered);
        }

        /// <inheritdoc cref="IDatabaseAccessor.QueryAsync{T}(IDatabaseTransaction?, string, object?, bool, CancellationToken)"/>
        public virtual async Task<IEnumerable<dynamic>> QueryAsync(IDatabaseTransaction? transaction, string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
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

            var result = await BaseConnection.QueryAsync(command);
            LoggingQueryResults(result, buffered, startTime, DateTime.UtcNow);
            return result;
        }

        /// <inheritdoc cref="IDatabaseReader.QueryAsync(string, object?, bool, CancellationToken)"/>
        public virtual Task<IEnumerable<dynamic>> QueryAsync(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return QueryAsync(null, statement, parameter, buffered, cancellationToken);
        }

        /// <inheritdoc cref="IDatabaseAccessor.QueryFirst{T}(IDatabaseTransaction?, string, object?)"/>
        public virtual T QueryFirst<T>(IDatabaseTransaction? transaction, string statement, object? parameter=null)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var result = BaseConnection.QueryFirst<T>(formattedStatement, parameter, transaction?.Transaction);
            LoggingQueryResult(result, startTime, DateTime.UtcNow);

            return result;
        }

        /// <inheritdoc cref="IDatabaseReader.QueryFirst{T}(string, object?)"/>
        public virtual T QueryFirst<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return QueryFirst<T>(null, statement, parameter);
        }

        public virtual async Task<T> QueryFirstAsync<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: transaction?.Transaction,
                cancellationToken: cancellationToken
            );

            var result = await BaseConnection.QueryFirstAsync<T>(command);
            LoggingQueryResult(result, startTime, DateTime.UtcNow);
            return result;
        }

        public virtual Task<T> QueryFirstAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return QueryFirstAsync<T>(null, statement, parameter, cancellationToken);
        }


        [return: MaybeNull]
        public virtual T QueryFirstOrDefault<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null)
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

            return QueryFirstOrDefault<T>(null, statement, parameter);
        }

        public Task<T?> QueryFirstOrDefaultAsync<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: transaction?.Transaction,
                cancellationToken: cancellationToken
            );

            return BaseConnection.QueryFirstOrDefaultAsync<T?>(command).ContinueWith(t => {
                LoggingQueryResult(t.Result, startTime, DateTime.UtcNow);
                return t.Result;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public Task<T?> QueryFirstOrDefaultAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return QueryFirstOrDefaultAsync<T>(null, statement, parameter, cancellationToken);
        }

        public virtual T QuerySingle<T>(IDatabaseTransaction? transaction, string statement, object? parameter)
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

            return QuerySingle<T>(null, statement, parameter);
        }

        public virtual async Task<T> QuerySingleAsync<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: transaction?.Transaction,
                cancellationToken: cancellationToken
            );

            var result = await BaseConnection.QuerySingleAsync<T>(command);
            LoggingQueryResult(result, startTime, DateTime.UtcNow);
            return result;
        }

        public virtual Task<T> QuerySingleAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return QuerySingleAsync<T>(null, statement, parameter, cancellationToken);
        }

        [return: MaybeNull]
        public virtual T QuerySingleOrDefault<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null)
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
        public virtual T QuerySingleOrDefault<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return QuerySingleOrDefault<T>(null, statement, parameter);
        }

        public virtual async Task<T?> QuerySingleOrDefaultAsync<T>(IDatabaseTransaction? transaction, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: transaction?.Transaction,
                cancellationToken: cancellationToken
            );
            var result = await BaseConnection.QuerySingleOrDefaultAsync<T>(command);
            LoggingQueryResult(result, startTime, DateTime.UtcNow);

            return result;
        }

        public virtual Task<T?> QuerySingleOrDefaultAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return QuerySingleOrDefaultAsync<T>(null, statement, parameter, cancellationToken);
        }

        public virtual int Execute(IDatabaseTransaction? transaction, string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var result = BaseConnection.Execute(formattedStatement, parameter, transaction?.Transaction);
            LoggingExecuteResult(result, startTime, DateTime.UtcNow);

            return result;
        }

        public virtual int Execute(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Execute(null, statement, parameter);
        }

        public virtual async Task<int> ExecuteAsync(IDatabaseTransaction? transaction, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);

            var startTime = DateTime.UtcNow;
            var command = new CommandDefinition(
                statement,
                parameters: parameter,
                transaction: transaction?.Transaction,
                cancellationToken: cancellationToken
            );
            var result = await BaseConnection.ExecuteAsync(command);
            LoggingExecuteResult(result, startTime, DateTime.UtcNow);

            return result;
        }

        public virtual Task<int> ExecuteAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return ExecuteAsync(null, statement, parameter, cancellationToken);
        }

        /// <summary>
        /// トランザクション開始。
        /// </summary>
        /// <returns></returns>
        public virtual IDatabaseTransaction BeginTransaction()
        {
            ThrowIfDisposed();

            return new DatabaseTransaction(true, this);
        }

        /// <summary>
        /// トランザクション開始。
        /// </summary>
        /// <param name="isolationLevel">トランザクションの分離レベル。</param>
        /// <returns></returns>
        public virtual IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            return new DatabaseTransaction(true, this, isolationLevel);
        }

        public virtual IDatabaseTransaction BeginReadOnlyTransaction()
        {
            ThrowIfDisposed();

            return new ReadOnlyDatabaseTransaction(true, this);
        }
        public virtual IDatabaseTransaction BeginReadOnlyTransaction(IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            return new ReadOnlyDatabaseTransaction(true, this, isolationLevel);
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
                IsOpened = false;
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
