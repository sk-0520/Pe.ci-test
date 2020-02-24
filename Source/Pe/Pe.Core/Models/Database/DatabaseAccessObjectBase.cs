using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// だお！
    /// </summary>
    public abstract class DatabaseAccessObjectBase
    {
        public DatabaseAccessObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            Commander = commander;
            StatementLoader = statementLoader;
            Implementation = implementation;
        }

        #region property

        /// <summary>
        /// データベースをお話担当。
        /// </summary>
        protected IDatabaseCommander Commander { get; }
        /// <summary>
        /// データベース文の読み込みストア。
        /// </summary>
        protected IDatabaseStatementLoader StatementLoader { get; }
        /// <summary>
        /// データベースの実装依存専用窓口。
        /// </summary>
        protected IDatabaseImplementation Implementation { get; }
        /// <summary>
        /// ログ出力担当。
        /// </summary>
        protected ILogger Logger { get; }
        /// <summary>
        /// ログ出力(<see cref="ILogger"/>)生成機構。
        /// </summary>
        protected ILoggerFactory LoggerFactory { get; }

        #endregion

        #region function

        /// <summary>
        /// <see cref="IDatabaseStatementLoader.LoadStatementByCurrent(Type, string)"/>のヘルパー巻数。
        /// </summary>
        /// <param name="callerMemberName"></param>
        /// <returns></returns>
        protected string LoadStatement([CallerMemberName] string callerMemberName = "")
        {
            var type = GetType();
            return StatementLoader.LoadStatementByCurrent(type, callerMemberName);
        }


        #endregion
    }
}
