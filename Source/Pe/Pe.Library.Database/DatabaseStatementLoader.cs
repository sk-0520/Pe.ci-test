using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// <see cref="IDatabaseStatementLoader"/> の最小実装。
    /// </summary>
    /// <remarks>
    /// <para><see cref="IDatabaseStatementLoader.LoadStatement(string)"/>だけ対応すればよろし。</para>
    /// </remarks>
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
