using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Model.Database
{
    /// <summary>
    /// だお！
    /// </summary>
    public abstract class DatabaseAccessObjectBase
    {
        DatabaseAccessObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            Commander = commander;
            StatementLoader = statementLoader;
            Implementation = implementation;
        }

        public DatabaseAccessObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILogger logger)
            : this(commander, statementLoader, implementation)
        {
            Logger = logger;
        }

        public DatabaseAccessObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : this(commander, statementLoader, implementation)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        /// <summary>
        /// データベース文。
        /// </summary>
        protected IDatabaseCommander Commander { get; }
        /// <summary>
        /// データベース文の読み込みストア。
        /// </summary>
        protected IDatabaseStatementLoader StatementLoader { get; }
        protected IDatabaseImplementation Implementation { get; }
        /// <summary>
        /// ログ出力担当。
        /// </summary>
        protected ILogger Logger { get; }

        #endregion

        #region function
        #endregion
    }
}
