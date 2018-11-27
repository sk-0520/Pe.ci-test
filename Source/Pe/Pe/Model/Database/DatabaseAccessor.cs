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

namespace ContentTypeTextNet.Pe.Main.Model.Database
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
        /// 複数のSQLが格納されたSQLを分割する。
        /// </summary>
        /// <param name="baseSql"></param>
        /// <returns></returns>
        IEnumerable<string> SplitSql(string baseSql);

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
    public abstract class DatabaseAccessorBase<TDbConnection> : ReaderWriterLocker, IDatabaseCommander, IDatabaseSqlImplementation, IDatabaseChecker
        where TDbConnection: IDbConnection, new()
    {
        public DatabaseAccessorBase(IDatabaseConnectionCreator<TDbConnection> connectionCreator, ILoggerFactory loggerFactory)
        {
            ConnectionCreator = connectionCreator;

            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            LazyConnection = new Lazy<TDbConnection>(() => ConnectionCreator.CreateConnection());
        }

        #region property

        protected IDatabaseConnectionCreator<TDbConnection> ConnectionCreator { get; }
        Lazy<TDbConnection> LazyConnection { get; }
        internal virtual TDbConnection Connection => LazyConnection.Value;

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        #endregion

        #region function

        public virtual IEnumerable<T> Query<T>(string sql, object param, IDbTransaction transaction, bool buffered)
        {
            var formattedSql = PreFormatSql(sql);

            Logger.LogDebug(formattedSql, param);
            return Connection.Query<T>(formattedSql, param, transaction, buffered);
        }

        public virtual IEnumerable<dynamic> Query(string sql, object param, IDbTransaction transaction, bool buffered)
        {
            var formattedSql = PreFormatSql(sql);

            Logger.LogDebug(formattedSql, param);
            return Connection.Query(formattedSql, param, transaction, buffered);
        }

        public virtual int Execute(string sql, object param, IDbTransaction transaction)
        {
            var formattedSql = PreFormatSql(sql);

            Logger.LogDebug(formattedSql, param);
            return Connection.Execute(formattedSql, param, transaction);
        }

        public void Open()
        {
            Connection.Open();
        }

        public virtual DatabaseTransaction<TDbConnection> BeginTransaction()
        {
            return new DatabaseTransaction<TDbConnection>(this);
        }

        public virtual DatabaseTransaction<TDbConnection> BeginTransaction(IsolationLevel isolationLevel)
        {
            return new DatabaseTransaction<TDbConnection>(this, isolationLevel);
        }


        #endregion

        #region IDatabaseCommand

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
        /// 複数のSQLが格納されたSQLを分割する。
        /// <para>既定では[行頭]--&lt;SPLIT&gt;[行末]が分割の対象となる</para>
        /// </summary>
        /// <param name="baseSql"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> SplitSql(string baseSql)
        {
            var reg = new Regex(@"^--<SPLIT(\:.*)?>[\r\n]+", RegexOptions.Multiline | RegexOptions.ExplicitCapture);
            var result = reg.Split(baseSql).Skip(1);
            return result;
        }

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
                    Connection.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
