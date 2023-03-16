using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Standard.Database
{
    /// <summary>
    /// データベースに対する問い合わせ文を取得する。
    /// <para>RDB を主軸に実装しているので基本的に SQL となる。</para>
    /// </summary>
    public interface IDatabaseStatementLoader
    {
        #region function

        /// <summary>
        /// キーからデータベース実行文を取得。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string LoadStatement(string key);
        /// <summary>
        /// 呼び出しクラス・メンバ名の完全名からデータベース実行文を取得する。
        /// </summary>
        /// <returns></returns>
        string LoadStatementByCurrent(Type callerType, [CallerMemberName] string callerMemberName = "");

        #endregion
    }

    /// <summary>
    /// <see cref="IDatabaseStatementLoader"/> の最小実装。
    /// <para><see cref="IDatabaseStatementLoader.LoadStatement(string)"/>だけ対応すればよろし。</para>
    /// </summary>
    public abstract class DatabaseStatementLoaderBase: IDatabaseStatementLoader
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="logger"></param>
        protected DatabaseStatementLoaderBase(ILogger logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="loggerFactory"></param>
        protected DatabaseStatementLoaderBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected ILogger Logger { get; }

        #endregion

        #region function
        #endregion

        #region IDatabaseStatementLoader

        public abstract string LoadStatement(string key);

        public string LoadStatementByCurrent(Type callerType, [CallerMemberName] string callerMemberName = "")
        {
            var key = callerType.FullName + "." + callerMemberName;
            return LoadStatement(key);
        }

        #endregion
    }
}
