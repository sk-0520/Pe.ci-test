using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Dapper;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database
{
    /// <summary>
    /// データベースとの会話用インターフェイス。
    /// </summary>
    public interface IDatabaseCommander
    {
        #region function

        /// <summary>
        /// 指定の型で問い合わせ。
        /// </summary>
        /// <typeparam name="T">問い合わせ型</typeparam>
        /// <param name="sql">データベース文。</param>
        /// <param name="param"><paramref name="sql"/>に対するパラメータ。</param>
        /// <param name="buffered"><see cref="Dapper.SqlMapper.Query"/>のbufferd</param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true);

        IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true);

        T QueryFirst<T>(string sql, object param = null);
        T QueryFirstOrDefault<T>(string sql, object param = null);
        T QuerySingle<T>(string sql, object param = null);

        int Execute(string sql, object param = null);

        DataTable GetDataTable(string sql, object param = null);

        #endregion
    }

    public interface IDatabaseAccessor : IDatabaseCommander
    {
        #region property

        IDbConnection BaseConnection { get; }
        IDatabaseFactory DatabaseFactory { get; }

        #endregion

        #region function

        IEnumerable<T> Query<T>(string sql, object param, IDatabaseTransaction transaction, bool buffered);
        IEnumerable<dynamic> Query(string sql, object param, IDatabaseTransaction transaction, bool buffered);

        T QueryFirst<T>(string sql, object param, IDatabaseTransaction transaction);
        T QueryFirstOrDefault<T>(string sql, object param, IDatabaseTransaction transaction);
        T QuerySingle<T>(string sql, object param, IDatabaseTransaction transaction);

        int Execute(string sql, object param, IDatabaseTransaction transaction);
        DataTable GetDataTable(string sql, object param, IDatabaseTransaction transaction);

        IDatabaseTransaction BeginTransaction();
        IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel);

        IResultFailureValue<Exception> Batch(Func<IDatabaseCommander, bool> action);
        IResultFailureValue<Exception> Batch(Func<IDatabaseCommander, bool> action, IsolationLevel isolationLevel);

        #endregion
    }

    /// <summary>
    /// DBアクセスに対してラップする。
    /// <para>DBまで行く前にプログラム側で制御する目的。</para>
    /// </summary>
    public class DatabaseAccessor : DisposerBase, IDatabaseAccessor
    {
        DatabaseAccessor(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;

            LazyConnection = new Lazy<IDbConnection>(() => {
                var con = DatabaseFactory.CreateConnection();
                con.Open();
                return con;
            });

            LazyImplementation = new Lazy<IDatabaseImplementation>(DatabaseFactory.CreateImplementation);
        }

        public DatabaseAccessor(IDatabaseFactory databaseFactory, ILogger logger)
            :this(databaseFactory)
        {
            Logger = logger;
        }

        public DatabaseAccessor(IDatabaseFactory databaseFactory, ILoggerFactory loggerFactory)
            : this(databaseFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        Lazy<IDbConnection> LazyConnection { get; }

        Lazy<IDatabaseImplementation> LazyImplementation { get; }
        protected IDatabaseImplementation Implementation => LazyImplementation.Value;

        protected ILogger Logger { get; }

        #endregion

        #region function

        protected virtual IResultFailureValue<Exception> BatchCore(Func<IDatabaseTransaction> transactionCreator, Func<IDatabaseCommander, bool> function)
        {
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

        #endregion

        #region IDatabaseAccessor

        public IDatabaseFactory DatabaseFactory { get; }

        public virtual IDbConnection BaseConnection => LazyConnection.Value;

        public virtual IEnumerable<T> Query<T>(string sql, object param, IDatabaseTransaction transaction, bool buffered)
        {
            var formattedSql = Implementation.PreFormatSql(sql);

            Logger.Trace(formattedSql, param);
            return BaseConnection.Query<T>(formattedSql, param, transaction?.Transaction, buffered);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true)
        {
            return Query<T>(sql, param, null, buffered);
        }

        public virtual IEnumerable<dynamic> Query(string sql, object param, IDatabaseTransaction transaction, bool buffered)
        {
            var formattedSql = Implementation.PreFormatSql(sql);

            Logger.Trace(formattedSql, param);
            return BaseConnection.Query(formattedSql, param, transaction?.Transaction, buffered);
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true)
        {
            return Query(sql, param, null, buffered);
        }

        public virtual T QueryFirst<T>(string sql, object param, IDatabaseTransaction transaction)
        {
            var formattedSql = Implementation.PreFormatSql(sql);

            Logger.Trace(formattedSql, param);
            return BaseConnection.QueryFirst<T>(formattedSql, param, transaction?.Transaction);
        }

        public virtual T QueryFirst<T>(string sql, object param = null)
        {
            return QueryFirst<T>(sql, param);
        }

        public virtual T QueryFirstOrDefault<T>(string sql, object param, IDatabaseTransaction transaction)
        {
            var formattedSql = Implementation.PreFormatSql(sql);

            Logger.Trace(formattedSql, param);
            return BaseConnection.QueryFirstOrDefault<T>(formattedSql, param, transaction?.Transaction);
        }

        public T QueryFirstOrDefault<T>(string sql, object param = null)
        {
            return QueryFirstOrDefault<T>(sql, param, null);
        }

        public virtual T QuerySingle<T>(string sql, object param, IDatabaseTransaction transaction)
        {
            var formattedSql = Implementation.PreFormatSql(sql);

            Logger.Trace(formattedSql, param);
            return BaseConnection.QuerySingle<T>(formattedSql, param, transaction?.Transaction);
        }

        public virtual T QuerySingle<T>(string sql, object param = null)
        {
            return QuerySingle<T>(sql, param);
        }

        public virtual int Execute(string sql, object param, IDatabaseTransaction transaction)
        {
            var formattedSql = Implementation.PreFormatSql(sql);

            Logger.Trace(formattedSql, param);
            return BaseConnection.Execute(formattedSql, param, transaction?.Transaction);
        }

        public int Execute(string sql, object param = null)
        {
            return Execute(sql, param, null);
        }

        public virtual DataTable GetDataTable(string sql, object param, IDatabaseTransaction transaction)
        {
            var formattedSql = Implementation.PreFormatSql(sql);

            Logger.Trace(formattedSql, param);

            var dataTable = new DataTable();
            dataTable.Load(BaseConnection.ExecuteReader(sql, param, transaction?.Transaction));
            return dataTable;
        }

        public DataTable GetDataTable(string sql, object param = null)
        {
            return GetDataTable(sql, param, null);
        }

        public virtual IDatabaseTransaction BeginTransaction()
        {
            return new DatabaseTransaction(this);
        }

        public virtual IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return new DatabaseTransaction(this, isolationLevel);
        }

        public IResultFailureValue<Exception> Batch(Func<IDatabaseCommander, bool> function)
        {
            return BatchCore(() => new DatabaseTransaction(this), function);
        }

        public IResultFailureValue<Exception> Batch(Func<IDatabaseCommander, bool> function, IsolationLevel isolationLevel)
        {
            return BatchCore(() => new DatabaseTransaction(this, isolationLevel), function);
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
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public class DatabaseAccessor<TDbConnection> : DatabaseAccessor
        where TDbConnection : IDbConnection
    {
        public DatabaseAccessor(IDatabaseFactory connectionFactory, ILogger logger)
            : base(connectionFactory, logger)
        { }

        public DatabaseAccessor(IDatabaseFactory connectionFactory, ILoggerFactory loggerFactory)
            : base(connectionFactory, loggerFactory)
        { }

        #region proeprty

        public TDbConnection Connection => (TDbConnection)BaseConnection;

        #endregion
    }
}
