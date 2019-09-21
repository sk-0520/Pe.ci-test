using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// データベースに対する実行文を取得する。
    /// <para>SQLだわな。</para>
    /// </summary>
    public interface IDatabaseStatementLoader
    {
        #region function

        /// <summary>
        /// キーから文を取得。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string LoadStatement(string key);
        /// <summary>
        /// 呼び出しクラス情報から文を取得する。
        /// </summary>
        /// <returns></returns>
        string LoadStatementByCurrent(Type caller, [CallerMemberName] string callerMemberName = "");

        #endregion
    }

    public abstract class DatabaseStatementLoaderBase : IDatabaseStatementLoader
    {
        public DatabaseStatementLoaderBase(ILogger logger)
        {
            Logger = logger;
        }

        public DatabaseStatementLoaderBase(ILoggerFactory loggerFactory)
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

        public virtual string LoadStatementByCurrent(Type callerType, [CallerMemberName] string callerMemberName = "")
        {
            var key = callerType.FullName + "." + callerMemberName;
            return LoadStatement(key);
        }

        #endregion
    }
}
