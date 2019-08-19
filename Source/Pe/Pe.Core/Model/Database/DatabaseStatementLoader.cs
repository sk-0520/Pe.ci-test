using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Model.Database
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
        string LoadStatementByCurrent();

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
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        protected ILogger Logger { get; }

        #endregion

        #region function

        protected MethodBase GetCurrentMember(int skipFrames = 1)
        {
            var stackFrame = new StackFrame(skipFrames + 1);
            return stackFrame.GetMethod()!;
        }

        #endregion

        #region IDatabaseStatementLoader

        public abstract string LoadStatement(string key);

        public abstract string LoadStatementByCurrent();

        #endregion
    }
}
