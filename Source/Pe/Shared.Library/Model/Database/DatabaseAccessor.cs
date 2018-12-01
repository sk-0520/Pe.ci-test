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
    public interface IDatabaseCommander
    {
        #region function

        IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true);

        IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true);

        int Execute(string sql, object param = null);

        #endregion
    }

    /// <summary>
    /// SQLの実装依存的ななんつーかそんな感じの処理。
    /// </summary>
    public interface IDatabaseSqlImplementation
    {
        /// <summary>
        /// SQL 実行前に実行する SQL に対して変換処理を実行。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        string PreFormatSql(string sql);
    }

    /// <summary>
    /// DB に対してのチェック処理。
    /// </summary>
    public interface IDatabaseChecker
    {
        /// <summary>
        /// キーとしての値が null か。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsNullKey(string key);

        /// <summary>
        /// ID としての値が null か。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsNullId(uint id);

        /// <summary>
        /// タイムスタンプとしての値が null か。
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        bool IsNullTimestamp(DateTime timestamp);
    }


    /// <summary>
    /// DBアクセスに対してラップする。
    /// <para>DBまで行く前にプログラム側で制御する目的。</para>
    /// </summary>
    public abstract class DatabaseAccessorBase : ReaderWriterLocker, IDatabaseCommander, IDatabaseSqlImplementation, IDatabaseChecker
    {
        public DatabaseAccessorBase(IDatabaseConnectionFactory connectionFactory, ILogger logger)
        {
            ConnectionFactory = connectionFactory;

            Logger = logger;

            LazyConnection = new Lazy<IDbConnection>(() => {
                var con = ConnectionFactory.CreateConnection();
                con.Open();
                return con;
            });
        }

        #region property

        protected IDatabaseConnectionFactory ConnectionFactory { get; }
        Lazy<IDbConnection> LazyConnection { get; }
        public virtual IDbConnection BaseConnection => LazyConnection.Value;

        protected ILogger Logger { get; }

        #endregion

        #region function

        public virtual IEnumerable<T> Query<T>(string sql, object param, IDbTransaction transaction, bool buffered)
        {
            var formattedSql = PreFormatSql(sql);

            Logger.Debug(formattedSql, param);
            return BaseConnection.Query<T>(formattedSql, param, transaction, buffered);
        }

        public virtual IEnumerable<dynamic> Query(string sql, object param, IDbTransaction transaction, bool buffered)
        {
            var formattedSql = PreFormatSql(sql);

            Logger.Debug(formattedSql, param);
            return BaseConnection.Query(formattedSql, param, transaction, buffered);
        }

        public virtual int Execute(string sql, object param, IDbTransaction transaction)
        {
            var formattedSql = PreFormatSql(sql);

            Logger.Debug(formattedSql, param);
            return BaseConnection.Execute(formattedSql, param, transaction);
        }

        public virtual IDatabaseTransaction BeginTransaction()
        {
            return new DatabaseTransaction(this);
        }

        public virtual IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return new DatabaseTransaction(this, isolationLevel);
        }

        protected virtual IResultFailureValue<Exception> BatchCore(Func<IDatabaseTransaction> transactionCreator, Action<IDatabaseCommander> action)
        {
            var transaction = transactionCreator();
            try {
                action(transaction);
                return ResultFailureValue.Success<Exception>();
            } catch(Exception ex) {
                transaction.Rollback();
                return ResultFailureValue.Failure(ex);
            }
        }

        public IResultFailureValue<Exception> Batch(Action<IDatabaseCommander> action)
        {
            return BatchCore(() => new DatabaseTransaction(this), action);
        }

        public IResultFailureValue<Exception> Batch(Action<IDatabaseCommander> action, IsolationLevel isolationLevel)
        {
            return BatchCore(() => new DatabaseTransaction(this, isolationLevel), action);
        }

        #endregion

        #region IDatabaseCommander

        public IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true)
        {
            return Query<T>(sql, param, null, buffered);
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true)
        {
            return Query(sql, param, null, buffered);
        }

        public int Execute(string sql, object param = null)
        {
            return Execute(sql, param, null);
        }

        #endregion

        #region IDatabaseSqlImplementation

        /// <summary>
        /// SQL 実行前に実行する SQL に対して変換処理を実行。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual string PreFormatSql(string sql)
        {
            return sql;
        }

        #endregion

        #region IDatabaseChecker

        public virtual bool IsNullKey(string key) => throw new NotImplementedException();

        public virtual bool IsNullId(uint id) => throw new NotImplementedException();

        public virtual bool IsNullTimestamp(DateTime timestamp) => throw new NotImplementedException();

        #endregion

        #region ReaderWriterLocker

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

    public abstract class DatabaseAccessorBase<TDbConnection> : DatabaseAccessorBase
        where TDbConnection : IDbConnection
    {
        public DatabaseAccessorBase(IDatabaseConnectionFactory connectionFactory, ILogger logger)
            : base(connectionFactory, logger)
        { }

        #region proeprty

        public TDbConnection Connection => (TDbConnection)BaseConnection;

        #endregion
    }
}
